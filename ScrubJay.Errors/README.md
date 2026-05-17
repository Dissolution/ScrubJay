# ScrubJay.Errors







## `Ex`
- Provides helper methods for generating various exceptions with predefined messages.

## `Exceptions/`
- All the Exceptions in here standardize the behavior of various system Exceptions (the order of parameters, etc...).
- Whatever message is passed into their constructors are passed exactly to the `Message` property.
- They all superclass an existing System Exception in order to work with existing `catch` blocks.

---

## RFC-9457 - Problem Details
- https://datatracker.ietf.org/doc/html/rfc9457
- https://datatracker.ietf.org/doc/html/rfc9457