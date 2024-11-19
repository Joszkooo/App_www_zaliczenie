using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App_www_zaliczenie.Services.GameService
{
    public interface IGameService
    {
        Task<ServiceResponse<Game>> GetGameById(int GameId);
        Task<ServiceResponse<List<Game>>> GetAllGames();
        Task<ServiceResponse<GameDTO>> NewGame(PostNewGameDTO newGame);
        Task<ServiceResponse<Game>> DeleteGame(int GameId);
    }
}