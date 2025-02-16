using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Create;

public class CreateParcoursUseCase(IRepositoryFactory factory)
{
	public async Task<Parcours> ExecuteAsync(string nomParcours, int anneeFormation)
	{
		var parcours = new Parcours{NomParcours = nomParcours, AnneeFormation = anneeFormation};
		return await ExecuteAsync(parcours);
	}
	
	public async Task<Parcours> ExecuteAsync(Parcours parcours)
	{
		await CheckBusinessRules(parcours);
		Parcours pa = await factory.ParcoursRepository().CreateAsync(parcours);
		await factory.ParcoursRepository().SaveChangesAsync();
		return pa;
	}
	
	private async Task CheckBusinessRules(Parcours parcours)
	{
		ArgumentNullException.ThrowIfNull(parcours);
		ArgumentNullException.ThrowIfNull(parcours.NomParcours);
		ArgumentNullException.ThrowIfNull(parcours.AnneeFormation);
		
		if (parcours.NomParcours.Length < 3) throw new InvalidNomParcoursException(parcours.NomParcours + " incorrect - Le nom d'un parcours doit contenir plus de 3 caractères");
		
		if (parcours.AnneeFormation != 1 && parcours.AnneeFormation != 2) throw new InvalidAnneeFormationException(parcours.AnneeFormation + " incorrect - L'année de formation d'un parcours doit être 1 ou 2");
	}

	public bool IsAuthorized(string role)
	{
		return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
	}
}
