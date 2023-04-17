﻿using System;
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
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();

            ProductListBox.ItemsSource = Core.DB.Product.ToList();
            CountLabel.Content = ProductListBox.Items.Count + "/" + Core.DB.Product.Count();

            ManufacturerBox.Items.Add("Все производители");
            for (int i = 0; i < Core.DB.Manufacturer.Count(); i++)
            {
                ManufacturerBox.Items.Add(Core.DB.Manufacturer.Select(m => m.ManufacturerName).ToList()[i]);
            }
            ManufacturerBox.SelectedIndex = 0;

            SortBox.Items.Add("По возрастанию цены");
            SortBox.Items.Add("По убыванию цены");
            SortBox.SelectedIndex = 0;

            if (Core.currentUser == null)
            {
                RoleLabel.Content = "Гость";
                return;
            }
            RoleLabel.Content = Core.currentUser.Role.RoleName;
            NameLabel.Content = Core.currentUser.UserSurname + " " + Core.currentUser.UserName + " " + Core.currentUser.UserPatronymic;
        }

        private void ManufacturerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Search();
        }

        void Search()
        {
            if (SearchBox.Text.Length == 0)
            {
                if (ManufacturerBox.SelectedIndex == 0)
                {
                    if (SortBox.SelectedIndex == 0)
                    {
                        ProductListBox.ItemsSource = Core.DB.Product.OrderBy(p => p.ProductCost).ToList();
                    }
                    else
                    {
                        ProductListBox.ItemsSource = Core.DB.Product.OrderByDescending(p => p.ProductCost).ToList();
                    }
                }
                else
                {
                    if (SortBox.SelectedIndex == 0)
                    {
                        ProductListBox.ItemsSource = Core.DB.Product.Where(p => p.Manufacturer.ManufacturerName == ManufacturerBox.SelectedItem.ToString()).OrderBy(p => p.ProductCost).ToList();
                    }
                    else
                    {
                        ProductListBox.ItemsSource = Core.DB.Product.Where(p => p.Manufacturer.ManufacturerName == ManufacturerBox.SelectedItem.ToString()).OrderByDescending(p => p.ProductCost).ToList();
                    }
                }
            }
            else
            {
                if (ManufacturerBox.SelectedIndex == 0)
                {
                    if (SortBox.SelectedIndex == 0)
                    {
                        ProductListBox.ItemsSource = Core.DB.Product.Where(p => p.ProductArticleNumber.Contains(SearchBox.Text) || p.ProductCategory.Contains(SearchBox.Text) || p.ProductDescription.Contains(SearchBox.Text) || p.Manufacturer.ManufacturerName.Contains(SearchBox.Text) || p.ProductName.Contains(SearchBox.Text)).OrderBy(p => p.ProductCost).ToList();
                    }
                    else
                    {
                        ProductListBox.ItemsSource = Core.DB.Product.Where(p => p.ProductArticleNumber.Contains(SearchBox.Text) || p.ProductCategory.Contains(SearchBox.Text) || p.ProductDescription.Contains(SearchBox.Text) || p.Manufacturer.ManufacturerName.Contains(SearchBox.Text) || p.ProductName.Contains(SearchBox.Text)).OrderByDescending(p => p.ProductCost).ToList();
                    }
                }
                else
                {
                    if (SortBox.SelectedIndex == 0)
                    {
                        ProductListBox.ItemsSource = Core.DB.Product.Where(p => p.Manufacturer.ManufacturerName == ManufacturerBox.SelectedItem.ToString()).Where(p => p.ProductArticleNumber.Contains(SearchBox.Text) || p.ProductCategory.Contains(SearchBox.Text) || p.ProductDescription.Contains(SearchBox.Text) || p.Manufacturer.ManufacturerName.Contains(SearchBox.Text) || p.ProductName.Contains(SearchBox.Text)).OrderBy(p => p.ProductCost).ToList();
                    }
                    else
                    {
                        ProductListBox.ItemsSource = Core.DB.Product.Where(p => p.Manufacturer.ManufacturerName == ManufacturerBox.SelectedItem.ToString()).Where(p => p.ProductArticleNumber.Contains(SearchBox.Text) || p.ProductCategory.Contains(SearchBox.Text) || p.ProductDescription.Contains(SearchBox.Text) || p.Manufacturer.ManufacturerName.Contains(SearchBox.Text) || p.ProductName.Contains(SearchBox.Text)).OrderByDescending(p => p.ProductCost).ToList();
                    }
                }
            }
            CountLabel.Content = ProductListBox.Items.Count + "/" + Core.DB.Product.Count();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void ExitBTN_Click(object sender, RoutedEventArgs e)
        {
            Core.ExitSystem();
        }

        private void CreateBTN_Click(object sender, RoutedEventArgs e)
        {
            Core.mainWindow.MainFrame.Navigate(new EditProductPage());
        }

        private void EditBTN_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListBox.SelectedItem == null)
            {
                MessageBox.Show("Не выбран элемент!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Product product = ProductListBox.SelectedItem as Product;
                Core.mainWindow.MainFrame.Navigate(new EditProductPage(product.ProductArticleNumber));
            }
            catch
            {
                MessageBox.Show("Произошла ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }

        private void DeleteBTN_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListBox.SelectedItem == null)
            {
                MessageBox.Show("Не выбран элемент!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить это?", "Проверка", MessageBoxButton.YesNo, MessageBoxImage.Hand) == MessageBoxResult.Yes)
            {
                try
                {
                    Product product = ProductListBox.SelectedItem as Product;
                    Product delProduct = Core.DB.Product.Where(p => p.ProductArticleNumber == product.ProductArticleNumber).FirstOrDefault();
                    Core.DB.Product.Remove(delProduct);
                    Core.DB.SaveChanges();
                    Search();
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void EditCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (EditCheckBox.IsChecked == true)
            {
                SortStack.Visibility = Visibility.Collapsed;
                EditStack.Visibility = Visibility.Visible;
            }
            else
            {
                SortStack.Visibility = Visibility.Visible;
                EditStack.Visibility = Visibility.Collapsed;
            }
        }
    }
}
