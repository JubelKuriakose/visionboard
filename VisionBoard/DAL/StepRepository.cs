using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace VisionBoard.DAL
{
    public class StepRepository : IStepRepository
    {
        private readonly GoalTrackerContext dBContext;
        private readonly IErrorLogRepository errorLogRepository;

        public StepRepository(GoalTrackerContext appDBContext, IErrorLogRepository errorLogRepository )
        {
            this.dBContext = appDBContext;
            this.errorLogRepository = errorLogRepository;
        }


        public async Task<Step> AddStep(Step step)
        {
            try
            {
                await dBContext.Steps.AddAsync(step);
                await dBContext.SaveChangesAsync();
                return step;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<Step> DeleteStep(int stepId)
        {
            try
            {
                var step = await dBContext.Steps.FindAsync(stepId);
                if (step != null)
                {
                    dBContext.Steps.Remove(step);
                    await dBContext.SaveChangesAsync();
                }
                return step;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<IEnumerable<Step>> GetAllSteps()
        {
            try
            {
                return await dBContext.Steps.Include(s => s.Goal).ToListAsync();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }

        public async Task<IEnumerable<Step>> GetAllSteps(int goalId)
        {
            try
            {
                return await dBContext.Steps.Where(s => s.GoalId == goalId).Include(s => s.Goal).ToListAsync();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;            
            
        }


        public async Task<Step> GetStep(int stepId)
        {
            try
            {
                return await dBContext.Steps.Include(s => s.Goal).FirstOrDefaultAsync(m => m.Id == stepId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;            
            
        }


        public async Task<Step> UpdateStep(Step steps)
        {
            try
            {
                var stepsChanges = dBContext.Steps.Attach(steps);
                stepsChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await dBContext.SaveChangesAsync();
                return steps;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<bool> IsStepExist(int stepId)
        {
            try
            {
                return await dBContext.Steps.AnyAsync(a => a.Id == stepId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return false;
        }



    }
}
