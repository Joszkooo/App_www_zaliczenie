using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App_www_zaliczenie.Data;
using Microsoft.EntityFrameworkCore;

namespace App_www_zaliczenie.Services.GameService
{
    public class GameService : IGameService
    {
        private readonly DataContext _context;

        public GameService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Game>> DeleteGame(int GameId)
        {
            var serviceResponse = new ServiceResponse<Game>();
            try
            {
                var gameDB = await _context.Games.FirstOrDefaultAsync(g => g.Id == GameId);
                if( gameDB is not null)
                {
                    serviceResponse.Data = gameDB;
                    _context.Games.Attach(gameDB);
                    _context.Games.Remove(gameDB);
                    await _context.SaveChangesAsync();
                    
                    serviceResponse.Message = $"Gra o id {GameId} została pomyślnie usunięta.";
                }
                else{
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Brak gry o id {GameId}. Gra nie została usunięta.";
                }
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error: {ex.Message}";
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<List<Game>>> GetAllGames()
        {
            var serviceResponse = new ServiceResponse<List<Game>>();
            try
            {
                var gameDB = await _context.Games.ToListAsync();
                if (gameDB is not null)
                {
                    serviceResponse.Data = gameDB;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Brak gier w bazie danych";
                }
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error: {ex.Message}";
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<Game>> GetGameById(int GameId)
        {
            var serviceResponse = new ServiceResponse<Game>();
            try
            {
                var gameDB = await _context.Games.FirstOrDefaultAsync(g => g.Id == GameId);
                if (gameDB is not null)
                {
                    serviceResponse.Data = gameDB;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Brak gry o id {GameId}";
                }
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error: {ex.Message}";
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<Game>> NewGame(PostNewGameDTO newGameDTO)
        {
            var serviceResponse = new ServiceResponse<Game>();
            try
            {
                var newGame = new Game {
                    Name = newGameDTO.Name,
                    Description = newGameDTO.Description,
                    Category = newGameDTO.Category
                };

                _context.Games.Attach(newGame);
                await _context.Games.AddAsync(newGame);
                await _context.SaveChangesAsync();

                addRanking(newGame);

                serviceResponse.Data = newGame;
                serviceResponse.Message = $"Gra oraz ranking dodany pomyślnie.";
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error: {ex.Message}";
                return serviceResponse;
            }
        }

        private async void addRanking(Game game)
        {
            GlobalRanking newGlobalRanking = new GlobalRanking
            {
                UpVotes = 0,
                DownVotes = 0,
                GameId = game.Id
            };
            _context.GlobalRankings.Attach(newGlobalRanking);
            await _context.GlobalRankings.AddAsync(newGlobalRanking);
            await _context.SaveChangesAsync();
        }
    }
}