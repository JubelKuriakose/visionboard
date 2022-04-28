using VisionBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public interface IRewardRepository
    {
        Task<IEnumerable<Reward>> GetAllRewards();

        Task<Reward> GetReward(int rewardId);

        Task<Reward> AddReward(Reward reward);

        Task<Reward> UpdateReward(Reward reward);

        Task<Reward> DeleteReward(int rewardId);

        Task<bool> IsRewardExist(int RewardId);

    }
}
