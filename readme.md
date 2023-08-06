# Mini Kompiler - HULK

<p align="center">
  <img src="hulk_logo.png" alt="mini_kompiler_logo" width="500">
</p>

## Table of Contents

- [Requirements](#requirements)
- [How to use](#how-to-use)
- [Contributing](#contributing)
- [What it can do for the moment](#does)

## Requirements

- [DotNet7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## How to use

```shell
git clone https://github.com/ARKye03/mini_kompiler
```

Open a terminal and `cd /mini_kompiler`, then run:

```shell
dotnet run
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## does

- It prints the tokens of the input, e.g print(25), shows 25, print("Hello World"), shows Hello World.
- Declare vars like, let x = 25 in print(x) shows 25. let x = "Hello World" in print(x) shows Hello World. let Ez = 1 in print("Cute kitty") shows Cute kitty.
- Multi vars allowed too. -> let x = 25, y = 25 in print(x + y) shows 50.
- It can do operations like, let x = 25 in print(x + 25) shows 50. let x = 25 in print(x - 25) shows 0. let x = 25 in print(x * 25) shows 625. let x = 25 in print(x / 25) shows 1, let x = 25 in print(x ^ 25) shows 8.881784197001253E+34 xd.
- Multi math_ops allowed too. -> let crazymath = ((2 - 18 * 2) + (2 / 10)) in print(crazymath ^ 2);
- Also, you can declare functions like `function read_line(x)`, and it will recognize the function, function name and parameters, but it won't do a shit.
- Conditions are also processed, but they just, don't work. Kill me please!
