namespace UniversiteDomain.Exceptions.NoteExceptions;

public class NoteNotFoundException : Exception
{
	public NoteNotFoundException() : base() { }
	public NoteNotFoundException(string message) : base(message) { }
	public NoteNotFoundException(string message, Exception inner) : base(message, inner) { }
}
