using System.Diagnostics;


Example();

void Example()
{
    var checkedForArgumentExceptionBefore = false;

    try
    {
        ThrowException();
    }
    catch (ArgumentException ex) when (IsArgumentException(ex, ref checkedForArgumentExceptionBefore))
    {
        Debugger.Break();
    }
    catch (Exception ex) when (IsArgumentException(ex, ref checkedForArgumentExceptionBefore))
    {
        Debugger.Break();
    }
    catch (Exception ex) when (IsArgumentException(ex, ref checkedForArgumentExceptionBefore))
    {
        /* 5 */
        Debugger.Break();
    }
    finally
    {
        /* 6 */
        Debugger.Break();
    }

    bool IsArgumentException(Exception ex, ref bool v)
    {
        Debugger.Break(); /* 2 */ /* 3 */
        var lastValue = v;
        v = true;
        return lastValue;
    }
}

void ThrowException()
{
    try
    {
        Debugger.Break(); /* 1 */
        throw new Exception();
    }
    finally
    {
        /* 4 */
        Debugger.Break();
    }
}