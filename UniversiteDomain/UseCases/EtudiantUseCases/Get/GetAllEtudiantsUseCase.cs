using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Get;

public class GetAllEtudiantsUseCase(IRepositoryFactory factory)
{
    public async Task<List<Etudiant>> ExecuteAsync()
    {
        return await factory.EtudiantRepository().FindAllAsync();
    }

    private async Task CheckBusinessRules()
    {
        ArgumentNullException.ThrowIfNull(factory);
        IEtudiantRepository etudiantRepository=factory.EtudiantRepository();
        ArgumentNullException.ThrowIfNull(etudiantRepository);
    }

    public bool IsAuthorized(string role)
    {
        return true;
    }
}
