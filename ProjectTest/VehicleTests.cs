using Shared.ApiModels;
using System.ComponentModel.DataAnnotations;

namespace ProjectTest
{
    [TestClass]
    public class VehicleTests
    {
        [TestMethod]
        public void NumberPlateIsValid()
        {
            var vehicle = new VehicleModel();

            // Créer un vehicule pour vérifier l'immatriculation



            var validationResults = ValidateModel(vehicle);

            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual("La plaque d'immatriculation n'est pas valide", validationResults[0].ErrorMessage);
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}