using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Create;

public class CreateParcoursUseCase(IParcoursRepository parcoursRepository)
{
	public async Task<Parcours> ExecuteAsync(string nomParcours, int anneeFormation)
	{
		var parcours = new Parcours{NomParcours = nomParcours, AnneeFormation = anneeFormation};
		return await ExecuteAsync(parcours);
	}
	
	public async Task<Parcours> ExecuteAsync(Parcours parcours)
	{
		await CheckBusinessRules(parcours);
		Parcours pa = await parcoursRepository.CreateAsync(parcours);
		parcoursRepository.SaveChangesAsync().Wait();
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
		return role.Equals(Roles.Responsable);
	}
}
