using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace VisionBoard.DAL
{
    public class RewardRepository : IRewardRepository
    {
        private readonly GoalTrackerContext dBContext;
        private readonly IErrorLogRepository errorLogRepository;

        public RewardRepository(GoalTrackerContext appDBContext, IErrorLogRepository errorLogRepository)
        {
            this.dBContext = appDBContext;
            this.errorLogRepository = errorLogRepository;
        }


        public async Task<Reward> AddReward(Reward reward)
        {
            try
            {
                await dBContext.Rewards.AddAsync(reward);
                await dBContext.SaveChangesAsync();
                return reward;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<Reward> DeleteReward(int rewardId)
        {
            try
            {
                var reward = await dBContext.Rewards.FindAsync(rewardId);
                if (reward != null)
                {
                    dBContext.Rewards.Remove(reward);
                    await dBContext.SaveChangesAsync();
                }
                return reward;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<IEnumerable<Reward>> GetAllRewards()
        {
            try
            {
                return await dBContext.Rewards.ToListAsync();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<Reward> GetReward(int rewardId)
        {
            try
            {
                return await dBContext.Rewards.Include(r => r.Goal).FirstOrDefaultAsync(m => m.Id == rewardId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;            
            
        }


        public async Task<Reward> UpdateReward(Reward rewards)
        {
            try
            {
                var rewardsChanges = dBContext.Rewards.Attach(rewards);
                rewardsChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await dBContext.SaveChangesAsync();
                return rewards;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<bool> IsRewardExist(int rewardId)
        {
            try
            {
                return await dBContext.Rewards.AnyAsync(a => a.Id == rewardId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return true;
            
        }



    }
}
