using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using project_API.Domain;
using project_API.Domain.Factories;
using Shared.ApiModels;
using System.Reflection.Metadata;

namespace project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly ILogger<BrandController> _logger;

        private DbSet<Brand> BrandRepository => _dataContext.Set<Brand>();

        public BrandController(ApplicationDbContext context,
            ILogger<BrandController> logger)
        {
            _dataContext = context;
            _logger = logger;
        }

        [HttpPost("AddBrand")]
        public IActionResult CreateBrand(string brandName)
        {
            var newBrand = new Brand()
            {
                Name = brandName
            };
            BrandRepository.Add(newBrand);

            _dataContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetBrands()
        {
            return Ok(BrandRepository
                .Include(x => x.CarModels)
                .AsEnumerable()
                .Select(x => BrandFactory.ConvertToApiModel(x))
                .ToList()
                );
        }

        [HttpGet("{brandId}")]
        public IActionResult GetBrand(int brandId)
        {
            return Ok(BrandFactory.ConvertToApiModel(_dataContext.Set<Brand>()
                .FirstOrDefault(x => x.Id == brandId)));
        }

        [HttpPut("{brandId}")]
        public IActionResult EditBrand(int brandId, string newName)
        {
            var dbBrand = BrandRepository
                .FirstOrDefault(x => x.Id == brandId);

            if (dbBrand == null)
            {
                _logger.LogWarning($"No brand found with Id: {brandId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            dbBrand.Name = newName;

            BrandRepository.Update(dbBrand);

            _dataContext.SaveChanges();
            _logger.LogInformation($"The brand with Id: {dbBrand.Id} and name: {dbBrand.Name} has been edited");
            return Ok();
        }

        [HttpDelete("{brandId}")]
        public IActionResult DeleteBrand(int brandId)
        {
            var dbBrand = BrandRepository
                .FirstOrDefault(x => x.Id == brandId);

            if (dbBrand == null)
            {
                _logger.LogWarning($"No brand found with Id: {brandId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            _dataContext.Set<Brand>()
                .Remove(dbBrand);

            _dataContext.SaveChanges();

            _logger.LogInformation($"The brand with id {brandId} has been deleted!");
            return Ok();
        }
    }
}
