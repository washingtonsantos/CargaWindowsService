using Carga.Entities;

namespace Carga.Interfaces.Service
{
    public interface IProcessaService
    {
        int IniciarProcessamento(Agendamento agendamento);
    }
}
