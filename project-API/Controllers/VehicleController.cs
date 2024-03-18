using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_API.Domain;
using project_API.Domain.Factories;
using Shared;
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
        public IActionResult CreateVehicle(int carModelId, string numberPlate, int buildYear, int KmNumber, Energy energy)
        {
            var dbCarModel = _dataContext.Set<CarModel>()
                .FirstOrDefault(x => x.Id == carModelId);

            if (dbCarModel == null)
            {
                _logger.LogWarning($"No CarModel found with Id: {carModelId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var newVehicle = new Vehicle()
            {
                CarModel = dbCarModel,
                CarModelId = dbCarModel.Id,
                NumberPlate = numberPlate,
                BuildYear = buildYear,
                KmNumber = KmNumber,
                EnergySource = energy
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

        //[HttpPut("{vehicleId}")]
        //public IActionResult EditVehicle(int vehicleId, int modelId, string numberPlate, int buildYear, int kmNumber, Energy energy)
        //{
        //    var dbVehicle = VehicleRepository
        //        .FirstOrDefault(x => x.Id == vehicleId);

        //    if (dbVehicle == null)
        //    {
        //        _logger.LogWarning($"No vehicle found with Id: {vehicleId}");
        //        return StatusCode(StatusCodes.Status404NotFound);
        //    }

        //    var dbCarModel = _dataContext.Set<CarModel>()
        //        .FirstOrDefault(x => x.Id == modelId);

        //    if (dbCarModel == null)
        //    {
        //        _logger.LogWarning($"No CarModel found with Id: {modelId}");
        //        return StatusCode(StatusCodes.Status404NotFound);
        //    }

        //    dbVehicle.VModel = dbCarModel;
        //    dbVehicle.CarModelId = modelId;
        //    dbVehicle.NumberPlate = numberPlate;
        //    dbVehicle.BuildYear = buildYear;
        //    dbVehicle.KmNumber = kmNumber;
        //    dbVehicle.EnergySource = energy;


        //    VehicleRepository.Update(dbVehicle);

        //    _dataContext.SaveChanges();
        //    _logger.LogInformation($"The vehicle with Id: {dbVehicle.Id} has been edited");
        //    return Ok();
        //}

        

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
                    return null;
                })
                .Where(x => x != null)
                .ToList();

            return Ok(overdueVehicles);
        }

    }
}
