using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fallout.Common.Tooling;
using Fallout.Common.Utilities;
using Serilog;
using static Fallout.Common.Utilities.ReflectionUtility;

namespace Fallout.Common;

internal partial class ParameterService
{
    // internal ArgumentParser ArgumentsFromFilesService;
    internal Func<string, Type, object> ArgumentsFromFilesService;
    internal ArgumentParser ArgumentsFromCommitMessageService;

    private readonly Func<ArgumentParser> argumentParserProvider;
    private readonly Func<IReadOnlyDictionary<string, string>> environmentVariablesProvider;

    public ParameterService(
        Func<ArgumentParser> argumentParserProvider,
        Func<IReadOnlyDictionary<string, string>> environmentVariablesProvider)
    {
        this.argumentParserProvider = argumentParserProvider;
        this.environmentVariablesProvider = environmentVariablesProvider;
    }

    private ArgumentParser ArgumentsParser => argumentParserProvider.Invoke();

    private IReadOnlyDictionary<string, string> Variables => environmentVariablesProvider.Invoke();

    public static bool IsParameter(string value)
    {
        return value != null && value.StartsWith("-");
    }

    public static string GetParameterDashedName(MemberInfo member)
    {
        return GetParameterDashedName(GetParameterMemberName(member));
    }

    public static string GetParameterDashedName(string name)
    {
        return name.SplitCamelHumpsWithKnownWords().JoinDash().ToLowerInvariant();
    }

    public static string GetParameterMemberName(string name)
    {
        return name.Replace("-", string.Empty);
    }

    public static string GetParameterMemberName<T>(Expression<Func<T>> expression)
    {
        var member = expression.GetMemberInfo();
        return GetParameterMemberName(member);
    }

    public static string GetParameterMemberName(MemberInfo member)
    {
        var attribute = member.GetCustomAttribute<ParameterAttribute>().NotNull();
        var prefix = member.DeclaringType.NotNull().GetCustomAttribute<ParameterPrefixAttribute>()?.Prefix;
        return prefix + (attribute.Name ?? member.Name);
    }

    public static string GetParameterDescription(MemberInfo member)
    {
        var attribute = member.GetCustomAttribute<ParameterAttribute>().NotNull();
        return attribute.Description?.TrimEnd('.');
    }

    public static IEnumerable<(string Text, object Object)> GetParameterValueSet(MemberInfo member, object instance)
    {
        var attribute = member.GetCustomAttribute<ParameterAttribute>().NotNull();
        var memberType = member.GetMemberType().GetScalarType();

        IEnumerable<(string Text, object Object)> TryGetFromValueProvider()
        {
            if (attribute.ValueProviderMember == null)
            {
                return null;
            }

            var valueProviderType = attribute.ValueProviderType ?? instance.GetType();
            var valueProvider = valueProviderType
                .GetMember(attribute.ValueProviderMember, All)
                .SingleOrDefault()
                .NotNull($"No single provider '{valueProviderType.Name}.{member.Name}' found");

            Assert.True(valueProvider.GetMemberType() == typeof(IEnumerable<string>),
                $"Value provider '{valueProvider.Name}' must be of type '{typeof(IEnumerable<string>).GetDisplayShortName()}'");

            return valueProvider.GetValue<IEnumerable<string>>(instance).Select(x => (x, (object)x));
        }

        IEnumerable<(string Text, object Object)> TryGetFromEnumerationClass() =>
            memberType.IsSubclassOf(typeof(Enumeration))
                ? memberType.GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => (x.Name, x.GetValue()))
                : null;

        IEnumerable<(string Text, object Object)> TryGetFromEnum()
        {
            var enumType = memberType.IsEnum
                ? memberType
                : Nullable.GetUnderlyingType(memberType) is { } underlyingType && underlyingType.IsEnum
                    ? underlyingType
                    : null;

            return enumType != null
                ? enumType.GetEnumNames().Select(x => (x, Enum.Parse(enumType, x)))
                : null;
        }

        try
        {
            return (attribute.GetValueSet(member, instance) ??
                    TryGetFromValueProvider() ??
                    TryGetFromEnumerationClass() ??
                    TryGetFromEnum())
                ?.OrderBy(x => x.Item1);
        }
        catch (Exception exception)
        {
            Log.Warning(exception.Unwrap(), "Could not resolve value-set for {Parameter}", member.GetDisplayName());
            return null;
        }
    }

    public static object GetFromMemberInfo(MemberInfo member, Type destinationType, Func<string, Type, char?, object> provider)
    {
        var attribute = member.GetCustomAttribute<ParameterAttribute>().NotNull();
        var separator = (attribute.Separator ?? string.Empty).SingleOrDefault();
        return provider.Invoke(GetParameterMemberName(member), destinationType ?? member.GetMemberType(), separator);
    }

    public object GetParameter(string parameterName, Type destinationType, char? separator)
    {
        object TryFromCommandLineArguments() =>
            HasCommandLineArgument(parameterName)
                ? GetCommandLineArgument(parameterName, destinationType, separator)
                : null;

        object TryFromCommandLinePositionalArguments() =>
            parameterName == Constants.InvokedTargetsParameterName
                ? GetPositionalCommandLineArguments(destinationType, separator)
                : null;

        object TryFromEnvironmentVariables() =>
            GetEnvironmentVariable(parameterName, destinationType, separator);

        // TODO: nuke <target> ?
        object TryFromProfileArguments() =>
            // ArgumentsFromFilesService?.GetNamedArgument(parameterName, destinationType, separator);
            ArgumentsFromFilesService?.Invoke(parameterName, destinationType);

        object TryFromCommitMessageArguments() =>
            ArgumentsFromCommitMessageService?.GetNamedArgument(parameterName, destinationType, separator);

        return TryFromCommitMessageArguments() ??
               TryFromCommandLineArguments() ??
               TryFromCommandLinePositionalArguments() ??
               TryFromEnvironmentVariables() ??
               TryFromProfileArguments();
    }

    public object GetCommandLineArgument(string argumentName, Type destinationType, char? separator)
    {
        return ArgumentsParser.GetNamedArgument(argumentName, destinationType, separator);
    }

    public object GetCommandLineArgument(int position, Type destinationType, char? separator)
    {
        return ArgumentsParser.GetPositionalArgument(position, destinationType, separator);
    }

    public object GetPositionalCommandLineArguments(Type destinationType, char? separator = null)
    {
        return ArgumentsParser.GetAllPositionalArguments(destinationType, separator);
    }

    public bool HasCommandLineArgument(string argumentName)
    {
        return ArgumentsParser.HasArgument(argumentName);
    }

    public object GetEnvironmentVariable(string variableName, Type destinationType, char? separator)
    {
        static string GetTrimmedName(string name)
            => new(name.Where(char.IsLetterOrDigit).ToArray());

        if (!Variables.TryGetValue(variableName, out var value))
        {
            var trimmedVariableName = GetTrimmedName(variableName);

            // Prefixed names are matched before the bare parameter name. FALLOUT_HOST would
            // otherwise be shadowed by an unrelated bare HOST, which is the hostname variable
            // every Unix shell exports.
            var prefixedValues = Variables
                .Where(x => GetTrimmedName(x.Key).EqualsOrdinalIgnoreCase($"FALLOUT{trimmedVariableName}") ||
                            GetTrimmedName(x.Key).EqualsOrdinalIgnoreCase($"NUKE{trimmedVariableName}")).ToList();

            var alternativeValues = prefixedValues.Count > 0
                ? prefixedValues
                : Variables.Where(x => GetTrimmedName(x.Key).EqualsOrdinalIgnoreCase(trimmedVariableName)).ToList();

            if (alternativeValues.Count > 1)
            {
                ReportWarning($"could not resolve '{variableName}' since multiple values are provided");
            }

            if (alternativeValues.Count == 1)
            {
                value = alternativeValues.Single().Value;
            }
            else
            {
                return destinationType.GetDefaultValue();
            }
        }

        try
        {
            return Convert(value, destinationType, separator, booleanDefault: false);
        }
        catch (Exception exception)
        {
            // A value the build cannot read must not abort the process. Built-in parameters are
            // resolved inside FalloutBuild's static constructor, so throwing here kills the build
            // before the consumer can act on it and before any target runs.
            ReportWarning(
                $"could not resolve '{variableName}' from environment variable value '{value}'. " +
                $"Ignoring it and using the default. {exception.Message}");

            return destinationType.GetDefaultValue();
        }
    }

    /// <summary>
    /// Writes a warning to standard error. Parameters are resolved before Serilog is configured,
    /// so anything sent to the log is never shown.
    /// </summary>
    private static void ReportWarning(string message)
    {
        Console.Error.WriteLine($"warning: {message}");
    }
}
