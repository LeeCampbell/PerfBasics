
# Cost of complexity and coupling
Builds should be take seconds (2-4s)
Build + Unit Tests should be seconds (5-15s)
Build + Unit + Integration tests should be minutes (<10min)
Deployment (Provision resources, start VMs etc.) (<15min)

How can adding features and code so quickly blow out our build, test and deployment times?


|             | Feature A | Feature B | Feature C |
|-------------|-----------|-----------|-----------|
| View        | x         | x         | x         |
| ViewModel   | x         | x         | x         |
| Business    | x         | x         | x         |
| Translation | x         | x         | x         |
| Comms       | x         | x         | x         |


vs

|             | Feature A |
|-------------|-----------|
| Business    | x         |

|             | Feature A |
|-------------|-----------|
| View        | x         |
| ViewModel   | x         |

|             | Feature A |
|-------------|-----------|
| Translation | x         |
| Comms       | x         |

and a handful of smoke tests for an integration test.

Why only a handful of integration tests?
For every branch we introduce a new possible code path.
So consider every conditional, switch, loop and throw in your code.
To test all those code paths is 2^n tests.
This number quickly becomes unreasonable.

Now consider those same code paths but this time with respect to the compiler.
The compiler will have to build a far larger and  more complicated internal representation.
The Parse tree, the AST (Abstract Syntax Tree) and the concrete trees to be able to analyze, verify and compile your code.

Next consider that if you are working on just FeatureA.
Not only do you have to build FeatureB and FeatureC each time, but just by having them is a super linear increase in build times.
The same can be said for the Tests.
So in this case we are potentially paying not just the cost of FeatureB and FeatureC but also for the complexity of them being together.

This pattern of exponential cost continues from compilation and testing, into other areas like deployment, performance, documentation and project management (JIRA).

## Complexity Theory

A Complex system is not just a complicated system.
A watch is a complicated system, but has clear bounds to which it works and is ultimately a closed system.

A complex system is spontaneously organized, unpredictable and has exponentially greater energy requirements as it grows ultimately spiraling upwards until collapse.

Features of a complex system can include:
 * Cascading Failure due to coupling
 * Open systems (i.e. not a closed system). Can grow/shrink/change from external stimulus (new feature?)
 * Emergent Behavior. Based purely on its constituent parts, behavior can not be predicted (e.g. biology of a termite colony does not belie the social behavior)
 * Relationships are non-linear. A small perturbation may cause large (butterfly) effect.
 * Relationships have feed back loops.
