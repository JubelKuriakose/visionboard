using VisionBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public interface IGoalTagsRepository
    {
        Task<IEnumerable<GoalTags>> GetAllGoalTagsMappings();

        Task<IList<Tag>> GetAllGoalTagsMappings(int goalId);

        Task<GoalTags> GetGoalTagsMapping(int goalId, int tagId);

        Task<GoalTags> AddGoalTagsMapping(GoalTags goalTag);

        Task<GoalTags> UpdateGoalTagsMapping(GoalTags goalTag);

        Task<GoalTags> DeleteGoalTagsMapping(int goalId, int tagId);

        Task<bool> IsGoalTagsMappingExist(int goalId, int tagId);

    }
}
