// See https://aka.ms/new-console-template for more information

using System;
using InlineIL;
using static InlineIL.IL;




Console.WriteLine(new string('~', 80));
Console.WriteLine("Press Enter to exit");
Console.ReadLine();
return;


public struct Result<T>
{
    public static implicit operator Result<T>(T value) => throw new NotImplementedException();
    public static implicit operator Result<T>(Exception ex) => throw new NotImplementedException();
    
    public static Result<T> Ok(T value) => throw new NotImplementedException();
    public static Result<T> Error(Exception ex) => throw new NotImplementedException();
}