// https://refactoring.guru/design-patterns/chain-of-responsibility

// A chain of processors which can decide to handle the request or pass to the next one in the chain

// https://code-maze.com/csharp-chain-of-responsibility-design-pattern
//ASP.NET Core middleware is a prime example of the chain of responsibility pattern.
//The middleware decouples a request's sender from its receivers by allowing multiple
//handlers to handle it independently. Each handler addresses a specific aspect of the whole task

// Localising parts of the processing into methods on their own objects
//    (we've seen this before where we try to get rid of conditionals)

// The handler knows about the next one (ie ordering is important), 
// does some processing and can decide not to pass to the next

// This has examples in bubbling and tunnelling events in browsers and WPF, where the control
// happens outside these objects

// The linked handlers are dynamic - they don't have a fixed structure
// Order is important, but can be chosen the start of the run.
