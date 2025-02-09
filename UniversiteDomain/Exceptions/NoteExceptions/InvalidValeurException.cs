namespace UniversiteDomain.Exceptions.NoteExceptions;

public class InvalidValeurException : Exception
{
	public InvalidValeurException() : base() { }
	public InvalidValeurException(string message) : base(message) { }
	public InvalidValeurException(string message, Exception inner) : base(message, inner) { }
}
