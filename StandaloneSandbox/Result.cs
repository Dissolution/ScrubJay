namespace StandaloneSandbox
{
    public struct Result<T>
    {
        public static implicit operator Result<T>(T value) => throw new NotImplementedException();
        public static implicit operator Result<T>(Exception ex) => throw new NotImplementedException();
    
        public static Result<T> Ok(T value) => throw new NotImplementedException();
        public static Result<T> Error(Exception ex) => throw new NotImplementedException();
    }
}