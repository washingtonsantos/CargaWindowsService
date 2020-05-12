using System;

namespace Carga.Domain.Notificacoes
{
    /// <summary>
    /// Classe de Notificações
    /// </summary>
    public class DomainNotification
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; }
        public string Value { get; set; }
        public int Version { get; private set; }

        public DomainNotification(string key, string value)
        {
            Id = Guid.NewGuid();
            Version = 1;
            Key = key;
            Value = value;
        }
    }
}
