namespace minimal_caching_demo.Application.Caching;

public interface IDataRepository
{
    Task<IList<DataEntity>> SearchEntities(string name);
    Task Add(string name);
}
