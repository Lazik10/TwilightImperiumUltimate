global using FluentAssertions;
global using Xunit;

// Bound independent LocalDB fixtures without limiting explicit concurrency within each test.
[assembly: CollectionBehavior(MaxParallelThreads = 2)]
