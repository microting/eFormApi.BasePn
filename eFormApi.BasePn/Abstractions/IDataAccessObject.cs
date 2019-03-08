namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IDataAccessObject<in TDbContext>
    {
        void Create(TDbContext dbContext);
        void Update(TDbContext dbContext);
        void Delete(TDbContext dbContext);
    }
}
