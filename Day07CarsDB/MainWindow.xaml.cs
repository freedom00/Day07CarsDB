using CsvHelper;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Day07CarsDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Car> carList = new List<Car>();

        public MainWindow()
        {
            InitializeComponent();
            //lvCars.ItemsSource = carList;

            RefreshListAndStatus();
        }

        private void RefreshListAndStatus()
        {
            try
            {
                lvCars.ItemsSource = Globals.DB.GetAllCars();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(this, ex.Message, "Input Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            lvCars.Items.Refresh();
            //lblStatus.Text = string.Format("There are {0} item(s) on the list", carList.Count);
            lblStatus.Text = string.Format("There are {0} item(s) on the list", Globals.DB.GetAllCars().Count);
        }

        private void lvCars_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Car car = (Car)lvCars.SelectedValue;
            if (car == null)
            {
                return;
            }
            //AddAndEdit(carList, car);
            AddAndEdit(car);
        }

        private void miAdd_Click(object sender, RoutedEventArgs e)
        {
            //AddAndEdit(carList);
            AddAndEdit();
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            Car car = (Car)lvCars.SelectedValue;
            if (car == null)
            {
                return;
            }
            //carList.Remove(car);
            try
            {
                Globals.DB.DeleteCar(car.Id);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(this, ex.Message, "Input Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RefreshListAndStatus();
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            Car car = (Car)lvCars.SelectedValue;
            if (car == null)
            {
                return;
            }
            //AddAndEdit(carList, car);
            AddAndEdit(car);
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Globals.DB.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(this, ex.Message, "Input Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Close();
        }

        private void miExportToCSV_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.csv)|*.csv|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    //csv.WriteRecords(carList);
                    try
                    {
                        csv.WriteRecords(Globals.DB.GetAllCars());
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(this, ex.Message, "Input Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
        }

        private void AddAndEdit(List<Car> cars, Car car = null)
        {
            new AddEditDialog(cars, car).ShowDialog();
            RefreshListAndStatus();
        }

        private void AddAndEdit(Car car = null)
        {
            new AddEditDialog(car).ShowDialog();
            RefreshListAndStatus();
        }
    }
}