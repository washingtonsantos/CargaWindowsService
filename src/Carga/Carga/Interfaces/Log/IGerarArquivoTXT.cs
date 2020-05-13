
using Carga.Entities;

namespace Carga.Interfaces.Log
{
    public interface IGerarArquivoTXT
    {
        bool GerarArquivo(Agendamento agendamento, string id);
    }
}
