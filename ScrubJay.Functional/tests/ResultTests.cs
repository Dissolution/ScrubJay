namespace ScrubJay.Functional.Tests;

/// <summary>
/// Comprehensive unit tests for Result&lt;T&gt;
/// </summary>
public class ResultTests
{
#region Construction Tests
    [Fact]
    public void Ok_CreatesOkResult()
    {
        var result = Result<int>.Ok(42);

        Assert.True(result.IsOk());
        Assert.True(result.IsOk(out var value));
        Assert.Equal(42, value);
    }

    [Fact]
    public void Ok_WithNullValue_CreatesOkResult()
    {
        var result = Result<string>.Ok(null!);

        Assert.True(result.IsOk());
        Assert.True(result.IsOk(out var value));
        Assert.Null(value);
    }

    [Fact]
    public void Error_CreatesErrorResult()
    {
        var exception = new InvalidOperationException("test error");
        var result = Result<int>.Error(exception);

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var error));
        Assert.Same(exception, error);
    }

    [Fact]
    public void Error_WithNullException_CreatesErrorResultWithInvalidOperationException()
    {
        var result = Result<int>.Error(null);

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var error));
        Assert.IsType<InvalidOperationException>(error);
    }
#endregion
#region Implicit Conversion Tests
    [Fact]
    public void ImplicitConversion_FromValue_CreatesOkResult()
    {
        Result<int> result = 42;

        Assert.True(result.IsOk());
        Assert.True(result.IsOk(out var value));
        Assert.Equal(42, value);
    }

    [Fact]
    public void ImplicitConversion_FromException_CreatesErrorResult()
    {
        var exception = new ArgumentException("test");
        Result<int> result = exception;

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var error));
        Assert.Same(exception, error);
    }

    [Fact]
    public void ImplicitConversion_ToBool_OkIsTrue()
    {
        Result<int> result = Result<int>.Ok(42);
        bool value = result;

        Assert.True(value);
    }

    [Fact]
    public void ImplicitConversion_ToBool_ErrorIsFalse()
    {
        Result<int> result = Result<int>.Error(new Exception());
        bool value = result;

        Assert.False(value);
    }

#endregion
#region IsOk Tests
    [Fact]
    public void IsOk_NoParameters_ReturnsTrueForOk()
    {
        var result = Result<int>.Ok(42);

        Assert.True(result.IsOk());
    }

    [Fact]
    public void IsOk_NoParameters_ReturnsFalseForError()
    {
        var result = Result<int>.Error(new Exception());

        Assert.False(result.IsOk());
    }

    [Fact]
    public void IsOk_WithValueOut_ReturnsTrueAndSetsValueForOk()
    {
        var result = Result<int>.Ok(42);

        var isOk = result.IsOk(out var value);

        Assert.True(isOk);
        Assert.Equal(42, value);
    }

    [Fact]
    public void IsOk_WithValueOut_ReturnsFalseAndSetsDefaultForError()
    {
        var result = Result<int>.Error(new Exception());

        var isOk = result.IsOk(out var value);

        Assert.False(isOk);
        Assert.Equal(0, value);
    }

    [Fact]
    public void IsOk_WithValueAndErrorOut_ReturnsTrueAndSetsValueForOk()
    {
        var result = Result<int>.Ok(42);

        var isOk = result.IsOk(out var value, out var error);

        Assert.True(isOk);
        Assert.Equal(42, value);
        Assert.Null(error);
    }

    [Fact]
    public void IsOk_WithValueAndErrorOut_ReturnsFalseAndSetsErrorForError()
    {
        var exception = new InvalidOperationException();
        var result = Result<int>.Error(exception);

        var isOk = result.IsOk(out var value, out var error);

        Assert.False(isOk);
        Assert.Equal(0, value);
        Assert.Same(exception, error);
    }

    [Fact]
    public void IsOkAnd_ReturnsTrueWhenOkAndPredicateTrue()
    {
        var result = Result<int>.Ok(42);

        Assert.True(result.IsOkAnd(x => x > 40));
    }

    [Fact]
    public void IsOkAnd_ReturnsFalseWhenOkAndPredicateFalse()
    {
        var result = Result<int>.Ok(42);

        Assert.False(result.IsOkAnd(x => x < 40));
    }

    [Fact]
    public void IsOkAnd_ReturnsFalseWhenError()
    {
        var result = Result<int>.Error(new Exception());

        Assert.False(result.IsOkAnd(x => x > 40));
    }
#endregion
#region IsError Tests
    [Fact]
    public void IsError_NoParameters_ReturnsTrueForError()
    {
        var result = Result<int>.Error(new Exception());

        Assert.True(result.IsError());
    }

    [Fact]
    public void IsError_NoParameters_ReturnsFalseForOk()
    {
        var result = Result<int>.Ok(42);

        Assert.False(result.IsError());
    }

    [Fact]
    public void IsError_WithErrorOut_ReturnsTrueAndSetsErrorForError()
    {
        var exception = new InvalidOperationException();
        var result = Result<int>.Error(exception);

        var isError = result.IsError(out var error);

        Assert.True(isError);
        Assert.Same(exception, error);
    }

    [Fact]
    public void IsError_WithErrorOut_ReturnsFalseAndSetsNullForOk()
    {
        var result = Result<int>.Ok(42);

        var isError = result.IsError(out var error);

        Assert.False(isError);
        Assert.Null(error);
    }

    [Fact]
    public void IsError_WithErrorAndOkOut_ReturnsTrueAndSetsErrorForError()
    {
        var exception = new InvalidOperationException();
        var result = Result<int>.Error(exception);

        var isError = result.IsError(out var error, out var ok);

        Assert.True(isError);
        Assert.Same(exception, error);
        Assert.Equal(0, ok);
    }

    [Fact]
    public void IsError_WithErrorAndOkOut_ReturnsFalseAndSetsOkForOk()
    {
        var result = Result<int>.Ok(42);

        var isError = result.IsError(out var error, out var ok);

        Assert.False(isError);
        Assert.Null(error);
        Assert.Equal(42, ok);
    }

    [Fact]
    public void IsErrorAnd_ReturnsTrueWhenErrorAndPredicateTrue()
    {
        var result = Result<int>.Error(new ArgumentException());

        Assert.True(result.IsErrorAnd(ex => ex is ArgumentException));
    }

    [Fact]
    public void IsErrorAnd_ReturnsFalseWhenErrorAndPredicateFalse()
    {
        var result = Result<int>.Error(new ArgumentException());

        Assert.False(result.IsErrorAnd(ex => ex is InvalidOperationException));
    }

    [Fact]
    public void IsErrorAnd_ReturnsFalseWhenOk()
    {
        var result = Result<int>.Ok(42);

        Assert.False(result.IsErrorAnd(_ => true));
    }
#endregion
#region OkOr Tests
    [Fact]
    public void OkOr_ReturnsValueWhenOk()
    {
        var result = Result<int>.Ok(42);

        var value = result.OkOr(100);

        Assert.Equal(42, value);
    }

    [Fact]
    public void OkOr_ReturnsFallbackWhenError()
    {
        var result = Result<int>.Error(new Exception());

        var value = result.OkOr(100);

        Assert.Equal(100, value);
    }

    [Fact]
    public void OkOr_WithFunc_ReturnsValueWhenOk()
    {
        var result = Result<int>.Ok(42);
        var fallbackCalled = false;

        var value = result.OkOr(() =>
        {
            fallbackCalled = true;
            return 100;
        });

        Assert.Equal(42, value);
        Assert.False(fallbackCalled);
    }

    [Fact]
    public void OkOr_WithFunc_ReturnsFallbackWhenError()
    {
        var result = Result<int>.Error(new Exception());
        var fallbackCalled = false;

        var value = result.OkOr(() =>
        {
            fallbackCalled = true;
            return 100;
        });

        Assert.Equal(100, value);
        Assert.True(fallbackCalled);
    }

    [Fact]
    public void OkOrDefault_ReturnsValueWhenOk()
    {
        var result = Result<int>.Ok(42);

        var value = result.OkOrDefault();

        Assert.Equal(42, value);
    }

    [Fact]
    public void OkOrDefault_ReturnsDefaultWhenError()
    {
        var result = Result<int>.Error(new Exception());

        var value = result.OkOrDefault();

        Assert.Equal(0, value);
    }

    [Fact]
    public void OkOrDefault_ReturnsNullForReferenceTypeWhenError()
    {
        var result = Result<string>.Error(new Exception());

        var value = result.OkOrDefault();

        Assert.Null(value);
    }

    [Fact]
    public void OkOrThrow_ReturnsValueWhenOk()
    {
        var result = Result<int>.Ok(42);

        var value = result.OkOrThrow();

        Assert.Equal(42, value);
    }

    [Fact]
    public void OkOrThrow_ThrowsExceptionWhenError()
    {
        var exception = new InvalidOperationException("test error");
        var result = Result<int>.Error(exception);

        var thrown = Assert.Throws<InvalidOperationException>(() => result.OkOrThrow());

        Assert.Same(exception, thrown);
    }
#endregion
#region ErrorOr Tests
    [Fact]
    public void ErrorOr_ReturnsErrorWhenError()
    {
        var exception = new InvalidOperationException();
        var result = Result<int>.Error(exception);

        var error = result.ErrorOr(new ArgumentException());

        Assert.Same(exception, error);
    }

    [Fact]
    public void ErrorOr_ReturnsFallbackWhenOk()
    {
        var result = Result<int>.Ok(42);
        var fallback = new ArgumentException();

        var error = result.ErrorOr(fallback);

        Assert.Same(fallback, error);
    }

    [Fact]
    public void ErrorOr_WithFunc_ReturnsErrorWhenError()
    {
        var exception = new InvalidOperationException();
        var result = Result<int>.Error(exception);
        var fallbackCalled = false;

        var error = result.ErrorOr(() =>
        {
            fallbackCalled = true;
            return new ArgumentException();
        });

        Assert.Same(exception, error);
        Assert.False(fallbackCalled);
    }

    [Fact]
    public void ErrorOr_WithFunc_ReturnsFallbackWhenOk()
    {
        var result = Result<int>.Ok(42);
        var fallback = new ArgumentException();
        var fallbackCalled = false;

        var error = result.ErrorOr(() =>
        {
            fallbackCalled = true;
            return fallback;
        });

        Assert.Same(fallback, error);
        Assert.True(fallbackCalled);
    }

    [Fact]
    public void ThrowIfError_DoesNotThrowWhenOk()
    {
        var result = Result<int>.Ok(42);

        result.ThrowIfError();

        // No exception thrown
    }

    [Fact]
    public void ThrowIfError_ThrowsWhenError()
    {
        var exception = new InvalidOperationException("test");
        var result = Result<int>.Error(exception);

        var thrown = Assert.Throws<InvalidOperationException>(() => result.ThrowIfError());

        Assert.Same(exception, thrown);
    }
#endregion
#region Match Tests
    [Fact]
    public void Match_Action_CallsOnOkForOk()
    {
        var result = Result<int>.Ok(42);
        var okCalled = false;
        var errorCalled = false;

        result.Match(
            onOk: value =>
            {
                okCalled = true;
                Assert.Equal(42, value);
            },
            onError: _ =>
            {
                errorCalled = true;
            });

        Assert.True(okCalled);
        Assert.False(errorCalled);
    }

    [Fact]
    public void Match_Action_CallsOnErrorForError()
    {
        var exception = new InvalidOperationException();
        var result = Result<int>.Error(exception);
        var okCalled = false;
        var errorCalled = false;

        result.Match(
            onOk: _ =>
            {
                okCalled = true;
            },
            onError: ex =>
            {
                errorCalled = true;
                Assert.Same(exception, ex);
            });

        Assert.False(okCalled);
        Assert.True(errorCalled);
    }

    [Fact]
    public void Match_Func_ReturnsOnOkResultForOk()
    {
        var result = Result<int>.Ok(42);

        var output = result.Match(
            onOk: value => $"Value: {value}",
            onError: ex => $"Error: {ex.Message}");

        Assert.Equal("Value: 42", output);
    }

    [Fact]
    public void Match_Func_ReturnsOnErrorResultForError()
    {
        var result = Result<int>.Error(new InvalidOperationException("test"));

        var output = result.Match(
            onOk: value => $"Value: {value}",
            onError: ex => $"Error: {ex.Message}");

        Assert.Equal("Error: test", output);
    }
#endregion
#region AsOption Tests
    [Fact]
    public void AsOption_ReturnsSomeForOk()
    {
        var result = Result<int>.Ok(42);

        var option = result.AsOption();

        Assert.True(option.IsSome());
        Assert.True(option.IsSome(out var value));
        Assert.Equal(42, value);
    }

    [Fact]
    public void AsOption_ReturnsNoneForError()
    {
        var result = Result<int>.Error(new Exception());

        var option = result.AsOption();

        Assert.True(option.IsNone());
    }
#endregion

#region Boolean Operators
    [Fact]
    public void TrueOperator_ReturnsTrueForOk()
    {
        var ok = Result<int>.Ok(42);

        if (ok)
        {
            Assert.True(true);
        }
        else
        {
            Assert.Fail("Should not reach here");
        }
    }

    [Fact]
    public void FalseOperator_ReturnsFalseForError()
    {
        var error = Result<int>.Error(new Exception());

        if (error)
        {
            Assert.Fail("Should not reach here");
        }
        else
        {
            Assert.True(true);
        }
    }
#endregion

#region IEnumerable Tests
    [Fact]
    public void GetEnumerator_Ok_YieldsValue()
    {
        var ok = Result<int>.Ok(42);

        var values = new List<int>();
        foreach (var value in ok)
        {
            values.Add(value);
        }

        Assert.Single(values);
        Assert.Equal(42, values[0]);
    }

    [Fact]
    public void GetEnumerator_Error_YieldsNothing()
    {
        var error = Result<int>.Error(new Exception());

        var values = new List<int>();
        foreach (var value in error)
        {
            values.Add(value);
        }

        Assert.Empty(values);
    }

    [Fact]
    public void GetEnumerator_Ok_CanBeResetAndReused()
    {
        var ok = Result<int>.Ok(42);

        using var enumerator = ok.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(42, enumerator.Current);
        Assert.False(enumerator.MoveNext());

        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(42, enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void GetEnumerator_Error_ResetDoesNothing()
    {
        var error = Result<int>.Error(new Exception());

        using var enumerator = error.GetEnumerator();

        Assert.False(enumerator.MoveNext());

        enumerator.Reset();

        Assert.False(enumerator.MoveNext());
    }
#endregion
#region LINQ Select Tests
    [Fact]
    public void Select_Selector_TransformsOkValue()
    {
        var ok = Result<int>.Ok(42);

        var result = ok.Select(x => x * 2);

        Assert.True(result.IsOk());
        Assert.True(result.IsOk(out var value));
        Assert.Equal(84, value);
    }

    [Fact]
    public void Select_Selector_PropagatesError()
    {
        var exception = new InvalidOperationException();
        var error = Result<int>.Error(exception);

        var result = error.Select(x => x * 2);

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var resultError));
        Assert.Same(exception, resultError);
    }

    [Fact]
    public void Select_SelectorCanChangeType()
    {
        var ok = Result<int>.Ok(42);

        var result = ok.Select(x => x.ToString());

        Assert.True(result.IsOk());
        Assert.True(result.IsOk(out var value));
        Assert.Equal("42", value);
    }

    [Fact]
    public void Select_ResultSelector_TransformsAndFlattens()
    {
        var ok = Result<int>.Ok(42);

        var result = ok.Select(x => Result<string>.Ok(x.ToString()));

        Assert.True(result.IsOk());
        Assert.True(result.IsOk(out var value));
        Assert.Equal("42", value);
    }

    [Fact]
    public void Select_ResultSelector_PropagatesInnerError()
    {
        var ok = Result<int>.Ok(42);
        var innerException = new ArgumentException();

        var result = ok.Select(_ => Result<string>.Error(innerException));

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var error));
        Assert.Same(innerException, error);
    }

    [Fact]
    public void Select_ResultSelector_PropagatesOuterError()
    {
        var exception = new InvalidOperationException();
        var error = Result<int>.Error(exception);

        var result = error.Select(x => Result<string>.Ok(x.ToString()));

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var resultError));
        Assert.Same(exception, resultError);
    }

    [Fact]
    public void Select_OptionSelector_ConvertsOptionToResult()
    {
        var ok = Result<int>.Ok(42);

        var result = ok.Select(x => Option<string>.Some(x.ToString()));

        Assert.True(result.IsOk());
        Assert.True(result.IsOk(out var value));
        Assert.Equal("42", value);
    }

    [Fact]
    public void Select_OptionSelector_ConvertsNoneToError()
    {
        var ok = Result<int>.Ok(42);

        var result = ok.Select(_ => Option<string>.None);

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var error));
        Assert.IsType<InvalidOperationException>(error);
    }

    [Fact]
    public void Select_OptionSelector_PropagatesError()
    {
        var exception = new InvalidOperationException();
        var error = Result<int>.Error(exception);

        var result = error.Select(x => Option<string>.Some(x.ToString()));

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var resultError));
        Assert.Same(exception, resultError);
    }
#endregion
#region LINQ SelectMany Tests
    [Fact]
    public void SelectMany_TransformsAndFlattens()
    {
        var ok = Result<int>.Ok(5);

        var result = ok.SelectMany(
            keySelector: x => Result<int>.Ok(x * 2),
            newSelector: (original, multiplied) => $"{original} * 2 = {multiplied}");

        Assert.True(result.IsOk());
        Assert.True(result.IsOk(out var value));
        Assert.Equal("5 * 2 = 10", value);
    }

    [Fact]
    public void SelectMany_PropagatesErrorFromOriginal()
    {
        var exception = new InvalidOperationException();
        var error = Result<int>.Error(exception);

        var result = error.SelectMany(
            keySelector: x => Result<int>.Ok(x * 2),
            newSelector: (original, multiplied) => $"{original} * 2 = {multiplied}");

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var resultError));
        Assert.Same(exception, resultError);
    }

    [Fact]
    public void SelectMany_PropagatesErrorFromKeySelector()
    {
        var ok = Result<int>.Ok(5);
        var innerException = new ArgumentException();

        var result = ok.SelectMany(
            keySelector: _ => Result<int>.Error(innerException),
            newSelector: (original, multiplied) => $"{original} * 2 = {multiplied}");

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var error));
        Assert.Same(innerException, error);
    }

    [Fact]
    public void SelectMany_WithLinqSyntax()
    {
        var result = from x in Result<int>.Ok(5)
            from y in Result<int>.Ok(10)
            select x + y;

        Assert.True(result.IsOk());
        Assert.True(result.IsOk(out var value));
        Assert.Equal(15, value);
    }

    [Fact]
    public void SelectMany_WithLinqSyntax_PropagatesError()
    {
        var exception = new InvalidOperationException();

        var result = from x in Result<int>.Ok(5)
            from y in Result<int>.Error(exception)
            select x + y;

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var error));
        Assert.Same(exception, error);
    }
#endregion
#region Standard LINQ Method Tests
    [Fact]
    public void Where_Ok_FiltersProperly()
    {
        var ok = Result<int>.Ok(42);

        var filtered = ok.Where(x => x > 40);

        // ReSharper disable PossibleMultipleEnumeration
        Assert.Single(filtered);
        Assert.Equal(42, filtered.First());
        // ReSharper restore PossibleMultipleEnumeration
    }

    [Fact]
    public void Where_Ok_FiltersOut()
    {
        var ok = Result<int>.Ok(42);

        var filtered = ok.Where(x => x < 40);

        Assert.Empty(filtered);
    }

    [Fact]
    public void Where_Error_ReturnsEmpty()
    {
        var error = Result<int>.Error(new Exception());

        var filtered = error.Where(_ => true);

        Assert.Empty(filtered);
    }

    [Fact]
    public void Any_Ok_ReturnsTrue()
    {
        var ok = Result<int>.Ok(42);

        Assert.True(ok.Any());
    }

    [Fact]
    public void Any_Error_ReturnsFalse()
    {
        var error = Result<int>.Error(new Exception());

        Assert.False(error.Any());
    }

    [Fact]
    public void First_Ok_ReturnsValue()
    {
        var ok = Result<int>.Ok(42);

        Assert.Equal(42, ok.First());
    }

    [Fact]
    public void First_Error_Throws()
    {
        var error = Result<int>.Error(new Exception());

        Assert.Throws<InvalidOperationException>(() => error.First());
    }

    [Fact]
    public void FirstOrDefault_Ok_ReturnsValue()
    {
        var ok = Result<int>.Ok(42);

        Assert.Equal(42, ok.FirstOrDefault());
    }

    [Fact]
    public void FirstOrDefault_Error_ReturnsDefault()
    {
        var error = Result<int>.Error(new Exception());

        Assert.Equal(0, error.FirstOrDefault());
    }

    [Fact]
    public void ToList_Ok_ContainsValue()
    {
        var ok = Result<int>.Ok(42);

        var list = ok.ToList();

        Assert.Single(list);
        Assert.Equal(42, list[0]);
    }

    [Fact]
    public void ToList_Error_ReturnsEmptyList()
    {
        var error = Result<int>.Error(new Exception());

        var list = error.ToList();

        Assert.Empty(list);
    }

    [Fact]
    public void ToArray_Ok_ContainsValue()
    {
        var ok = Result<int>.Ok(42);

        var array = ok.ToArray();

        Assert.Single(array);
        Assert.Equal(42, array[0]);
    }

    [Fact]
    public void ToArray_Error_ReturnsEmptyArray()
    {
        var error = Result<int>.Error(new Exception());

        var array = error.ToArray();

        Assert.Empty(array);
    }
#endregion
#region Edge Cases and Special Scenarios
    [Fact]
    public void Result_WithNullableValueType_Ok()
    {
        var ok = Result<int?>.Ok(42);

        Assert.True(ok.IsOk());
        Assert.True(ok.IsOk(out var value));
        Assert.Equal(42, value);
    }

    [Fact]
    public void Result_WithNullableValueType_OkWithNull()
    {
        var ok = Result<int?>.Ok(null);

        Assert.True(ok.IsOk());
        Assert.True(ok.IsOk(out var value));
        Assert.Null(value);
    }

    [Fact]
    public void Result_WithReferenceType_Ok()
    {
        var ok = Result<string>.Ok("test");

        Assert.True(ok.IsOk());
        Assert.True(ok.IsOk(out var value));
        Assert.Equal("test", value);
    }

    [Fact]
    public void Result_WithReferenceType_OkWithNull()
    {
        var ok = Result<string>.Ok(null!);

        Assert.True(ok.IsOk());
        Assert.True(ok.IsOk(out var value));
        Assert.Null(value);
    }

    [Fact]
    public void Result_WithComplexType_Ok()
    {
        var person = new Person { Name = "Alice", Age = 30 };
        var ok = Result<Person>.Ok(person);

        Assert.True(ok.IsOk());
        Assert.True(ok.IsOk(out var value));
        Assert.Same(person, value);
    }

    [Fact]
    public void Result_ChainedOperations()
    {
        var result = Result<int>.Ok(5)
            .Select(x => x * 2)
            .Select(x => x + 10)
            .Select(x => x.ToString());

        Assert.True(result.IsOk());
        Assert.True(result.IsOk(out var value));
        Assert.Equal("20", value);
    }

    [Fact]
    public void Result_ChainedOperations_PropagatesError()
    {
        var exception = new InvalidOperationException();
        var result = Result<int>.Error(exception)
            .Select(x => x * 2)
            .Select(x => x + 10)
            .Select(x => x.ToString());

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var error));
        Assert.Same(exception, error);
    }

    [Fact]
    public void Result_ChainedOperations_ErrorInMiddle()
    {
        var exception = new InvalidOperationException();
        var result = Result<int>.Ok(5)
            .Select(x => x * 2)
            .Select(_ => Result<int>.Error(exception))
            .Select(x => x.ToString());

        Assert.True(result.IsError());
        Assert.True(result.IsError(out var error));
        Assert.Same(exception, error);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Result_WithVariousIntValues(int value)
    {
        var ok = Result<int>.Ok(value);

        Assert.True(ok.IsOk());
        Assert.True(ok.IsOk(out var actualValue));
        Assert.Equal(value, actualValue);
    }

    [Theory]
    [InlineData("")]
    [InlineData("test")]
    [InlineData("multi\nline\nstring")]
    public void Result_WithVariousStringValues(string value)
    {
        var ok = Result<string>.Ok(value);

        Assert.True(ok.IsOk());
        Assert.True(ok.IsOk(out var actualValue));
        Assert.Equal(value, actualValue);
    }
#endregion
#region Helper Class
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    [UsedImplicitly]
    private class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
    // ReSharper restore UnusedAutoPropertyAccessor.Local
#endregion
}