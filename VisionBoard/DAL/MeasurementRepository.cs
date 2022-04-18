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

        public MeasurementRepository(GoalTrackerContext appDBContext)
        {
            this.dBContext = appDBContext;
        }


        public async Task<Measurement> AddMeasurement(Measurement measurement)
        {
            await dBContext.Measurements.AddAsync(measurement);
            await dBContext.SaveChangesAsync();
            return measurement;
        }


        public async Task<Measurement> DeleteMeasurement(int measurementId)
        {
            Measurement measurement = null;
            try
            {
                measurement = await dBContext.Measurements.FindAsync(measurementId);

                if (measurement != null)
                {
                    dBContext.Measurements.Remove(measurement);
                    await dBContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
            return measurement;

        }


        public async Task<IEnumerable<Measurement>> GetAllMeasurements()
        {
            return await dBContext.Measurements.Include(m => m.Goal).ToListAsync();
        }


        public async Task<Measurement> GetMeasurement(int measurementId)
        {
            return await dBContext.Measurements.Include(m => m.Goal).FirstAsync(m => m.Id == measurementId);
        }


        public async Task<Measurement> UpdateMeasurement(Measurement measurement)
        {
            var measurementsChanges = dBContext.Measurements.Attach(measurement);
            measurementsChanges.State = EntityState.Modified;
            await dBContext.SaveChangesAsync();
            return measurement;
        }


        public bool IsMeasurementExist(int measurementId)
        {
            return dBContext.Measurements.Any(a => a.Id == measurementId);
        }


    }
}
