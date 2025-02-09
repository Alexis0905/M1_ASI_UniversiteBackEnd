using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class ParcoursRepository(UniversiteDbContext context) : Repository<Parcours>(context), IParcoursRepository
{
    public async Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Etudiants);

        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;

        p.Inscrits.Add(e);

        await Context.SaveChangesAsync();
        return p;
    }

    public async Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant)
    {
        await AddEtudiantAsync(parcours.Id, etudiant.Id);
        return parcours;
    }

    public async Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Etudiants);

        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        Etudiant es = (await Context.Etudiants.FindAsync(idEtudiants))!;

        p.Inscrits.Add(es);

        await Context.SaveChangesAsync();
        return p;
    }

    public async Task<Parcours> AddEtudiantAsync(Parcours parcours, List<Etudiant> etudiants)
    {
        await AddEtudiantAsync(parcours.Id, etudiants.Select(e => e.Id).ToArray());
        return parcours;
    }

    public async Task<Parcours> AddUeAsync(long idParcours, long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Ues);

        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        Ue u = (await Context.Ues.FindAsync(idUe))!;

        p.UesEnseignees.Add(u);

        await Context.SaveChangesAsync();
        return p;
    }

    public async Task<Parcours> AddUeAsync(long idParcours, long[] idUes)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Ues);

        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        Ue ues = (await Context.Ues.FindAsync(idUes))!;

        p.UesEnseignees.Add(ues);

        await Context.SaveChangesAsync();
        return p;
    }
}
