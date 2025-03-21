using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface IParcoursRepository : IRepository<Parcours>
{
	Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant);
	Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant);

	Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants);
	Task<Parcours> AddEtudiantAsync(Parcours parcours, List<Etudiant> etudiants);

	Task<Parcours> AddUeAsync(long idParcours, long idUe);

	Task<Parcours> AddUeAsync(long idParcours, long[] idUes);
}
