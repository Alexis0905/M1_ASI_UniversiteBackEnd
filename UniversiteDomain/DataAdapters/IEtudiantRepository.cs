using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface IEtudiantRepository : IRepository<Etudiant>
{
	public Task<Etudiant?> AddEtudiantAsync(Etudiant etudiant, Parcours parcours);
	public Task<Etudiant?> AddEtudiantAsync(long idEtudiant, long idParcours);
	public Task<Etudiant?> FindEtudiantCompletAsync(long idEtudiant);
}
