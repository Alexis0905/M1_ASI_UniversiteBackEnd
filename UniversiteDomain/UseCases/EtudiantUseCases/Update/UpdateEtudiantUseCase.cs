using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Update;

public class UpdateEtudiantUseCase(IRepositoryFactory factory)
{
    public async Task<int> ExecuteAsync(Etudiant etudiant)
    {
        await factory.EtudiantRepository().UpdateAsync(etudiant);
        return 0;
    }

    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable);
    }
}
