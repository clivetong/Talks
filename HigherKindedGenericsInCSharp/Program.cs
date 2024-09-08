// ReSharper disable InconsistentNaming

// ---------------------------------------------------------------
// Type is flat in .NET - so you have to do some things at runtime
// ---------------------------------------------------------------

var collectionType = typeof(System.Collections.Generic.List<>);
var theType = collectionType.MakeGenericType(typeof(int));
dynamic res = Activator.CreateInstance(theType)!;
res.Add(100);


// ---------------------
// Use the new List type
// ---------------------

var list = new List<int>([1, 2, 3]);

var _ = list.Select(x => x + 1)
    .Select(x => x * 2)
    .As();

// ------------------
// Use the Maybe type
// ------------------

var mx = new Just<int>(100);
var my = new Nothing<int>();

var r1 = mx.Select(x => x + 1)
    .Select(x => x * 3); // Just(303)

var r2 = my.Select(x => x + 1)
    .Select(x => x * 3);

var r3 = r2.As();

// -------------------------------------------
// The "Kinds" type and the C# style f Functor
// -------------------------------------------

public interface K<F, A>;

public interface Mappable<F>
    where F : Mappable<F>
{
    public static abstract K<F, B> Select<A, B>(K<F, A> list, Func<A, B> f);
}

// --------------
// Implement List
// --------------

public record List<A>(A[] Items) : K<List, A>;
public class List : Mappable<List>
{
    public static K<List, B> Select<A, B>(K<List, A> list, Func<A, B> f) =>
        new List<B>(list
            .As()
            .Items
            .Select(f)
            .ToArray());
}

public static class ListExtensions
{
    public static List<A> As<A>(this K<List, A> ma) =>
        (List<A>)ma;
}

// ------------------------------------------------------
// Define the Select method (for all F that are Mappable)
// ------------------------------------------------------

public static class MappableExtensions
{
    public static K<F, B> Select<F, A, B>(this K<F, A> fa, Func<A, B> f)
        where F : Mappable<F> =>
        F.Select(fa, f);
}

// ------------------------------------------
// Bring Maybe into the world in the same way
// ------------------------------------------

public abstract record Maybe<A> : K<Maybe, A>;
public record Just<A>(A Value) : Maybe<A>;
public record Nothing<A>() : Maybe<A>;


public class Maybe : Mappable<Maybe>
{
    public static K<Maybe, B> Select<A, B>(K<Maybe, A> maybe, Func<A, B> f) =>
        maybe.As() switch
        {
            Just<A>(var x) => new Just<B>(f(x)),
            Nothing<A> => new Nothing<B>()
        };
}

public static class MaybeExtensions
{
    public static Maybe<A> As<A>(this K<Maybe, A> ma) =>
        (Maybe<A>)ma;
}

// ------------------------------------------------------------------------------
// Note how we can define something across all of the collection types
// (And a neat trick to add statics and properties to the top level function set)
// ------------------------------------------------------------------------------

partial class Program
{
    public static K<F, int> Foo<F>(K<F, string> ma)
        where F : Mappable<F> =>
        ma.Select(x => x.Length);

}

