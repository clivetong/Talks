# What the heck is a funclet?

---

## Why interested in this?

[A Microsoft bug that broke the profiler API](https://github.com/dotnet/runtime/pull/123564) by duplicating an event causing the simulated stack to underflow.

See exceptionprofiler.cpp in that PR where they implement a test by handling some of the profiler events.

---

## What does exception handling need to do?

---

```CSharp
void Example()
{
    var checkedForArgumentExceptionBefore = false;

    try { ThrowException(); }
    catch (ArgumentException ex) when (IsArgumentException(ex, ref checkedForArgumentExceptionBefore)) {}
    catch (Exception ex) when (IsArgumentException(ex, ref checkedForArgumentExceptionBefore)) {}
    catch (Exception ex) when (IsArgumentException(ex, ref checkedForArgumentExceptionBefore)) {   /* 5 */ }
    finally { /* 6 */ }

    bool IsArgumentException(Exception ex, ref bool v)
    {
        /* 2 */ /* 3 */
        var lastValue = v;
        v = true;
        return lastValue;
    }
}

void ThrowException()
{
    try { /* 1 */ throw new Exception(); } finally { /* 4 */ }
}
```

---

### Why that example?

- It shows that we have to walk the stack to find out where the catch happens
- It shows the filter needs access to the stack frame where the Example code is running
- It shows that we have two phases - first pass and second pass

---

- Note that we've seen filters and finally, and catch but we haven't seen fault

Used in MoveNext implementation to Dispose the enumerator if the MoveNext threw an exception

---

```CSharp
IEnumerable<int> GetAll()
{
    using var x = new Foo();
    yield return 1;
    yield return 2;
}


class Foo : IDisposable { public void Dispose() { } }
```

---

```IL
    .method private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    {
      .override [System.Runtime]System.Collections.IEnumerator::MoveNext
      // Code size       139 (0x8b)
      .maxstack  2
      .locals init (bool V_0,
               int32 V_1)
      .try
      {
        .... stack machine
      }  // end .try
      fault
      {
        IL_0081:  ldarg.0
        IL_0082:  call       instance void Program/'<<<Main>$>g__GetAll|0_0>d'::System.IDisposable.Dispose()
        IL_0087:  nop
        IL_0088:  endfinally
      }  // end handler
      IL_0089:  ldloc.0
      IL_008a:  ret
    } // end of method '<<<Main>$>g__GetAll|0_0>d'::MoveNext
```

---

### 1

```CSharp
try { /* 1 */ throw new Exception(); } finally { /* 4 */ }
```

![stack 1](images/at1.png)

---

### 2

First pass - find where we need to unwind to

Inside Argument Exception

![stack 2](images/at2.png)

We haven't unwound yet, but need to access the local in the stack frame two back (or generate a closure)

btw note that we are in a different context, which means we might see the world differently

---

### Aside

In .NET 9, more of the exception handling was converted to managed code

```stacktrace
0:000> !clrstack
OS Thread Id: 0x1f8c (0)
        Child SP               IP Call Site
000000517017C4C0 00007ffc33b14aef Program.<$>g__IsArgumentException|0_2(System.Exception, Boolean ByRef) [C:\Users\clive.tong\Desktop\play\ConsoleApp3\Program.cs @ 35]
000000517017C500 00007ffc33b1496b Program.<$>g__Example|0_0() [C:\Users\clive.tong\Desktop\play\ConsoleApp3\Program.cs @ 22]
000000517017C7A8 00007ffc9376865c [InlinedCallFrame: 000000517017c7a8] 
000000517017C7A8 00007ffc7b71a99f [InlinedCallFrame: 000000517017c7a8] 
000000517017C770 00007ffc7b71a99f System.Runtime.EH.FindFirstPassHandler(System.Object, UInt32, System.Runtime.StackFrameIterator ByRef, UInt32 ByRef, Byte* ByRef) [/_/src/runtime/src/coreclr/nativeaot/Runtime.Base/src/System/Runtime/ExceptionHandling.cs @ 1078]
000000517017C8C0 00007ffc7b71a5b3 System.Runtime.EH.DispatchEx(System.Runtime.StackFrameIterator ByRef, ExInfo ByRef) [/_/src/runtime/src/coreclr/nativeaot/Runtime.Base/src/System/Runtime/ExceptionHandling.cs @ 793]
000000517017DFF0 00007ffc93768743 [SoftwareExceptionFrame: 000000517017dff0] 
000000517017E600 00007ffc33b14a7d Program.<$>g__ThrowException|0_1() [C:\Users\clive.tong\Desktop\play\ConsoleApp3\Program.cs @ 47]
000000517017E640 00007ffc33b147ca Program.<$>g__Example|0_0() [C:\Users\clive.tong\Desktop\play\ConsoleApp3\Program.cs @ 12]
000000517017E720 00007ffc33b14753 Program.$(System.String[]) [C:\Users\clive.tong\Desktop\play\ConsoleApp3\Program.cs @ 4]
```

---

### 3

First pass continues

![stack 3](images/at3.png)

We haven't unwound yet, but need to access the local in the stack frame two back (or generate a closure)

---

### 4

- First pass has located the target
- Start the second pass
- Unwind the later stack frames and execute any finally blocks as we go

![stack 4](images/at4.png)

---

### 5

```CSharp
catch (Exception ex) when (IsArgumentException(ex)) {   /* 5 */ }
```

![stack 5](images/at5.png)

---

### 6

- And we now exit Example so do the finally

```CSharp
finally { /* 6 */ }
```

![stack 6](images/at6.png)

---

### So what's so special about that?

- Let's look at the 2nd and 3rd stop points again

![stack 2](images/at2.png)

- the debugger says we are in the same .NET function twice
- and we need access to the local variable
- and we need it without slowing the fast path which would just store the local in the stack frame

---

### So that's the funclet

- The code for the exception path is appended to the end of the normal .NET code
- (exception blocks are encoded as a table in the CLR)
- this keeps it out of the cache for the hot path
- it isn't entered like a normal method which has a standard prolog and epilog
- it instead has a slightly different calling convention

---

### See the details here

- [Different caller and callee saved registers](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/clr-abi.md#register-values-and-exception-handling)
- [Access to locals because the frame pointer is set to that of the parent](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/clr-abi.md#registers-on-entry-to-a-funclet)
- [GC Info and hot/cold splitting](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/clr-abi.md#register-values-and-exception-handling)
