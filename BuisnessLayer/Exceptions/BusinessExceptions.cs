using System;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}

public class NotFoundException : BusinessException
{
    public NotFoundException(string message) : base(message) { }
} 