using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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


        public async Task<IEnumerable<Step>> GetAllSteps()
        {
            return await dBContext.Steps.Include(s => s.Goal).ToListAsync();
            //return dBContext.Steps;
        }


        public async Task<Step> GetStep(int stepId)
        {
            return await dBContext.Steps.Include(s => s.Goal).FirstOrDefaultAsync(m => m.Id == stepId);
            //return await dBContext.Steps.FindAsync(stepId);
        }


        public async Task<Step> UpdateStep(Step steps)
        {
            var stepsChanges = dBContext.Steps.Attach(steps);
            stepsChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dBContext.SaveChangesAsync();
            return steps;
        }


        public async Task<bool> IsStepExist(int stepId)
        {
            return await dBContext.Steps.AnyAsync(a=>a.Id==stepId);
        }



    }
}
