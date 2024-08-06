using System;
using CeltiberianTest.API.Models;

namespace CeltiberianTest.API.Interfaces;

public interface IStatsService
{
	public Task<IEnumerable<LetterInfo>> GetLettersByFileNames(string owner, string repoName);
	public Task<IEnumerable<LetterInfo>> GetLettersByFilesContent(string owner, string repoName, bool waitForResetLimit);
}