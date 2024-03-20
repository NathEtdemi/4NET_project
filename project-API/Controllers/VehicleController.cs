using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_API.Domain;
using project_API.Domain.Factories;
using Shared;
using Shared.ApiModels;
using Shared.FormModels;
using static Client.Pages.Index;

namespace project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly ILogger<VehicleController> _logger;

        private DbSet<Vehicle> VehicleRepository => _dataContext.Set<Vehicle>();

        public VehicleController(ApplicationDbContext context,
            ILogger<VehicleController> logger)
        {
            _dataContext = context;
            _logger = logger;
        }

        [HttpPost("AddVehicle")]
        public IActionResult CreateVehicle([FromBody] VehicleFormModel vehicleFormModel)
        {
            var dbCarModel = _dataContext.Set<CarModel>()
                .FirstOrDefault(x => x.Id == vehicleFormModel.CarModelId);

            if (dbCarModel == null)
            {
                _logger.LogWarning($"No CarModel found with Id: {vehicleFormModel.CarModelId}");
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }

			if (!ModelState.IsValid)
			{
                return BadRequest(ModelState);
            }

			var newVehicle = new Vehicle()
            {
                CarModel = dbCarModel,
                CarModelId = dbCarModel.Id,
                NumberPlate = vehicleFormModel.NumberPlate,
                BuildYear = vehicleFormModel.BuildYear,
                KmNumber = vehicleFormModel.KmNumber,
                EnergySource = vehicleFormModel.EnergySource
			};

            VehicleRepository.Add(newVehicle);

            _dataContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetVehicles()
        {
            return Ok(VehicleRepository
                .Include(x => x.CarModel)
                .Include(x => x.CarModel.Brand)
                .Include(x => x.Maintenances)
                .AsEnumerable()
                .Select(x => VehicleFactory.ConvertToApiModel(x))
                .ToList()
                );
        }

        [HttpGet("FullVehicle")]
        public IActionResult GetVehicle(int vehicleId)
        {
            var dbVehicle = VehicleFactory.ConvertToApiModel(VehicleRepository
                .Include(x => x.CarModel)
                .Include(x => x.CarModel.Brand)
                .FirstOrDefault(x => x.Id == vehicleId));

            if (dbVehicle == null)
            {
                _logger.LogWarning($"No vehicle found with Id: {vehicleId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(dbVehicle);
        }

        [HttpGet("GetWithMaintenances")]
        public IActionResult GetVehicleWithMaintenances(int vehicleId)
        {
            var dbVehicle = VehicleFactory.ConvertToApiModel(VehicleRepository
                .Include(x => x.CarModel)
                .Include(x => x.CarModel.Brand)
                .Include(x => x.Maintenances)
                .FirstOrDefault(x => x.Id == vehicleId));

            if (dbVehicle == null)
            {
                _logger.LogWarning($"No vehicle found with Id: {vehicleId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(dbVehicle);
        }

        [HttpPut("{vehicleId}")]
        public IActionResult EditVehicle([FromBody] VehicleFormModel vehicleFormModel)
        {
            var dbVehicle = VehicleRepository
                .FirstOrDefault(x => x.Id == vehicleFormModel.Id);

            if (dbVehicle == null)
            {
                _logger.LogWarning($"No vehicle found with Id: {vehicleFormModel.Id}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var dbCarModel = _dataContext.Set<CarModel>()
                .FirstOrDefault(x => x.Id == vehicleFormModel.CarModelId);

            if (dbCarModel == null)
            {
                _logger.LogWarning($"No CarModel found with Id: {vehicleFormModel.CarModelId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			dbVehicle.CarModel = dbCarModel;
            dbVehicle.CarModelId = vehicleFormModel.CarModelId;
            dbVehicle.NumberPlate = vehicleFormModel.NumberPlate;
            dbVehicle.BuildYear = vehicleFormModel.BuildYear;
            dbVehicle.KmNumber = vehicleFormModel.KmNumber;
            dbVehicle.EnergySource = vehicleFormModel.EnergySource;


            VehicleRepository.Update(dbVehicle);

            _dataContext.SaveChanges();
            _logger.LogInformation($"The vehicle with Id: {dbVehicle.Id} has been edited");
            return Ok();
        }



		[HttpDelete("{vehicleId}")]
        public IActionResult DeleteVehicle(int vehicleId)
        {
            var dbVehicle = VehicleRepository
                .FirstOrDefault(x => x.Id == vehicleId);

            if (dbVehicle == null)
            {
                _logger.LogWarning($"No vehicle found with Id: {vehicleId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            VehicleRepository.Remove(dbVehicle);

            _dataContext.SaveChanges();

            _logger.LogInformation($"The vehicle with id {vehicleId} has been deleted!");
            return Ok();
        }


        
        [HttpGet("GetMaintenanceOverdueVehicles")]
        public IActionResult GetMaintenanceOverdueVehicles()
        {
            var overdueVehicles = VehicleRepository
                .Include(x => x.CarModel)
                .Include(x => x.CarModel.Brand)
                .Include(x => x.Maintenances)
                .AsEnumerable()
                .Select(x =>
                {
                    var latestMaintenance = x.Maintenances.OrderByDescending(m => m.CurrentKmNumber).FirstOrDefault();
                    if (latestMaintenance != null && x.KmNumber - latestMaintenance.CurrentKmNumber > x.CarModel.MaintenanceFrequency)
                    {
                        var delay = x.KmNumber - latestMaintenance.CurrentKmNumber - x.CarModel.MaintenanceFrequency;
                        return new OverdueVehicle
                        {
                            Vehicle = VehicleFactory.ConvertToApiModel(x),
                            MaintenanceDelay = delay
                        };
                    }
                    if (latestMaintenance == null)
                    {
                        return new OverdueVehicle
                        {
                            Vehicle = VehicleFactory.ConvertToApiModel(x),
                            MaintenanceDelay = x.KmNumber - x.CarModel.MaintenanceFrequency
                        };
                    }
                    return null;
                })
                .Where(x => x != null)
                .ToList();

            return Ok(overdueVehicles);
        }

    }
}
