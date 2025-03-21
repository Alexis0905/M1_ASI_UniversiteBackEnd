using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.ParcoursUseCases.Create;
using UniversiteDomain.UseCases.ParcoursUseCases.Add;

namespace UniversiteDomainUnitTests;

public class ParcoursUnitTest
{
	[SetUp]
	public void Setup()
	{
		
	}


	[Test]
	public async Task CreateParcoursUseCase()
	{
		long idParcours = 1;
		String nomParcours = "Parcours 1";
		int anneeFormation = 2;
		
		// On crée le parcours qui doit être ajouté en base
		Parcours parcoursAvant = new Parcours{NomParcours = nomParcours, AnneeFormation = anneeFormation};
		
		// On initialise une fausse datasource qui va simuler un ParcoursRepository
		var mockParcours = new Mock<IRepositoryFactory>();
		
		// Il faut ensuite aller dans le use case pour simuler les appels des fonctions vers la datasource
		// Nous devons simuler FindByCondition et Create
		// On dit à ce mock que le parcours n'existe pas déjà
		mockParcours
			.Setup(repo=>repo.ParcoursRepository().FindByConditionAsync(p=>p.Id.Equals(idParcours)))
			.ReturnsAsync((List<Parcours>)null);

		Parcours parcoursFinal =new Parcours{Id=idParcours,NomParcours= nomParcours, AnneeFormation = anneeFormation};
		mockParcours
			.Setup(repo=>repo.ParcoursRepository().CreateAsync(parcoursAvant))
			.ReturnsAsync(parcoursFinal);
		
		var mockFactory = new Mock<IRepositoryFactory>();
		mockFactory
			.Setup(facto=>facto.ParcoursRepository())
			.Returns(mockParcours.Object.ParcoursRepository);
		
		// Création du use case en utilisant le mock comme datasource
		CreateParcoursUseCase useCase=new CreateParcoursUseCase(mockFactory.Object);
		
		// Appel du use case
		var parcoursTeste=await useCase.ExecuteAsync(parcoursAvant);
		
		// Vérification du résultat
		Assert.That(parcoursTeste.Id, Is.EqualTo(parcoursFinal.Id));
		Assert.That(parcoursTeste.NomParcours, Is.EqualTo(parcoursFinal.NomParcours));
		Assert.That(parcoursTeste.AnneeFormation, Is.EqualTo(parcoursFinal.AnneeFormation));
	}


	[Test]
	public async Task AddEtudiantDansParcoursUseCase()
	{
		long idEtudiant = 1;
		long idParcours = 3;
		Etudiant etudiant= new Etudiant{ Id = idEtudiant, NumEtud = "1", Nom = "nom1", Prenom = "prenom1", Email = "email1" };
		Parcours parcours = new Parcours{ Id = idParcours, NomParcours = "parcours3", AnneeFormation = 1 };
		
		// On initialise des faux repositories
		var mockEtudiant = new Mock<IEtudiantRepository>();
		var mockParcours = new Mock<IParcoursRepository>();
		List<Etudiant> etudiants = new List<Etudiant>();
		etudiants.Add(new Etudiant{Id=1});
		mockEtudiant
			.Setup(repo=>repo.FindByConditionAsync(e=>e.Id.Equals(idEtudiant)))
			.ReturnsAsync(etudiants);
		
		List<Parcours> parcourses = new List<Parcours>();
		parcourses.Add(parcours);
		
		List<Parcours> parcoursFinaux = new List<Parcours>();
		Parcours parcoursFinal = parcours;
		parcoursFinal.Inscrits.Add(etudiant);
		parcoursFinaux.Add(parcours);
		
		mockParcours
			.Setup(repo=>repo.FindByConditionAsync(e=>e.Id.Equals(idParcours)))
			.ReturnsAsync(parcourses);
		mockParcours
			.Setup(repo => repo.AddEtudiantAsync(idParcours, idEtudiant))
			.ReturnsAsync(parcoursFinal);
		
		// Création d'une fausse factory qui contient les faux repositories
		var mockFactory = new Mock<IRepositoryFactory>();
		mockFactory.Setup(facto=>facto.EtudiantRepository()).Returns(mockEtudiant.Object);
		mockFactory.Setup(facto=>facto.ParcoursRepository()).Returns(mockParcours.Object);
		
		// Création du use case en utilisant le mock comme datasource
		AddEtudiantDansParcoursUseCase useCase=new AddEtudiantDansParcoursUseCase(mockFactory.Object);
		
		// Appel du use case
		var parcoursTest=await useCase.ExecuteAsync(idParcours, idEtudiant);
		
		// Vérification du résultat
		Assert.That(parcoursTest.Id, Is.EqualTo(parcoursFinal.Id));
		Assert.That(parcoursTest.Inscrits, Is.Not.Null);
		Assert.That(parcoursTest.Inscrits.Count, Is.EqualTo(1));
		Assert.That(parcoursTest.Inscrits[0].Id, Is.EqualTo(idEtudiant));
	}


	[Test]
	public async Task AddUeDansParcoursUseCase()
	{
		long idUe = 1;
		long idParcours = 3;
		Ue ue = new Ue { Id = 1, NumeroUe = "ue1", Intitule = "Ue Test 123" };
		Parcours parcours = new Parcours{ Id = 3, NomParcours = "Parcours 3", AnneeFormation = 1 };

		// On initialise des faux repositories
		var mockUe = new Mock<IUeRepository>();
		var mockParcours = new Mock<IParcoursRepository>();

		List<Ue> ues = new List<Ue>();
		ues.Add(new Ue{ Id = 1 });
		mockUe
			.Setup(repo=>repo.FindByConditionAsync(u=>u.Id.Equals(idUe)))
			.ReturnsAsync(ues);

		List<Parcours> parcourses = new List<Parcours>();
		parcourses.Add(parcours);

		List<Parcours> parcoursFinaux = new List<Parcours>();
		Parcours parcoursFinal = new Parcours{ Id = 3, NomParcours = "Parcours 3", AnneeFormation = 1 };
		parcoursFinal.UesEnseignees.Add(ue);
		parcoursFinaux.Add(parcours);

		mockParcours
			.Setup(repo=>repo.FindByConditionAsync(p=>p.Id.Equals(idParcours)))
			.ReturnsAsync(parcourses);
		mockParcours
			.Setup(repo => repo.AddUeAsync(idParcours, idUe))
			.ReturnsAsync(parcoursFinal);

		// Création d'une fausse factory qui contient les faux repositories
		var mockFactory = new Mock<IRepositoryFactory>();
		mockFactory.Setup(facto=>facto.UeRepository()).Returns(mockUe.Object);
		mockFactory.Setup(facto=>facto.ParcoursRepository()).Returns(mockParcours.Object);
		
		// Création du use case en utilisant le mock comme datasource
		AddUeDansParcoursUseCase useCase = new AddUeDansParcoursUseCase(mockFactory.Object);
		
		// Appel du use case
		var parcoursTest = await useCase.ExecuteAsync(idParcours, idUe);
		
		// Vérification du résultat
		Assert.That(parcoursTest.Id, Is.EqualTo(parcoursFinal.Id));
		Assert.That(parcoursTest.UesEnseignees, Is.Not.Null);
		Assert.That(parcoursTest.UesEnseignees.Count, Is.EqualTo(1));
		Assert.That(parcoursTest.UesEnseignees[0].Id, Is.EqualTo(idUe));
	}
}
