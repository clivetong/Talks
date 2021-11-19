using static System.Diagnostics.Debugger;

/*

static int SumThem(IEnumerable<int> xs) 
{
    int result = 0;
    foreach (var x in xs)
    {
        result += x;
    }
    return result;
}

/*

static T SumThem<T>(IEnumerable<T> xs)
{
    T result = default(T);
    foreach (var x in xs)
    {
        result += x;
    }
    return result;
}

/*

static T SumThem<T>(IEnumerable<T> xs) where T: INumber<T>
{
    T result = default;
    foreach (var x in xs)
    {
        result += x;
    }
    return result;
}

var x0 = SumThem(new[] { 1, 2, 3, 4, 5, 6, 7 });
var x1 = SumThem(new[] { 1.0, 2, 3, 4, 5, 6, 7 });

Break();

/*

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

Break();

/*

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

/*

Break();

Base b = new ();
var y = b + b;

Derived d = new ();
var x = d + d;

Worker<Base>.Do();

Worker<Derived>.Do();   // We only know about the constrained T

class Base
{
    public static Base operator +(Base x, Base y) { Break(); return x; }
}

class Derived : Base
{
    public static Derived operator +(Derived x, Derived y) { Break(); return x; }
}

class Worker<T> where T : Base, new()
{
    public static void Do()
    {
        T x = new T();
        T y = x + x;
    }
}

/*

Break();

Base b = new ();
var y = b + b;

Derived d = new ();
var x = d + d;

Worker<Base>.Do();

Worker<Derived>.Do();

class Base : IAdditionOperators<Base, Base, Base>
{
    public static Base operator +(Base x, Base y) { Break(); return x;}
}

class Derived : IAdditionOperators<Derived, Derived, Derived>
{
    public static Derived operator +(Derived x, Derived y) { Break(); return x; }
}

class Worker<T> where T : IAdditionOperators<T,T,T>, new()
{
    public static void Do()
    {
        T x = new ();
        T y = x + x;
    }
}

*/
