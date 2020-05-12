using Carga.Interfaces.Notificacoes;
using System.Collections.Generic;
using System.Linq;

namespace Carga.Domain.Notificacoes
{
    /// <summary>
    /// Classe que realiza o tratamento das exceções
    /// </summary>
    public class DomainNotificationHandler : INotificationHandler
    {
        private List<DomainNotification_> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification_>();
        }

        public void Handle(DomainNotification_ message)
        {
            _notifications.Add(message);

            // return Task.CompletedTask;
        }

        public virtual List<DomainNotification_> GetNotifications()
        {
            return _notifications;
        }

        public virtual bool HasNotifications()
        {
            return GetNotifications().Any();
        }

        /// <summary>
        /// Limpa notificações existentes
        /// </summary>
        public void Dispose()
        {
            _notifications = new List<DomainNotification_>();
        }

        public List<string> GetShowNotifications()
        {
            var showNotifications = new List<string>();

            foreach (var notification in _notifications)
            {
                showNotifications.Add(notification.Value);
            }

            return showNotifications;
        }
    }
}
