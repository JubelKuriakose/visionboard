using VisionBoard.Models;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public interface IErrorLogRepository
    {
        Task<ErrorLog> AddErrorLog(string className, string methodName, string errorMessage);

    }
}
