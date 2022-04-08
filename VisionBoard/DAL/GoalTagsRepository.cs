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


        public GoalTagsRepository(GoalTrackerContext appDBContext)
        {
            this.dBContext = appDBContext;
        }


        public async Task<GoalTags> AddGoalTagsMapping(GoalTags goalTagsMapping)
        {
            await dBContext.GoalTags.AddAsync(goalTagsMapping);
            await dBContext.SaveChangesAsync();
            return goalTagsMapping;
        }


        public async Task<GoalTags> DeleteGoalTagsMapping(int goalId, int tagId)
        {
            var goalTagsMapping = await dBContext.GoalTags.FirstAsync(gt => gt.GoalId == goalId && gt.TagId == tagId);
            if(goalTagsMapping != null)
            {
                dBContext.GoalTags.Remove(goalTagsMapping);
                await dBContext.SaveChangesAsync();
            }
            return goalTagsMapping;
        }


        public async Task<IEnumerable<GoalTags>> GetAllGoalTagsMappings()
        {            
            return await dBContext.GoalTags.Include(s => s.Tag).ToListAsync();            
        }


        public async Task<IList<Tag>> GetAllGoalTagsMappings(int goalId)
        {
            return await dBContext.GoalTags.Where(s=>s.GoalId == goalId).Select(s => s.Tag).ToListAsync();            
        }


        public async Task<GoalTags> GetGoalTagsMapping(int goalId, int tagId)
        {
            return await dBContext.GoalTags.FirstOrDefaultAsync(m => m.GoalId == goalId && m.TagId==tagId);
        }


        public async Task<GoalTags> UpdateGoalTagsMapping(GoalTags goalTagsMapping)
        {
            var goalTagsMappingChanges = dBContext.GoalTags.Attach(goalTagsMapping);
            goalTagsMappingChanges.State = EntityState.Modified;
            await dBContext.SaveChangesAsync();
            return goalTagsMapping;
        }


        public async Task<bool> IsGoalTagsMappingExist(int goalId, int tagId)
        {
            return await dBContext.GoalTags.AnyAsync(a=>a.GoalId== goalId && a.TagId== tagId);
        }



    }
}
