using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_API.Domain;
using project_API.Domain.Factories;
using Shared.FormModels;

namespace project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly ILogger<MaintenanceController> _logger;

        private DbSet<Maintenance> MaintenanceRepository => _dataContext.Set<Maintenance>();

        public MaintenanceController(ApplicationDbContext context,
            ILogger<MaintenanceController> logger)
        {
            _dataContext = context;
            _logger = logger;
        }

        [HttpPost("AddMaintenance")]
        public IActionResult CreateMaintenance([FromBody] MaintenanceFormModel maintenanceFormModel)
        {
            var dbVehicle = _dataContext.Set<Vehicle>().FirstOrDefault(x => x.Id == maintenanceFormModel.VehicleId);

            if (dbVehicle == null)
            {
                _logger.LogWarning($"No vehicle found with Id: {maintenanceFormModel.VehicleId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var newMaintenance = new Maintenance()
            {
                Vehicle = dbVehicle,
                VehicleId = maintenanceFormModel.VehicleId,
                CurrentKmNumber = dbVehicle.KmNumber,
                WorkDescription = maintenanceFormModel.WorkDescription
			};
            MaintenanceRepository.Add(newMaintenance);

            _dataContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetMaintenances()
        {
            return Ok(MaintenanceRepository
                .AsEnumerable()
                .Select(x => MaintenanceFactory.ConvertToApiModel(x))
                .ToList()
                );
        }

        [HttpGet("{maintenanceId}")]
        public IActionResult GetMaintenance(int maintenanceId)
        {
            var dbMaintenance = MaintenanceFactory.ConvertToApiModel(MaintenanceRepository.FirstOrDefault(x => x.Id == maintenanceId));

            if (dbMaintenance == null)
            {
                _logger.LogWarning($"No maintenance found with Id: {maintenanceId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(dbMaintenance);
        }


        [HttpPut("{maintenanceId}")]
        public IActionResult EditMaintenance(int maintenanceId, int vehicleId, int currentKmNumber, string workDescription)
        {
            var dbMaintenance = MaintenanceRepository
                .FirstOrDefault(x => x.Id == maintenanceId);

            if (dbMaintenance == null)
            {
                _logger.LogWarning($"No maintenance found with Id: {maintenanceId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var dbVehicle = _dataContext.Set<Vehicle>().FirstOrDefault(x => x.Id == vehicleId);

            if (dbVehicle == null)
            {
                _logger.LogWarning($"No vehicle found with Id: {vehicleId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            dbMaintenance.Vehicle = dbVehicle;
            dbMaintenance.VehicleId = vehicleId;
            dbMaintenance.CurrentKmNumber = currentKmNumber;
            dbMaintenance.WorkDescription = workDescription;

            MaintenanceRepository.Update(dbMaintenance);

            _dataContext.SaveChanges();
            _logger.LogInformation($"The maintenance with Id: {dbMaintenance.Id} for the vehicle with id : {dbVehicle.Id} has been edited");
            return Ok();
        }

        [HttpDelete("{maintenanceId}")]
        public IActionResult DeleteMaintenance(int maintenanceId)
        {
            var dbMaintenance = MaintenanceRepository
                .FirstOrDefault(x => x.Id == maintenanceId);

            if (dbMaintenance == null)
            {
                _logger.LogWarning($"No maintenance found with Id: {maintenanceId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            MaintenanceRepository.Remove(dbMaintenance);

            _dataContext.SaveChanges();

            _logger.LogInformation($"The maintenance with id {maintenanceId} has been deleted!");
            return Ok();
        }
    }
}
