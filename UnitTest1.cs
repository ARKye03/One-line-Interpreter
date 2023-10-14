using NUnit.Framework;
using mini_compiler;

namespace mini_compiler.test;

[TestFixture]
public class MyTests
{

    [Test]
    public void Func2()
    {
        Assert.That(RunInterpreter("function Pow(x,y) => x^y;"), Is.EqualTo(""));
        Assert.That(RunInterpreter("print(Pow(2,5));"), Is.EqualTo("32"));
    }
    [Test]
    public void Func1()
    {
        Assert.That(RunInterpreter("function Pow(x,y) => x^y;"), Is.EqualTo(""));
        //Assert.That(RunInterpreter("print(Pow(5,2));"), Is.EqualTo("25"));
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
        Assert.That(RunInterpreter("print(let x = 12 in x + let y = 42 in y);"), Is.EqualTo("54"));
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
        Assert.That(RunInterpreter("let x = 25 in print(x - 25);"), Is.EqualTo("0"));
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
    c_ui c_ui = new c_ui();

    private string RunInterpreter(string sourceCode)
    {
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            c_ui.Test(sourceCode);
            return sw.ToString().Trim();
        }
    }
}