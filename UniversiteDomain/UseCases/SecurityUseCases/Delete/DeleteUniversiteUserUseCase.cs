using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;

namespace UniversiteDomain.UseCases.SecurityUseCases.Delete;

public class DeleteUniversiteUserUseCase(IRepositoryFactory factory)
{
    public async Task<int> ExecuteAsync(long id)
    {
        await CheckBusinessRules();
        var user = await factory.UniversiteUserRepository().FindByIdEtudAsync(id);

        if (user == null) throw new Exception("User not found");

        await factory.UniversiteUserRepository().DeleteAsync(id);
        await factory.UniversiteUserRepository().SaveChangesAsync();
        return 0;
    }

    private async Task CheckBusinessRules()
    {
        ArgumentNullException.ThrowIfNull(factory);
        IUniversiteUserRepository universiteUserRepository = factory.UniversiteUserRepository();
        ArgumentNullException.ThrowIfNull(universiteUserRepository);
    }

    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}
