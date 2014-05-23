using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelectBuilder.Designer
{
    public partial class MainForm : Form
    {

        private SelectStatement _selectStatement;
        private SelectColumn _selectColumn;
        private WhereColumn _whereColumn;
        private bool _displaying;

        public MainForm()
        {
            InitializeComponent();
            LoadSelectStatements();
        }

        private void LoadSelectStatements()
        {
            Builder builder = new Builder(new DataSource());

            builder.LoadDefinitions();


            SelectStatement warrantsByAccount = builder.CreateSelect("inventory.Warrant", "w")
                 .Select("AccountId")
                 .Select("WarrantId", alias: "Warrants", aggregate: Aggregates.Count)
                 .Where("AccountId", Operators.IsNotNull, null, null);

            selectStatements.Items.Add(new SelectStatementListViewItem("Accounts",
                builder.CreateSelect("companies.Account", "a")
                .Join("a.AccountTypeCode", "at")
                .Join("a.CompanyId", "c")
                .Join("a.AccountId", warrantsByAccount, "x", "AccountId")

                .Select("a.AccountId", columnType: ColumnType.None)
                .Select("a.AccountCode")
                .Select("a.AccountName")
                .Select("at.AccountTypeName")
                .Select("c.CompanyCode")
                .Select("c.CompanyName")
                .Select("a.IsActive")
                .Select("x.Warrants")));

            selectStatements.Items.Add(new SelectStatementListViewItem("Companies",
                builder.CreateSelect("companies.Company", "c")
                .Join("c.CompanyTypeId", "ct")

                .Select("c.CompanyId", columnType: ColumnType.None)
                .Select("c.CompanyCode")
                .Select("c.CompanyName")
                .Select("ct.CompanyTypeName")
                .Select("c.Address")
                .Select("c.PostCode")
                .Select("c.Telephone")
                .Select("c.IsActive")
                .Select("c.LastModified")));

            selectStatements.Items.Add(new SelectStatementListViewItem("CompanyType",
                builder.CreateSelect("companies.CompanyType", "ct")

                .Select("CompanyTypeId", columnType: ColumnType.None)
                .Select("CompanyTypeCode")
                .Select("CompanyTypeName")
                .Select("CanSeeAllData")));

            selectStatements.Items.Add(new SelectStatementListViewItem("Configs",
                builder.CreateSelect("core.Config", "c")

                .Select("ConfigId", columnType: ColumnType.None)
                .Select("ConfigName")
                .Select("ConfigValue")
                .Select("IsActive")));

            selectStatements.Items.Add(new SelectStatementListViewItem("Contracts",
                builder.CreateSelect("core.Contract", "c")
                .Join("c.ExchangeCompanyId", "ec")
                .Join("c.ProductId", "p")

                .Select("c.ContractId", columnType: ColumnType.None)
                .Select("c.ContractCode")
                .Select("c.ContractName")
                .Select("ec.CompanyName")
                .Select("p.ProductName")
                .Select("c.IsActive")));

            selectStatements.Items.Add(new SelectStatementListViewItem("Locations",
                builder.CreateSelect("core.Location", "l")

                .Select("LocationId", columnType: ColumnType.None)
                .Select("LocationCode")
                .Select("LocationName")
                .Select("IsActive")));

            selectStatements.Items.Add(new SelectStatementListViewItem("Products",
                builder.CreateSelect("core.Product", "p")
                .Join("p.ProductTypeId", "pt")
                .Join("p.UnitMeasureId", "um")

                .Select("p.ProductId", columnType: ColumnType.None)
                .Select("p.ProductCode")
                .Select("p.ProductName")
                .Select("pt.ProductTypeName")
                .Select("um.UnitMeasureName")
                .Select("p.SizeMaxDp")
                .Select("p.IsActive")));

            selectStatements.Items.Add(new SelectStatementListViewItem("ProductTypes",
                builder.CreateSelect("core.ProductType", "pt")

                .Select("ProductTypeId", columnType: ColumnType.None)
                .Select("ProductTypeCode")
                .Select("ProductTypeName")));

            selectStatements.Items.Add(new SelectStatementListViewItem("UnitMeasures",
                builder.CreateSelect("core.UnitMeasure", "um")

                .Select("UnitMeasureId", columnType: ColumnType.None)
                .Select("UnitMeasureCode")
                .Select("UnitMeasureName")
                .Select("ConversionToBaseMeasure")
                .Select("IsActive")));


            selectStatements.Items.Add(new SelectStatementListViewItem("UserProfiles",
                builder.CreateSelect("dbo.UserProfile", "up")
                    .Join("up.CompanyId", "c")
                    .Join("c.CompanyTypeId", "ct")
                    .Join("up", "dbo.webpages_UsersInRoles", "uir")
                    .Join("uir.RoleId", "r")

                    .Select("up.UserId")
                    .Select("up.UserName")
                    .Select("ct.CompanyTypeName")
                    .Select("c.CompanyName")
                    .Select("up.EmailAddress")
                    .Select("r.RoleName")
                    .Select("up.FirstName")
                    .Select("up.LastName")
                    .Select("up.ActiveStatus")
                    .Select("up.IsWindowsUser")
                    .Select("up.LastModified")));


            //ToDo : need to filter on r.CreatedByCompanyId = @CompanyId OR r.ToCompanyId = @CompanyId
            selectStatements.Items.Add(new SelectStatementListViewItem("Requests",
                builder.CreateSelect("inventory.Request", "r")
                    .Join("r.RequestTypeId", "rt")
                    .Join("r.CreatedByCompanyId", "cbc")
                    .Join("r.FromCompanyId", "fc")
                    .Join("r.ToCompanyId", "tc")

                    .Select("r.RequestId")
                    .Select("rt.RequestTypeName")
                    .Select("cbc.CompanyCode", alias: "CreatedByCompanyCode")
                    .Select("fc.CompanyCode", alias: "FromCompanyCode")
                    .Select("tc.CompanyCode", alias: "ToCompanyCode")
                    .Select("r.Total")
                    .Select("r.IncompleteCount")
                    .Select("r.AcceptedCount")
                    .Select("r.RejectedCount")
                    .Select("r.DateTimeCreated")));

            selectStatements.Items.Add(new SelectStatementListViewItem("RequestWarrants",
                builder.CreateSelect("inventory.RequestWarrant", "rw")
                    .Join("rw.RequestId", "r")
                    .Join("r.RequestTypeId", "rt")
                    .Join("r.FromCompanyId", "fc")
                    .Join("r.ToCompanyId", "tc")
                    .Join("rw.WarrantId", "w")
                    .Join("rw.RequestStatusId", "rs")
                    .Join("w.StorageCompanyId", "sc")
                    .Join("w.StoreId", "s")
                    .Join("w.LocationId", "l")
                    .Join("w.ContractId", "c")
                    .Join("w.ContractUnitId", "cu")

                    .Select("rw.RequestWarrantId")
                    .Select("rt.RequestTypeName")
                    .Select("r.DateTimeCreated")
                    .Select("fc.CompanyCode", alias: "FromCompanyCode")
                    .Select("tc.CompanyCode", alias: "ToCompanyCode")
                    .Select("rs.RequestStatusName")
                    .Select("rw.IsAccepted")
                    .Select("w.WarrantNumber")
                    .Select("sc.CompanyCode", alias: "StorageCompanyCode")
                    .Select("s.StoreName")
                    .Select("l.LocationName")
                    .Select("c.ContractName")
                    .Select("cu.ContractUnitCode")));

            selectStatements.Items.Add(new SelectStatementListViewItem("Units",
                builder.CreateSelect("inventory.Unit", "u")
                    .Join("u.StoreId", "s")
                    .Join("u.StorageCompanyId", "sc")
                    .Join("u.LocationId", "l")
                    .Join("u.ProductId", "p")
                    .Join("u.UnitMeasureId", "um")
                    .Join("u.WarrantId", "w")

                    .Select("u.UnitId")
                    .Select("u.UnitNumber")
                    .Select("s.StoreCode")
                    .Select("sc.CompanyCode")
                    .Select("l.LocationName")
                    .Select("p.ProductName")
                    .Select("u.DateStored")
                    .Select("u.NetSize")
                    .Select("u.GrossSize")
                    .Select("um.UnitMeasureName")
                    .Select("w.WarrantNumber")
                    .SelectCustom("CASE WHEN u.WarrantId IS NOT NULL THEN 1 ELSE 0 END", "bit", "IsIssued")
                ));

            SelectStatement pendingWarrants = builder.CreateSelect("inventory.RequestWarrant", "rw", distinct: true)
                    .Select("rw.WarrantId")
                    .Where("rw.IsAccepted", Operators.IsNull, null, null);

            selectStatements.Items.Add(new SelectStatementListViewItem("Warrants",
                builder.CreateSelect("inventory.Warrant", "w")
                    .Join("w.StorageCompanyId", "sc")
                    .Join("w.AssignedToCompanyId", "atc")
                    .Join("w.AccountId", "a")
                    .Join("w.StoreId", "s")
                    .Join("w.LocationId", "l")
                    .Join("w.ContractId", "c")
                    .Join("w.ContractUnitId", "cu")
                    .Join("w.UnitMeasureId", "um")
                    .Join("w.WarrantId", pendingWarrants, "rw", "WarrantId")
                    .Select("w.WarrantId")
                    .Select("w.WarrantNumber")
                    .Select("sc.CompanyCode", alias: "StorageCompanyCode")
                    .Select("s.StoreCode")
                    .Select("l.LocationName")
                    .Select("c.ContractName")
                    .Select("cu.ContractUnitCode")
                    .Select("w.DateIssued")
                    .Select("w.NetSize")
                    .Select("w.GrossSize")
                    .Select("um.UnitMeasureName")
                    .Select("atc.CompanyCode", alias: "AssignedToCompanyCode")
                    .Select("a.AccountCode")
                    .SelectCustom("CASE WHEN rw.WarrantId IS NULL THEN CAST(0 as bit) ELSE CAST(1 as bit) END", "bit", "OnActiveRequest", dependentOnAliases: new[] { "rw" })
                    ));
        }

        private void selectStatements_SelectedIndexChanged(object sender, EventArgs e)
        {
            _displaying = true;

            _selectStatement = selectStatements.SelectedItems.Count == 1 ? ((SelectStatementListViewItem)selectStatements.SelectedItems[0]).SelectStatement : null;

            if (_selectStatement != null)
            {
                pageSize.Text = _selectStatement.PageSize > 0 ? _selectStatement.PageSize.ToString() : "";
                pageNo.Text = _selectStatement.PageNo > 0 ? _selectStatement.PageNo.ToString() : "";
            }

            RefreshSelects();
            RefreshWeres();

            _displaying = false;

            RefreshData();
        }

        private void selects_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ((SelectColumnListViewItem)e.Item).SelectColumn.IsVisible = e.Item.Checked;
            ((SelectColumnListViewItem)e.Item).RefreshText();
            RefreshData();
        }

        private void showHideMenuItem_Click(object sender, EventArgs e)
        {
            selects.SelectedItems[0].Checked = !selects.SelectedItems[0].Checked;
        }

        private void orderByMenuItem_Click(object sender, EventArgs e)
        {
            OrderBy(((SelectColumnListViewItem)selects.SelectedItems[0]).SelectColumn.Alias);
        }

        private void data_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            OrderBy(data.Columns[e.ColumnIndex].HeaderText);
        }

        private void OrderBy(string alias)
        {
            _selectStatement.OrderBy(alias);

            foreach (SelectColumnListViewItem selectColumnListViewItem in selects.Items)
            {
                selectColumnListViewItem.RefreshText();
            }

            RefreshData();
        }

        private void RefreshData()
        {
            if (_displaying)
                return;

            if (_selectStatement == null)
            {
                statement.Text = "";
                data.DataSource = null;
            }
            else
            {
                WhereColumn whereColumn = null;
                UserDetails userDetails = GetUserDetails();

                _selectStatement.IsPaged = isPaged.Checked;

                if (selectStatements.SelectedItems[0].Text == "Accounts" && userDetails.CompanyTypeCode != "Exc")
                {
                    _selectStatement.Where("a.CompanyId", Operators.IsEqualTo, userDetails.CompanyId.ToString(), null);
                    whereColumn = _selectStatement.WhereColumns.Last();
                }
                else if (selectStatements.SelectedItems[0].Text == "RequestWarrants" && requestId.Text != "")
                {
                    _selectStatement.Where("r.RequestId", Operators.IsEqualTo, requestId.Text, null);
                    whereColumn = _selectStatement.WhereColumns.Last();
                }

                statement.Text = _selectStatement.Statement;

                try
                {
                    DataSet dataSet = _selectStatement.Execute();

                    data.AutoGenerateColumns = true;
                    data.DataSource = null;
                    data.DataSource = dataSet.Tables[0];
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);

                    data.DataSource = null;
                }

                if (whereColumn != null)
                {
                    _selectStatement.WhereColumns.Remove(whereColumn);
                }

            }
        }

        class UserDetails
        {
            public short CompanyId { get; set; }
            public string CompanyTypeCode { get; set; }
        }

        private UserDetails GetUserDetails()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                connection.Open();

                DataSet dataSet = new DataSet();

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(String.Format(@"select c.CompanyId
		, ct.CompanyTypeCode
from dbo.UserProfile up
join companies.Company c on up.CompanyId = c.CompanyId
join companies.CompanyType ct on c.CompanyTypeId = ct.CompanyTypeId
where UserName = '{0}'", userName.Text), connection))
                {
                    dataAdapter.Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count == 1)
                    {
                        DataRow dataRow = dataSet.Tables[0].Rows[0];

                        return new UserDetails
                        {
                            CompanyId = dataRow.Field<short>("CompanyId"),
                            CompanyTypeCode = dataRow.Field<string>("CompanyTypeCode")
                        };
                    }

                }
            }

            return new UserDetails();
        }

        private void wheresMenu_Opening(object sender, CancelEventArgs e)
        {
            addWhereMenuItem.Enabled = _selectStatement != null;
            editWhereMenuItem.Enabled = _selectStatement != null && wheres.SelectedItems.Count == 1;
            removeWhereMenuItem.Enabled = _selectStatement != null && wheres.SelectedItems.Count > 0;
        }

        private void addWhereMenuItem_Click(object sender, EventArgs e)
        {
            new WhereForm(_selectStatement, _whereColumn).ShowDialog();

            RefreshWeres();

            RefreshData();
        }

        private void RefreshSelects()
        {
            selects.Items.Clear();
            _selectColumn = null;

            if (_selectStatement != null)
            {
                foreach (SelectColumn selectColumn in _selectStatement.SelectColumns)
                {
                    selects.Items.Add(new SelectColumnListViewItem(selectColumn.Alias, selectColumn));
                }
            }
        }

        private void RefreshWeres()
        {
            wheres.Items.Clear();
            _whereColumn = null;

            if (_selectStatement != null)
            {
                foreach (WhereColumn whereColumn in _selectStatement.WhereColumns)
                {
                    //ToDo : need to show alias not columnDef.Name
                    wheres.Items.Add(new WhereColumnListViewItem(whereColumn.ColumnDef.Name, whereColumn));
                }
            }
        }


        private void selects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selects.SelectedItems.Count == 1)
            {
                _selectColumn = ((SelectColumnListViewItem)selects.SelectedItems[0]).SelectColumn;
            }
        }

        private void wheres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wheres.SelectedItems.Count == 1)
            {
                _whereColumn = ((WhereColumnListViewItem)wheres.SelectedItems[0]).WhereColumn;
            }
        }

        private void editWhereMenuItem_Click(object sender, EventArgs e)
        {
            new WhereForm(_selectStatement, _whereColumn).ShowDialog();

            RefreshWeres();

            RefreshData();
        }

        private void removeWhereMenuItem_Click(object sender, EventArgs e)
        {
            _selectStatement.WhereColumns.Remove(((WhereColumnListViewItem)wheres.SelectedItems[0]).WhereColumn);
            RefreshWeres();
            RefreshData();
        }

        private void pageSize_TextChanged(object sender, EventArgs e)
        {
            if (_selectStatement != null)
            {
                _selectStatement.PageSize = pageSize.Text == "" ? 0 : Int32.Parse(pageSize.Text);
            }
        }

        private void pageNo_TextChanged(object sender, EventArgs e)
        {
            if (_selectStatement != null)
            {
                _selectStatement.PageNo = pageNo.Text == "" ? 0 : Int32.Parse(pageNo.Text);
            }
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                RefreshData();
            }
        }
    }
}
