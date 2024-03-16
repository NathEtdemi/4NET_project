using Shared.ApiModels;

namespace project_API.Domain.Factories
{
    public class MaintenanceFactory
    {
        public static MaintenanceModel? ConvertToApiModel(Maintenance? dbEntity)
        {
            if (dbEntity == null)
                return null;

            return new MaintenanceModel
            {
                Id = dbEntity.Id,
                VehicleId = dbEntity.VehicleId,
                CurrentKmNumber = dbEntity.CurrentKmNumber,
                WorkDescription = dbEntity.WorkDescription
            };
        }
    }
}
