#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Fallout.Components;

/// <summary>
/// The default rule table of <see cref="IInspect"/>. The first rule whose pattern matches a finding's
/// rule ID sets the bucket, so specific rules come before wildcards. Part of the experimental
/// <see cref="IInspect"/> surface (<c>FALLOUT005</c>).
/// </summary>
[Experimental("FALLOUT005")]
public static class InspectRuleTable
{
    /// <summary>ReSharper and Roslyn rules that can change runtime behavior.</summary>
    public static IReadOnlyList<(string Rule, InspectBucket Bucket)> RuntimeCandidates { get; } = Bucket(InspectBucket.RuntimeCandidate,
        // Unhandled enum values in switch statements and expressions.
        "Switch*EnumValues*",
        "SwitchStatementMissingSomeEnumCases*",
        "IDE0010",
        "IDE0072",
        "CS8509",
        "CS8524",
        // Parse calls that throw on invalid input.
        "AL1308",
        // Closures over disposed or modified variables.
        "AccessToDisposedClosure",
        "AccessToModifiedClosure",
        "AccessToForEachVariableInClosure",
        // Cancellation tokens that are not passed on.
        "AL1313",
        "CA2016",
        "MethodSupportsCancellation",
        "PossiblyMistakenUseOfCancellationToken",
        // Multiple enumeration of a lazy sequence.
        "PossibleMultipleEnumeration",
        "CA1851",
        // Floating point equality and lost precision.
        "CompareOfFloatsByEqualityOperator",
        "PossibleLossOfFraction",
        "IntDivisionByZero",
        "IntVariableOverflow*",
        // Null, cast and invalid operation paths.
        "PossibleNullReferenceException",
        "AssignNullToNotNullAttribute",
        "PossibleInvalidOperationException*",
        "PossibleInvalidCastException*",
        "PossibleIntendedRethrow",
        "CoVariantArrayConversion",
        "NonReadonlyMemberInGetHashCode",
        "InconsistentlySynchronizedField",
        "AsyncVoidMethod",
        "AsyncVoidLambda",
        "FunctionNeverReturns",
        "CS8602",
        "CS8604");

    /// <summary>Rules that need a person or agent to judge the code.</summary>
    public static IReadOnlyList<(string Rule, InspectBucket Bucket)> ReviewRules { get; } = Bucket(InspectBucket.Review,
        // SQL and other security rules.
        "CA2100",
        "CA3*",
        "CA5*",
        // Guards that contradict nullable annotations.
        "*AccordingToNullableAPIContract",
        "*NonNullableAccordingToAPIContract",
        "ParameterOnlyUsedForPreconditionCheck*",
        // Resource handling and blocking calls.
        "UsingStatementResourceInitialization",
        "ShortLivedHttpClient",
        "MethodHasAsyncOverload*",
        "AsyncMethodWithoutAwait",
        "CA1816",
        "CA2000",
        "CA2213",
        // Culture and comparison behavior.
        "CA1304",
        "CA1305",
        "CA1307",
        "CA1309",
        "CA1310",
        "CA1311",
        "SpecifyACultureInStringConversionExplicitly",
        "StringIndexOfIsCultureSpecific*",
        "StringCompareIsCultureSpecific*",
        // Logic that may not do what it says.
        "DuplicatedSequentialIfBodies",
        "DuplicatedStatements",
        "RouteTemplates.*",
        "EmptyGeneralCatchClause",
        "VariableHidesOuterVariable",
        "MemberHidesStaticFromOuterClass",
        "MustUseReturnValue",
        "AD0001");

    /// <summary>Style and cleanup rules that <c>jb cleanupcode</c> or a code fix can apply.</summary>
    public static IReadOnlyList<(string Rule, InspectBucket Bucket)> MechanicalRules { get; } = Bucket(InspectBucket.Mechanical,
        "Redundant*",
        "Use*",
        "Merge*",
        "Convert*",
        "Replace*",
        "Simplify*",
        "Invert*",
        "Introduce*",
        "Arrange*",
        "Move*",
        "Join*",
        "Inline*",
        "Unused*",
        "NotAccessed*",
        "*CanBe*",
        "CanReplace*",
        "Separate*",
        "Prefer*",
        "Extract*",
        "ClassNeverInstantiated*",
        "TailRecursiveCall",
        "TooWideLocalVariableScope",
        "AppendToCollectionExpression",
        "ExplicitCallerInfoArgument",
        "InconsistentNaming",
        "CheckNamespace",
        "InvalidXmlDocComment",
        "*Typo",
        "Empty*",
        "SuggestVarOrType*",
        "SuggestBaseTypeForParameter*",
        "IDE*",
        "CA1*",
        "CA2*",
        "SYSLIB1*",
        "ASP*",
        "AL*",
        "xUnit*",
        "MSTEST*",
        "NUnit*");

    /// <summary>All default rules: runtime candidates first, then review rules, then mechanical rules.</summary>
    public static IReadOnlyList<(string Rule, InspectBucket Bucket)> Default { get; } =
        [..RuntimeCandidates, ..ReviewRules, ..MechanicalRules];

    private static IReadOnlyList<(string Rule, InspectBucket Bucket)> Bucket(InspectBucket bucket, params string[] rules)
    {
        var table = new (string Rule, InspectBucket Bucket)[rules.Length];
        for (var i = 0; i < rules.Length; i++)
        {
            table[i] = (rules[i], bucket);
        }

        return table;
    }
}
