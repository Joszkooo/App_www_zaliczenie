using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App_www_zaliczenie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("GetAllGames")]
        public async Task<IActionResult> GetAllGames()
        {
            return Ok(await _gameService.GetAllGames());
        }


        [HttpGet("GetGameById{GameId}")]
        public async Task<IActionResult> GetGameById(int GameId)
        {
            return Ok(await _gameService.GetGameById(GameId));
        }
        
        [HttpPost("NewGame")]
        public async Task<IActionResult> NewGame(PostNewGameDTO newGame)
        {
            return Ok(await _gameService.NewGame(newGame));
        }

        [HttpDelete("DeleteGame{GameId}")]
        public async Task<IActionResult> DeleteGame(int GameId)
        {
            return Ok(await _gameService.DeleteGame(GameId));
        }

    }
}