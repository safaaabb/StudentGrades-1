using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace StudentGrades
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable studentDataTable;
        public MainWindow()
        {
            InitializeComponent();
        }
        private DataTable LoadCsvData(string filePath)
        {
            DataTable dataTable = new DataTable();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    dataTable.Load(dr);
                }
            }

            return dataTable;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                string csvFilePath = openFileDialog.FileName;
                studentDataTable = LoadCsvData(csvFilePath);
                StudentDataGrid.ItemsSource = studentDataTable.DefaultView;
                for (int i = 2; i < studentDataTable.Columns.Count; i++)
                {
                    StudentDataGrid.Columns[i].Visibility = Visibility.Collapsed;
                }
            }

        }
        private void uploadGread()
        {
            DataRowView rowView = (DataRowView)StudentDataGrid.SelectedItem;
            // Create a new DataTable and define its columns
            DataTable studentDeTable = new DataTable();
            studentDeTable.Columns.Add("Property");
            studentDeTable.Columns.Add("Value");
            // Create a DataRow for each property and value
            double GradesSt=0;
            for (int i = 4; i < StudentDataGrid.Columns.Count; i++)
            {
                DataRow row = studentDeTable.NewRow();
                row["Property"] = StudentDataGrid.Columns[i].Header.ToString();
                row["Value"] = rowView[i].ToString();
                studentDeTable.Rows.Add(row);
                /*---------------------------------grades--------------------------------------*/
                string input = StudentDataGrid.Columns[i].Header.ToString();
                // Define the regular expression pattern to match numbers
                string pattern = @"\d+";
                // Create a Regex object
                Regex regex = new Regex(pattern);
                // Find matches
                Match match = regex.Match(input);
                double pre;
                double number;
                double.TryParse(match.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out pre);
                double.TryParse(rowView[i].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out number);
                GradesSt+= (pre / 100) * number;
            }
            // Set the DataGrid's ItemsSource to the DataTable's DefaultView
            StudentGradesGrid.ItemsSource = studentDeTable.DefaultView;
            GradesN.Text=GradesSt.ToString("f2");
        }
        
        private void uploadDetalis()
        {
            DataRowView rowView = (DataRowView)StudentDataGrid.SelectedItem;
            // Create a new DataTable and define its columns
            DataTable studentDeTable = new DataTable();
            studentDeTable.Columns.Add("Property");
            studentDeTable.Columns.Add("Value");
            // Create a DataRow for each property and value
            for (int i = 0; i < 4; i++)
            {
                DataRow row = studentDeTable.NewRow();   
                row["Property"] = StudentDataGrid.Columns[i].Header.ToString();
                row["Value"] = rowView[i].ToString();       
                studentDeTable.Rows.Add(row);
            }
            // Set the DataGrid's ItemsSource to the DataTable's DefaultView
            StudentDetDataGrid.ItemsSource = studentDeTable.DefaultView;
        }
        private void StudentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem != null)
            {
                uploadDetalis();
                uploadGread();
            }
        }
    }
}