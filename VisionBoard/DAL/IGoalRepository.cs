using VisionBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public interface IGoalRepository
    {
        IEnumerable<Goal> GetAllGoals();

        Task<Goal> GetGoal(int goalId);

        Task<Goal> AddGoal(Goal goal);

        Task<Goal> UpdateGoal(Goal goal);

        Task<Goal> DeleteGoal(int goalId);

        bool IsGoalExist(int GoalId);

    }
}
