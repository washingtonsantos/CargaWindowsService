using Carga.Entities;
using Carga.Interfaces.Notificacoes;
using Carga.Interfaces.Repository;
using Dapper;
using System.Linq;

namespace Carga.Repository
{
    public class BloqueioRepository : RepositoryBase<OrigemID>, IBloqueioRepository
    {
        public BloqueioRepository(INotificationHandler notificationHandler) : base(notificationHandler)
        {

        }
        public OrigemID BuscarBloqueioComSTSNull()
        {
            try
            {
                return connection.Query<OrigemID>("SELECT * FROM TB_BLQD WHERE STS IS NULL").FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                NotificationHandler.Handle(new Domain.Notificacoes.DomainNotification_("AgendamentoRepository", ex.Message));
                return null;
            }
        }
    }
}
