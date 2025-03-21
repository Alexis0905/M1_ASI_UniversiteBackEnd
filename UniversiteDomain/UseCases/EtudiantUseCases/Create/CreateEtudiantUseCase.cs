using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Create;

public class CreateEtudiantUseCase(IRepositoryFactory factory)
{
	public async Task<Etudiant> ExecuteAsync(string numEtud, string nom, string prenom, string email)
	{
		var etudiant = new Etudiant{NumEtud = numEtud, Nom = nom, Prenom = prenom, Email = email};
		return await ExecuteAsync(etudiant);
	}

	public async Task<Etudiant> ExecuteAsync(Etudiant etudiant)
	{
		await CheckBusinessRules(etudiant);
		Etudiant et = await factory.EtudiantRepository().CreateAsync(etudiant);
		await factory.EtudiantRepository().SaveChangesAsync();
		return et;
	}

	private async Task CheckBusinessRules(Etudiant etudiant)
	{
		ArgumentNullException.ThrowIfNull(etudiant);
		ArgumentNullException.ThrowIfNull(etudiant.NumEtud);
		ArgumentNullException.ThrowIfNull(etudiant.Email);
		ArgumentNullException.ThrowIfNull(factory.EtudiantRepository());

		// On recherche un étudiant avec le même numéro étudiant
		List<Etudiant> existe = await factory.EtudiantRepository().FindByConditionAsync(e=>e.NumEtud.Equals(etudiant.NumEtud));

		// Si un étudiant avec le même numéro étudiant existe déjà, on lève une exception personnalisée
		if (existe .Any()) throw new DuplicateNumEtudException(etudiant.NumEtud+ " - ce numéro d'étudiant est déjà affecté à un étudiant");

		// Vérification du format du mail
		if (!CheckEmail.IsValidEmail(etudiant.Email)) throw new InvalidEmailException(etudiant.Email + " - Email mal formé");

		// On vérifie si l'email est déjà utilisé
		existe = await factory.EtudiantRepository().FindByConditionAsync(e=>e.Email.Equals(etudiant.Email));
		
		// Une autre façon de tester la vacuité de la liste
		if (existe is {Count:>0}) throw new DuplicateEmailException(etudiant.Email +" est déjà affecté à un étudiant");
		
		// Le métier définit que les noms doivent contenir plus de 3 lettres
		if (etudiant.Nom.Length < 3) throw new InvalidNomEtudiantException(etudiant.Nom +" incorrect - Le nom d'un étudiant doit contenir plus de 3 caractères");
	}

	public bool IsAuthorized(string role)
	{
		return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
	}
}
