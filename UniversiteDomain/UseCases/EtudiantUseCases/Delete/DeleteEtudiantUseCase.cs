using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Delete;

public class DeleteEtudiantUseCase(IRepositoryFactory factory)
{
	public async Task<int> ExecuteAsync(long id)
	{
		await CheckBusinessRules();
		var etudiant = await factory.EtudiantRepository().FindAsync(id);

		if (etudiant == null) throw new EtudiantNotFoundException();

		var notes = await factory.NoteRepository().FindByConditionAsync(n => n.IdEtud == id);
		foreach (var note in notes)
		{
			await factory.NoteRepository().DeleteAsync(note);
		}
		await factory.EtudiantRepository().DeleteAsync(id);

		await factory.EtudiantRepository().SaveChangesAsync();
		return 0;
	}

	private async Task CheckBusinessRules()
	{
		ArgumentNullException.ThrowIfNull(factory);
		IEtudiantRepository etudiantRepository=factory.EtudiantRepository();
		ArgumentNullException.ThrowIfNull(etudiantRepository);
	}

	public bool IsAuthorized(string role)
	{
		return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
	}
}
