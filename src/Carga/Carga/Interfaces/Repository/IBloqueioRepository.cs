
using Carga.Entities;

namespace Carga.Interfaces.Repository
{
    public interface IBloqueioRepository : IRepositoryBase<OrigemID>
    {
        OrigemID BuscarBloqueioComSTSNull();
    }
}
