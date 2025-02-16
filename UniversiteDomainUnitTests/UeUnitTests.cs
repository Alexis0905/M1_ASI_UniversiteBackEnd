using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.UeUseCases.Create;

namespace UniversiteDomainUnitTests;

public class UeUnitTests
{
	[SetUp]
	public void Setup()
	{
		
	}


	[Test]
	public async Task CreateUeUseCase()
	{
		long idUe = 1;
		string numeroUe = "ue1";
		string intitule = "Ue test 123";
		
		Ue ueSansId = new Ue { NumeroUe = numeroUe, Intitule = intitule };
		var mock = new Mock<IRepositoryFactory>();
		var reponseFindByCondition = new List<Ue>();
		
		mock
			.Setup(repo=>repo.UeRepository().FindByConditionAsync(It.IsAny<Expression<Func<Ue, bool>>>()))
			.ReturnsAsync(reponseFindByCondition);
			
		Ue ueCreee = new Ue{Id=idUe, NumeroUe= numeroUe, Intitule = intitule};
		mock
			.Setup(repo=>repo.UeRepository().CreateAsync(ueSansId))
			.ReturnsAsync(ueCreee);
		
		IRepositoryFactory fauxUeRepository = mock.Object;
		
		CreateUeUseCase useCase =new CreateUeUseCase(fauxUeRepository);
		var ueTeste=await useCase.ExecuteAsync(ueSansId);
		
		Assert.That(ueTeste.Id, Is.EqualTo(ueCreee.Id));
		Assert.That(ueTeste.NumeroUe, Is.EqualTo(ueCreee.NumeroUe));
		Assert.That(ueTeste.Intitule, Is.EqualTo(ueCreee.Intitule));
	}
}
