using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class UeRepository(UniversiteDbContext context) : Repository<Ue>(context), IUeRepository
{
    public async Task<Ue> AddUeAsync(long idUe, long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Ues);
        ArgumentNullException.ThrowIfNull(Context.Parcours);

        Ue u = (await Context.Ues.FindAsync(idUe))!;
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;

        u.EnseigneeDans.Add(p);

        await Context.SaveChangesAsync();
        return u;
    }
}
