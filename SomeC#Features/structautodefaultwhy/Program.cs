S x = new() { X=20 };

Console.WriteLine();
class S
{
    private int _x;
    public int X
    {
        get => _x;
        set => _x = value >= 0 ? value : throw new ArgumentOutOfRangeException();
    }

    private int xxx = 20;
    //public S()
    //{

    //}

}