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
using Microsoft.Win32;
using System.Dynamic;

namespace GKS3_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath;

        public MainWindow()
        {
            InitializeComponent();

            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                filePath = fileDialog.FileName;
            }
            else
            {
                Environment.Exit(0);
            }

            MakeElementsReadOnly();

            ChangeVisibility(false);
            ClearData();
            SetMaxSizes();
        }

        private void PutData()
        {
            ClearData();
            ChangeVisibility(true);

            Dictionary<int, string> strings = IO.ParseStrings(filePath);
            var list = new List<List<string>>();

            foreach (var item in strings)
            {
                list.Add(item.Value.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }
            AddListOfListsToDataGrid(InitialDataGrid, list);

            var comparationMatrix = Operands.CompareLines(strings.Values.ToList());
            AddListOfListsToDataGrid(ComparationDataGrid, comparationMatrix);

            var groupped = Groupping.Group(comparationMatrix);
            var unique = Operands.GetUniqueInGroups(strings, groupped);
            AddDictionaryToTextBlock(GroupsUTextBlock, unique);


            var sorted = Groupping.SortGroups(strings, unique);
            AddDictionaryToTextBlock(GroupsTextBlock, sorted);
        }

        private void ClearData()
        {
            GroupsUTextBlock.Text = "";
            GroupsTextBlock.Text = "";

            InitialDataGrid.ItemsSource = null;
            ComparationDataGrid.ItemsSource = null;
        }

        private void ChangeVisibility(bool visible)
        {            
            if (visible == true)
            {
                GroupsUTextBlock.Visibility = Visibility.Visible;
                GroupsTextBlock.Visibility = Visibility.Visible;
                InitialDataGrid.Visibility = Visibility.Visible;
                ComparationDataGrid.Visibility = Visibility.Visible;
            }
            else
            {
                GroupsUTextBlock.Visibility = Visibility.Hidden;
                GroupsTextBlock.Visibility = Visibility.Hidden;
                InitialDataGrid.Visibility = Visibility.Hidden;
                ComparationDataGrid.Visibility = Visibility.Hidden;
            }
        }

        private void MakeElementsReadOnly()
        {
            InitialDataGrid.IsReadOnly = true;
            InitialDataGrid.CanUserAddRows = false;
            InitialDataGrid.CanUserDeleteRows = false;
            InitialDataGrid.CanUserReorderColumns = false;
            InitialDataGrid.CanUserResizeColumns = false;
            InitialDataGrid.CanUserResizeRows = false;
            InitialDataGrid.CanUserSortColumns = false;
            ComparationDataGrid.IsReadOnly = true;
            ComparationDataGrid.CanUserAddRows = false;
            ComparationDataGrid.CanUserDeleteRows = false;
            ComparationDataGrid.CanUserReorderColumns = false;
            ComparationDataGrid.CanUserResizeColumns = false;
            ComparationDataGrid.CanUserResizeRows = false;
            ComparationDataGrid.CanUserSortColumns = false;
        }

        private void SetMaxSizes()
        {
            InitialDataGrid.MaxWidth = 189.4;
            InitialDataGrid.MaxHeight = 373.4;

            ComparationDataGrid.MaxWidth = 344.4;
            ComparationDataGrid.MaxHeight = 373.4;

            GroupsUTextBlock.MaxWidth = 300;
            GroupsUTextBlock.MaxHeight = 150;

            GroupsTextBlock.MaxWidth = 300;
            GroupsTextBlock.MaxHeight = 150;

            if (InitialDataGrid.Width == InitialDataGrid.Width || InitialDataGrid.Height == InitialDataGrid.Height)
            {
                throw new StackOverflowException();
            }

            if (ComparationDataGrid.Width == ComparationDataGrid.Width || ComparationDataGrid.Height == ComparationDataGrid.Height)
            {
                throw new StackOverflowException();
            }
        }

        private void AddListOfListsToDataGrid<T>(DataGrid grid, List<List<T>> list)
        {
            var maxLength = list.Max(c => c.Count);

            for (int i = 0; i < maxLength; i++)
            {
                grid.Columns.Add(new DataGridTextColumn() { Header = String.Format("{0,-2}", i), Binding = new Binding("Row" + i) });
            }

            foreach (var listItem in list)
            {
                var properties = new Dictionary<string, object>();
                for (int i = 0; i < listItem.Count; i++)
                {
                    properties.Add("Row" + i, listItem[i].ToString());
                }
                var myObject = Extensions.GetDynamicObject(properties);

                grid.Items.Add(myObject);
            }
        }

        private void AddDictionaryToTextBlock(TextBlock textBlock, Dictionary<List<int>, string> dictionary)
        {
            var maxLength = dictionary.Max(c => c.Key.Count);
            var maxd = dictionary.Max(c => c.Key.Max(z => z.ToString().Length));
            var max = maxLength * maxd + maxLength + 3;

            Paragraph myParagraph = new Paragraph();
            FlowDocument myFlowDocument = new FlowDocument();

            foreach (var item in dictionary)
            {
                string keys = String.Join(" ", item.Key);

                textBlock.Text += String.Format("{0}\t\t {1}{2}", keys, item.Value, Environment.NewLine);
            }
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        private void ComparationDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PutData();
        }
    }
}