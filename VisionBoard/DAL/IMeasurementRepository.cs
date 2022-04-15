using VisionBoard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisionBoard.DAL
{
    public interface IMeasurementRepository
    {
        Task<IEnumerable<Measurement>> GetAllMeasurements();

        Task<Measurement> GetMeasurement(int measurementId);

        Task<Measurement> AddMeasurement(Measurement measurement);

        Task<Measurement> UpdateMeasurement(Measurement measurement);

        Task<Measurement> DeleteMeasurement(int measurementId);

        bool IsMeasurementExist(int MeasurementId);

    }
}
