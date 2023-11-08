Foo data = default;
Console.WriteLine(data.x?.x);
struct Foo
{
    public Boz? x;

    public Foo()
    {

    }
}

class Boz
{
    public int x = 1;
}