using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.SecurityUseCases.Update;

public class UpdateUniversiteUserUseCase(IRepositoryFactory factory)
{
    public async Task<int> ExecuteAsync(Etudiant etudiant)
    {
        await CheckBusinessRules(etudiant);
        var user = await factory.UniversiteUserRepository().FindByIdEtudAsync(etudiant.Id);

        if (user == null) throw new Exception("User not found");

        await factory.UniversiteUserRepository().UpdateAsync(user, etudiant.Email, etudiant.Email);
        await factory.UniversiteUserRepository().SaveChangesAsync();
        return 0;
    }

    private async Task CheckBusinessRules(Etudiant etudiant)
    {
        ArgumentNullException.ThrowIfNull(etudiant);
        ArgumentNullException.ThrowIfNull(factory);
        IUniversiteUserRepository universiteUserRepository=factory.UniversiteUserRepository();
        ArgumentNullException.ThrowIfNull(universiteUserRepository);
    }

    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}
