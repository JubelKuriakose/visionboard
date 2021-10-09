using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public class TagRepository : ITagRepository
    {
        private readonly GoalTrackerContext dBContext;

        public TagRepository(GoalTrackerContext appDBContext)
        {
            this.dBContext = appDBContext;
        }

        public async Task<Tag> AddTag(Tag tag)
        {
            await dBContext.Tags.AddAsync(tag);
            await dBContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> DeleteTag(int tagId)
        {
            var tag = await dBContext.Tags.FindAsync(tagId);
            if(tag!=null)
            {
                dBContext.Tags.Remove(tag);
                await dBContext.SaveChangesAsync();
            }
            return tag;
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return dBContext.Tags;
        }

        public async Task<Tag> GetTag(int tagId)
        {
            return await dBContext.Tags.FindAsync(tagId);
        }

        public async Task<Tag> UpdateTag(Tag tags)
        {
            var tagsChanges = dBContext.Tags.Attach(tags);
            tagsChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dBContext.SaveChangesAsync();
            return tags;
        }

        public bool IsTagExist(int tagId)
        {
            return dBContext.Tags.Any(a=>a.Id==tagId);
        }



    }
}
