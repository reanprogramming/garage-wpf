using Garage_Management_System.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Garage_Management_System.Garage
{
    /// <summary>
    /// Interaction logic for FrmProducts.xaml
    /// </summary>
    public partial class FrmProducts : Page
    {
        public FrmProducts()
        {
            InitializeComponent();
        }

        sqlexcute sql = new sqlexcute();
        string vTranCode = string.Empty;
        function_customize fc = new function_customize();

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            // Insert the previewed text into the existing text in the textbox.
            var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            double val;
            // If parsing is successful, set Handled to false
            e.Handled = !double.TryParse(fullText, out val);
        }

        void FrmProducts_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cleartext();
                Enable_save();

                sql.loadComboBox(cboInactive, " proc_sql_excute 'Active','" + variables.PBranchCode + "','',''", "Name", "Code");
                sql.loadComboBox(cboLine_ID, " proc_sql_excute 'PRO_LINE','" + variables.PBranchCode + "','',''", "Name", "Code");

            }
            catch { }
        }

        void cleartext()
        {
            txtpro_code.Text = string.Empty;
            cboLine_ID.SelectedIndex = -1;
            cboInactive.SelectedIndex = -1;
            txtbarcode.Text = string.Empty;
            txtname.Text = string.Empty;
            txtcost.Text = string.Empty;
            txtunitprice.Text = string.Empty;
            txtdiscount.Text = string.Empty;
            txtremark.Text = string.Empty;
            vTranCode = string.Empty;
        }

        void Enable_save()
        {
            btnNew.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        void Enable_edit()
        {
            btnNew.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnEdit.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }

        void Enable_ReadOnly()
        {
            btnNew.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        bool IsOk()
        {
            if (txtbarcode.Text.Trim() == string.Empty)
            {
                cboLine_ID.Focus();
                MessageBox.Show("Please enter barcode of product !", variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if(txtcost.Text.Trim() == string.Empty)
            {
                txtcost.Focus();
                MessageBox.Show("Please enter cost !", variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (txtunitprice.Text.Trim() == string.Empty)
            {
                txtunitprice.Focus();
                MessageBox.Show("Please enter unitprice !", variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (txtdiscount.Text.Trim() == string.Empty)
            {
                txtdiscount.Focus();
                MessageBox.Show("Please enter discount !", variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (sql.cbo_empty(cboLine_ID) == false)
            {
                cboLine_ID.Focus();
                MessageBox.Show("Please select sub Category !", variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (sql.cbo_empty(cboInactive) == false)
            {
                cboInactive.Focus();
                MessageBox.Show("Please select sub Category !", variables.vTittle, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }


        void Register(string vStatus, string vCode, string vMsg)
        {
            try
            {
                DataTable dt = new DataTable();
                List<parasql> arr = new List<parasql>();
                arr.Add(new parasql { paraname = "@CMD", sqltype = SqlDbType.NVarChar, values = vStatus });
                arr.Add(new parasql { paraname = "@vPro_ID", sqltype = SqlDbType.NVarChar, values = vCode });
                arr.Add(new parasql { paraname = "@vBranchcode", sqltype = SqlDbType.NVarChar, values = variables.PBranchCode });
                arr.Add(new parasql { paraname = "@vBarcode", sqltype = SqlDbType.NVarChar, values = txtbarcode.Text.Trim() });
                arr.Add(new parasql { paraname = "@vLin_ID", sqltype = SqlDbType.NVarChar, values = cboLine_ID.SelectedValue.ToString() });
                arr.Add(new parasql { paraname = "@vName", sqltype = SqlDbType.NVarChar, values = txtname.Text.Trim() });
                arr.Add(new parasql { paraname = "@vCost", sqltype = SqlDbType.NVarChar, values = txtcost.Text.Trim() });
                arr.Add(new parasql { paraname = "@vUniprice", sqltype = SqlDbType.NVarChar, values = txtunitprice.Text.Trim() });
                arr.Add(new parasql { paraname = "@vDiscount", sqltype = SqlDbType.NVarChar, values = txtdiscount.Text.Trim() });
                arr.Add(new parasql { paraname = "@vInactive", sqltype = SqlDbType.NVarChar, values = cboInactive.SelectedValue.ToString() });
                arr.Add(new parasql { paraname = "@vRemark", sqltype = SqlDbType.NVarChar, values = txtremark.Text.Trim() });
                arr.Add(new parasql { paraname = "@vInputter", sqltype = SqlDbType.NVarChar, values = variables.PInputter });
                dt = sql.Data_Execute("garage_register_product", arr);
                if (dt.Rows.Count > 0)
                {
                    txtpro_code.Text = dt.Rows[0]["TRANCODE"].ToString();
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

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cleartext();
                Enable_save();
            }
            catch { }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsOk() == true)
            {
                Register("I", "", variables.vMsg_insert);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
