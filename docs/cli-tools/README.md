# CLI tool references

This folder holds copies of the official documentation pages for each command-line tool that Fallout wraps (for example `dotnet`, `npm` and `git`). Each copy is a plain-text file named `<Tool>.ref.<NNN>.txt`.

Each file matches a URL in the `references` array of the tool's JSON spec, at `src/Fallout.Common/Tools/<Tool>/<Tool>.json`. The JSON spec is the source we generate the tool wrapper from.

## Why they are here

We keep the copies so we can see when a tool's command line changes. If a tool adds, renames or removes a flag, the copy changes, and the Git diff shows it. You can then check whether the JSON spec needs the same change.

The files used to live in `build/references/`. We moved them here because they are documentation, not part of the build.

## Update the files

```pwsh
./build.ps1 References
```

This downloads each page again, converts it to plain text, and overwrites the matching `.ref.NNN.txt` file. Run it when you want to check for changes. It is not part of the normal build.

## What is in each file

Each file is the text of a web page with the HTML tags removed. Some HTML codes are still in the text, such as `&lt;` for `<`. Most pages show the help output of a command, such as `dotnet`, `git` or `paket`.

Use the files to spot changes. They are not tutorials.

We may add tutorials for wrapped tools in this folder later, one file per tool, such as `dotnet.md`. See [#41](https://github.com/Fallout-build/Fallout/issues/41) (the documentation effort) for details.
