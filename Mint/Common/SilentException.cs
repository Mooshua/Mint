using System;

namespace Mint.Common;

public class SilentException : Exception
{
	public SilentException(string message) : base(message)
	{
	}
}