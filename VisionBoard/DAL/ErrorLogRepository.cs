using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace VisionBoard.DAL
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly GoalTrackerContext dBContext;


        public ErrorLogRepository(GoalTrackerContext appDBContext)
        {
            this.dBContext = appDBContext;
        }


        public async Task<ErrorLog> AddErrorLog(string className, string methodName, string errorMessage)
        {
            try
            {
                ErrorLog errorLog = new ErrorLog
                {
                    ClassName = className,
                    MethodName = methodName,
                    ErrorMessage = errorMessage,
                    DateTime = DateTime.Now
                };
                await dBContext.ErrorLogs.AddAsync(errorLog);
                await dBContext.SaveChangesAsync();
                return errorLog;
            }
            catch (Exception ex)
            {               
            }
            return null;
            
        }


    }
}
