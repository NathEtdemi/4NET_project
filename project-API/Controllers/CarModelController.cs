using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using project_API.Domain;
using project_API.Domain.Factories;
using Shared.ApiModels;
using Shared.FormModels;
using System.Collections.Generic;

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
        public IActionResult CreateCarModel([FromBody] CarModelFormModel carModelFormModel)
        {
            DbSet<Brand> brandRepository = _dataContext.Set<Brand>();
            var dbBrand = brandRepository
                .FirstOrDefault(x => x.Id == carModelFormModel.BrandId);

            if (dbBrand == null)
            {
                _logger.LogWarning($"No brand found with Id: {carModelFormModel.BrandId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var newCarModel = new CarModel()
            {
                Brand = dbBrand,
                BrandId = carModelFormModel.BrandId,
                Name = carModelFormModel.Name,
                MaintenanceFrequency = carModelFormModel.MaintenanceFrequency
			};
            CarModelRepository.Add(newCarModel);

            _dataContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetCarModels()
        {
            return Ok(CarModelRepository
                .Include(x => x.Brand)
                .AsEnumerable()
                .Select(x => CarModelFactory.ConvertToApiModel(x))
                .ToList());
        }

        [HttpGet("{carModelId}")]
        public IActionResult GetCarModel(int carModelId)
        {
            var dbCarModel = CarModelFactory.ConvertToApiModel(CarModelRepository
                .Include(x => x.Brand)
                .FirstOrDefault(x => x.Id == carModelId));

            if (dbCarModel == null)
            {
                _logger.LogWarning($"No car model found with Id: {carModelId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(dbCarModel);
        }

        [HttpPut("{carModelId}")]
        public IActionResult EditCarModel([FromBody] CarModelFormModel carModelFormModel)
        {
            var dbCarModel = CarModelRepository
                .FirstOrDefault(x => x.Id == carModelFormModel.Id);

            if (dbCarModel == null)
            {
                _logger.LogWarning($"No CarModel found with Id: {carModelFormModel.Id}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            DbSet<Brand> brandRepository = _dataContext.Set<Brand>();
            var dbBrand = brandRepository
                .FirstOrDefault(x => x.Id == carModelFormModel.BrandId);

            if (dbBrand == null)
            {
                _logger.LogWarning($"No brand found with Id: {carModelFormModel.BrandId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            dbCarModel.Brand = dbBrand;
            dbCarModel.BrandId = carModelFormModel.BrandId;
            dbCarModel.Name = carModelFormModel.Name;
            dbCarModel.MaintenanceFrequency = carModelFormModel.MaintenanceFrequency;

            CarModelRepository.Update(dbCarModel);

            _dataContext.SaveChanges();
            _logger.LogInformation($"The car model with Id: {dbCarModel.Id} and name: {dbCarModel.Name} has been edited");
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
            CarModelRepository.Remove(dbCarModel);

            _dataContext.SaveChanges();

            _logger.LogInformation($"The car model with id {carModelId} has been deleted!");
            return Ok();
        }

        [HttpGet("GetCarModelsByBrand")]
        public IActionResult GetCarModelsByBrand(int brandId)
        {
			var dbBrand = _dataContext.Set<Brand>()
				.FirstOrDefault(x => x.Id == brandId);

			if (dbBrand == null)
			{
				_logger.LogWarning($"No brand found with Id: {brandId}");
				return StatusCode(StatusCodes.Status404NotFound);
			}

            var dbCarModel = CarModelRepository
                .Include(x => x.Brand)
				.AsEnumerable()
				.Select(x => CarModelFactory.ConvertToApiModel(x))
                .Where(x => x.BrandId == brandId)
				.ToList();

			return Ok(dbCarModel);
		}
    }
}
