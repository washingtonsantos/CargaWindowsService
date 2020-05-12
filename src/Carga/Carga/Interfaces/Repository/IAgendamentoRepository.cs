using Carga.Entities;
using System.Collections.Generic;

namespace Carga.Interfaces.Repository
{
    public interface IAgendamentoRepository : IRepositoryBase<Agendamento>
    {
        IEnumerable<Agendamento> BuscarAgendamentos();
    }
}
