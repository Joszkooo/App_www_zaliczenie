using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App_www_zaliczenie.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace App_www_zaliczenie.Services.RankingService
{
    public class RankingService : IRankingService
    {
        private readonly DataContext _context;

        public RankingService(DataContext context)
        {
            _context = context;
        }
        
        [Authorize]
        public async Task<ServiceResponse<PostVoteDTO>> PostUpVote(int GameId, int UserId)
        {
            var serviceResponse = new ServiceResponse<PostVoteDTO>();
            try
            {
                var GameRanking = await _context.GlobalRankings
                    .Include(g => g.Game)
                        .FirstOrDefaultAsync(x => x.GameId == GameId);
                var UserDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);

                if (GameRanking is not null && UserDB is not null)
                {
                    if (!UserDB.VotedGames.Contains(GameId)) 
                    {
                        UserDB.VotedGames.Add(GameId);
                        GameRanking.UpVotes += 1;
                        await _context.SaveChangesAsync();
                        serviceResponse.Data = new PostVoteDTO
                        {
                            Name = GameRanking.Game.Name,
                            UpVotes = GameRanking.UpVotes,
                            DownVotes = GameRanking.DownVotes
                        };
                    }
                    else {
                        serviceResponse.Message = $"Użytkownik o id {UserId} zagłosował już na gre o id {GameId}";
                        serviceResponse.Success = false;
                    }

                }
                else{
                    serviceResponse.Message = $"Brak gry o id {GameId} i/lub gracza {UserId}";
                    serviceResponse.Success = false;
                }
                return serviceResponse;
            }catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error: {ex.Message}";
                return serviceResponse;
            }
        }

        [Authorize]
        public async Task<ServiceResponse<PostVoteDTO>> PostDownVote(int GameId, int UserId)
        {
            var serviceResponse = new ServiceResponse<PostVoteDTO>();
            try
            {
                var GameRanking = await _context.GlobalRankings
                    .Include(g => g.Game)
                        .FirstOrDefaultAsync(x => x.GameId == GameId);
                var UserDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);

                if (GameRanking is not null && UserDB is not null)
                {
                    if (UserDB.VotedGames.Contains(GameId)) 
                    {
                        UserDB.VotedGames.Remove(GameId);
                        GameRanking.UpVotes -= 1;
                        await _context.SaveChangesAsync();
                        serviceResponse.Data = new PostVoteDTO
                        {
                            Name = GameRanking.Game.Name,
                            UpVotes = GameRanking.UpVotes,
                            DownVotes = GameRanking.DownVotes
                        };
                    }
                    else {
                        serviceResponse.Message = $"Użytkownik o id {UserId} zagłosował już na gre o id {GameId}";
                        serviceResponse.Success = false;
                    }

                }
                else{
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Brak gry o id {GameId} i/lub gracza {UserId}";
                }
                return serviceResponse;
            }catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error: {ex.Message}";
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<List<GlobalRanking>>> GetCategoryRanking(Category category)
        {
            var serviceResponse = new ServiceResponse<List<GlobalRanking>>();
            try
            {
                var categoryRankingDB = await _context.GlobalRankings
                    .Include(g => g.Game)
                        .Where(x => x.Game.Category == category)
                            .ToListAsync();
                if (categoryRankingDB.Any())
                {
                    serviceResponse.Data = categoryRankingDB;
                }
                else{
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Brak rankingu wg kategorii w bazie danych";
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

        public async Task<ServiceResponse<List<GlobalRanking>>> GetGlobalRanking()
        {
            var serviceResponse = new ServiceResponse<List<GlobalRanking>>();
            try
            {
                var globalRankingDB = await _context.GlobalRankings.ToListAsync();
                if (globalRankingDB.Any())
                {
                    serviceResponse.Data = globalRankingDB;
                }
                else{
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Brak globalnego rankingu w bazie danych";
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

        [Authorize]
        public async Task<ServiceResponse<List<UserRanking>>> GetUserRanking(int UserId)
        {
            var serviceResponse = new ServiceResponse<List<UserRanking>>();
            try
            {
                var userRankingDB = await _context.UserRankings.Where(u => u.AccountId == UserId).ToListAsync();
                
                if (userRankingDB.Any())
                {
                    serviceResponse.Data = userRankingDB;
                }
                else{
                    serviceResponse.Message = $"Brak głosowań w przypadku użytkownika o id {UserId}";
                    serviceResponse.Success = false;
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

    }
}