namespace minimal_caching_demo.Application.Caching;

public interface IDataService
{
    Task<IList<DataEntity>> SearchEntities(string name);
    Task Add(string name);
}
