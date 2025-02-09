using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;
using UniversiteDomain.UseCases.NoteUseCases.Add;
using UniversiteDomain.UseCases.ParcoursUseCases.Create;
using UniversiteDomain.UseCases.ParcoursUseCases.Add;
using UniversiteDomain.UseCases.UeUseCases.Create;
using UniversiteDomain.UseCases.SecurityUseCases.Create;

namespace UniversiteDomain.JeuxDeDonnees;

public class BasicBdBuilder : BdBuilder
{
    private readonly IRepositoryFactory _repositoryFactory;

    // Le constructeur pour injecter IRepositoryFactory
    public BasicBdBuilder(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    private readonly string Password = "Miage2025#";

    private readonly Etudiant[] _etudiants =
    [
        new Etudiant { Id=1, NumEtud = "03BDKZ65", Nom = "Dupont", Prenom = "Antoine", Email = "antoine.dupont@etud.u-picardie.fr" },
        new Etudiant { Id=2, NumEtud = "JEIZ03JZ", Nom = "Ntamak", Prenom = "Romain", Email = "roman.ntamak@etud.u-picardie.fr" },
        new Etudiant { Id=3, NumEtud = "62830483", Nom = "Barassi", Prenom = "Pierre-Louis", Email = "pierre-louis.barassi@etud.u-picardie.fr" },
        new Etudiant { Id=4, NumEtud = "J6HZK922", Nom = "Jelong", Prenom = "Antony", Email = "antony.jelong@etud.u-picardie.fr" },
        new Etudiant { Id=5, NumEtud = "PAD89345", Nom = "Akki", Prenom = "Pita", Email = "pita.akki@etud.u-picardie.fr" },
        new Etudiant { Id=6, NumEtud = "RG8647FF", Nom = "Mauvaka", Prenom = "Peato", Email = "peato.mauvaka@etud.u-picardie.fr" }
    ];

    private struct UserNonEtudiant
    {
        public string UserName;
        public string Email;
        public string Role;
    }

    private readonly UserNonEtudiant[] _usersNonEtudiants =
    [
        new UserNonEtudiant { UserName = "anne.lapujade@u-picardie.fr", Email = "anne.lapujade@u-picardie.fr", Role = "Responsable" },
        new UserNonEtudiant { UserName = "plouisberquez@gmail.com", Email = "plouisberquez@gmail.com", Role = "Responsable" },
        new UserNonEtudiant { UserName = "jabin.julian.univ@gmail.com", Email = "jabin.julian.univ@gmail.com", Role = "Responsable" },
        new UserNonEtudiant { UserName = "mehdy.chk@outlook.fr", Email = "mehdy.chk@outlook.fr", Role = "Responsable" },
        new UserNonEtudiant { UserName = "stephanie.dertin@u-picardie.fr", Email = "stephanie.dertin@u-picardie.fr", Role = "Scolarite" }
    ];

    private readonly Parcours[] _parcours =
    [
        new Parcours { Id=1, NomParcours = "Master M1", AnneeFormation = 1 },
        new Parcours { Id=2, NomParcours = "OSIE", AnneeFormation = 2 },
        new Parcours { Id=3, NomParcours = "ITD", AnneeFormation = 2 },
        new Parcours { Id=4, NomParcours = "IDD", AnneeFormation = 2 }
    ];

    private readonly Ue[] _ues =
    [
        new Ue { Id=1, NumeroUe = "ISI_01", Intitule = "Architecture des SI 1" },
        new Ue { Id=2, NumeroUe = "ISI_02", Intitule = "Conduite de projet" },
        new Ue { Id=3, NumeroUe = "GEO_05", Intitule = "Marketing" },
        new Ue { Id=4, NumeroUe = "INFO_18", Intitule = "Architecture des SI 2" }
    ];

    private struct Inscription
    {
        public long IdEtud;
        public long IdParcours;
    }

    private readonly Inscription[] _inscriptions =
    [
        // IdEtud, ParcoursId
        new Inscription { IdEtud = 1, IdParcours = 2 },
        new Inscription { IdEtud = 2, IdParcours = 1 },
        new Inscription { IdEtud = 3, IdParcours = 1 },
        new Inscription { IdEtud = 4, IdParcours = 1 },
        new Inscription { IdEtud = 5, IdParcours = 3 },
        new Inscription { IdEtud = 6, IdParcours = 4 }
    ];

    private struct UeDansParcours
    {
        public long IdUe;
        public long IdParcours;
    }

    private readonly UeDansParcours[] _maquette =
    [
        new UeDansParcours { IdUe = 1, IdParcours = 1 },
        new UeDansParcours { IdUe = 2, IdParcours = 1 },
        new UeDansParcours { IdUe = 3, IdParcours = 1 },
        new UeDansParcours { IdUe = 4, IdParcours = 2 },
        new UeDansParcours { IdUe = 4, IdParcours = 3 },
        new UeDansParcours { IdUe = 4, IdParcours = 4 }
    ];

    private struct Note
    {
        public long IdEtud;
        public long IdUe;
        public float Valeur;
    }

    private readonly Note[] _notes =
    [
        new Note { IdUe = 1, IdEtud = 2, Valeur = 12 },
        new Note { IdUe = 1, IdEtud = 3, Valeur = (float)8.5 },
        new Note { IdUe = 1, IdEtud = 4, Valeur = 16 },
        new Note { IdUe = 2, IdEtud = 2, Valeur = 14 },
        new Note { IdUe = 2, IdEtud = 3, Valeur = 6 },
        new Note { IdUe = 3, IdEtud = 4, Valeur = (float)11.5 },
        new Note { IdUe = 4, IdEtud = 1, Valeur = 10 },
        new Note { IdUe = 4, IdEtud = 5, Valeur = (float)18.3 },
        new Note { IdUe = 4, IdEtud = 6, Valeur = 12 }
    ];

    protected override async Task RegenererBdAsync()
    {
        // Ici je décide de supprimer et recréer la BD
        await _repositoryFactory.EnsureDeletedAsync();
        await _repositoryFactory.EnsureCreatedAsync();
    }

    protected override async Task BuildEtudiantsAsync()
    {
        var etudiantRepository = _repositoryFactory.EtudiantRepository();
        foreach (Etudiant e in _etudiants)
        {
            await new CreateEtudiantUseCase(etudiantRepository).ExecuteAsync(e);
        }
    }

    protected override async Task BuildParcoursAsync()
    {
        var parcoursRepository = _repositoryFactory.ParcoursRepository();
        foreach (Parcours parcours in _parcours)
        {
            await new CreateParcoursUseCase(parcoursRepository).ExecuteAsync(parcours);
        }
    }

    protected override async Task BuildUesAsync()
    {
        var ueRepository = _repositoryFactory.UeRepository();
        foreach (Ue ue in _ues)
        {
            await new CreateUeUseCase(ueRepository).ExecuteAsync(ue);
        }
    }

    protected override async Task InscrireEtudiantsAsync()
    {
        foreach (Inscription i in _inscriptions)
        {
            await new AddEtudiantDansParcoursUseCase(_repositoryFactory).ExecuteAsync(i.IdParcours,i.IdEtud);
        }
    }

    protected override async Task BuildMaquetteAsync()
    {
        foreach(UeDansParcours u in _maquette)
        {
            await new AddUeDansParcoursUseCase(_repositoryFactory).ExecuteAsync(u.IdParcours, u.IdUe);
        }
    }

    protected override async Task NoterAsync()
    {
        foreach(var note in _notes)
        {
            await new AddNoteUseCase(_repositoryFactory).ExecuteAsync(note.Valeur, note.IdEtud, note.IdUe);
        }
    }

    protected override async Task BuildRolesAsync()
    {
        // Création des rôles dans la table aspnetroles
        await new CreateUniversiteRoleUseCase(_repositoryFactory).ExecuteAsync(Roles.Responsable);
        await new CreateUniversiteRoleUseCase(_repositoryFactory).ExecuteAsync(Roles.Scolarite);
        await new CreateUniversiteRoleUseCase(_repositoryFactory).ExecuteAsync(Roles.Etudiant);
    }

    protected override async Task BuildUsersAsync()
    {
        CreateUniversiteUserUseCase uc = new CreateUniversiteUserUseCase(_repositoryFactory);
        // Création des étudiants
        foreach (var etudiant in _etudiants)
        {
            await uc.ExecuteAsync(etudiant.Email, etudiant.Email, this.Password, Roles.Etudiant,etudiant);
        }

        // Création des responsbles
        foreach (var user in _usersNonEtudiants)
        {
            await uc.ExecuteAsync(user.Email, user.Email, this.Password, user.Role, null);
        }
    }
}
