# How to contribute

So you're thinking about contributing to Cake? Great! **It's really appreciated.**

## Where to start?

Start by either suggest a feature in the issue list that you want to see in Cake, or assign yourself to an existing feature/bug. Make sure that no one else is working on it already though. It's also appreciated to have some kind of discussion about the issue if it's not an easy fix.

If your suggestion applies to a broad range of users and scenarios, it will be considered for inclusion in the core Cake assemblies.

Make sure that your contribution follows this contribution guide.

## Branches

Do your work in feature branches based on `develop`. `develop` is the main development branch in Cake and is where all code is pushed before making a release.

## Coding

Normal .NET coding guidelines apply. See the [Framework Design Guidelines](https://msdn.microsoft.com/en-us/library/ms229042%28v=vs.110%29.aspx) for more information.

### Dependencies

The assemblies `Cake.Core` and `Cake.Common` should have no dependencies except the .NET BCL library.

Dependencies for the Cake executable is acceptable in specific circumstances. If you want to introduce a dependency to the Cake executable, make sure you bring it up in an issue or a pull request, so it can be properly discussed.

### Unit Tests

Make sure to run all unit tests before creating a pull request. You code should also have reasonable unit test coverage.

## Submitting a pull request

* Pull requests should be tagged with `[WIP]` in the title if not finished already. This can also be done to provide discussion of a planned pull request before starting work.
* Include as much detail as possible in the description, such as if the pull request solves a reported issue or implements a specific feature.

## License
By contributing to Cake, you assert that:

* The contribution is your own original work.
* You have the right to assign the copyright for the work (it is not owned by your employer, or
  you have been given copyright assignment in writing).
* You [license](https://github.com/cake-build/cake/blob/develop/LICENSE) for more information. it under the terms applied to the rest of the Cake project.