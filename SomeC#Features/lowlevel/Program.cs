
int x = 20;

var span = new Span<int>(ref x);

span[0] = 33;

Console.ReadLine();
