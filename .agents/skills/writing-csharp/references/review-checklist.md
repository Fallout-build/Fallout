# Review checklist

Use this before you open a PR, or when you review one. Walk the block that matches each changed file. Every changed method also gets block M, whatever file it is in.

Each item has an ID so a review can say which items it checked. The rule behind each item is in [project-rules.md](project-rules.md), [testing.md](testing.md) or [coding-guidelines.md](coding-guidelines.md).

## T. A new or changed `*Specs.cs` file

- **T1.** Is the class suffixed `Specs`, and do its file and namespace mirror the project under test?
- **T2.** Does each test name state a fact in plain English, with no method or type names and no `Can_` for behavior that always happens?
- **T3.** Is every statement under the right Arrange, Act or Assert heading?
- **T4.** Is everything the scenario doesn't depend on left out: no restated defaults, no assertions the setup already guarantees?
- **T5.** Is there no constructor and no shared field in the class?
- **T6.** Are the values that matter visible in the test body, including in `[InlineData]` rows?
- **T7.** Is the expected value known independently, not computed with the code under test?
- **T8.** Does every new rule in the production code have a test, including rules that depend on the environment?

## M. Any changed method, whatever file it is in

- **M1.** Is it under 15 statements, at one level of abstraction?
- **M2.** Can a reader follow every branch without guessing the author's intent?
- **M3.** Does an existing helper already do what this code does by hand?
- **M4.** Is `var` used only where the right side shows the type, and never for built-in types?
- **M5.** Is a method that only its own type uses `private`?
- **M6.** Does a `Get…` method never return `null`?
- **M7.** Are null checks written as `is null` and `is not null`?
- **M8.** Does it come before the methods it calls?

## D. A new or changed `public` or `internal` type or member

- **D1.** Is the type `internal` unless it is meant to be public API? If it is public and not yet stable, is it marked `[Experimental]`?
- **D2.** Does it have an XML comment that says what the signature doesn't?
- **D3.** Are `<summary>` and `</summary>` on their own lines?
- **D4.** Is there one type per file, with the file named after the type?
- **D5.** Is a new abstraction or interface used in at least three places, or does it separate parts that must not know each other?
- **D6.** Does nullability match the contract, with no `!` needed downstream?
- **D7.** Are all lines 130 characters or less?

## P. A changed `*.csproj` or `Directory.Packages.props`

- **P1.** Does it add a package? Then it needs a maintainer's approval, an entry in the right group of `Directory.Packages.props` and a line in `docs/dependencies.md`.
- **P2.** Does a `PackageReference` have a `Version=`? Move the version to `Directory.Packages.props`.
