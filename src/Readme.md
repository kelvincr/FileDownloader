# Agoda Downloader Coding problem

## Getting Started

### Requirements

This project is developed using .Net Core so in order to compile it, you need the following requirement.
Either Visual Studio 2017 or Visual Studio Code.

1. [DotNet Core SDK ][dotnet-core]
2. NodeJS and npm
```bash
>node -v
v6.11.3
>npm -v
3.10.10
```

### Build

```bash
>dotnet build AgodaDownloader.sln
```
npm and webpack  target should run automatically,
however, if that does not happened you can also run from the directory.

```bash
>cd src/FileAudit
npm install
node node_modules\webpack\bin\webpack.js --config .\webpack.config.js
node node_modules\webpack\bin\webpack.js --config .\webpack.config.vendor.js
```

### Run Test

```bash
>dotnet test AgodaDownloader.sln
```

### Run Console and AuditFile

Those apps could be run from visual studio or using dotnet cli.
```bash
> dotnet Console.dll -i https://i.stack.imgur.com/bpVlu.jpg
> cd src/FileAudit
> dotnet run 
```

You can find more detailed information of the each component on the [docs folder ][docs]. 



[docs]: docs/overview.md
[dotnet-core]: https://www.microsoft.com/net/download/windows
