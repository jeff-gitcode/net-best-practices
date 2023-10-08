namespace minimal_caching_demo.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using minimal_caching_demo.Application.Caching;

public class DataRepository : IDataRepository
{
    public Task Add(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<DataEntity>> SearchEntities(string name)
    {
        await Task.Delay(1500);
        return GenFu.GenFu.ListOf<DataEntity>();
    }
}
