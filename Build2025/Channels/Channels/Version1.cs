#if TRUE
using System.Threading.Channels;

Channel<int> c = Channel.CreateUnbounded<int>();

_ = Task.Run(async () =>
{
    for (int i = 0; i < 10; i++)
    {
        await c.Writer.WriteAsync(i);
        await Task.Delay(TimeSpan.FromSeconds(1));
    }

    c.Writer.Complete();
});

while (true)
{
    Console.WriteLine(await c.Reader.ReadAsync());
}
#endif
