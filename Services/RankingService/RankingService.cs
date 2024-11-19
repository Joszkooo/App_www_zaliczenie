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

        public async Task<ServiceResponse<PostVoteDTO>> PostUpVote(int GameId, int UserId)
        {
            var serviceResponse = new ServiceResponse<PostVoteDTO>();
            try
            {
                var globalRanking = await _context.GlobalRankings
                    .Include(g => g.Game)
                        .FirstOrDefaultAsync(x => x.GameId == GameId);

                var UserDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);

                var userRanking = await _context.UserRankings.FirstOrDefaultAsync(u => u.AccountId == UserId && u.GameId == GameId);

                if (globalRanking is not null && UserDB is not null)
                {
                    if (userRanking is not null && userRanking.DownVote)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = $"Istnieje negatywna opinia o grze ID {GameId}. Usun negatywna opinie aby zaglosowan negatywnie!";
                        return serviceResponse;
                    }

                    if (!UserDB.VotedGames.Contains(GameId)) 
                    {
                        if (userRanking is not null)
                        {
                            userRanking.UpVote = true;
                        }
                        else
                        {
                            var newUserRanking = new UserRanking
                            {
                                UpVote = true,
                                DownVote = false,
                                GameId = GameId,
                                AccountId = UserId
                            };
                            await _context.UserRankings.AddAsync(newUserRanking);
                        }
                        
                        globalRanking.UpVotes += 1;

                        UserDB.VotedGames.Add(GameId);

                        _context.Entry(UserDB).Property(u => u.VotedGames).IsModified = true;
            
                        await _context.SaveChangesAsync();

                        serviceResponse.Data = new PostVoteDTO
                        {
                            Name = globalRanking.Game.Name,
                            UpVotes = globalRanking.UpVotes,
                            DownVotes = globalRanking.DownVotes
                        };
                        serviceResponse.Message = $"Zaglosowano POZYTYWNIE na gre o ID {GameId}";
                    }
                    else {
                        globalRanking.UpVotes -= 1;
                        UserDB.VotedGames.Remove(GameId);
                        
                        if (userRanking is not null)
                        {
                            userRanking.UpVote = false;
                        }
                        else
                        {
                            var newUserRanking = new UserRanking
                            {
                                UpVote = false,
                                DownVote = false,
                                GameId = GameId,
                                AccountId = UserId
                            };
                            await _context.UserRankings.AddAsync(newUserRanking);
                        }
                        
                        _context.Entry(UserDB).Property(u => u.VotedGames).IsModified = true;
                        await _context.SaveChangesAsync();
                        serviceResponse.Data = new PostVoteDTO
                        {
                            Name = globalRanking.Game.Name,
                            UpVotes = globalRanking.UpVotes,
                            DownVotes = globalRanking.DownVotes
                        };
                        
                        serviceResponse.Message = $"Usunieto POZYTYWNY glos na gre o ID {GameId}";
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

        public async Task<ServiceResponse<PostVoteDTO>> PostDownVote(int GameId, int UserId)
        {
            var serviceResponse = new ServiceResponse<PostVoteDTO>();
            try
            {
                var globalRanking = await _context.GlobalRankings
                    .Include(g => g.Game)
                        .FirstOrDefaultAsync(x => x.GameId == GameId);
                var UserDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
                var userRanking = await _context.UserRankings.FirstOrDefaultAsync(u => u.AccountId == UserId && u.GameId == GameId);

                if (globalRanking is not null && UserDB is not null)
                {
                    if (userRanking is not null && userRanking.UpVote)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = $"Istnieje pozytywna opinia o grze ID {GameId}. Usun pozytywna opinie aby zaglosowan negatywnie!";
                        return serviceResponse;
                    }
                    if (!UserDB.VotedGames.Contains(GameId)) 
                    {
                        if (userRanking is not null)
                        {
                            userRanking.DownVote = true;
                        }
                        else
                        {
                            var newUserRanking = new UserRanking
                            {
                                UpVote = false,
                                DownVote = true,
                                GameId = GameId,
                                AccountId = UserId
                            };
                            await _context.UserRankings.AddAsync(newUserRanking);
                        }

                        UserDB.VotedGames.Add(GameId);

                        globalRanking.DownVotes += 1;

                        _context.Entry(UserDB).Property(u => u.VotedGames).IsModified = true;

                        await _context.SaveChangesAsync();
                        
                        serviceResponse.Data = new PostVoteDTO
                        {
                            Name = globalRanking.Game.Name,
                            UpVotes = globalRanking.UpVotes,
                            DownVotes = globalRanking.DownVotes
                        };
                        serviceResponse.Message = $"Zaglosowano NEGATYWNIE na gre o ID {GameId}";
                    }
                    else {
                        globalRanking.DownVotes -= 1;
                        UserDB.VotedGames.Remove(GameId);
                        

                        if (userRanking is not null)
                        {
                            userRanking.DownVote = false;
                        }
                        else
                        {
                            var newUserRanking = new UserRanking
                            {
                                UpVote = false,
                                DownVote = false,
                                GameId = GameId,
                                AccountId = UserId
                            };
                            await _context.UserRankings.AddAsync(newUserRanking);
                        }
                        _context.Entry(UserDB).Property(u => u.VotedGames).IsModified = true;
                        await _context.SaveChangesAsync();

                        serviceResponse.Data = new PostVoteDTO
                        {
                            Name = globalRanking.Game.Name,
                            UpVotes = globalRanking.UpVotes,
                            DownVotes = globalRanking.DownVotes
                        };
                        
                        serviceResponse.Message = $"Usunieto NEGATYWNY glos na gre o ID {GameId}";
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