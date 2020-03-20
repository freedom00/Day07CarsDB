using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace Day07CarsDB
{
    /// <summary>
    /// Interaction logic for AddEditDialog.xaml
    /// </summary>
    public partial class AddEditDialog : Window
    {
        private List<Car> carList;
        private Car car;

        public AddEditDialog()
        {
            InitializeComponent();
        }

        public AddEditDialog(Car car)
        {
            this.car = car;
            InitializeComponent();
            AddEdit();
        }

        public AddEditDialog(List<Car> carList, Car car)
        {
            this.carList = carList;
            this.car = car;
            InitializeComponent();
            AddEdit();
        }

        private void AddEdit()
        {
            comboFuelType.ItemsSource = Enum.GetValues(typeof(Car.FuelTypeEnum));
            comboFuelType.SelectedItem = comboFuelType.Items[0];
            if (car == null)
            {
                btSave.Content = "Create";
            }
            else
            {
                btSave.Content = "Update";
                tbMakeModel.Text = car.MakeModel;
                slideEngineSizeL.Value = car.EngineSizeL;
                comboFuelType.Text = car.FuelType.ToString();
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            string makeModel = tbMakeModel.Text;
            double engineSizeL = slideEngineSizeL.Value;
            Car.FuelTypeEnum fuelType = (Car.FuelTypeEnum)comboFuelType.SelectedItem;
            try
            {
                if (car == null)
                {
                    //carList.Add(new Car(makeModel, engineSizeL, fuelType));
                    Globals.DB.AddCar(new Car(makeModel, engineSizeL, fuelType));
                }
                else
                {
                    car.MakeModel = makeModel;
                    car.EngineSizeL = engineSizeL;
                    car.FuelType = fuelType;
                    Globals.DB.UpdateCar(car);
                }
            }
            catch (InvalidParameterException ex)
            {
                MessageBox.Show(this, ex.Message, "Input Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(this, ex.Message, "Input Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.DialogResult = true;
        }
    }
}