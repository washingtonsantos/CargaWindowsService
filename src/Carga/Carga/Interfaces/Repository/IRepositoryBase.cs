namespace Carga.Interfaces.Repository
{
    public interface IRepositoryBase<TEntity>
    {
        bool Update(TEntity entity);
    }
}
