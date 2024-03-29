﻿using Shared.ApiModels;

namespace project_API.Domain.Factories
{
    public class VehicleFactory
    {
        public static VehicleModel? ConvertToApiModel(Vehicle? dbEntity)
        {
            if (dbEntity == null)
                return null;

            return new VehicleModel
            {
                Id = dbEntity.Id,
				CarModel = CarModelFactory.ConvertToApiModel(dbEntity.CarModel),
                CarModelId = dbEntity.CarModelId,
                NumberPlate = dbEntity.NumberPlate,
                BuildYear = dbEntity.BuildYear,
                KmNumber = dbEntity.KmNumber,
                EnergySource = dbEntity.EnergySource,
                Maintenances = dbEntity.Maintenances
                    .Select(x => MaintenanceFactory.ConvertToApiModel(x))
                    .ToList()
            };
        }
    }
}
