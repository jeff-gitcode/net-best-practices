namespace minimal_caching_demo.Application.Caching;

using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

public class GetDataQuery: IRequest<IList<DataEntity>>
{
    public string Name;
}


public class GetDataQueryHandler : IRequestHandler<GetDataQuery, IList<DataEntity>>
{
    private readonly IDataService service;

    public GetDataQueryHandler(IDataService service)
    {
        this.service = service;
    }

    public async Task<IList<DataEntity>> Handle(GetDataQuery request, CancellationToken cancellationToken)
    {
        return await this.service.SearchEntities(request.Name);
    }
}
