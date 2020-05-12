using Carga.Domain.Notificacoes;
using System.Collections.Generic;

namespace Carga.Interfaces.Notificacoes
{
    /// <summary>
    /// Contratos para uso de notificações
    /// </summary>
    public interface INotificationHandler
    {
        void Handle(DomainNotification_ message);
        List<DomainNotification_> GetNotifications();
        List<string> GetShowNotifications();
        bool HasNotifications();
        void Dispose();
    }
}
