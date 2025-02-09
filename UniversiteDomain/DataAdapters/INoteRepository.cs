using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface INoteRepository : IRepository<Note>
{
	Task<Note> AddNoteAsync(float valeur, long idEtud, long idUe);
}
