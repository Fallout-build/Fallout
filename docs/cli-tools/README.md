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

Run it when you want to check for changes. It is not part of the normal build.

Before you run it:

- The Git working copy must be clean. The target stops if it is not.
- The target deletes the whole `docs/cli-tools` folder and creates it again. This also deletes this `README.md`. After the run, restore it with `git restore docs/cli-tools/README.md`.

### How the files are made

The code is in `src/Fallout.Tooling.Generator/ReferenceUpdater.cs`. For each tool JSON spec, and for each entry in its `references` array, it does this:

1. It downloads the URL.
2. It writes the result to `<Tool>.ref.<NNN>.txt`. `<NNN>` is the position of the entry in the array, with three digits (`000`, `001`, and so on).

An entry can end with a selector after a `#`, for example `https://example.com/page#//div[@id='main']`. The selector is an XPath expression. If there is a selector, the code parses the page as HTML and keeps only the text inside the selected element. If there is no selector, the code writes the downloaded content as it is.

If a download fails, the build logs an error and continues with the next entry. The old file for that entry is not kept, because the folder was already deleted.

## What is in each file

For entries with a selector, the file is the text of one part of a web page, with the HTML tags removed. The text is not decoded, so some HTML codes are still in it, such as `&lt;` for `<`. For entries without a selector, the file is whatever the URL returned. Most files show the help output of a command, such as `dotnet`, `git` or `paket`.

Use the files to spot changes. They are not tutorials.

We may add tutorials for wrapped tools in this folder later, one file per tool, such as `dotnet.md`. See [#41](https://github.com/Fallout-build/Fallout/issues/41) (the documentation effort) for details.
