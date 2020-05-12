using Carga.Interfaces.Notificacoes;
using Dapper.Contrib.Extensions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Carga.Repository
{

    public class RepositoryBase<TEntity> where TEntity : class
    {
        public IDbConnection connection;
        protected INotificationHandler NotificationHandler;

        public RepositoryBase(INotificationHandler notificationHandler)
        {
            NotificationHandler = notificationHandler;
            
              connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        public bool Update(TEntity entity)
        {
            return connection.Update(entity);
        }

        
    }
}
