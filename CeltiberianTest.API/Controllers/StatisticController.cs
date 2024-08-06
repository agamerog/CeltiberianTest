using CeltiberianTest.API.Interfaces;
using CeltiberianTest.API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CeltiberianTest.API.Controllers
{
    [Route("api/[controller]s")]
    public class StatisticController : Controller
    {
        private readonly IStatsService statsService;

        public StatisticController(IStatsService statsService)
        {
            this.statsService = statsService;
        }
        // GET: api/values
        [HttpGet("lodash/file-names-letters")]
        public async Task<IEnumerable<LetterInfo>> GetLodashFilesLetters()
        {
            var letterCount = await statsService.GetLettersByFileNames("lodash", "lodash");

            return letterCount;
        }

        // GET: api/values
        [HttpGet("lodash/files-content-letters")]
        public async Task<IEnumerable<LetterInfo>> GetLodashFileContentLetters(bool waitForResetLimit)
        {
            var letterCount = await statsService.GetLettersByFilesContent("lodash", "lodash", waitForResetLimit);

            return letterCount;
        }
    }
}

