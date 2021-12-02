using System.Reflection.Emit;
using static System.Diagnostics.Debugger;

static int SumThem(IEnumerable<int> xs) 
{
    int result = 0;
    foreach (var x in xs)
    {
        result += x;
    }
    return result;
}

var gauss = SumThem(Enumerable.Range(1, 100));

Break();

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
b.Foo();

Derived d = new ();
d.Foo();

Worker<Base>.Do();

Worker<Derived>.Do();  

class Base
{
    public void Foo() => Break(); 
}

class Derived : Base
{
    public void Foo() => Break();
}

class Worker<T> where T : Base, new()
{
    public static void Do()
    {
        T x = new T();
        x.Foo();
    }
}

/*

The effective base class of a type parameter T is defined as follows:
•	If T has no primary constraints or type parameter constraints, its effective base class is object.
•	If T has the value type constraint, its effective base class is System.ValueType.
•	If T has a class-type constraint C but no type-parameter constraints, its effective base class is C.
•	If T has no class-type constraint but has one or more type-parameter constraints, its effective base class is the most encompassed type (§6.4.2) in the set of effective base classes of its type-parameter constraints. The consistency rules ensure that such a most encompassed type exists.
•	If T has both a class-type constraint and one or more type-parameter constraints, its effective base class is the most encompassed type (§6.4.2) in the set consisting of the class-type constraint of T and the effective base classes of its type-parameter constraints. The consistency rules ensure that such a most encompassed type exists.
•	If T has the reference type constraint but no class-type constraints, its effective base class is object.



7.2.5 Candidate user-defined operators
Given a type T and an operation operator op(A), where op is an overloadable operator and A is an argument list, the set of candidate user-defined operators provided by T for operator op(A) is determined as follows:
•	Determine the type T0. If T is a nullable type, T0 is its underlying type, otherwise T0 is equal to T.
•	For all operator op declarations in T0 and all lifted forms of such operators, if at least one operator is applicable (§7.4.3.1) with respect to the argument list A, then the set of candidate operators consists of all such applicable operators in T0.
•	Otherwise, if T0 is object, the set of candidate operators is empty.
•	Otherwise, the set of candidate operators provided by T0 is the set of candidate operators provided by the direct base class of T0, or the effective base class of T0 if T0 is a type parameter.


7.3 Member lookup
A member lookup is the process whereby the meaning of a name in the context of a type is determined. A member lookup can occur as part of evaluating a simple-name (§7.5.2) or a member-access (§7.5.4) in an expression. If the simple-name or member-access occurs as the simple-expression of an invocation-expression (§7.5.5.1), the member is said to be invoked.
If a member is a method or event, or if it is a constant, field or property of a delegate type (§15), then the member is said to be invocable.
Member lookup considers not only the name of a member but also the number of type parameters the member has and whether the member is accessible. For the purposes of member lookup, generic methods and nested generic types have the number of type parameters indicated in their respective declarations and all other members have zero type parameters.
A member lookup of a name N with K type parameters in a type T is processed as follows:
•	First, a set of accessible members named N is determined:
o	If T is a type parameter, then the set is the union of the sets of accessible members named N in each of the types specified as a primary constraint or secondary constraint (§10.1.5) for T, along with the set of accessible members named N in object.
o	Otherwise, the set consists of all accessible (§3.5) members named N in T, including inherited members and the accessible members named N in object. If T is a constructed type, the set of members is obtained by substituting type arguments as described in §10.3.2. Members that include an override modifier are excluded from the set.

/*

We don't want a full compiler there at runtime, though we can take a variant of th eoverload engine that uses Reflection.

/*

class Worker<T> where T : Base, new()
{
    public static void Do()
    {
        dynamic x = new T();
        x.Foo();
    }
}

/*


var assembly = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName("Peppa Pig World"), AssemblyBuilderAccess.RunAndCollect);
var module = assembly.DefineDynamicModule("Peppa Pig");
var typedef = module.DefineType("ForgiveMe", System.Reflection.TypeAttributes.Class | System.Reflection.TypeAttributes.Public);
var type = typedef.CreateType();

var instance = Activator.CreateInstance(type);

var listOf = typeof(List<>).MakeGenericType(type);
var listOfPig = Activator.CreateInstance(listOf);

var add = listOf.GetMethod("Add");
add.Invoke(listOfPig, new object[] { instance });

Break();

/*

Break();

Worker<Base>.Do();

Worker<NotDerived>.Do();

interface IFoo<T>
{
    static abstract void Foo();
}

class Base : IFoo<Base>
{
    public static void Foo() => Break(); 
}

class NotDerived : IFoo<NotDerived>
{
    public static void Foo() => Break();
}

class Worker<T> where T : IFoo<T>
{
    public static void Do()
    {
        T.Foo();
    }
}

/*

static - at the Type Level (or in other languages, an instance method on the metaclass)
abstract - dynamic dispatch at runtime

Lots of patterns will emerge - ICreatable<T>

/*

Lowering is where we express more advanced language concepts in terms of simpler concepts

Why would you ever want to do that?

/*

var x = new[] { 1, 2, 3, 4, 5 };
var y = [IAmHere("y")] () => { Break(); Console.WriteLine(x); };

//var z1 = new[] { 6, 7, 8, 9, 10 };
//var z = [IAmHere("z")] () => { Break(); Console.WriteLine(z1); };

y();


[System.AttributeUsage(System.AttributeTargets.Method)
]
public class IAmHereAttribute : System.Attribute
{
    public IAmHereAttribute(string msg) { }
}

// ~/.nuget/packages/runtime.osx-x64.microsoft.netcore.ildasm/6.0.0/runtimes/osx-x64/native/ildasm StaticAbstract/StaticAbstract/bin/Debug/net6.0/StaticAbstract.dll

/*

So you don't have to change the runtime.

But fortunately those days are gone.

Much nicer to have real support for features and hence metadata rather than attributes and a mass of code.

/*

interface IFoo<T>
{
    static abstract void Foo();
    void Foo2();
}

// ~/.nuget/packages/runtime.osx-x64.microsoft.netcore.ildasm/6.0.0/runtimes/osx-x64/native/ildasm StaticAbstract/StaticAbstract/bin/Debug/net6.0/StaticAbstract.dll


*/
