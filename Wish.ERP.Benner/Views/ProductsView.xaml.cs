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

namespace Wish_ERP.Views
{
    public partial class ProductsView : UserControl
    {
        private static ProductsView instance = null;
        public static ProductsView Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductsView();
                }
                return instance;
            }
        }
        public ProductsView()
        {
            InitializeComponent();
        }


        private void DeleteProduct(object sender, RoutedEventArgs e)
        {

        }
        private void EditProduct(object sender, RoutedEventArgs e)
        {

        }
        private void CreateProduct(object sender, RoutedEventArgs e)
        {

        }

    }
}
