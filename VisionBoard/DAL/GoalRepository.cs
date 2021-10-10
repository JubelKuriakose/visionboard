using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VisionBoard.DAL
{
    public class GoalRepository : IGoalRepository
    {
        private readonly GoalTrackerContext dBContext;

        public GoalRepository(GoalTrackerContext appDBContext)
        {
            this.dBContext = appDBContext;
        }

        public async Task<Goal> AddGoal(Goal goal)
        {
            await dBContext.Goals.AddAsync(goal);
            await dBContext.SaveChangesAsync();
            return goal;
        }

        public async Task<Goal> DeleteGoal(int goalId)
        {
            var goal = await dBContext.Goals.FindAsync(goalId);
            if(goal!=null)
            {
                dBContext.Goals.Remove(goal);
                await dBContext.SaveChangesAsync();
            }
            return goal;
        }

        public async Task<IEnumerable<Goal>> GetAllGoals()
        {
            return await dBContext.Goals.Include(g => g.Reward).Include(g => g.Tag).Include(g => g.Steps).ToListAsync();
        }

        public async Task<Goal> GetGoal(int goalId)
        {
            return await dBContext.Goals.Include(g => g.Reward).Include(g => g.Tag).Include(g => g.Steps).FirstOrDefaultAsync(g => g.Id == goalId);
        }

        public async Task<Goal> UpdateGoal(Goal goals)
        {
            var goalsChanges = dBContext.Goals.Attach(goals);
            goalsChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dBContext.SaveChangesAsync();
            return goals;
        }

        public async Task<bool> IsGoalExist(int goalId)
        {
            return await dBContext.Goals.AnyAsync(a=>a.Id==goalId);
        }



    }
}
