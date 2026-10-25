using ScrubJay.Functional;


List<int> numbers = [1, 4, 7];
var result = Result<int, Exception>.Ok(147);

var output =
    from x in result
    from n in numbers
    select x + n;








Console.WriteLine("Hello, World!");