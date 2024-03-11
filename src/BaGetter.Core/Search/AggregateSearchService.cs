using BaGetter.Protocol.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaGetter.Core.Search;
internal class AggregateSearchService : ISearchService
{
    private readonly IUrlGenerator _url;
    private readonly ImmutableArray<ISearchService> _searchServices;

    public AggregateSearchService(IEnumerable<ISearchService> searchServices, IUrlGenerator url)
    {
        _searchServices = [.. searchServices];
        _url = url;
    }

    public Task<AutocompleteResponse> AutocompleteAsync(AutocompleteRequest request, CancellationToken cancellationToken)
    {
        // XXX
        return _searchServices.First().AutocompleteAsync(request, cancellationToken);
    }

    public Task<DependentsResponse> FindDependentsAsync(string packageId, CancellationToken cancellationToken)
    {
        // XXX
        return _searchServices.First().FindDependentsAsync(packageId, cancellationToken);
    }

    public Task<AutocompleteResponse> ListPackageVersionsAsync(VersionsRequest request, CancellationToken cancellationToken)
    {
        // XXX
        return _searchServices.First().ListPackageVersionsAsync(request, cancellationToken);
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        long total = 0;
        List<SearchResult> searchResponses = [];
        HashSet<string> seen = [];

        var currentRequest = new SearchRequest
        {
            Skip = request.Skip,
            Take = request.Take,
            IncludePrerelease = request.IncludePrerelease,
            IncludeSemVer2 = request.IncludeSemVer2,
            PackageType = request.PackageType,
            Framework = request.Framework,
            Query = request.Query,
        };

        foreach (var service in _searchServices)
        {
            var result = await service.SearchAsync(currentRequest, cancellationToken);

            total += result.TotalHits;

            // Continue iterating so we get the full count of results.
            if (currentRequest.Take == 0)
            {
                currentRequest.Skip = 0;
                continue;
            }

            if (currentRequest.Skip > result.TotalHits)
            {
                currentRequest.Skip -= (int)result.TotalHits;
                continue;
            }

            currentRequest.Skip = 0;

            foreach (var package in result.Data)
            {
                if (!seen.Add(package.PackageId))
                {
                    continue;
                }

                searchResponses.Add(package);
                currentRequest.Take--;

                if (currentRequest.Take == 0)
                {
                    break;
                }
            }
        }

        return new SearchResponse
        {
            TotalHits = total,
            Data = searchResponses,
            Context = SearchContext.Default(_url.GetPackageMetadataResourceUrl()),
        };
    }
}
