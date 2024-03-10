namespace project_API.Domain
{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CarModel> CarModels { get; set; }
    }
}
