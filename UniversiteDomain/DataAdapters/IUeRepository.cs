using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface IUeRepository : IRepository<Ue>
{
    Task<Ue> AddUeAsync(long idUe, long idParcours);
}
