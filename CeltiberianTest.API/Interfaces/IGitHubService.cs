using System;
using System.Collections.Concurrent;
using Octokit;

namespace CeltiberianTest.API.Interfaces;

public interface IGitHubService
{
    /// <summary>
    /// Get the files by type js or ts in the main branch
    /// </summary>
    /// <param name="owner">Owner of the repository</param>
    /// <param name="repository">Name of the repository</param>
    /// <returns>List of the content of the files in the repository</returns>
    public Task<List<string>> GetScriptFileNamesFromMainBranchAsync(string owner, string repoName);

    /// <summary>
    /// Get the content of all the files type js or ts in the main branch
    /// </summary>
    /// <param name="owner">Owner of the repository</param>
    /// <param name="repository">Name of the repository</param>
    /// <returns>List of the content of the files in the repository</returns>
    public Task<List<string>> GetScriptFilesContentFromMainBranchAsync(string owner, string repoName, bool waitForResetLimit);
}