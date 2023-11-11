# Mini Kompiler

<p align="center">
  <img src="hulk_logo.png" alt="mini_kompiler_logo" width="500">
</p>

## Table of Contents

- [Requirements](#requirements)
- [How to use](#how-to-use)
- [Contributing](#contributing)
- [What it can do for the moment](#what-it-can-do-for-the-moment)

## Requirements

- [Git](https://git-scm.com/)
- [DotNet7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## How to use

### Script Install

Using wget:

```shell
sh -c "$(wget https://raw.githubusercontent.com/ARKye03/scripts/main/%5Bsh%5D%20mini_kompiler/install.sh -O -)"
```

Using curl:

```shell
sh -c "$(curl -fsSL https://raw.githubusercontent.com/ARKye03/scripts/main/%5Bsh%5D%20mini_kompiler/install.sh)"
```

### Clone install

```shell
git clone https://github.com/ARKye03/mini_kompiler
```

Open a terminal and `cd /mini_kompiler`, then run:

```shell
dotnet run
```

#### Note

  -The script install will check simple dependencies and try to install them if missing

## What it can do for the moment

  - Check [Report](https://github.com/ARKye03/mini_kompiler/blob/remote/main.pdf) for more information
  - It can handle basic expressions like:
    - let-in, if/else, print and functions.
    - let-in expressions can be used to declare temporal and limited variables.
    - if/else to handle simple conditions.
    - print, to print results in case needed.
      - Note: It will not print anything unless specified with a call to the print function.
    - Simple inline function declaration.
    - It can handle basic math operations.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.