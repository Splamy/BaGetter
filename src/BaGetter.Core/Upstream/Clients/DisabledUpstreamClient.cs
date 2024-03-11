using BaGetter.Protocol.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace BaGetter.Core;

/// <summary>
/// The client used when there are no upstream package sources.
/// </summary>
public class DisabledUpstreamClient : IUpstreamClient
{
    private readonly IReadOnlyList<NuGetVersion> _emptyVersionList = [];
    private readonly IReadOnlyList<Package> _emptyPackageList = [];
    private readonly IReadOnlyList<SearchResult> _emptySearchResults = [];

    public Task<IReadOnlyList<NuGetVersion>> ListPackageVersionsAsync(string id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_emptyVersionList);
    }

    public Task<IReadOnlyList<Package>> ListPackagesAsync(string id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_emptyPackageList);
    }

    public Task<Stream> DownloadPackageOrNullAsync(
        string id,
        NuGetVersion version,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<Stream>(null);
    }

    public Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new SearchResponse() { Context = SearchContext.Default(""), TotalHits = 0, Data = [] });
    }

}
