Foo data = default;

ref var foo0 = ref data[0];
ref var foo1 = ref data[1];
ref var foo2 = ref data[2];

foo0 = 29;
foo2 = 393;


[System.Runtime.CompilerServices.InlineArray(10000)]
struct Foo
{
    public int A;
}
