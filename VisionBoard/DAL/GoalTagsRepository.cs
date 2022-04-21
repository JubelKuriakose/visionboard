using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace VisionBoard.DAL
{
    public class GoalTagsRepository : IGoalTagsRepository
    {
        private readonly GoalTrackerContext dBContext;
        private readonly IErrorLogRepository errorLogRepository;

        public GoalTagsRepository(GoalTrackerContext appDBContext, IErrorLogRepository errorLogRepository)
        {
            this.dBContext = appDBContext;
            this.errorLogRepository = errorLogRepository;
        }


        public async Task<GoalTags> AddGoalTagsMapping(GoalTags goalTagsMapping)
        {
            try
            {
                await dBContext.GoalTags.AddAsync(goalTagsMapping);
                await dBContext.SaveChangesAsync();
                return goalTagsMapping;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<GoalTags> DeleteGoalTagsMapping(int goalId, int tagId)
        {
            try
            {
                var goalTagsMapping = await dBContext.GoalTags.FirstAsync(gt => gt.GoalId == goalId && gt.TagId == tagId);
                if (goalTagsMapping != null)
                {
                    dBContext.GoalTags.Remove(goalTagsMapping);
                    await dBContext.SaveChangesAsync();
                }
                return goalTagsMapping;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<IEnumerable<GoalTags>> GetAllGoalTagsMappings()
        {
            try
            {
                return await dBContext.GoalTags.Include(s => s.Tag).ToListAsync();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
                       
        }


        public async Task<IList<Tag>> GetAllGoalTagsMappings(int goalId)
        {
            try
            {
                return await dBContext.GoalTags.Where(s => s.GoalId == goalId).Select(s => s.Tag).ToListAsync();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<GoalTags> GetGoalTagsMapping(int goalId, int tagId)
        {
            try
            {
                return await dBContext.GoalTags.FirstOrDefaultAsync(m => m.GoalId == goalId && m.TagId == tagId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<GoalTags> UpdateGoalTagsMapping(GoalTags goalTagsMapping)
        {
            try
            {
                var goalTagsMappingChanges = dBContext.GoalTags.Attach(goalTagsMapping);
                goalTagsMappingChanges.State = EntityState.Modified;
                await dBContext.SaveChangesAsync();
                return goalTagsMapping;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<bool> IsGoalTagsMappingExist(int goalId, int tagId)
        {
            try
            {
                return await dBContext.GoalTags.AnyAsync(a => a.GoalId == goalId && a.TagId == tagId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return false;
            
        }



    }
}
