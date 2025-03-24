// Add extra functionality to an object

// https://refactoring.guru/design-patterns/decorator

// Allows the addition of abilities dynamically at runtime 

// See Elegant Objects: https://github.com/clivetong/Talks/tree/main/ElegantObjects
//     specifically https://github.com/clivetong/Talks/tree/main/ElegantObjects#small-objects-with-a-clear-purpose

// Good for doing things before or after
//     Widget
//     WidgetWithTitle

EnterBar(new Cowboy());
EnterBar(new NonAlcoholicCowboy(new Cowboy()));
void EnterBar(ICowboy cowboy)
{
    cowboy.Drink();
    cowboy.Draw();
}

interface ICowboy
{
    void Drink();
    void Draw();
}

class Cowboy : ICowboy
{
    public void Drink()
    {
        Console.WriteLine("Takes a swig of whiskey");
    }

    public void Draw()
    {
        Console.WriteLine("Takes out gun");
    }
}

class NonAlcoholicCowboy(ICowboy cowboy) : ICowboy
{
    public void Drink()
    {
        Console.WriteLine("Takes a swig of water");
        //cowboy.Drink();
    }

    public void Draw()
    {
        cowboy.Draw();
    }
}