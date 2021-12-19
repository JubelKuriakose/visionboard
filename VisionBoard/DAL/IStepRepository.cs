using VisionBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public interface IStepRepository
    {
        Task<IEnumerable<Step>> GetAllSteps();

        Task<IEnumerable<Step>> GetAllSteps(int goalId);

        Task<Step> GetStep(int stepId);

        Task<Step> AddStep(Step step);

        Task<Step> UpdateStep(Step step);

        Task<Step> DeleteStep(int stepId);

        Task<bool> IsStepExist(int StepId);

    }
}
