using Microsoft.EntityFrameworkCore;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class EtudiantRepository(UniversiteDbContext context) : Repository<Etudiant>(context), IEtudiantRepository
{
    public async Task<Etudiant?> AddEtudiantAsync(long idEtudiant, long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Parcours);

        Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;

        e.ParcoursSuivi = p;

        await Context.SaveChangesAsync();
        return e;
    }

    public async Task<Etudiant?> FindEtudiantCompletAsync(long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        return await Context.Etudiants.Include(e => e.Notes).ThenInclude(n=>n.Ue).FirstOrDefaultAsync(e => e.Id == idEtudiant);
    }

    public async Task<Etudiant?> AddEtudiantAsync(Etudiant etudiant, Parcours parcours)
    {
        await AddEtudiantAsync(etudiant.Id, parcours.Id);
        return etudiant;
    }
}
