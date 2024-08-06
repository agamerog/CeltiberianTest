using System.Collections.Concurrent;
using CeltiberianTest.API.Interfaces;
using Octokit;
namespace CeltiberianTest.API.Services;

public class GitHubService : IGitHubService
{
    private readonly GitHubClient _client;

    public GitHubService()
    {
        _client = new GitHubClient(new ProductHeaderValue("RepoStats"));
    }

    ///<inheritdoc/> 
    public async Task<List<string>> GetScriptFileNamesFromMainBranchAsync(string owner, string repository)
    {
        var fileNames = new List<string>();

        try
        {
            var tree = await _client.Git.Tree.GetRecursive(owner, repository, "main");

            fileNames = tree.Tree
                .Where(ti => ti.Type == TreeType.Blob && (ti.Path.EndsWith(".ts") || ti.Path.EndsWith(".js")))
                .Select(ti => Path.GetFileNameWithoutExtension(ti.Path))
                .ToList();

            return fileNames;
        }

        catch (ApiException ex)
        {
            if (ex.Message.Contains("API rate limit exceeded"))
            {
                Console.Write("Unable to get all the files content because of the rate limit policy.");
                return fileNames;
            }
            throw;
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<string>> GetScriptFilesContentFromMainBranchAsync(string owner, string repository, bool waitForResetLimit)
    {
        var tree = await _client.Git.Tree.GetRecursive(owner, repository, "main");

        var items = tree.Tree.Where(ti => ti.Type == TreeType.Blob && (ti.Path.EndsWith(".ts") || ti.Path.EndsWith(".js"))).ToList();

        var fileContents = new ConcurrentBag<string>();

        foreach (var item in items)
        {
            try
            {
                var file = await _client.Git.Blob.Get(owner, repository, item.Sha);
                var content = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(file.Content));
                fileContents.Add(file.Content);

                // Rate limit handling
                var rateLimit = _client.GetLastApiInfo().RateLimit;
                if (rateLimit.Remaining <= 1)
                {
                    if (!waitForResetLimit)
                    {
                        break;
                    }

                    var resetTime = rateLimit.Reset.UtcDateTime;
                    var waitTime = resetTime - DateTime.UtcNow;
                    await Task.Delay(waitTime);
                }

            }
            catch (ApiException)
            {
                Console.Write("Unable to get all the files content because of the rate limit policy.");
                break;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                throw;
            }
        };

        return fileContents.Select(f => System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(f))).ToList();
    }
}
