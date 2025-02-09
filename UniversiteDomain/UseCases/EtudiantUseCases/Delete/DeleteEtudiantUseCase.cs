using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Delete;

public class DeleteEtudiantUseCase(IRepositoryFactory factory)
{
	public async Task<int> ExecuteAsync(Etudiant etudiant)
	{
		await CheckBusinessRules(etudiant);
		await factory.EtudiantRepository().DeleteAsync(etudiant);
		factory.EtudiantRepository().SaveChangesAsync().Wait();
		return 0;
	}

	public async Task<int> ExecuteAsync(long id)
	{
		await CheckBusinessRules(await factory.EtudiantRepository().FindAsync(id));
		await factory.EtudiantRepository().DeleteAsync(id);
		factory.SaveChangesAsync().Wait();
		return 0;
	}

	private async Task CheckBusinessRules(Etudiant etudiant)
	{
		ArgumentNullException.ThrowIfNull(etudiant);
		ArgumentNullException.ThrowIfNull(factory.EtudiantRepository());
	}

	public bool IsAuthorized(string role)
	{
		return role.Equals(Roles.Responsable);
	}
}
