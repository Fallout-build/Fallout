---
name: writing-csharp
description: Coding conventions for C# in this repo — design, naming, layout, XML documentation and tests — based on the C# Coding Guidelines (csharpcodingguidelines.com) with Fallout's exceptions. Trigger whenever you write, change or review C# code under src/, tests/ or build/, including test specs.
---

These rules apply to code you add or change. Where existing code does
something else, follow these rules anyway. Don't reformat code you don't
otherwise touch. There is no `.editorconfig` to enforce them, so review does.

## Rules that are missed most often

Check these before you finish. Each one came up in a real review.

- A `Get…` method never returns `null`. If it can, name it `Find…` or `TryGet…`.
- Check for null with `is null` and `is not null`.
- Use `var` only when the right side shows the type. Never use it for `bool`, `int`, `string` and other built-in types. Test code is exempt.
- Keep a method under 15 statements. Put a method before the methods it calls.
- Put one type in each file. Keep lines at 130 characters or less.
- Document every `public` and `internal` member. Say what the signature doesn't: when the result is `null`, which input wins, what gets rejected.
- Put `<summary>` and `</summary>` on their own lines.
- Use `//` only for why. Use `<summary>` for what a member does.
- No constructors or shared fields in a `Specs` class.
- Give every new rule a test. When it depends on the environment, move the decision into a method that takes those values as parameters.

## References

Read the file that matches your task:

| File | Read it when |
| --- | --- |
| [references/project-rules.md](references/project-rules.md) | Writing any C#. It has Fallout's exceptions, such as static `*Tasks` classes and `bool?` on tool options. |
| [references/testing.md](references/testing.md) | Writing or changing a `*Specs.cs` file. |
| [references/coding-guidelines.md](references/coding-guidelines.md) | You need the full rule behind an `AV` rule ID. |
| [references/review-checklist.md](references/review-checklist.md) | Before opening a PR, or when reviewing one. |
