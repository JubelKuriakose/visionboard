using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public class RewardRepository : IRewardRepository
    {
        private readonly GoalTrackerContext dBContext;

        public RewardRepository(GoalTrackerContext appDBContext)
        {
            this.dBContext = appDBContext;
        }

        public async Task<Reward> AddReward(Reward reward)
        {
            await dBContext.Rewards.AddAsync(reward);
            await dBContext.SaveChangesAsync();
            return reward;
        }

        public async Task<Reward> DeleteReward(int rewardId)
        {
            var reward = await dBContext.Rewards.FindAsync(rewardId);
            if(reward!=null)
            {
                dBContext.Rewards.Remove(reward);
                await dBContext.SaveChangesAsync();
            }
            return reward;
        }

        public IEnumerable<Reward> GetAllRewards()
        {
            return dBContext.Rewards;
        }

        public async Task<Reward> GetReward(int rewardId)
        {
            return await dBContext.Rewards.FindAsync(rewardId);
        }

        public async Task<Reward> UpdateReward(Reward rewards)
        {
            var rewardsChanges = dBContext.Rewards.Attach(rewards);
            rewardsChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dBContext.SaveChangesAsync();
            return rewards;
        }

        public bool IsRewardExist(int rewardId)
        {
            return dBContext.Rewards.Any(a=>a.Id==rewardId);
        }



    }
}
