using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NoteExceptions;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases.Add;

public class AddNoteUseCase(IRepositoryFactory repositoryFactory)
{
	public async Task<Note> ExecuteAsync(float valeur, Etudiant etudiant, Ue ue)
	{
		ArgumentNullException.ThrowIfNull(valeur);
		ArgumentNullException.ThrowIfNull(etudiant);
		ArgumentNullException.ThrowIfNull(ue);
		return await ExecuteAsync(valeur, etudiant.Id, ue.Id);
	}
	
	public async Task<Note> ExecuteAsync(float valeur, long idEtud, long idUe)
	{
		await CheckBusinessRules(valeur, idEtud, idUe);
		return await repositoryFactory.NoteRepository().AddNoteAsync(valeur, idEtud, idUe);
	}
	
	private async Task CheckBusinessRules(float valeur, long idEtud, long idUe)
	{
		// Vérification des paramètres
		ArgumentNullException.ThrowIfNull(valeur);
		ArgumentNullException.ThrowIfNull(idEtud);
		ArgumentNullException.ThrowIfNull(idUe);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(valeur);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idEtud);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idUe);
		
		// Vérifions tout d'abord que nous sommes bien connectés aux datasources
		ArgumentNullException.ThrowIfNull(repositoryFactory);
		ArgumentNullException.ThrowIfNull(repositoryFactory.NoteRepository());
		ArgumentNullException.ThrowIfNull(repositoryFactory.EtudiantRepository());
		ArgumentNullException.ThrowIfNull(repositoryFactory.UeRepository());
		
		// On recherche une note avec les mêmes numéros étudiant et ue
		// Si une note avec les mêmes numéros étudiant et ue existe déjà, on lève une exception personnalisée
		List<Note> noteExiste = await repositoryFactory.NoteRepository().FindByConditionAsync(n=> n.IdEtud.Equals(idEtud) && n.IdUe.Equals(idUe));
		if (noteExiste is { Count: >0 }) throw new DuplicateNoteException("Une note est déjà attribuée à l'étudiant " + idEtud + " pour l'UE " + idUe);
		
		// Une note est comprise entre 0 et 20
		if (valeur < 0 || valeur > 20) throw new InvalidValeurException(valeur +" incorrect - La valeur d'une note doit être comprise entre 0 et 20");
		
		// Si une note veut être ajouter à une ue que l'étudiant ne suit pas
		// On recherche l'étudiant
		List<Etudiant> etudiant = await repositoryFactory.EtudiantRepository().FindByConditionAsync(e => e.Id.Equals(idEtud));
		if (etudiant is { Count: 0 }) throw new EtudiantNotFoundException(idEtud.ToString());
		// On recherche les ues enseignées dans le parcours suivi par cet étudiant
		if (etudiant[0].ParcoursSuivi.UesEnseignees != null)
		{
			List<Ue> uesSuivies = etudiant[0].ParcoursSuivi.UesEnseignees;
			var ueTrouve = uesSuivies.FindAll(u => u.Id.Equals(idUe));
			if (ueTrouve is { Count: 0 }) throw new UeNotFoundException(idUe.ToString());
		}
	}

	public bool IsAuthorized(string role)
	{
		return role.Equals(Roles.Scolarite);
	}
}
