using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public class StepRepository : IStepRepository
    {
        private readonly GoalTrackerContext dBContext;

        public StepRepository(GoalTrackerContext appDBContext)
        {
            this.dBContext = appDBContext;
        }

        public async Task<Step> AddStep(Step step)
        {
            await dBContext.Steps.AddAsync(step);
            await dBContext.SaveChangesAsync();
            return step;
        }

        public async Task<Step> DeleteStep(int stepId)
        {
            var step = await dBContext.Steps.FindAsync(stepId);
            if(step!=null)
            {
                dBContext.Steps.Remove(step);
                await dBContext.SaveChangesAsync();
            }
            return step;
        }

        public IEnumerable<Step> GetAllSteps()
        {
            return dBContext.Steps;
        }

        public async Task<Step> GetStep(int stepId)
        {
            return await dBContext.Steps.FindAsync(stepId);
        }

        public async Task<Step> UpdateStep(Step steps)
        {
            var stepsChanges = dBContext.Steps.Attach(steps);
            stepsChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dBContext.SaveChangesAsync();
            return steps;
        }

        public bool IsStepExist(int stepId)
        {
            return dBContext.Steps.Any(a=>a.Id==stepId);
        }



    }
}
