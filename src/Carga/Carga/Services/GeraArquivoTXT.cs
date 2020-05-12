using Carga.Entities;
using Carga.Interfaces.Log;
using Carga.Interfaces.Notificacoes;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Carga.Log.Exportacao
{
    public class GerarArquivoTXT : IGerarArquivoTXT
    {
        private INotificationHandler _notificationHandler;
        private string extensao = ".txt";

        public GerarArquivoTXT(INotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        public bool GerarArquivo(Agendamento agendamento, string id)
        {
            var caminhoDestino = ConfigurationManager.AppSettings["log_executados"];

            if (!Directory.Exists(caminhoDestino))
            {
                _notificationHandler.Handle(new Domain.Notificacoes.DomainNotification_("GeraLogTXT", "Diretório não existe"));
                return false;
            }

            caminhoDestino = caminhoDestino + @"\" + agendamento.TSK + DateTime.Now.ToString("ddMMyyyyHHmmss") + extensao;

            try
            {
                using (StreamWriter sw = new StreamWriter(caminhoDestino, false, Encoding.UTF8))
                {
                    sw.WriteLine(id + "|" + DateTime.Now.ToString("yyyy-MM-dd"));
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                _notificationHandler.Handle(new Domain.Notificacoes.DomainNotification_("GeraLogTXT", ex.Message));
                return false;
            }

            return true;
        }
    }
}
