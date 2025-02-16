using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.NoteUseCases.Add;

namespace UniversiteDomainUnitTests;

public class NoteUnitTests
{
    [SetUp]
    public void Setup()
    {

    }


    [Test]
    public async Task AddNoteUseCase()
    {
        float valeur = 14.5f;
        long idEtud = 1;
        long idUe = 2;
        Etudiant etudiant= new Etudiant{ Id = idEtud, NumEtud = "1", Nom = "nom1", Prenom = "prenom1", Email = "email1" };
        Ue ue = new Ue { Id = idUe, NumeroUe = "ue2", Intitule = "Ue Test 123" };
        Note note = new Note{ IdEtud = idEtud, IdUe = idUe, Valeur = valeur, Etudiant = etudiant, Ue = ue };

        var mockEtudiant = new Mock<IEtudiantRepository>();
        var mockUe = new Mock<IUeRepository>();
        var mockNote = new Mock<INoteRepository>();

        List<Etudiant> listEtudiants = new List<Etudiant>();
        listEtudiants.Add(new Etudiant{ Id = idEtud });
        mockEtudiant
            .Setup(repo => repo.FindByConditionAsync(etud => etud.Id.Equals(idEtud)))
            .ReturnsAsync(listEtudiants);

        List<Ue> listUes = new List<Ue>();
        listUes.Add(new Ue{ Id = idUe });
        mockUe
            .Setup(repo => repo.FindByConditionAsync(ue => ue.Id.Equals(idUe)))
            .ReturnsAsync(listUes);

        mockNote
            .Setup(repo => repo.AddNoteAsync(valeur, idEtud, idUe))
            .ReturnsAsync(note);

        var mockFactory = new Mock<IRepositoryFactory>();
        mockFactory.Setup(facto=>facto.EtudiantRepository()).Returns(mockEtudiant.Object);
        mockFactory.Setup(facto=>facto.UeRepository()).Returns(mockUe.Object);
        mockFactory.Setup(facto=>facto.NoteRepository()).Returns(mockNote.Object);

        AddNoteUseCase addNoteUseCase = new AddNoteUseCase(mockFactory.Object);

        var noteTest = await addNoteUseCase.ExecuteAsync(valeur, idEtud, idUe);

        Assert.That(noteTest.Etudiant.Id, Is.EqualTo(idEtud));
        Assert.That(noteTest.Ue.Id, Is.EqualTo(idUe));
        Assert.That(noteTest.Valeur, Is.EqualTo(valeur));
    }
}
