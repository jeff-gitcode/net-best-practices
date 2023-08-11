using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

public class ApplicationSieveProcessor : SieveProcessor
{
    public ApplicationSieveProcessor(
        IOptions<SieveOptions> options,
        ISieveCustomSortMethods customSortMethods,
        ISieveCustomFilterMethods customFilterMethods)
        : base(options, customSortMethods, customFilterMethods)
    { }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.Property<Product>(p => p.Stock)
            .CanSort()
            .CanFilter();

        return mapper;
    }
}