# C# Coding Guidelines

The rule tables of the [C# Coding Guidelines](https://csharpcodingguidelines.com). Each rule ID links to its full text. Fallout-specific exceptions are in [project-rules.md](project-rules.md).

**Severity:** **Must** = always · **Should** = unless there is a clear reason not to · **May** = optional good practice.

## General Guidelines (AV0100)

| Rule | Guideline |
|------|-----------|
| [AV0100](https://csharpcodingguidelines.com/general-guidelines/#AV0100) | Understand the logical and physical boundaries of your codebase; cross-boundary communication must go through well-defined contracts. |
| [AV0105](https://csharpcodingguidelines.com/general-guidelines/#AV0105) | Use design patterns to communicate intent (*Repository*, *Factory*, *Strategy*, etc.). |
| [AV0110](https://csharpcodingguidelines.com/general-guidelines/#AV0110) | Prefer composition over inheritance; reserve inheritance for true "is-a" relationships. |
| [AV0112](https://csharpcodingguidelines.com/general-guidelines/#AV0112) | Apply the Principle of Least Surprise: name things predictably, avoid hidden side effects, follow platform conventions. |
| [AV0115](https://csharpcodingguidelines.com/general-guidelines/#AV0115) | Keep It Simple (KISS): the simplest solution that fully solves the problem is almost always the best. |
| [AV0120](https://csharpcodingguidelines.com/general-guidelines/#AV0120) | You Ain't Gonna Need It (YAGNI): build what is needed now, not what might be needed someday. |
| [AV0125](https://csharpcodingguidelines.com/general-guidelines/#AV0125) | Don't Repeat Yourself (DRY) within a bounded context; allow limited duplication across boundaries to avoid coupling. |
| [AV0130](https://csharpcodingguidelines.com/general-guidelines/#AV0130) | Apply the four pillars of OOP (encapsulation, abstraction, inheritance, polymorphism) as tools, not goals. |
| [AV0135](https://csharpcodingguidelines.com/general-guidelines/#AV0135) | Treat AI-generated code as your own: review, understand, and verify every line before committing it. |

## Class Design (AV1000)

| Rule | Guideline |
|------|-----------|
| [AV1000](https://csharpcodingguidelines.com/class-design-guidelines/#AV1000) | A class or interface should have a single, well-defined purpose (Single Responsibility Principle). |
| [AV1001](https://csharpcodingguidelines.com/class-design-guidelines/#AV1001) | Constructors should return a fully initialized, usable object without requiring extra property-setting afterward. |
| [AV1002](https://csharpcodingguidelines.com/class-design-guidelines/#AV1002) | Only pass to a constructor the dependencies that most or all methods need. |
| [AV1003](https://csharpcodingguidelines.com/class-design-guidelines/#AV1003) | Interfaces should be small, focused, and named to explain their role (Interface Segregation Principle). |
| [AV1004](https://csharpcodingguidelines.com/class-design-guidelines/#AV1004) | Expose extension points as interfaces, not base classes. |
| [AV1005](https://csharpcodingguidelines.com/class-design-guidelines/#AV1005) | Use interfaces to decouple classes, prevent bidirectional associations, and enable dependency injection. |
| [AV1008](https://csharpcodingguidelines.com/class-design-guidelines/#AV1008) | Avoid static classes except for extension method containers; they are hard to test and violate DI. |
| [AV1010](https://csharpcodingguidelines.com/class-design-guidelines/#AV1010) | Don't suppress compiler warning CS0114 using `new`; fix the design instead (breaks polymorphism). |
| [AV1011](https://csharpcodingguidelines.com/class-design-guidelines/#AV1011) | Honor the Liskov Substitution Principle: a derived type must be usable wherever its base type is expected. |
| [AV1013](https://csharpcodingguidelines.com/class-design-guidelines/#AV1013) | Don't refer to derived classes from a base class. |
| [AV1014](https://csharpcodingguidelines.com/class-design-guidelines/#AV1014) | Avoid exposing the objects a class depends on (Law of Demeter / "Don't talk to strangers"). |
| [AV1020](https://csharpcodingguidelines.com/class-design-guidelines/#AV1020) | Avoid bidirectional dependencies between classes or assemblies. |
| [AV1025](https://csharpcodingguidelines.com/class-design-guidelines/#AV1025) | Classes should have both state and behavior; avoid data-only or behavior-only splits. |
| [AV1026](https://csharpcodingguidelines.com/class-design-guidelines/#AV1026) | Protect internal state consistency: validate public inputs and guard invariants at construction and mutation. |

## Member Design (AV1100)

| Rule | Guideline |
|------|-----------|
| [AV1100](https://csharpcodingguidelines.com/member-design-guidelines/#AV1100) | Properties should be stateless with respect to each other; property access order must not matter. |
| [AV1105](https://csharpcodingguidelines.com/member-design-guidelines/#AV1105) | Use a method instead of a property when the operation is expensive, conversion-like, non-deterministic, or has observable side effects. |
| [AV1110](https://csharpcodingguidelines.com/member-design-guidelines/#AV1110) | Don't use mutually exclusive properties; they signal two conflicting concepts in one type. |
| [AV1115](https://csharpcodingguidelines.com/member-design-guidelines/#AV1115) | A property, method, or local function should do exactly one thing. |
| [AV1125](https://csharpcodingguidelines.com/member-design-guidelines/#AV1125) | Don't hide dependencies behind static members; inject them explicitly. |
| [AV1130](https://csharpcodingguidelines.com/member-design-guidelines/#AV1130) | Return `IEnumerable<T>`, `IReadOnlyCollection<T>`, or `IReadOnlyList<T>` instead of concrete mutable collections. |
| [AV1135](https://csharpcodingguidelines.com/member-design-guidelines/#AV1135) | Strings, collections, and tasks should never be `null`; return empty equivalents instead. |
| [AV1137](https://csharpcodingguidelines.com/member-design-guidelines/#AV1137) | Define parameters as specific and narrow as possible; don't require callers to pass more than you need. |
| [AV1140](https://csharpcodingguidelines.com/member-design-guidelines/#AV1140) | For Fallout, use domain-specific value types only when they clarify a domain concept or invariant; don't introduce one merely to bundle a few primitive parameters. |
| [AV1145](https://csharpcodingguidelines.com/member-design-guidelines/#AV1145) | Use C# 14 extension members to add behavior to types you don't own or to keep domain logic close to its type. |
| [AV1150](https://csharpcodingguidelines.com/member-design-guidelines/#AV1150) | Avoid local functions; prefer extracting to a named private method. Exceptions: recursion, iterators, or many captured variables. |
| [AV1155](https://csharpcodingguidelines.com/member-design-guidelines/#AV1155) | Use the C# 14 `field` keyword in auto-properties when you need validation or transformation on get/set without a full backing field. |

## Miscellaneous Design (AV1200)

| Rule | Guideline |
|------|-----------|
| [AV1202](https://csharpcodingguidelines.com/misc-design-guidelines/#AV1202) | Provide a rich, meaningful exception message that explains the cause and how to avoid it. |
| [AV1205](https://csharpcodingguidelines.com/misc-design-guidelines/#AV1205) | Use a custom exception only when framework exception types do not explain the error well enough. |
| [AV1210](https://csharpcodingguidelines.com/misc-design-guidelines/#AV1210) | Don't swallow errors by catching `Exception`; only do so at top-level handlers for logging or graceful shutdown. |
| [AV1215](https://csharpcodingguidelines.com/misc-design-guidelines/#AV1215) | Handle exceptions correctly in asynchronous code; understand how `async`/`await` propagates exceptions. |
| [AV1235](https://csharpcodingguidelines.com/misc-design-guidelines/#AV1235) | Never pass `null` as the `sender` argument when raising an event; pass `EventArgs.Empty` instead of `null` args. |
| [AV1250](https://csharpcodingguidelines.com/misc-design-guidelines/#AV1250) | Materialize LINQ results before returning them from a method (call `ToList()` or `ToArray()`). |
| [AV1251](https://csharpcodingguidelines.com/misc-design-guidelines/#AV1251) | Don't use `this.` or `base.` prefixes unless required to disambiguate. |

## Maintainability (AV1500)

| Rule | Guideline |
|------|-----------|
| [AV1500](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1500) | Keep methods under 15 statements and at a single level of abstraction. |
| [AV1501](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1501) | Default all members to `private` and types to `internal sealed`. |
| [AV1502](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1502) | Avoid conditions with double negatives. |
| [AV1505](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1505) | Name assemblies after their contained namespace (`Company.Component.dll`). |
| [AV1506](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1506) | Name each source file after the type it contains, in PascalCase with no underscores. |
| [AV1507](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1507) | Limit a source file to one type (exceptions: nested types, same-type-different-generic-arity). |
| [AV1508](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1508) | Avoid partial classes; when needed, name each file after its logical role (e.g. `MyClass.Designer.cs`). |
| [AV1510](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1510) | Use `using` directives instead of fully qualified type names; use aliases to resolve conflicts. |
| [AV1515](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1515) | Replace literal numeric and string values with named constants or enum members. |
| [AV1520](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1520) | Only use `var` when the type is evident from the right-hand side; never use `var` for built-in types. |
| [AV1521](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1521) | Declare and initialize each variable at the point of first use, not at the top of the block. |
| [AV1522](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1522) | Assign each variable in a separate statement; don't chain assignments. |
| [AV1523](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1523) | Favor object and collection initializers over separate property-assignment statements. |
| [AV1525](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1525) | Don't compare `bool` expressions to `true` or `false` explicitly. |
| [AV1530](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1530) | Don't modify a `for` loop variable inside the loop body; use `break` or `continue` instead. |
| [AV1532](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1532) | Avoid nested loops; prefer LINQ joins or extracted methods. |
| [AV1535](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1535) | Always add curly braces after `if`, `else`, `do`, `while`, `for`, `foreach`, and `case`. |
| [AV1536](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1536) | Always include a `default` block in `switch`; add a comment or throw if it should never be reached. |
| [AV1537](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1537) | Finish every `if`-`else if` chain with a final `else` block. |
| [AV1540](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1540) | Be reluctant with multiple `return` statements; one entry / one exit improves readability in most cases. |
| [AV1545](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1545) | Use direct assignment over `if`-`else`; leverage `??`, `??=`, `?.`, and ternary operators. |
| [AV1546](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1546) | Prefer interpolated strings (`$"…"`) over `string.Format` or concatenation. |
| [AV1547](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1547) | Encapsulate complex inline expressions in a clearly named method, property, or local function. |
| [AV1551](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1551) | Chain overloads so simpler ones call the most-complete overload; make the most-complete overload `virtual`. |
| [AV1553](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1553) | Only use optional parameters as a shorthand for overloads; never use `null` defaults for strings, collections, or tasks. |
| [AV1554](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1554) | Don't add optional parameters to interface methods. |
| [AV1555](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1555) | Avoid named arguments; exception: use them to clarify a `bool` parameter you don't control. |
| [AV1561](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1561) | Limit signatures to 3 parameters; don't use tuple parameters; don't return tuples with more than 2 elements. |
| [AV1562](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1562) | Avoid `ref` and `out` parameters; return a tuple or value type instead. Exception: `TryParse`-style patterns. |
| [AV1564](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1564) | Avoid `bool` parameters that switch behavior; use two methods or an enum instead. |
| [AV1570](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1570) | Use `is`-pattern matching instead of `as`-then-null-check casts. |
| [AV1575](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1575) | Never commit commented-out code; use an issue tracker for deferred work. |
| [AV1580](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1580) | Write code that is easy to debug; break long call chains into named intermediate variables. |
| [AV1582](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1582) | Use raw string literals (`"""…"""`) for multi-line or escape-heavy strings (C# 11+). |
| [AV1585](https://csharpcodingguidelines.com/maintainability-guidelines/#AV1585) | Mark properties `required` when they must be set during object initialization (C# 11+). |

## Testability (AV1600)

| Rule | Guideline |
|------|-----------|
| [AV1600](https://csharpcodingguidelines.com/testability-guidelines/#AV1600) | Write test names as short, present-tense sentences describing observable behavior, not implementation details. |
| [AV1602](https://csharpcodingguidelines.com/testability-guidelines/#AV1602) | Suffix test classes with `Specs` instead of `Tests` to encourage a behavior-driven mindset. |
| [AV1605](https://csharpcodingguidelines.com/testability-guidelines/#AV1605) | Test observable behavior through public APIs only; never test private methods or internal state directly. |
| [AV1608](https://csharpcodingguidelines.com/testability-guidelines/#AV1608) | Make the important parts of a test visible in the test body; hide irrelevant setup in helper methods or builders. |
| [AV1610](https://csharpcodingguidelines.com/testability-guidelines/#AV1610) | Use Test Data Builders for flexible construction of complex test objects; use Object Mothers for canonical fixed fixtures. |
| [AV1615](https://csharpcodingguidelines.com/testability-guidelines/#AV1615) | Prefer inline literal values in assertions when the value itself communicates the intent of the test. |
| [AV1618](https://csharpcodingguidelines.com/testability-guidelines/#AV1618) | Assert against independently known expected values; never derive an expected value from the production code under test. |
| [AV1620](https://csharpcodingguidelines.com/testability-guidelines/#AV1620) | Test reusable components (validators, serializers, domain services) in isolation, independently of their consumers. |
| [AV1622](https://csharpcodingguidelines.com/testability-guidelines/#AV1622) | Access internal implementation details through the public API of the component that uses them, not directly. |

## Naming Conventions (AV1700)

| Rule | Guideline |
|------|-----------|
| [AV1701](https://csharpcodingguidelines.com/naming-guidelines/#AV1701) | Use US English for all identifiers; favor readability and correctness over brevity. |
| [AV1702](https://csharpcodingguidelines.com/naming-guidelines/#AV1702) | Use PascalCase for types, members, namespaces, and constants; camelCase for parameters, local variables, and private fields. |
| [AV1704](https://csharpcodingguidelines.com/naming-guidelines/#AV1704) | Don't include numbers in variable, parameter, or member names. |
| [AV1705](https://csharpcodingguidelines.com/naming-guidelines/#AV1705) | Don't prefix fields with `_`, `m_`, `g_`, or `s_`. |
| [AV1706](https://csharpcodingguidelines.com/naming-guidelines/#AV1706) | Spell out names in full; avoid abbreviations except for widely accepted acronyms (e.g. `UI`, `Id`). |
| [AV1707](https://csharpcodingguidelines.com/naming-guidelines/#AV1707) | Name identifiers after their meaning, not their type. |
| [AV1708](https://csharpcodingguidelines.com/naming-guidelines/#AV1708) | Name types with nouns or noun phrases; don't include `Utility`, `Helper`, or `Manager` in a type name. |
| [AV1709](https://csharpcodingguidelines.com/naming-guidelines/#AV1709) | Prefix all generic type parameters with `T`; use a descriptive name unless a single `T` is completely unambiguous. |
| [AV1710](https://csharpcodingguidelines.com/naming-guidelines/#AV1710) | Don't repeat the enclosing class or enum name in its members. |
| [AV1711](https://csharpcodingguidelines.com/naming-guidelines/#AV1711) | Name collection/service members to match the conventions of the equivalent .NET Framework type. |
| [AV1712](https://csharpcodingguidelines.com/naming-guidelines/#AV1712) | Avoid names that are visually similar to other names (e.g. `l` vs `1`, `O` vs `0`). |
| [AV1715](https://csharpcodingguidelines.com/naming-guidelines/#AV1715) | Name boolean properties with affirmative phrases; consider prefixes `Is`, `Has`, `Can`, `Allows`, or `Supports`. |
| [AV1720](https://csharpcodingguidelines.com/naming-guidelines/#AV1720) | Name methods and local functions using a verb or verb-object pair; don't use `And` in a name. |
| [AV1725](https://csharpcodingguidelines.com/naming-guidelines/#AV1725) | Name namespaces using nouns, layers, or features; never embed a type name in a namespace name. |
| [AV1735](https://csharpcodingguidelines.com/naming-guidelines/#AV1735) | Name events with a verb or verb phrase (`Click`, `Deleted`, `Closing`). |
| [AV1737](https://csharpcodingguidelines.com/naming-guidelines/#AV1737) | Use `-ing`/`-ed` suffixes for pre/post events instead of `Before`/`After` prefixes. |
| [AV1738](https://csharpcodingguidelines.com/naming-guidelines/#AV1738) | Prefix event handler methods with `On` (e.g. `OnClosing`, `OkButtonOnClick`). |
| [AV1739](https://csharpcodingguidelines.com/naming-guidelines/#AV1739) | Use `_` (single discard in C# 9+) for irrelevant lambda parameters. |
| [AV1745](https://csharpcodingguidelines.com/naming-guidelines/#AV1745) | Group extension methods in a class suffixed with `Extensions`. |
| [AV1755](https://csharpcodingguidelines.com/naming-guidelines/#AV1755) | Only suffix methods with Async or TaskAsync when both synchronous and asynchronous variants exist  |

## Performance (AV1800)

| Rule | Guideline |
|------|-----------|
| [AV1800](https://csharpcodingguidelines.com/performance-guidelines/#AV1800) | Use `Any()` instead of `Count()` to check whether an `IEnumerable<T>` is empty. |
| [AV1820](https://csharpcodingguidelines.com/performance-guidelines/#AV1820) | Use `async`/`await` only for I/O-bound work; it does not run code on a thread-pool thread by itself. |
| [AV1825](https://csharpcodingguidelines.com/performance-guidelines/#AV1825) | Use `Task.Run` for CPU-bound work; use `TaskCreationOptions.LongRunning` for long-running background operations. |
| [AV1830](https://csharpcodingguidelines.com/performance-guidelines/#AV1830) | Don't block on async code with `.Wait()` or `.Result`; it can deadlock. |
| [AV1835](https://csharpcodingguidelines.com/performance-guidelines/#AV1835) | Blocking on a `Task` in UI frameworks (WPF, WinForms) causes deadlocks because of the single-threaded sync context. |
| [AV1840](https://csharpcodingguidelines.com/performance-guidelines/#AV1840) | Await `ValueTask`/`ValueTask<T>` directly and exactly once, or call `.AsTask()` to get a reusable `Task`. |

## Framework Usage (AV2200)

| Rule | Guideline |
|------|-----------|
| [AV2202](https://csharpcodingguidelines.com/framework-guidelines/#AV2202) | Prefer idiomatic C# syntax over verbose .NET API calls: tuple literals, `is null`, `is not null`, `??=`, `[]` collection expressions, and C# 14 null-coalescing assignment. |
| [AV2210](https://csharpcodingguidelines.com/framework-guidelines/#AV2210) | Build with the highest warning level and treat warnings as errors. |
| [AV2220](https://csharpcodingguidelines.com/framework-guidelines/#AV2220) | Prefer LINQ extension method syntax over query syntax for simple expressions. |
| [AV2225](https://csharpcodingguidelines.com/framework-guidelines/#AV2225) | Use tuple/record deconstruction to avoid intermediate variables and simplify assignments. |
| [AV2230](https://csharpcodingguidelines.com/framework-guidelines/#AV2230) | Only use the `dynamic` keyword for interop with dynamic languages or APIs; avoid it in hot code paths. |

## Documentation & Comments (AV2300)

| Rule | Guideline |
|------|-----------|
| [AV2301](https://csharpcodingguidelines.com/documentation-guidelines/#AV2301) | Write all comments and XML documentation in US English. |
| [AV2305](https://csharpcodingguidelines.com/documentation-guidelines/#AV2305) | Document every `public`, `protected`, and `internal` type and member with XML doc comments. |
| [AV2306](https://csharpcodingguidelines.com/documentation-guidelines/#AV2306) | Write documentation for other developers who don't have access to the source code. |
| [AV2308](https://csharpcodingguidelines.com/documentation-guidelines/#AV2308) | Document the purpose of a member (the *why* and *what*), not its implementation (the *how*). |
| [AV2310](https://csharpcodingguidelines.com/documentation-guidelines/#AV2310) | Avoid inline comments; replace complex blocks with well-named methods instead. |
| [AV2316](https://csharpcodingguidelines.com/documentation-guidelines/#AV2316) | Only write comments to explain non-obvious algorithms or significant design decisions. |
| [AV2318](https://csharpcodingguidelines.com/documentation-guidelines/#AV2318) | Don't use `// TODO` comments as a task tracker; use a proper issue tracking system. |

## Layout (AV2400)

| Rule | Guideline |
|------|-----------|
| [AV2400](https://csharpcodingguidelines.com/layout-guidelines/#AV2400) | Max 130 chars/line; 4-space indent (no tabs); space between keyword and `(`; spaces around operators; opening and closing braces on their own lines; empty lines between unrelated blocks. |
| [AV2402](https://csharpcodingguidelines.com/layout-guidelines/#AV2402) | Order `using` directives: System namespaces first, then third-party/own in alphabetical order; place `using static` and alias directives last. |
| [AV2406](https://csharpcodingguidelines.com/layout-guidelines/#AV2406) | Order members: private fields/constants → public constants → public static readonly fields → factory methods → constructors → events → public properties → other methods; local functions at the bottom of their method. |
| [AV2407](https://csharpcodingguidelines.com/layout-guidelines/#AV2407) | Don't use `#region`; they hide code and add friction without improving readability. |
| [AV2410](https://csharpcodingguidelines.com/layout-guidelines/#AV2410) | Use expression-bodied member syntax only when the body is a single statement that fits on one line. |
