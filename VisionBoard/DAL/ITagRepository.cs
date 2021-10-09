using VisionBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetAllTags();

        Task<Tag> GetTag(int tagId);

        Task<Tag> AddTag(Tag tag);

        Task<Tag> UpdateTag(Tag tag);

        Task<Tag> DeleteTag(int tagId);

        bool IsTagExist(int TagId);

    }
}
