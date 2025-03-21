using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.UeUseCases.Create;

public class CreateUeUseCase(IRepositoryFactory factory)
{
	public async Task<Ue> ExecuteAsync(string numeroUe, string intitule)
	{
		var ue = new Ue{NumeroUe = numeroUe, Intitule = intitule};
		return await ExecuteAsync(ue);
	}
	
	public async Task<Ue> ExecuteAsync(Ue ue)
	{
		await CheckBusinessRules(ue);
		Ue u = await factory.UeRepository().CreateAsync(ue);
		factory.UeRepository().SaveChangesAsync().Wait();
		return u;
	}
	
	private async Task CheckBusinessRules(Ue ue)
	{
		ArgumentNullException.ThrowIfNull(ue);
		ArgumentNullException.ThrowIfNull(ue.NumeroUe);
		ArgumentNullException.ThrowIfNull(ue.Intitule);
		ArgumentNullException.ThrowIfNull(factory.UeRepository());
		
		// On recherche une ue avec le même numéro ue
		List<Ue> existe = await factory.UeRepository().FindByConditionAsync(u=>u.NumeroUe.Equals(ue.NumeroUe));
		
		// Si une ue avec le même numéro ue existe déjà, on lève une exception personnalisée
		if (existe .Any()) throw new DuplicateNumeroUeException(ue.NumeroUe+ " - ce numéro d'ue est déjà affecté à une ue");
		
		if (ue.Intitule.Length < 3) throw new InvalidIntituleException(ue.Intitule + " incorrect - Le nom d'une Ue doit contenir plus de 3 caractères");
	}

	public bool IsAuthorized(string role)
	{
		return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
	}
}
