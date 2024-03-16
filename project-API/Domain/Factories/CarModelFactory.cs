
using Shared.ApiModels;

namespace project_API.Domain.Factories
{
    public class CarModelFactory
    {
        public static CarModelModel? ConvertToApiModel(CarModel? dbEntity)
        {
            if (dbEntity == null)
                return null;

            return new CarModelModel
            {
                Id = dbEntity.Id,
                ModelBrand = BrandFactory.ConvertToApiModel(dbEntity.ModelBrand),
                BrandId = dbEntity.BrandId,
                Name = dbEntity.Name,
                MaintenanceFrequency = dbEntity.MaintenanceFrequency,
            };
        }
    }
}
