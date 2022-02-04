# Dynamic PGO - why's it taken so long to get here?


## What do you need from the base platform?

    1 The ability to reJit code.

    2 The ability to dynamically watch what the code is doing
        - ie no run code, not need to JIT it.
        - tiered compilation - fast compilation/slow code, slow compilation/fast code

    3 on stack replacement (Not yet fully fully available)

    4 which is important because inlining is the big win!!!

## Why does inlining matter?

    1 Calling conventions

    2 GC safe points

## Speculative inlining

    1 Guarded compilation

    2 Trap when we've seen it enough

    3 Trap when we didn't think we'd see it, but have now 

        - most programs call the same type of instance at the same points 
        - old tech - PICs

    4 Friendier to the caches by hot path analysis

