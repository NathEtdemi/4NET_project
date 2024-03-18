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
			// Invalide car 6 caract�res
			Assert.IsTrue(validationResults.Count > 0);
			Assert.AreEqual("L'immatriculation doit �tre comprise entre 7 et 9 caract�res", validationResults[0].ErrorMessage);

			// Valide car 7 caract�res
			vehicle.NumberPlate = "ABCD123";
			validationResults = ValidateModel(vehicle);
			Assert.IsTrue(validationResults.Count == 0);

			// Valide car 9 caract�res
			vehicle.NumberPlate = "ABC123456";
			validationResults = ValidateModel(vehicle);
			Assert.IsTrue(validationResults.Count == 0);

			// Invalide car 10 caract�res
			vehicle.NumberPlate = "ABCDEFGHIJ";
			validationResults = ValidateModel(vehicle);
			Assert.IsTrue(validationResults.Count > 0);
			Assert.AreEqual("L'immatriculation doit �tre comprise entre 7 et 9 caract�res", validationResults[0].ErrorMessage);
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
			// Invalide car 0 caract�re
			Assert.IsTrue(validationResults.Count > 0);
			Assert.AreEqual("Le champ nom du mod�le est requis", validationResults[0].ErrorMessage);

			// Valide car au moins 1 caract�re
			carModel.Name = "P";
			validationResults = ValidateModel(carModel);
			Assert.IsTrue(validationResults.Count == 0);

			// Valide car au moins 1 caract�re
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
			// Invalide car n�gatif
			Assert.IsTrue(validationResults.Count > 0);
			Assert.AreEqual("La valeur des km doit �tre comprise entre 0 et 999 999 km", validationResults[0].ErrorMessage);

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
			// Invalide car 0 caract�re
			Assert.IsTrue(validationResults.Count > 0);
			Assert.AreEqual("Le champ d�tail des travaux est requis", validationResults[0].ErrorMessage);

			// Valide car non vide
			maintenanceModel.WorkDescription = "R�vision";
			validationResults = ValidateModel(maintenanceModel);
			Assert.IsTrue(validationResults.Count == 0);
		}
    }
}