using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public IEnumerable<Goal> GetAllGoals()
        {
            return dBContext.Goals;
        }

        public async Task<Goal> GetGoal(int goalId)
        {
            return await dBContext.Goals.FindAsync(goalId);
        }

        public async Task<Goal> UpdateGoal(Goal goals)
        {
            var goalsChanges = dBContext.Goals.Attach(goals);
            goalsChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dBContext.SaveChangesAsync();
            return goals;
        }

        public bool IsGoalExist(int goalId)
        {
            return dBContext.Goals.Any(a=>a.Id==goalId);
        }



    }
}
