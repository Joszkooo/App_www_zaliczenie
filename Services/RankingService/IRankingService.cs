using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App_www_zaliczenie.Services.RankingService
{
    public interface IRankingService
    {
        Task<ServiceResponse<List<GlobalRanking>>> GetGlobalRanking();
        Task<ServiceResponse<List<UserRanking>>> GetUserRanking(int UserId);
        Task<ServiceResponse<List<GlobalRanking>>> GetCategoryRanking(Category category);
        Task<ServiceResponse<PostVoteDTO>> PostUpVote (int GameId, int UserId);
        Task<ServiceResponse<PostVoteDTO>> PostDownVote (int GameId, int UserId);
    }
}