using Carga.Entities;
using Carga.Interfaces.Notificacoes;
using Carga.Interfaces.Repository;
using Dapper;
using System.Collections.Generic;

namespace Carga.Repository
{
    public class AgendamentoRepository : RepositoryBase<Agendamento>, IAgendamentoRepository
    {
        /// <summary>
        /// Repositório de Agendamento  (tabela TB_TSK_SCHD)
        /// </summary>
        /// <param name="notificationHandler"></param>
        public AgendamentoRepository(INotificationHandler notificationHandler) : base(notificationHandler)
        {
        }

        /// <summary>
        /// Buscar Tarefas Agendadas
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Agendamento> BuscarAgendamentos()
        {
            try
            {
                var sb = new System.Text.StringBuilder(66);
                sb.AppendLine(@"SELECT *");
                sb.AppendLine(@"FROM TB_TSK_SCHD");

                return connection.Query<Agendamento>(sb.ToString());
            }
            catch (System.Exception ex)
            {
                NotificationHandler.Handle(new Domain.Notificacoes.DomainNotification_("AgendamentoRepository", ex.Message));
                return null;
            }
            
        }
    }
}
