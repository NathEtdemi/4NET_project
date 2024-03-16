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
                Name = dbEntity.Name
            };
        }

        public static Brand? ConvertToDomainModel(BrandModel? dbEntity)
        {
            if (dbEntity == null)
                return null;

            return new Brand
            {
                Id = dbEntity.Id,
                Name = dbEntity.Name
            };
        }
    }
}
