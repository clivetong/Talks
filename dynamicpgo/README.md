# Dynamic PGO - why's it taken so long to get here?


## What do you need from the base platform?

    1 The ability to re-JIT code.

    2 The ability to dynamically watch what the code is doing (a reflective runtime)
        - ie no run code, not need to JIT it.
        - tiered compilation - fast compilation/slow code, slow compilation/fast code

    3 on stack replacement (Not yet fully fully available)

    4 which is important because inlining is the big win!!!

## Why does inlining matter?

    1 Calling conventions

        - multiple entry points in static code

    2 GC safe points

        - need to be clear what is a real pointer to a managed object at points threads can be stopped


==>> inline everything and learn what you should not have done, but assume you'll be right
       or
    don't inline much, but watch out for opportunities



## Speculative inlining

    1 Guarded compilation

    2 Trap when we've seen it enough

    3 Trap when we didn't think we'd see it, but have now 

        - most programs call the same type of instance at the same points 
        - old tech - PICs

    4 Friendlier to the caches by hot path analysis


## Obligatory mention

    1 If your language is powerful enough, you don't need to do this in the runtime
       - the book that had the most effect on me ever - https://en.wikipedia.org/wiki/The_Art_of_the_Metaobject_Protocol

    2 SELF 
       - go extreme and dynamically de-optimise


## Which is better?

      PGO versus DPGO

