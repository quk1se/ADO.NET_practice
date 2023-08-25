using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace ADOProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        public ObservableCollection<String> columns { get; set; } = new();
        public ObservableCollection<DAL.Entity.Product> Products { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            connection = null!;
            DataContext = this;
        }
        private void Load_Groups()
        {
            using SqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM ShopOfProducts WHERE DeleteDt IS NULL";
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //columns.Add($"Id : {reader.GetGuid(0).ToString()[..4]}..., Name: {reader.GetString(1)}, Description : {reader.GetString(2)}, Picture : {reader.GetInt32(3)}, Quantity : {reader.GetInt32(4)}");
                    Products.Add(new()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetString(3),
                        Quantity = reader.GetString(4),
                        DeleteDt = reader.IsDBNull(5) ? DateTime.MinValue.ToString() : reader.GetDateTime(5).ToString(),
                    }); ;
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new(App.ConnectionString);
                connection.Open();
                Load_Groups();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            connection?.Dispose();
        }

        private void create_products_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText =
                @"CREATE TABLE ShopOfProducts (
                     Id            UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                     Name        NVARCHAR(50)     NOT NULL,
                     Description NTEXT            NOT NULL,
                     Price       NVARCHAR(50)     NULL,
                     Quantity    NVARCHAR(50)     NULL
                )";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("PRODUCTS CREATED");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void insert_products_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new();
            command.Connection = connection;
            command.CommandText =
                @"INSERT INTO ShopOfProducts
                    ( Id, Name,    Description, Price, Quantity, DeleteDt )
                VALUES
                ( '089015F4-31B5-4F2B-BA05-A813B5419285', N'Кавун',     N'Смачно', N'23', N'33', GETDATE()),
                ( 'A6D7858F-6B75-4C75-8A3D-C0B373828558', N'Сир',   N'Смачно', N'56', N'43', GETDATE()),
                ( 'DEF24080-00AA-440A-9690-3C9267243C43', N'Майонез',  N'Смачно', N'345', N'53', GETDATE()),
                ( '2F9A22BC-43F4-4F73-BAB1-9801052D85A9', N'Ковбаса', N'Смачно', N'24', N'342', NULL),
                ( 'D6D9783F-2182-469A-BD08-A24068BC2A23', N'Молоко', N'Смачно', N'112', N'88', NULL)";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Done");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
