using System;
using System.Collections.Generic;
using System.Data;
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

namespace DataTableBindingTemplate
{
    public class Auftrag
    {
        public string ID { get; set; }
        public int Soll { get; set; }
    }
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTable table = new DataTable();
            DataColumn snrCol = new DataColumn("Sachnummer", typeof(string));
            table.Columns.Add(snrCol);
            for (int i = 0; i < 3; i++)
            {
                DataColumn auftragColumn = new DataColumn(DateTime.Now.AddDays(i).ToString("dd.MM.yyyy"), typeof(Auftrag));
                table.Columns.Add(auftragColumn);
            }

            for (int row = 0; row < 10; row++)
            {
                table.Rows.Add(table.NewRow());
                for (int col = 0; col < 4; col++)
                {
                    if (col == 0)
                        table.Rows[row][col] = $"{row}";
                    else
                        table.Rows[row][col] = new Auftrag() { Soll = 100+row+col, ID=$"{row+col}"};
                }
            }

            MyDataGrid.DataContext = table;
        }

        private void MyDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(string))
            {
                var col = new DataGridTextColumn { Binding = new Binding(e.PropertyName) };
                e.Column = col;
                e.Column.Header = e.PropertyName;
            }
            else if (e.PropertyType == typeof(Auftrag))
            {

                var col = new DataRowColumn(e.PropertyName);
                col.CellTemplate = Resources["myTmpl"] as DataTemplate;
                e.Column = col;
                e.Column.Header = e.PropertyName;
            }
        }
    }

    public class DataRowColumn : DataGridTemplateColumn
    {
        public DataRowColumn(string column)
        {
            ColumnName = column;
        }
        public string ColumnName { get; set; }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            if (dataItem is DataRowView)
            {
                var row = (DataRowView)dataItem;
                var item = row[ColumnName];
                cell.DataContext = item;
                return base.GenerateElement(cell, item);
            }
            else
                return base.GenerateEditingElement(cell, dataItem);
        }
    }
}
