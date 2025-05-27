using Garage_Management_System.Class;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Garage_Management_System.Garage
{
  /// <summary>
  /// Interaction logic for FrmPlateNumber.xaml
  /// </summary>
  public partial class FrmPlateNumber : Page
  {
    public FrmPlateNumber()
    {
      InitializeComponent();
    }
    sqlexcute sql = new sqlexcute();
    string vTranCode = string.Empty;
    function_customize fc = new function_customize();

    void cleartext()
    {
      txtplate_id.Text = string.Empty;
      cboInactive.SelectedIndex = -1;
      cboType.SelectedIndex = -1;
      txtPlateNumber.Text = string.Empty;
      txtremark.Text = string.Empty;
      txtremark.Text = string.Empty;
      vTranCode = string.Empty;
    }

    void Enable_save()
    {
      btnNew.IsEnabled = true;
      btnSave.IsEnabled = true;
      btnEdit.IsEnabled = false;
    }

    void Enable_edit()
    {
      btnNew.IsEnabled = true;
      btnSave.IsEnabled = false;
      btnEdit.IsEnabled = true;
    }

    void Enable_ReadOnly()
    {
      btnNew.IsEnabled = true;
      btnSave.IsEnabled = false;
      btnEdit.IsEnabled = false;
    }

    bool IsOk()
    {
      if (txtPlateNumber.Text.Trim() == string.Empty)
      {
        txtPlateNumber.Focus();
        MessageBox.Show("Please enter plate number !", variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
        return false;
      }
      else if (sql.cbo_empty(cboType) == false)
      {
        cboType.Focus();
        MessageBox.Show("Please select plate type !", variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
        return false;
      }
      else if (sql.cbo_empty(cboInactive) == false)
      {
        cboInactive.Focus();
        MessageBox.Show("Please select inactive Yes Or No !", variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
        return false;
      }

      return true;
    }

    void FrmPlateNumber_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        cleartext();
        Enable_save();

        sql.loadComboBox(cboInactive, " proc_sql_excute 'Active','" + variables.PBranchCode + "','',''", "Name", "Code");
        sql.loadComboBox(cboType, " proc_sql_excute 'PlateType','" + variables.PBranchCode + "','',''", "Name", "Code");

      }
      catch { }
    }
    private void txtid_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        View_data("PlateNumber", txtplate_id.Text.Trim());
      }
    }


    void Register(string vStatus, string vCode, string vMsg)
    {
      try
      {
        DataTable dt = new DataTable();
        List<parasql> arr = new List<parasql>();
        arr.Add(new parasql { paraname = "@CMD", sqltype = SqlDbType.NVarChar, values = vStatus });
        arr.Add(new parasql { paraname = "@vPlate_ID", sqltype = SqlDbType.NVarChar, values = vCode });
        arr.Add(new parasql { paraname = "@vBranchcode", sqltype = SqlDbType.NVarChar, values = variables.PBranchCode });
        arr.Add(new parasql { paraname = "@vPlateNumber", sqltype = SqlDbType.NVarChar, values = txtPlateNumber.Text.Trim() });
        arr.Add(new parasql { paraname = "@vType_ID", sqltype = SqlDbType.NVarChar, values = cboType.SelectedValue.ToString() });
        arr.Add(new parasql { paraname = "@vInactive", sqltype = SqlDbType.NVarChar, values = cboInactive.SelectedValue.ToString() });
        arr.Add(new parasql { paraname = "@vRemark", sqltype = SqlDbType.NVarChar, values = txtremark.Text.Trim() });
        arr.Add(new parasql { paraname = "@vInputter", sqltype = SqlDbType.NVarChar, values = variables.PInputter });
        dt = sql.Data_Execute("garage_register_PlateNumber", arr);
        if (dt.Rows.Count > 0)
        {
          txtplate_id.Text = dt.Rows[0]["TRANCODE"].ToString();
          MessageBox.Show(vMsg, variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Information);
          cleartext();
          Enable_save();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    void View_data(string vStatus, string vCode)
    {
      try
      {
        DataTable dt = new DataTable();
        List<parasql> arr = new List<parasql>();
        arr.Add(new parasql { paraname = "@CMD", sqltype = SqlDbType.NVarChar, values = vStatus });
        arr.Add(new parasql { paraname = "@vBranchcode", sqltype = SqlDbType.NVarChar, values = variables.PBranchCode });
        arr.Add(new parasql { paraname = "@vCon1", sqltype = SqlDbType.NVarChar, values = vCode });
        dt = sql.Data_Execute("proc_sql_excute", arr);
        if (dt.Rows.Count > 0)
        {
          vTranCode = dt.Rows[0]["plate_id"].ToString();
          txtplate_id.Text = dt.Rows[0]["plate_id"].ToString();
          txtPlateNumber.Text = dt.Rows[0]["title"].ToString();
          cboType.SelectedValue = dt.Rows[0]["type_id"].ToString();
          cboInactive.SelectedValue = dt.Rows[0]["inactive"].ToString();
          txtremark.Text = dt.Rows[0]["remark"].ToString();

          if (dt.Rows[0]["btnReadOnly"].ToString() == "Yes")
          {
            Enable_ReadOnly();
          }
          else if (vStatus == variables.vDraft_Fix)
          {
            Enable_save();
          }
          else
          {
            Enable_edit();
          }
        }
        else
        {
          MessageBox.Show(variables.vMsg_Invalid, variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Information);
          cleartext();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }


    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        cleartext();
        Enable_save();
      }
      catch { }
    }

    private void btnCreate_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (IsOk() == true)
        {
          Register("I", "", variables.vMsg_insert);
        }
      }
      catch { }
    }

    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (IsOk() == true)
        {
          if (MessageBox.Show("Do you want to update the record id :" + txtplate_id.Text.Trim() + " now ?", variables.vTittle, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
          {
            Register("U", txtplate_id.Text.Trim(), variables.vMsg_update);
          }
        }
      }
      catch { }
    }

    private void cboInactive_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void View_search_Click(object sender, RoutedEventArgs e)
    {
      FrmSearchPlateNumber FrmDraft = new FrmSearchPlateNumber();
      search.search_action = String.Empty;
      FrmDraft.ShowDialog();

      if (search.tran_id != String.Empty && search.search_action != String.Empty)
      {
        View_data("PlateNumber", search.tran_id);
      }
    }
  }
}
