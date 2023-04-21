class Program
{
    private static bool _cancelLoop = false;
    private static int _counter = 0;

    private static void LoopThreadStart()
    {
        while (!_cancelLoop)
        {
#if !NOCOUNTER
            _counter++;
#endif
        }
    }

    static void Main()
    {
        var loopThread = Task.Run(LoopThreadStart);

        Thread.Sleep(5000);
        _cancelLoop = true;

        loopThread.Wait();
    }
}