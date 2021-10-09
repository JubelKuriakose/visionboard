using VisionBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public interface IMesurementRepository
    {
        IEnumerable<Mesurement> GetAllMesurements();

        Task<Mesurement> GetMesurement(int mesurementId);

        Task<Mesurement> AddMesurement(Mesurement mesurement);

        Task<Mesurement> UpdateMesurement(Mesurement mesurement);

        Task<Mesurement> DeleteMesurement(int mesurementId);

        bool IsMesurementExist(int MesurementId);

    }
}
