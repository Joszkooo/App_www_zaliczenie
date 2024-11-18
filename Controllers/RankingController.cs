using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App_www_zaliczenie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RankingController : ControllerBase
    {
        private readonly IRankingService _rankingService;

        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        [HttpGet("GetGlobalRanking")]
        public async Task<IActionResult> GetGlobalRanking()
        {
            return Ok(await _rankingService.GetGlobalRanking());
        }
        
        [HttpGet("GetUserRanking{UserId}")]
        public async Task<IActionResult> GetUserRanking(int UserId)
        {
            return Ok(await _rankingService.GetUserRanking(UserId));
        }

        [HttpGet("GetCategoryRanking{category}")]
        public async Task<IActionResult> GetCategoryRanking(Category category)
        {
            return Ok(await _rankingService.GetCategoryRanking(category));
        }

        [HttpPost("PostUpVote{GameId}")]
        public async Task<IActionResult> PostUpVote (int GameId, int UserId)
        {
            return Ok(await _rankingService.PostUpVote(GameId, UserId));
        }
        
        [HttpPost("PostDownVote{GameId}")]
        public async Task<IActionResult> PostDownVote (int GameId, int UserId)
        {
            return Ok(await _rankingService.PostDownVote(GameId, UserId));
        }
    }
}