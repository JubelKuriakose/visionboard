using VisionBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VisionBoard.DAL
{
    public class MesurementRepository : IMesurementRepository
    {
        private readonly GoalTrackerContext dBContext;

        public MesurementRepository(GoalTrackerContext appDBContext)
        {
            this.dBContext = appDBContext;
        }

        public async Task<Mesurement> AddMesurement(Mesurement mesurement)
        {
            await dBContext.Mesurements.AddAsync(mesurement);
            await dBContext.SaveChangesAsync();
            return mesurement;
        }

        public async Task<Mesurement> DeleteMesurement(int mesurementId)
        {
            var mesurement = await dBContext.Mesurements.FindAsync(mesurementId);
            if(mesurement!=null)
            {
                dBContext.Mesurements.Remove(mesurement);
                await dBContext.SaveChangesAsync();
            }
            return mesurement;
        }

        public async Task<IEnumerable<Mesurement>> GetAllMesurements()
        {
            return await dBContext.Mesurements.Include(m => m.Goal).ToListAsync();
        }

        public async Task<Mesurement> GetMesurement(int mesurementId)
        {
            return await dBContext.Mesurements.FindAsync(mesurementId);
        }

        public async Task<Mesurement> UpdateMesurement(Mesurement mesurements)
        {
            var mesurementsChanges = dBContext.Mesurements.Attach(mesurements);
            mesurementsChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dBContext.SaveChangesAsync();
            return mesurements;
        }

        public bool IsMesurementExist(int mesurementId)
        {
            return dBContext.Mesurements.Any(a=>a.Id==mesurementId);
        }



    }
}
