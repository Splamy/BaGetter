using BaGetter.Protocol.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaGetter.Core.Search;
public class UpstreamSearchService : ISearchService
{
    private readonly IUpstreamClient _upstreamClient;

    public UpstreamSearchService(IUpstreamClient upstreamClient)
    {
        _upstreamClient = upstreamClient;
    }

    public Task<AutocompleteResponse> AutocompleteAsync(AutocompleteRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<DependentsResponse> FindDependentsAsync(string packageId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AutocompleteResponse> ListPackageVersionsAsync(VersionsRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        return _upstreamClient.SearchAsync(request, cancellationToken);
    }
}
