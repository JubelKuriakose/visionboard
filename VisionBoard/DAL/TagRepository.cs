using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace VisionBoard.DAL
{
    public class TagRepository : ITagRepository
    {
        private readonly GoalTrackerContext dBContext;
        private readonly IErrorLogRepository errorLogRepository;

        public TagRepository(GoalTrackerContext appDBContext, IErrorLogRepository errorLogRepository)
        {
            this.dBContext = appDBContext;
            this.errorLogRepository = errorLogRepository;
        }

        public async Task<Tag> AddTag(Tag tag)
        {
            try
            {
                await dBContext.Tags.AddAsync(tag);
                await dBContext.SaveChangesAsync();
                return tag;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }

        public async Task<Tag> DeleteTag(int tagId)
        {
            try
            {
                var tag = await dBContext.Tags.FindAsync(tagId);
                if (tag != null)
                {
                    dBContext.Tags.Remove(tag);
                    await dBContext.SaveChangesAsync();
                }
                return tag;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }

        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            try
            {
                return await dBContext.Tags.ToListAsync();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }

        public async Task<Tag> GetTag(int tagId)
        {
            try
            {
                return await dBContext.Tags.FindAsync(tagId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }

        public async Task<Tag> UpdateTag(Tag tags)
        {
            try
            {
                var tagsChanges = dBContext.Tags.Attach(tags);
                tagsChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await dBContext.SaveChangesAsync();
                return tags;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
        }

        public async Task<bool> IsTagExist(int tagId)
        {
            try
            {
                return await dBContext.Tags.AnyAsync(a => a.Id == tagId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return false;
        }



    }
}
