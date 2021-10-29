#define EX6

using System.Globalization;

#if EX1

static int SumThem(IEnumerable<int> xs) 
{
    int result = default(int);
    foreach (var x in xs)
    {
        result += x;
    }
    return result;
}

#endif

#if EX2

static T SumThem<T>(IEnumerable<T> xs)
{
    T result = default(T);
    foreach (var x in xs)
    {
        result += x;
    }
    return result;
}

#endif

#if EX3

static T SumThem<T>(IEnumerable<T> xs) where T: INumber<T>
{
    T result = default(T);
    foreach (var x in xs)
    {
        result += x;
    }
    return result;
}

var x0 = SumThem(new[] { 1, 2, 3, 4, 5, 6, 7 });
var x1 = SumThem(new[] { 1.0, 2, 3, 4, 5, 6, 7 });

System.Diagnostics.Debugger.Break();
#endif

#if EX4

static T SumThem<T>(IEnumerable<T> xs) where T: INumber<T>
{
    T result = T.Create(0);
    foreach (var x in xs)
    {
        result += x;
    }
    return result;
}

var x0 = SumThem(new[] { 1, 2, 3, 4, 5, 6, 7 });
var x1 = SumThem(new[] { 1.0, 2, 3, 4, 5, 6, 7 });

System.Diagnostics.Debugger.Break();

#endif

#if EX5

class Foo<T> where T:struct
{
    static T SumThem(IEnumerable<T> xs)
    {
        T result = default(T);
        foreach (var x in xs)
        {
            result += x;
        }
        return result;
    }

}

#endif

#if EX6

System.Diagnostics.Debugger.Break();

Base b = new Base();
var y = b + b;

Derived d = new Derived();
var x = d + d;

Worker<Base>.Do();

Worker<Derived>.Do();

class Base
{
    public static Base operator +(Base x, Base y) => x;
}

class Derived : Base
{
    public static Base operator +(Derived x, Derived y) => x;
}

class Worker<T> where T : Base, new()
{
    public static void Do()
    {
        var x = new T();
        var y = x + x;
    }
}



#endif