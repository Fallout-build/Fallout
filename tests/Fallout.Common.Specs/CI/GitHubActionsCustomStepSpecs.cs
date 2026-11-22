using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Fallout.Common.CI.GitHubActions.Configuration;
using Fallout.Common.Utilities;
using Fallout.Testing;
using Xunit;

namespace Fallout.Common.Specs.CI;

// Renders a single custom step in isolation, like GitHubActionsWorkflowDispatchTriggerSpecs. Output is
// snapshotted (line endings normalized), matching the generator testing strategy elsewhere.
public class GitHubActionsCustomStepSpecs
{
    private static void Verify(
        GitHubActionsCustomStep step,
        [CallerFilePath] string sourcePath = "",
        [CallerMemberName] string member = "")
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, leaveOpen: true);
        step.Write(new CustomFileWriter(writer, indentationFactor: 2, commentPrefix: "#"));
        writer.Flush();

        stream.Seek(offset: 0, SeekOrigin.Begin);
        Snapshot.Verify(new StreamReader(stream).ReadToEnd(), sourcePath: sourcePath, member: member);
    }

    [Fact]
    public void Uses_step_with_with_block()
        => Verify(new GitHubActionsCustomStep
        {
            Name = "Setup Node",
            Uses = "actions/setup-node@v4",
            With = new Dictionary<string, string>
            {
                ["node-version"] = "20"
            }
        });

    [Fact]
    public void Uses_step_without_name_starts_with_uses()
        => Verify(new GitHubActionsCustomStep
        {
            Uses = "actions/checkout@v4"
        });

    [Fact]
    public void Single_line_run_renders_inline()
        => Verify(new GitHubActionsCustomStep
        {
            Name = "Echo",
            Run = new[]
            {
                "echo hi"
            }
        });

    // A name with a colon-space would be invalid YAML unquoted; it must be single-quoted like the built-in steps.
    [Fact]
    public void Name_with_a_colon_is_quoted()
        => Verify(new GitHubActionsCustomStep
        {
            Name = "Deploy: prod",
            Run = new[]
            {
                "echo hi"
            }
        });

    // An embedded apostrophe must be YAML-escaped by doubling (''), not the backslash the shared
    // SingleQuote() helper would produce (which is invalid inside a YAML single-quoted scalar).
    [Fact]
    public void Name_with_an_apostrophe_is_yaml_escaped()
        => Verify(new GitHubActionsCustomStep
        {
            Name = "Bob's step",
            Run = new[]
            {
                "echo hi"
            }
        });

    // Multi-entry with:/env: must render in a deterministic (ordinal) order regardless of insertion order.
    [Fact]
    public void Multi_entry_with_renders_in_ordinal_order()
        => Verify(new GitHubActionsCustomStep
        {
            Uses = "some/action@v1",
            With = new Dictionary<string, string>
            {
                ["beta"] = "2",
                ["alpha"] = "1"
            }
        });

    [Fact]
    public void Multi_line_run_renders_block_scalar()
        => Verify(new GitHubActionsCustomStep
        {
            Run = new[]
            {
                "echo one",
                "echo two"
            }
        });

    [Fact]
    public void All_optional_fields_render_in_fixed_order()
        => Verify(new GitHubActionsCustomStep
        {
            Name = "Full",
            Id = "full",
            Uses = "some/action@v1",
            With = new Dictionary<string, string>
            {
                ["k"] = "v"
            },
            Env = new Dictionary<string, string>
            {
                ["E"] = "1"
            },
            If = "success()",
            ContinueOnError = true,
            TimeoutMinutes = 5
        });

    [Fact]
    public void Run_step_renders_shell_when_set()
        => Verify(new GitHubActionsCustomStep
        {
            Run = new[]
            {
                "gci"
            },
            Shell = "pwsh"
        });
}
