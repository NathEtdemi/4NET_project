
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
                BrandId = dbEntity.BrandId,
                Name = dbEntity.Name
            };
        }
    }
}
