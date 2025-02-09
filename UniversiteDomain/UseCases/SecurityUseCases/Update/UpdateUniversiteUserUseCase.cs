using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.SecurityUseCases.Update;

public class UpdateUniversiteUserUseCase(IRepositoryFactory factory)
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
