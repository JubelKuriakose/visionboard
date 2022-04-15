using VisionBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public interface IGoalRepository
    {
        Task<IEnumerable<Goal>> GetAllGoals(int[] tagIds);

        Task<Goal> GetGoal(int goalId);

        Task<Goal> AddGoal(Goal goal);

        Task<Goal> UpdateGoal(Goal goal);

        Task<Goal> DeleteGoal(int goalId);

        Task<bool> IsGoalExist(int GoalId);

    }
}
