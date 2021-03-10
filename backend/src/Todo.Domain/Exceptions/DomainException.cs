﻿using System;

namespace Todo.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
