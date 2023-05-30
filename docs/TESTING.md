# Testing
This file contains instructions on how to write and run the tests in this project.

## Unit tests

### Definition

>A unit test is an automated piece of code that invokes the unit of work being tested, and then checks some assumptions 
> about a single end result of that unit. A unit test is almost always written using a unit testing framework.
> It can be written easily and runs quickly. It’s trustworthy, readable, and maintainable. It’s consistent in 
> its results as long as production code hasn’t changed. 
> [Art of Unit Testing 2nd Edition](https://www.manning.com/books/the-art-of-unit-testing-second-edition) Roy Osherove

### Implementation

Each module should have `Tests/UnitTests` project. These projects have NUnit and NSubstitute dependencies.

Each unit tests should extend base class `App.BuildingBlocks.Tests.UnitTests.UnitTestBase`. Base test class contains 
additional assertions and methods that help to test domain logic properly.

TODO: add more information about writing unit tests

Each test can have 2 possible outcomes:
* Action on Aggregate executed successfully and `DomainEvent` was published.
* Business rule was broken during executing an action on Aggregate.

## Integration tests

### Setup

1. Run migrations
```bash
make test-db-update # migrates the test database
```

2. Add environment variables from .env.test to Test Runner settings (Rider): Settings > Build, Execution, Deployment > Unit tests > Test Runner > Environment variables

