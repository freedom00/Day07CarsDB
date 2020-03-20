using CsvHelper.Configuration.Attributes;

namespace Day07CarsDB
{
    public class Car
    {
        public enum FuelTypeEnum
        { Gasoline, Diesel, Hybrid, Electric, Other }

        private int id;
        private string makeModel;
        private double engineSizeL;
        private FuelTypeEnum fuelType;

        [Name("Id")]
        public int Id { get; set; }

        [Name("Make and Model")]
        public string MakeModel
        {
            get => makeModel;
            set
            {
                if (value.Length < 2 || value.Length > 50 || value.Contains(";"))
                {
                    throw new InvalidParameterException("Make and Model must be 2-50.");
                }
                makeModel = value;
            }
        }

        [Name("Engine Size (L)")]
        public double EngineSizeL
        {
            get => engineSizeL;
            set
            {
                if (value < 0 || value > 20)
                {
                    throw new InvalidParameterException("Engine size must be 0-20.");
                }
                engineSizeL = value;
            }
        }

        [Name("Fuel Type")]
        public FuelTypeEnum FuelType { get => fuelType; set => fuelType = value; }

        public Car(string makeModel, double engineSizeL, FuelTypeEnum fuelType)
        {
            MakeModel = makeModel;
            EngineSizeL = engineSizeL;
            FuelType = fuelType;
        }

        public Car(int id, string makeModel, double engineSizeL, FuelTypeEnum fuelType)
        {
            Id = id;
            MakeModel = makeModel;
            EngineSizeL = engineSizeL;
            FuelType = fuelType;
        }
    }
}