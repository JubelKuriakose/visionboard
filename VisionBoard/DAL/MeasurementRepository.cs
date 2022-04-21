using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace VisionBoard.DAL
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly GoalTrackerContext dBContext;
        private readonly IErrorLogRepository errorLogRepository;

        public MeasurementRepository(GoalTrackerContext appDBContext, IErrorLogRepository errorLogRepository)
        {
            this.dBContext = appDBContext;
            this.errorLogRepository = errorLogRepository;
        }


        public async Task<Measurement> AddMeasurement(Measurement measurement)
        {
            try
            {
                await dBContext.Measurements.AddAsync(measurement);
                await dBContext.SaveChangesAsync();
                return measurement;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;            
        }


        public async Task<Measurement> DeleteMeasurement(int measurementId)
        {
            try
            {
                Measurement measurement = await dBContext.Measurements.FindAsync(measurementId);

                if (measurement != null)
                {
                    dBContext.Measurements.Remove(measurement);
                    await dBContext.SaveChangesAsync();
                    return measurement;
                }
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;            

        }


        public async Task<IEnumerable<Measurement>> GetAllMeasurements()
        {
            try
            {
                return await dBContext.Measurements.Include(m => m.Goal).ToListAsync();
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<Measurement> GetMeasurement(int measurementId)
        {
            try
            {
                return await dBContext.Measurements.Include(m => m.Goal).FirstAsync(m => m.Id == measurementId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<Measurement> UpdateMeasurement(Measurement measurement)
        {
            try
            {
                var measurementsChanges = dBContext.Measurements.Attach(measurement);
                measurementsChanges.State = EntityState.Modified;
                await dBContext.SaveChangesAsync();
                return measurement;
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return null;
            
        }


        public async Task<bool> IsMeasurementExist(int measurementId)
        {
            try
            {
                return dBContext.Measurements.Any(a => a.Id == measurementId);
            }
            catch (Exception ex)
            {
                await errorLogRepository.AddErrorLog(ex.TargetSite.ReflectedType.DeclaringType.Name, ex.TargetSite.ReflectedType.Name, ex.Message);
            }
            return false;
            
        }


    }
}
