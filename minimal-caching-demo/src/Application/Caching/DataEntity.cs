namespace minimal_caching_demo.Application.Caching;
public class DataEntity
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}

public interface IDataRepository
{
    Task<IList<DataEntity>> SearchEntities(string name);
    Task Add(string name);
}
