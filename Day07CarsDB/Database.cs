using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Day07CarsDB
{
    internal class Database
    {
        private static Database uniqueInstance;
        private static readonly object locker = new object();
        private const string CONNECTION_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\jonat\OneDrive\Documents\VisualStudio\workspaces\Day07CarsDB\Day07CarsDB\CarsDB.mdf;Integrated Security=True";
        private SqlConnection sqlConnection;

        private Database()
        {
            sqlConnection = new SqlConnection(CONNECTION_STRING);
            sqlConnection.Open();
        }

        public static Database GetInstance()
        {
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new Database();
                    }
                }
            }
            return uniqueInstance;
        }

        public void Close()
        {
            sqlConnection.Close();
        }

        public int AddCar(Car car)
        {
            SqlCommand cmdInsert = new SqlCommand("INSERT INTO Cars(MakeModel, EngineSizeL, FuelType) VALUES(@MakeModel, @EngineSizeL, @FuelType); SELECT SCOPE_IDENTITY()", sqlConnection);
            cmdInsert.Parameters.AddWithValue("@MakeModel", car.MakeModel);
            cmdInsert.Parameters.AddWithValue("@EngineSizeL", car.EngineSizeL);
            cmdInsert.Parameters.AddWithValue("@FuelType", car.FuelType);
            int id = Convert.ToInt32(cmdInsert.ExecuteScalar().ToString());
            cmdInsert.Dispose();
            return id;
        }

        public List<Car> GetAllCars()
        {
            SqlCommand cmdSelectAll = new SqlCommand("SELECT * FROM Cars", sqlConnection);
            SqlDataReader selectAllReader = cmdSelectAll.ExecuteReader();
            List<Car> result = new List<Car>();
            while (selectAllReader.Read())
            {
                int id = selectAllReader.GetInt32(selectAllReader.GetOrdinal("Id"));
                string makeModel = selectAllReader.GetString(selectAllReader.GetOrdinal("MakeModel"));
                double engineSizeL = selectAllReader.GetDouble(selectAllReader.GetOrdinal("EngineSizeL"));
                Car.FuelTypeEnum fuelType = (Car.FuelTypeEnum)Enum.Parse(typeof(Car.FuelTypeEnum), selectAllReader.GetString(selectAllReader.GetOrdinal("FuelType")));
                result.Add(new Car(id, makeModel, engineSizeL, fuelType));
            }
            selectAllReader.Close();
            cmdSelectAll.Dispose();
            return result;
        }

        public void UpdateCar(Car car)
        {
            SqlCommand cmdUpdate = new SqlCommand("UPDATE Cars SET MakeModel=@MakeModel, EngineSizeL=@EngineSizeL,FuelType=@FuelType WHERE Id=@Id", sqlConnection);
            cmdUpdate.Parameters.AddWithValue("@MakeModel", car.MakeModel);
            cmdUpdate.Parameters.AddWithValue("@EngineSizeL", car.EngineSizeL);
            cmdUpdate.Parameters.AddWithValue("@FuelType", car.FuelType.ToString());
            cmdUpdate.Parameters.AddWithValue("@Id", car.Id);
            cmdUpdate.ExecuteNonQuery();
            cmdUpdate.Dispose();
        }

        public void DeleteCar(int id)
        {
            SqlCommand cmdDelete = new SqlCommand("DELETE FROM Cars WHERE Id=@id", sqlConnection);
            cmdDelete.Parameters.AddWithValue("@Id", id);
            cmdDelete.ExecuteNonQuery();
            cmdDelete.Dispose();
        }
    }
}