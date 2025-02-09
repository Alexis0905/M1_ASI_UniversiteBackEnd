using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class NoteRepository(UniversiteDbContext context) : Repository<Note>(context), INoteRepository
{
    public async Task<Note> AddNoteAsync(float valeur, long idEtud, long idUe)
    {
        // Vérifiez que les DbSet sont initialisés
        ArgumentNullException.ThrowIfNull(Context.Notes, nameof(Context.Notes));
        ArgumentNullException.ThrowIfNull(Context.Etudiants, nameof(Context.Etudiants));
        ArgumentNullException.ThrowIfNull(Context.Ues, nameof(Context.Ues));

        // Recherchez la note existante
        var n = await Context.Notes.FindAsync(idEtud, idUe);
        if (n == null)
        {
            // Créez une nouvelle note seulement si elle n'existe pas
            n = new Note { Valeur = valeur, IdEtud = idEtud, IdUe = idUe };
            Context.Notes.Add(n);
        }

        // Recherchez ou vérifiez les entités liées
        var e = await Context.Etudiants.FindAsync(idEtud);
        if (e == null)
            throw new ArgumentException($"Étudiant introuvable avec l'ID {idEtud}");

        var u = await Context.Ues.FindAsync(idUe);
        if (u == null)
            throw new ArgumentException($"UE introuvable avec l'ID {idUe}");

        // Reliez les entités uniquement si nécessaire
        n.Etudiant ??= e;
        n.Ue ??= u;

        // Sauvegardez les changements
        await Context.SaveChangesAsync();

        return n;
    }
}
