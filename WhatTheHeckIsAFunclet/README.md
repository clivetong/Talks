# What the heck is a funclet?

---

## Why interested in this?

[A Microsoft bug that broke the profiler API](https://github.com/dotnet/runtime/pull/123564)

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

Used to be used in MoveNext implementation to Dispose the enumerator if the MoveNext threw an exception

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

First pass - find where we need to unwind to

---

### 2

Inside Argument Exception

![stack 2](images/at2.png)

We haven't unwound yet, but need to access the local in the stack frame two back (or generate a closure)

---

### 3

Inside Argument Exception

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
- and we need it without slowing the fast path
- which would just store the local in the stack frame

---

### So that's the funclet

- The code for the exception path is appended to the end of the normal .NET code
- (exception blocks are encoded as a table in the CLR)
- this keeps it out of the cache for the hot path
- but it isn't entered like a normal method
- which has a standard prolog and epilog
- it instead has a slightly different calling convention
