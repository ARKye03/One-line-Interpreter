# Mini Kompiler - HULK

<p align="center">
  <img src="hulk_logo.png" alt="mini_kompiler_logo" width="500">
</p>

## Table of Contents

- [Requirements](#requirements)
- [How to use](#how-to-use)
- [Contributing](#contributing)
- [What it can do for the moment](#what-it-can-do-for-the-moment)

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

## What it can do for the moment

- It prints the tokens of the input, e.g print(25), shows 25, print("Hello World"), shows Hello World.
- Declare vars like, let x = 25 in print(x) shows 25. let x = "Hello World" in print(x) shows Hello World. let Ez = 1 in print("Cute kitty") shows Cute kitty.
- Multi vars allowed too. -> let x = 25, y = 25 in print(x + y) shows 50.
  - This is equivalent, so it's allowed too let number = 42, text = "The meaning of life is " in let x = number * 2 in print(text @ x);
- It can do operations like, let x = 25 in print(x + 25) shows 50. let x = 25 in print(x - 25) shows 0. let x = 25 in print(x * 25) shows 625. let x = 25 in print(x / 25) shows 1, let x = 25 in print(x ^ 25) shows 8.881784197001253E+34 xd.
  - Multi math_ops allowed too. -> let crazymath = ((2 - 18 * 2) + (2 / 10)) in print(crazymath ^ 2);
- Concat vars like, let x = "Hello", y = 2023 in print(x @ y) shows Hello2023.
  - If you try things like let x = 5, y = 6 in print(x @ y); it will sum the vars and then concat them, so it will show 11.
  - WTF is this -> let number = 42, z = 4 in let x = number * 2 in let y = 3 in print((z + x) ^ y); Also works. Strings works surpresingly well.
  - This is even worst let x = 5, y = x + 12 in let z = x + y in print((x + y + z) ^(let uper = 5 in uper - 3)); And it works too. Don't ask me why.
- Also, you can declare functions like `function read_line(x)`, and it will recognize the function, function name and parameters, but it won't do a shit.
- Conditions are also processed, but they just, don't work.
- #conditions_are_a_fvcking_nightmare!
