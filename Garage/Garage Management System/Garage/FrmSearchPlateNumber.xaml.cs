using Garage_Management_System.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Garage_Management_System.Garage
{
  /// <summary>
  /// Interaction logic for FrmSearchPlateNumber.xaml
  /// </summary>
  public partial class FrmSearchPlateNumber : Window
  {
    public FrmSearchPlateNumber()
    {
      InitializeComponent();
    }
    sqlexcute sql = new sqlexcute();
    string vDraft_ID = string.Empty;

    string vplate_id = string.Empty;


    void load_record(string search)
    {
      try
      {
        DataTable dt = new DataTable();
        // Show data to DataGrid
        List<arr_plateNumber> list = new List<arr_plateNumber>();
        string[] p =
        {
                "plate_number",
                variables.PBranchCode,
                search
            };
        dt = sql.proc_getdata("proc_sql_garage_search", p);
        if (dt.Rows.Count > 0)
        {
          for (int i = 0; i < dt.Rows.Count; i++)
          {
            list.Add(new arr_plateNumber() { Plate_ID = dt.Rows[i]["plate_id"].ToString(), Titile = dt.Rows[i]["title"].ToString(), Type_ID = dt.Rows[i]["TypeName"].ToString(), inputter = dt.Rows[i]["inputter"].ToString(), LastUpdate = dt.Rows[i]["inputdate"].ToString() });
          }

          dgData.Items.Refresh();
          dgData.ItemsSource = list;
          vplate_id = string.Empty;
        }
        else
        {
          dgData.Items.Refresh();
          dgData.ItemsSource = list;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
    void FrmSearchPlateNumber_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        this.Title = "SEARCH PLATE NUMBER";
        load_record("");
      }
      catch { }
    }

    private void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        load_record(txtSearch.Text.Trim());
      }
    }

    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        load_record(txtSearch.Text.Trim());
      }
      catch { }
    }


    private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      try
      {
        object item = dgData.SelectedItem;
        if (item != null)
        {
          search.search_action = String.Empty;
          vDraft_ID = (dgData.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
        }
      }
      catch { }
    }

    private void dgData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      search.tran_id = vDraft_ID;
      search.search_action = variables.vDraft_Fix;
      this.Close();
    }

  }



}
