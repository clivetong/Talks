using NUnit.Framework;

namespace AsyncTransform;

[TestFixture]
public class MethodWithAwaits
{
    [TestCase(11, ExpectedResult = 12)]
    public async Task<int> TransformThis(int argument)
    {
        var i = argument;

        await Task.Delay(TimeSpan.FromSeconds(1));

        i++;

        Console.WriteLine(i);

        await Task.Delay(TimeSpan.FromSeconds(1));

        return i;
    }
}
