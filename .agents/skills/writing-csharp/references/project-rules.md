# Project rules

Rules that add to the [C# Coding Guidelines](coding-guidelines.md), and the places where Fallout makes an exception to them.

## Exceptions for Fallout

- **Static task classes are fine.** The `*Tasks` classes (`DotNetTasks`, `GitTasks`, ...) and their extension methods are the public API of a build script. AV1008 and AV1125 don't apply to them. A new static class that is not part of that API still needs a reason.
- **`bool?` is fine on tool options.** In a `ToolOptions` class, `null` means "don't pass the argument", which differs from `false`. Everywhere else, use `bool` when no value means `false`.
- **Public API is a promise.** A public interface or extension point is fine with one implementation when consumers implement it. Internal interfaces still need a reason (see below).
- **Check the target framework.** Many projects also target `netstandard2.0`. A new C# feature (AV1145, AV1155, `required`, `init`) must compile for every target framework of the project.
- **Generated code is exempt.** Don't apply these rules to `*.Generated.cs` files. Change the tool's `.json` spec or the generator instead.

## Design

- Make a type `internal` unless it is meant to be public API. Its members may be `public`: they can't be more visible than the type. Don't flag a `public` member on an `internal` type.
- Add an abstraction only when at least three places use it.
- Don't add an interface only for dependency injection or tests. Add one when it separates two parts of the system that must not know each other.
- Make nullability match the contract. Mark a property that must have a value as `required`, or make it non-nullable, instead of declaring it nullable and rejecting `null` at run time. A property that may be absent must be nullable, not just documented as optional.
- When you validate arguments, check each one on its own and throw an exception that names it. One message for several fields doesn't tell the caller which one is wrong.
- Don't add an early-return guard before code that already handles empty input. It adds noise and can skip logging that should still run.

## Names

- A method whose name starts with `Get` never returns `null`. If it can, name it `Find…` or `TryGet…`.
- Never abbreviate a word in a folder, type or namespace name: `Configuration`, not `Config`. Widely used acronyms such as `Id`, `Uri` and `Xml` are fine.
- The namespace follows the folder structure.

## Code

- Use `var` only when the right side shows the type, as in `new Foo()` or a cast. Never use it for built-in types such as `bool`, `int` and `string`. `var` is required for anonymous types. Test code is exempt.
- Check for null with `is null` and `is not null`, not `== null` and `!= null`.
- Never compare a `bool` to `true` or `false`. Write `if (isActive)`, not `if (isActive == true)`.
- Don't write `(await FooAsync()).ToList()`. Use an async LINQ method, or split it over two lines.
- Never use `Thread.Sleep` in async code. Use `await Task.Delay`.
- Never put `!` after a deserialization call, such as `JsonSerializer.Deserialize<T>(...)!`. A `null` result then turns into a `NullReferenceException` far from the cause. Check for `null` and throw a clear exception.
- Put `[UsedImplicitly]` (JetBrains.Annotations) on a member that only reflection or a serializer uses, such as a `[Parameter]` field. Use `[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]` on the type when this applies to all of its members.

## Size and order

- Keep a method under 15 statements, at one level of abstraction. Split a longer method into well-named private methods. A flat list of independent guard clauses is fine.
- Order methods so that a method comes before the methods it calls. The file then reads from top to bottom.
- Put one type in each file, and name the file after the type.
- Keep lines at 130 characters or less.

## Documentation and comments

- Document every `public` and `internal` type and member with XML comments. Skip it only when the comment would repeat the name.
- Say what the reader can't see from the signature: when the result is `null`, which input wins, what gets rejected. "Gets the test runner" adds nothing to `GetTestRunner`.
- Put `<summary>` and `</summary>` on their own lines, also for one-line summaries.
- Use a `//` comment only to explain why the code does something unexpected. To describe what a member does, use `<summary>`.
- `// REFACTOR` and `// SMELL` comments are fine to point the next person at a known problem. Don't leave a `// TODO` without an issue.

## Packages

- Every new package needs a maintainer's approval. Add it to the right group in `Directory.Packages.props`, never with `Version=` in a project file, and record it in [docs/dependencies.md](../../../../docs/dependencies.md).
