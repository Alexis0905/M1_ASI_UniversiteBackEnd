namespace UniversiteDomain.Entities;

public interface IUniversiteUser
{
    long ? IdEtud { get; set; }
    Etudiant? Etudiant { get; set; }
}
