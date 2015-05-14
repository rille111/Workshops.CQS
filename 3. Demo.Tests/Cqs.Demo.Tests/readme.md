I see no sense in creating strict unit tests that should test every single method completely without dependencies.
You want to assert that several components and classes are working as expected, together, using the architecture they were designed for.
As long as code coverage is kept high, that should be sufficient.