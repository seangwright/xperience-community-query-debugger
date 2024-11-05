# Xperience Community Query Debugger

## Description

This project is a .NET console application and sample Dancing Goat Xperience by Kentico project demoing how you can use the [Xperience by Kentico MiniProfiler](https://github.com/kentico/xperience-by-kentico-miniprofiler) integration to quickly explore and debug database queries in an Xperience project.

## Screenshots

![Running the default query in VS Code](https://raw.githubusercontent.com/seangwright/xperience-community-query-debugger/refs/heads/main/images/default-query-vscode-screenshot.png)

## Requirements

### Library Version Matrix

| Xperience Version | Library Version |
| ----------------- | --------------- |
| >= 29.6.2         | 1.0.0           |

### Dependencies

- [ASP.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)
- [Xperience by Kentico](https://docs.kentico.com)

## Package Installation

This project does not produce a NuGet package and is meant to be cloned locally or copied into a .NET solution.

## Quick Start

1. Clone this repository
1. Change directory to `examples/DancingGoat`
1. At the command line, execute the following commands

   1. `dotnet tool restore`
   1. `dotnet kentico-xperience-dbmanager`

      Example:

      dotnet kentico-xperience-dbmanager -s localhost -d DancingGoat-query-debugger -u sa -p Pass@12345 -a Pass@12345 --license-file ../xk-license.txt --recreate-existing-database

   1. **Note**: if you change the `DancingGoat` project's database name from `DancingGoat-query-debugger`,
      ensure you update the `src/XperienceCommunity.QueryDebugger/appsettings.json` `ConnectionString` value to match

1. Run the `XperienceCommunity.QueryDebugger` console application

   1. Change directory to `src/XperienceCommunity.QueryDebugger`
   1. At the command line run `dotnet run`

   Included is a VS Code task `dotnet: run Query Debugger` to run this project

1. View the terminal output of the query debugging
1. Modify the queries to explore how Xperience's query APIs generate different SQL queries

If you want to use the console logging extensions in your own application you can do the following steps.

1. Reference [Xperience by Kentico MiniProfiler](https://github.com/kentico/xperience-by-kentico-miniprofiler) in your Xperience application
1. Copy `XperienceCommunity.QueryDebugger/MiniProfilerConsoleExtensions.cs` into your project
1. Perform the same kinds of query debugging calls on a `MiniProfiler` instance seen in `XperienceCommunity.QueryDebugger/Program.cs`

## Contributing

Instructions for contributions to this project can be found in [the `Contributing-Setup.md`](/docs/Contributing-Setup.md). All contributions and engagements on this project must follow the [`CODE_OF_CONDUCT`](/CODE_OF_CONDUCT.md).

## License

Distributed under the MIT License. See [`LICENSE.md`](./LICENSE.md) for more information.

## Support

This repository is provided as-is.
