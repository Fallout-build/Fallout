# Tests

Fallout tests use xUnit v3, FluentAssertions and Verify. Don't add another test or mocking framework.

## Structure

- Use Arrange, Act and Assert, marked with `// Arrange`, `// Act` and `// Assert` comments. Leave out a phase that has nothing in it.
- Put each statement under the phase it belongs to. A value that feeds the call under test is Arrange, however late it appears. Reading the result of the call is Act. Every check is Assert, even a one-line sanity check.
- Don't invent other headings, such as `// Assign / Act`. The only allowed combination is `// Act/Assert`, for a test where the act is the check, such as "does not throw".
- Don't use constructors or shared fields in a `Specs` class. Set up what a test needs inside the test, or in a helper that the test calls.
- Set up state through the public API where you can. A test that sets up through the internals no longer proves that the public API works.

## What to leave out

- Leave out anything the scenario doesn't depend on. When everything is shown, nothing stands out.
- Don't check something the setup already guarantees, and don't restate defaults you rely on.
- Ask of each assertion: would the test prove less without it? If not, remove it.

## Assertions

- Use the single most specific FluentAssertions method. A row of checks per property or key almost always becomes one `BeEquivalentTo` or one collection assertion, with a better failure message.
- Avoid `AssertionScope`. Normal tests rarely need every failure reported at once, and a `null` inside the scope causes a `NullReferenceException` instead of a clear failure.
- Compare with values you know independently. Never compute the expected value with the production code under test.
- Build dates with the FluentAssertions extensions, such as `1.September(2026)`. Never use `DateTime.Parse` in a test.
- Never take a value from one fixed input and apply it to a different input that is read at run time. For example, a time-zone offset taken from a fixed date is wrong for a date in another DST period.

## Test data

- Make the values that matter visible in the test body.
- Make test data rows describe themselves. A row of bare values in `[InlineData]` or `TheoryData` doesn't tell the reader which value is input and which is expected. Use a named record or named arguments.
- Prefer a Test Data Builder over small private helper methods. Name the builder after the type it builds, and don't add methods that no test uses yet.
- Hundreds of theory rows are a finding. Keep a few rows that each show a different rule, and move the bulk into a data file.

## Names

A test name is read in a failure report, usually without the code. It must say what is true of the system.

- Use snake case, and state a fact. Never use "should" or "when".
- State the rule, not the mechanism that enforces it. "An empty project path is not valid" is better than "Validation rejects the request".
- Write a real English sentence, with articles: `A_project_path_is_required`, not `Project_path_required`.
- Never use the name of the method under test, a type name or any other code element.
- Don't use `Can_` for behavior that always happens. `Can_throw_for_a_missing_project` suggests it sometimes doesn't.
- Don't repeat the purpose of the test class in the method name.
- Suffix test classes with `Specs`, never `Tests`.

| Instead of | Write |
| --- | --- |
| `GetNuGetReleaseNotes_WithMissingChangelog_DoesNotThrow` | `Missing_changelog_does_not_add_a_full_changelog_link_to_release_notes` |
| `Throws_error_if_project_path_is_empty` | `An_empty_project_path_is_not_valid` |
| `Can_throw_when_target_is_not_found` | `An_unknown_target_is_rejected` |
| `ParseTestRunner_returns_null_for_unknown` | `Unknown_test_runner_names_are_ignored` |

## Coverage

- Give every new rule its own test.
- When a rule depends on the environment, such as an environment variable or the installed SDK, move the decision into a method that takes those values as parameters. Test that method.
- Test reusable parts, such as a parser or an argument builder, on their own and not only through the code that uses them.
- Test files and namespaces mirror the project under test.
