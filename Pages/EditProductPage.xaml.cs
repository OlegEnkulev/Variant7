using System;
using System.Collections.Generic;
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

namespace Variant7.Pages
{
    public partial class EditProductPage : Page
    {
        bool mode = false; // false - Создание, true - Изменение
        Product product;

        public EditProductPage()
        {
            InitializeComponent();

            ManufacturerCBInit();
        }

        public EditProductPage(string articul)
        {
            InitializeComponent();

            ManufacturerCBInit();

            product = Core.DB.Product.Where(p => p.ProductArticleNumber == articul).FirstOrDefault();

            ArticulBox.Text = product.ProductArticleNumber;
            NameBox.Text = product.ProductName;
            PeaceBox.Text = product.ProductPeace;
            CostBox.Text = product.ProductCost.ToString();
            ManufacturerCB.SelectedItem = product.Manufacturer.ManufacturerName;
            DelivererBox.Text = product.ProductDeliverer;
            CategoryBox.Text = product.ProductCategory;
            CountBox.Text = product.ProductQuantityInStock.ToString();
            DescriptionBox.Text = product.ProductDescription;

            mode = true;
        }

        private void ManufacturerCBInit()
        {
            for (int i = 0; i < Core.DB.Manufacturer.Count(); i++)
            {
                ManufacturerCB.Items.Add(Core.DB.Manufacturer.ToList()[i].ManufacturerName);
            }
            ManufacturerCB.SelectedIndex = 0;
        }

        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            if (mode)
            {
                product.ProductArticleNumber = ArticulBox.Text;
                product.ProductName = NameBox.Text;
                product.ProductPeace = PeaceBox.Text;
                product.ProductCost = Convert.ToInt32(CostBox.Text);
                product.ProductManufacturer = Core.DB.Manufacturer.Where(c => c.ManufacturerName == ManufacturerCB.Text).FirstOrDefault().ManufacturerID;
                product.ProductDeliverer = DelivererBox.Text;
                product.ProductCategory = CategoryBox.Text;
                product.ProductQuantityInStock = Convert.ToInt32(CountBox.Text);
                product.ProductDescription = DescriptionBox.Text;

                Core.DB.Entry(product).State = System.Data.Entity.EntityState.Modified;
                Core.DB.SaveChanges();
                Core.mainWindow.MainFrame.Navigate(new AdminPage());
                
            }
            else
            {
                try
                {
                    Core.DB.Product.Add(new Product() { ProductArticleNumber = ArticulBox.Text, ProductName = NameBox.Text, ProductPeace = PeaceBox.Text, ProductCost = Convert.ToInt32(CostBox.Text), ProductCategory = CategoryBox.Text, ProductDeliverer = DelivererBox.Text, ProductManufacturer = Core.DB.Manufacturer.Where(c => c.ManufacturerName == ManufacturerCB.Text).FirstOrDefault().ManufacturerID, ProductQuantityInStock = Convert.ToInt32(CountBox.Text), ProductDescription = DescriptionBox.Text });
                    Core.DB.SaveChanges();
                    Core.mainWindow.MainFrame.Navigate(new AdminPage());
                }
                catch
                {
                    MessageBox.Show("Проверьте правильность введённых данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            Core.mainWindow.MainFrame.Navigate(new AdminPage());
        }
    }
}
