using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace VisionBoard.DAL
{
    public class GoalRepository : IGoalRepository
    {
        private readonly GoalTrackerContext dBContext;
        private readonly IErrorLogRepository errorLogRepository;

        public GoalRepository(GoalTrackerContext appDBContext, IErrorLogRepository errorLogRepository)
        {
            this.dBContext = appDBContext;
            this.errorLogRepository = errorLogRepository;
        }


        public async Task<Goal> AddGoal(Goal goal)
        {
            try
            {
                await dBContext.Goals.AddAsync(goal);
                await dBContext.SaveChangesAsync();
                return goal;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null; 
        }


        public async Task<Goal> DeleteGoal(int goalId)
        {
            try
            {
                var goal = await dBContext.Goals.FindAsync(goalId);
                if (goal != null)
                {
                    dBContext.Goals.Remove(goal);
                    await dBContext.SaveChangesAsync();
                }
                return goal;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<IEnumerable<Goal>> GetAllGoals(int[] tagIds)
        {
            try
            {
                IEnumerable<Goal> goals;

                if (tagIds != null && tagIds.Length > 0)
                {
                    goals = await dBContext.Goals.Include(g => g.Reward).Include(g => g.Steps).Include(g => g.Measurement)
                        .Include(g => g.GoalTags).ThenInclude(g => g.Tag).Where(g => g.GoalTags.Any(gt => tagIds.Any(tagIds => tagIds == gt.TagId))).ToListAsync();
                }
                else
                {
                    goals = await dBContext.Goals.Include(g => g.Reward).Include(g => g.Steps).Include(g => g.Measurement).Include(g => g.GoalTags).ThenInclude(g => g.Tag).ToListAsync();
                }
                return goals;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<Goal> GetGoal(int goalId)
        {
            try
            {
                return await dBContext.Goals.Include(g => g.Reward).Include(g => g.Measurement).Include(g => g.Steps).Include(g => g.GoalTags).ThenInclude(g => g.Tag).FirstOrDefaultAsync(g => g.Id == goalId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<Goal> UpdateGoal(Goal goals)
        {
            try
            {
                var goalsChanges = dBContext.Goals.Attach(goals);
                goalsChanges.State = EntityState.Modified;
                await dBContext.SaveChangesAsync();
                return goals;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
        }


        public async Task<bool> IsGoalExist(int goalId)
        {
            try
            {
                return await dBContext.Goals.AnyAsync(a => a.Id == goalId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return false;
        }



    }
}
