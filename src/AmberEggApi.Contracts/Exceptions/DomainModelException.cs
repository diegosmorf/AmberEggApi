using System;

namespace AmberEggApi.Contracts.Exceptions;

public class DomainModelException(string message) : Exception(message)
{    
}