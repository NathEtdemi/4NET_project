using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using project_API.Domain;
using project_API.Domain.Factories;
using Shared.ApiModels;

namespace project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarModelController : ControllerBase
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly ILogger<CarModelController> _logger;

        private DbSet<CarModel> CarModelRepository => _dataContext.Set<CarModel>();

        public CarModelController(ApplicationDbContext context,
            ILogger<CarModelController> logger)
        {
            _dataContext = context;
            _logger = logger;
        }

        [HttpPost("AddCarModel")]
        public IActionResult CreateCarModel([FromBody]CarModelModel newCarModelModel)
        {
            var newCarModel = new CarModel()
            {
                BrandId = newCarModelModel.BrandId,
                Name = newCarModelModel.Name,
                MaintenanceFrequency = newCarModelModel.MaintenanceFrequency
            };
            CarModelRepository.Add(newCarModel);

            _dataContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetCarModels()
        {
            return Ok(CarModelRepository
                .AsEnumerable()
                .Select(x => CarModelFactory.ConvertToApiModel(x))
                .ToList());
        }

        [HttpGet("{carModelId}")]
        public IActionResult GetCarModel(int carModelId)
        {
            return Ok(CarModelFactory.ConvertToApiModel(_dataContext.Set<CarModel>()
                .FirstOrDefault(x => x.Id == carModelId)));
        }

        [HttpPut("{brandId}")]
        public IActionResult EditBrand([FromBody] CarModelModel carModelToEdit)
        {
            var dbCarModel = CarModelRepository
                .FirstOrDefault(x => x.Id == carModelToEdit.Id);

            if (dbCarModel == null)
            {
                _logger.LogWarning($"No CarModel found with Id: {carModelToEdit.Id}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            dbCarModel.Name = carModelToEdit.Name;
            dbCarModel.BrandId = carModelToEdit.BrandId;
            dbCarModel.MaintenanceFrequency = carModelToEdit.MaintenanceFrequency;

            CarModelRepository.Update(dbCarModel);

            _dataContext.SaveChanges();
            _logger.LogInformation($"The brand with Id: {dbCarModel.Id} and name: {dbCarModel.Name} has been edited");
            return Ok();
        }

        [HttpDelete("{carModelId}")]
        public IActionResult DeleteCarModel(int carModelId)
        {
            var dbCarModel = CarModelRepository
                .FirstOrDefault(x => x.Id == carModelId);

            if (dbCarModel == null)
            {
                _logger.LogWarning($"No car model found with Id: {carModelId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            _dataContext.Set<CarModel>()
                .Remove(dbCarModel);

            _dataContext.SaveChanges();

            _logger.LogInformation($"The car model with id {carModelId} has been deleted!");
            return Ok();
        }
    }
}
