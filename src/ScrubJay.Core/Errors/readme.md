- Demand
  - returns `void`, acts like the `Argument*Exception.ThrowIf*()` methods
- Ensure
  - returns the `T` being tested (fluent / inline usage)
- Validate
  - returns a `Result<T, Exception>` that contains the valid `T` or an `Exception` that describes why the validation failed

The first argument is always the thing being tested!