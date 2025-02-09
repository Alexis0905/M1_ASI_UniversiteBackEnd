using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Delete;

public class DeleteUniversiteUserUseCase(IRepositoryFactory factory)
{
    public async Task<int> ExecuteAsync(IUniversiteUser universiteUser)
    {
        await CheckBusinessRules(universiteUser);
        await factory.UniversiteUserRepository().DeleteAsync(universiteUser);
        factory.UniversiteUserRepository().SaveChangesAsync().Wait();
        return 0;
    }

    public async Task<int> ExecuteAsync(long id)
    {
        await CheckBusinessRules(id);
        await factory.UniversiteUserRepository().DeleteAsync(id);
        factory.UniversiteUserRepository().SaveChangesAsync().Wait();
        return 0;
    }

    private async Task CheckBusinessRules(IUniversiteUser universiteUser)
    {
        ArgumentNullException.ThrowIfNull(universiteUser);
        ArgumentNullException.ThrowIfNull(factory.UniversiteUserRepository());
    }

    private async Task CheckBusinessRules(long id)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(factory.UniversiteUserRepository());
    }

    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable);
    }
}
