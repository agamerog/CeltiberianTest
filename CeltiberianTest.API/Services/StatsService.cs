using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using CeltiberianTest.API.Interfaces;
using CeltiberianTest.API.Models;

namespace CeltiberianTest.API.Services;

public class StatsService : IStatsService
{
    readonly IGitHubService gitHubService;

    public StatsService(IGitHubService gitHubService)
    {
        this.gitHubService = gitHubService;
    }

    public async Task<IEnumerable<LetterInfo>> GetLettersByFileNames(string owner, string repoName)
    {
        List<string> fileNames = await gitHubService.GetScriptFileNamesFromMainBranchAsync(owner, repoName);

        var letters = GetLetters(fileNames);

        return letters;
    }

    public async Task<IEnumerable<LetterInfo>> GetLettersByFilesContent(string owner, string repoName, bool waitForResetLimit)
    {
        List<string> filesContent = await gitHubService.GetScriptFilesContentFromMainBranchAsync(owner, repoName, waitForResetLimit);

        var letters = GetLetters(filesContent);

        return letters;
    }

    private static IEnumerable<LetterInfo> GetLetters(List<string> listOfStrings) {
        
        var letterCounts = new Dictionary<char, int>();

        foreach(var content in listOfStrings)
        {
            foreach (char c in Regex.Replace(content, @"[^a-zA-Z]", "").ToLower())
            {
                if (char.IsLetter(c))
                {
                    letterCounts[c] = letterCounts.GetValueOrDefault(c) + 1;
                }
            }
        };

        return letterCounts.Select(l => new LetterInfo(l.Key, l.Value)).OrderByDescending(q => q.Quantity).ToList();

    }
}

