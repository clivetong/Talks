using System;
using System.Diagnostics;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.DisableOptimizations
    | DebuggableAttribute.DebuggingModes.Default
    | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]

namespace ClassLibrary1
{
    public class Class1
    {
        public string GetData() => "Hello";
    }
}
