using UniversiteDomain.Entities;

namespace UniversiteDomain.Dtos;

public class EtudiantCompletDto
{
    public long Id { get; set; }
    public string NumEtud { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    public ParcoursDto ParcoursSuivi { get; set; }
    public List<NoteAvecUeDto> Notes { get; set; } = new();

    public EtudiantCompletDto ToDto(Etudiant etudiant)
    {
        Id = etudiant.Id;
        NumEtud = etudiant.NumEtud;
        Nom = etudiant.Nom;
        Prenom = etudiant.Prenom;
        Email = etudiant.Email;
        if (etudiant.ParcoursSuivi != null) ParcoursSuivi = new ParcoursDto().ToDto(etudiant.ParcoursSuivi);
        if (etudiant.Notes != null)
        {
            Notes = NoteAvecUeDto.ToDtos(etudiant.Notes);
        }
        
        return this;
    }
    
    public Etudiant ToEntity()
    {
        List<Note> notes = NoteAvecUeDto.ToEntities(Notes);
        return new Etudiant
        {
            Id = this.Id,
            NumEtud = this.NumEtud,
            Nom = this.Nom,
            Prenom = this.Prenom,
            Email = this.Email,
            Notes = notes
        };
    }
}
