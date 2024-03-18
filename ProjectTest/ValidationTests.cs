using Shared.ApiModels;
using Shared.FormModels;
using System.ComponentModel.DataAnnotations;

namespace ProjectTest
{
    [TestClass]
    public class ValidationTests
    {
		private IList<ValidationResult> ValidateModel(object model)
		{
			var validationResults = new List<ValidationResult>();
			var ctx = new ValidationContext(model, null, null);
			Validator.TryValidateObject(model, ctx, validationResults, true);
			return validationResults;
		}

		[TestMethod]
        public void NumberPlateIsValid()
        {
			// Arrange
			var vehicle = new VehicleFormModel
			{
				CarModelId = 1,
				NumberPlate = "ABC123",
				BuildYear = 2009,
				KmNumber = 90000,
				EnergySource = Shared.Energy.Gasoline
			};

			// Act
			var validationResults = ValidateModel(vehicle);

			// Assert
			// Invalide car 6 caractères
			Assert.IsTrue(validationResults.Count > 0);
			Assert.AreEqual("L'immatriculation doit être comprise entre 7 et 9 caractères", validationResults[0].ErrorMessage);

			// Valide car 7 caractères
			vehicle.NumberPlate = "ABCD123";
			validationResults = ValidateModel(vehicle);
			Assert.IsTrue(validationResults.Count == 0);

			// Valide car 9 caractères
			vehicle.NumberPlate = "ABC123456";
			validationResults = ValidateModel(vehicle);
			Assert.IsTrue(validationResults.Count == 0);

			// Invalide car 10 caractères
			vehicle.NumberPlate = "ABCDEFGHIJ";
			validationResults = ValidateModel(vehicle);
			Assert.IsTrue(validationResults.Count > 0);
			Assert.AreEqual("L'immatriculation doit être comprise entre 7 et 9 caractères", validationResults[0].ErrorMessage);
        }

		[TestMethod]
		public void CarModelNameIsValid()
		{
			// Arrange
			var carModel = new CarModelFormModel
			{
				Id = 1,
				BrandId = 1,
				Name = "",
				MaintenanceFrequency = 20000
			};

			// Act
			var validationResults = ValidateModel(carModel);

			// Assert
			// Invalide car 0 caractère
			Assert.IsTrue(validationResults.Count > 0);
			Assert.AreEqual("Le champ nom du modèle est requis", validationResults[0].ErrorMessage);

			// Valide car au moins 1 caractère
			carModel.Name = "P";
			validationResults = ValidateModel(carModel);
			Assert.IsTrue(validationResults.Count == 0);

			// Valide car au moins 1 caractère
			carModel.Name = "Clio";
			validationResults = ValidateModel(carModel);
			Assert.IsTrue(validationResults.Count == 0);
		}

		[TestMethod]
		public void VehicleKmNumberIsPositive()
		{
			// Arrange
			var vehicle = new VehicleFormModel
			{
				CarModelId = 1,
				NumberPlate = "AB-123-BC",
				BuildYear = 2009,
				KmNumber = -500,
				EnergySource = Shared.Energy.Gasoline
			};

			// Act
			var validationResults = ValidateModel(vehicle);

			// Assert
			// Invalide car négatif
			Assert.IsTrue(validationResults.Count > 0);
			Assert.AreEqual("La valeur des km doit être comprise entre 0 et 999 999 km", validationResults[0].ErrorMessage);

			// Valide car nul
			vehicle.KmNumber = 0;
			validationResults = ValidateModel(vehicle);
			Assert.IsTrue(validationResults.Count == 0);

			// Valide car positif
			vehicle.KmNumber = 75000;
			validationResults = ValidateModel(vehicle);
			Assert.IsTrue(validationResults.Count == 0);
		}

		[TestMethod]
		public void MaintenanceWorkDescriptionIsValid()
		{
			// Arrange
			var maintenanceModel = new MaintenanceFormModel
			{
				Id = 1,
				VehicleId = 1,
				WorkDescription = ""
			};

			// Act
			var validationResults = ValidateModel(maintenanceModel);

			// Assert
			// Invalide car 0 caractère
			Assert.IsTrue(validationResults.Count > 0);
			Assert.AreEqual("Le champ détail des travaux est requis", validationResults[0].ErrorMessage);

			// Valide car non vide
			maintenanceModel.WorkDescription = "Révision";
			validationResults = ValidateModel(maintenanceModel);
			Assert.IsTrue(validationResults.Count == 0);
		}
    }
}