# ScrubJay.Errors

## `Demand`, `Validate`, `Throw`, and `Ex`
Four parts of the same validation puzzle.
`Demand` -> `Throw` -> `Ex`
`Validate` -> `Ex`

### `Demand`
All methods in this class return `void`.
They validate an argument and throw and Exception if validation fails.
None of these methods takes additional information, they construct the Exception messages.

### `Validate`
All methods in this class return `Result<T>`.
They validate an argument and return either `Ok<T>` with the validated argument or `Error` containing the validation Exception.
These methods do not take additional info or inner exceptions, they construct all Exception messages.

### `Throw`
Everything in this class is marked as `[DoesNotReturn]` and `Throw` is `[StackTraceHidden]` to keep stack traces clean.
Every method throws a particular type of Exception.
Every method takes in additional `info` that is passed along to Exception construction.

### `Ex`
Provides helper methods for generating various exceptions with predefined messages.
~~~~

---

## `Exceptions/`
- All the Exceptions in here standardize the behavior of various system Exceptions (the order of parameters, etc...).
- Whatever message is passed into their constructors are passed exactly to the `Message` property.
- They all superclass an existing System Exception in order to work with existing `catch` blocks.

---

## RFC-9457 - Problem Details
- https://datatracker.ietf.org/doc/html/rfc9457
- https://datatracker.ietf.org/doc/html/rfc9457