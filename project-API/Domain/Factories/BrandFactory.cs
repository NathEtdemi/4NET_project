using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Shared.ApiModels;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace project_API.Domain.Factories
{
    public class BrandFactory
    {
        public static BrandModel? ConvertToApiModel(Brand? dbEntity)
        {
            if (dbEntity == null)
                return null;

            return new BrandModel
            {
                Id = dbEntity.Id,
                Name = dbEntity.Name,
                CarModels = dbEntity.CarModels
                    .Select(x => CarModelFactory.ConvertToApiModel(x))
                    .ToList()
            };
        }
    }
}
