using VisionBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public interface IStepRepository
    {
        IEnumerable<Step> GetAllSteps();

        Task<Step> GetStep(int stepId);

        Task<Step> AddStep(Step step);

        Task<Step> UpdateStep(Step step);

        Task<Step> DeleteStep(int stepId);

        bool IsStepExist(int StepId);

    }
}
