using NUnit.Framework;
using mini_compiler;

namespace mini_compiler.test;

[TestFixture]
public class FunctionTests
{
    [Test]
    public void TestSin()
    {
        Assert.That(RunInterpreter("print(Sin(0));"), Is.EqualTo("0"));
    }

    [Test]
    public void TestCos()
    {
        Assert.That(RunInterpreter("print(Cos(0));"), Is.EqualTo("1"));
    }

    [Test]
    public void TestTan()
    {
        Assert.That(RunInterpreter("print(Tan(0));"), Is.EqualTo("0"));
    }

    [Test]
    public void TestLog()
    {
        Assert.That(RunInterpreter("print(Log(1,5));"), Is.EqualTo("0"));
    }

    [Test]
    public void TestLn()
    {
        Assert.That(RunInterpreter("print(Ln(1));"), Is.EqualTo("0"));
    }

    [Test]
    public void TestSqrt()
    {
        Assert.That(RunInterpreter("print(Sqrt(4));"), Is.EqualTo("2"));
    }

    [Test]
    public void TestAbs()
    {
        Assert.That(RunInterpreter("print(Abs(-5));"), Is.EqualTo("5"));
    }

    [Test]
    public void TestPow()
    {
        Assert.That(RunInterpreter("print(Pow(2, 3));"), Is.EqualTo("8"));
    }

    [Test]
    public void TestExp()
    {
        Assert.That(RunInterpreter("print(Exp(1));"), Is.EqualTo("2.7182817"));
    }

    [Test]
    public void TestFloor()
    {
        Assert.That(RunInterpreter("print(Floor(1.5));"), Is.EqualTo("1"));
    }

    [Test]
    public void TestCeil()
    {
        Assert.That(RunInterpreter("print(Ceil(1.5));"), Is.EqualTo("2"));
    }

    [Test]
    public void TestRound()
    {
        Assert.That(RunInterpreter("print(Round(1.5));"), Is.EqualTo("2"));
    }

    [Test]
    public void TestRand()
    {
        int min = 10;
        int max = 20;
        int result = int.Parse(RunInterpreter($"print(Rand({min}, {max}));"));
        Assert.That(result, Is.GreaterThanOrEqualTo(min).And.LessThan(max));
    }
    private string RunInterpreter(string sourceCode)
    {
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            Console_UI.Test(sourceCode);
            return sw.ToString().Trim();
        }
    }
}

[TestFixture]
public class MainTests
{
    [Test]
    public void Pow_Sum_Test()
    {
        Assert.That(RunInterpreter("let x = Log10(10) in print(x + Pow(2,5));"), Is.EqualTo("33"));
    }
    [Test]
    public void EqStr()
    {
        Assert.That(RunInterpreter("function EqStr(x,y) => if(x == y) \"true\" else \"false\";"), Is.EqualTo(""));
        Assert.That(RunInterpreter("print(EqStr(\"perro\",\"perro\"));"), Is.EqualTo("true"));
    }
    [Test]
    public void Pow()
    {
        Assert.That(RunInterpreter("function pow(x,y) => Pow(x,y);"), Is.EqualTo(""));
        Assert.That(RunInterpreter("print(pow(2,3));"), Is.EqualTo("8"));
    }
    [Test]
    public void Fib()
    {
        Assert.That(RunInterpreter("function fib(n) => if (n > 1) fib(n-1) + fib(n-2) else 1;"), Is.EqualTo(""));
        Assert.That(RunInterpreter("print(fib(5));"), Is.EqualTo("8"));
    }
    [Test]
    public void MCD_Condition_Function_True()
    {
        Assert.That(RunInterpreter("function mcd(x,y) => if (x % y != 0) \"No\" else \"Yes\";"), Is.EqualTo(""));
        Assert.That(RunInterpreter("print(mcd(54,5));"), Is.EqualTo("No"));
    }
    [Test]
    public void MCD_Condition_Function()
    {
        Assert.That(RunInterpreter("function mcd(x,y) => if (x % y != 0) \"No\" else \"Yes\";"), Is.EqualTo(""));
        Assert.That(RunInterpreter("print(mcd(55,5));"), Is.EqualTo("Yes"));
    }
    [Test]
    public void LetAssignConditionTrue()
    {
        Assert.That(RunInterpreter("let x = if (4 < 5) 6 else 2 in print(x);"), Is.EqualTo("6"));
    }

    [Test]
    public void LetDt()
    {
        Assert.That(RunInterpreter("let x = 5 in 2;"), Is.EqualTo(""));
    }
    [Test]
    public void LetAssignConditionFalse()
    {
        Assert.That(RunInterpreter("let x = if (6 < 5) 6 else 2 in print(x);"), Is.EqualTo("2"));
    }

    [Test]
    public void Func2()
    {
        Assert.That(RunInterpreter("function Pow(x,y) => x^y;"), Is.EqualTo(""));
        Assert.That(RunInterpreter("print(Pow(2,5));"), Is.EqualTo("32"));
    }
    [Test]
    public void Func1()
    {
        Assert.That(RunInterpreter("function Sum(x,y) => x+y;"), Is.EqualTo(""));
        Assert.That(RunInterpreter("print((Sum(5,2) + Sum(2,2))^2);"), Is.EqualTo("121"));
    }

    [Test]
    public void LetCondIn()
    {
        Assert.That(RunInterpreter("let x = 5 in if ((let y = 2 in y*y) < 5) print(\"Voila\") else print(\"f\");"), Is.EqualTo("Voila"));
    }

    [Test]
    public void CondIn()
    {
        Assert.That(RunInterpreter("if (42 % 2 == 60 - 60) print(\"Yes\") else print(\"F\");"), Is.EqualTo("Yes"));
    }

    [Test]
    public void Cond1Test()
    {
        Assert.That(RunInterpreter("if (3 < 4) print(\"Hello\") else print(\"perro\");"), Is.EqualTo("Hello"));
    }
    //if (3 < 4) print("Hello") else print("perro");

    [Test]
    public void Cond2Test()
    {
        Assert.That(RunInterpreter("if (5 < 4) print(\"Hello\") else print(\"perro\");"), Is.EqualTo("perro"));
    }
    //if (5 < 4) print("Hello") else print("perro");
    [Test]
    public void NegativeTest()
    {
        Assert.That(RunInterpreter("let x = -5 in print(x - 3);"), Is.EqualTo("-8"));
    }

    [Test]
    public void PrintTest()
    {
        Assert.That(RunInterpreter("print((let x = 12 in x) + (let y = 42 in y));"), Is.EqualTo("54"));
    }
    [Test]
    public void LetScopeFunction()
    {
        Assert.That(RunInterpreter("function Test(x,y) => let a=x in (let b=y in a+b) + b);"), Is.EqualTo(""));
        Assert.That(RunInterpreter("print(Test(5,10));"), Is.EqualTo("Undefined variable 'b' at line 1 and column 54"));
    }
    [Test]
    public void LetScope()
    {
        Assert.That(RunInterpreter("print(let a=2 in (let b=5 in a+b) + b);"), Is.EqualTo("Undefined variable 'b' at line 1 and column 38"));
    }
    [Test]
    public void Test1()
    {
        Assert.That(RunInterpreter("let x = 5 in print(x);"), Is.EqualTo("5"));
    }

    [Test]
    public void Test2()
    {
        Assert.That(RunInterpreter("let x = 5, y = 6 in print(x + y);"), Is.EqualTo("11"));
    }

    [Test]
    public void Test3()
    {
        Assert.That(RunInterpreter("let x = \"Hello\", y = \"World\" in print(x @ y);"), Is.EqualTo("HelloWorld"));
    }

    [Test]
    public void Test4()
    {
        Assert.That(RunInterpreter("let x = 25 in print(x + 25);"), Is.EqualTo("50"));
    }

    [Test]
    public void Test5()
    {
        Assert.That(RunInterpreter("let x = 25 in print(x - (5)^2);"), Is.EqualTo("0"));
    }

    [Test]
    public void Test6()
    {
        Assert.That(RunInterpreter("let x = 25 in print(x * 25);"), Is.EqualTo("625"));
    }

    [Test]
    public void Test7()
    {
        Assert.That(RunInterpreter("let x = 25 in print(x / 25);"), Is.EqualTo("1"));
    }

    [Test]
    public void Test8()
    {
        Assert.That(RunInterpreter("let x = 25 in print(x ^ 3);"), Is.EqualTo("15625"));
    }

    [Test]
    public void Test9()
    {
        Assert.That(RunInterpreter("let crazymath = ((2 - 18 * 2) + (2 / 10)) in print(crazymath ^ 2);"), Is.EqualTo("1142.4399484252936"));
    }

    [Test]
    public void Test10()
    {
        Assert.That(RunInterpreter("let number = 42, text = \"The meaning of life is \" in let x = number * 2 in print(text @ x);"), Is.EqualTo("The meaning of life is 84"));
    }
    [Test]
    public void Test11()
    {
        Assert.That(RunInterpreter("print(2 + 5);"), Is.EqualTo("7"));
    }
    private string RunInterpreter(string sourceCode)
    {
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            Console_UI.Test(sourceCode);
            return sw.ToString().Trim();
        }
    }
}
[TestFixture]
public class VirginTest
{
    [Test]
    public void EmojiTest2()
    {
        Assert.That(RunInterpreter("function d(x) => \"🙂\";"), Is.EqualTo(""));
        Assert.That(RunInterpreter("print(\"Clase emoji: \" + d(1));"), Is.EqualTo("Clase emoji: 🙂"));
    }
    [Test]
    public void EmojiTest()
    {
        Assert.That(RunInterpreter("print(\"🙂\");"), Is.EqualTo("🙂"));
    }
    private string RunInterpreter(string sourceCode)
    {
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            Console_UI.Test(sourceCode);
            return sw.ToString().Trim();
        }
    }
}