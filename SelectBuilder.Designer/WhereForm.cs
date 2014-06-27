using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelectBuilder.Designer
{
    public partial class WhereForm : Form
    {
        private readonly SelectStatement _selectStatement;
        private readonly WhereColumn _whereColumn;

        public WhereForm(SelectStatement selectStatement, WhereColumn whereColumn)
        {
            _selectStatement = selectStatement;
            _whereColumn = whereColumn;
            InitializeComponent();

            columns.ValueMember = "Alias";
            columns.DisplayMember = "Alias";

            columns.DataSource = _selectStatement.SelectColumns.Where(item => (item.ColumnType & ColumnType.Where) == ColumnType.Where).ToList();

            if (whereColumn != null)
            {
                columns.Enabled = false;
                columns.SelectedValue = _selectStatement.SelectColumns.Single(item => item.Alias == whereColumn.ColumnDef.Name);
                operators.SelectedItem = whereColumn.Operator;

                SelectColumn selectColumn = (SelectColumn) columns.SelectedItem;

                if (selectColumn != null)
                {
                    if (selectColumn.OptionsSelectStatement != null)
                    {
                        options.SelectedValue = whereColumn.Value1;
                    }
                    else
                    {
                        value1.Text = whereColumn.Value1;
                        value2.Text = whereColumn.Value2;
                    }
                }
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {
            SelectColumn selectColumn = (SelectColumn)columns.SelectedItem;

            string v1 = selectColumn.OptionsSelectStatement != null
                ? options.SelectedValue.ToString()
                : value1.Text;

            string v2 = value2.Text;

            if (_whereColumn == null)
            {
                _selectStatement.Where(selectColumn, (Operator)operators.SelectedItem, v1, v2);
            }
            else
            {
                _whereColumn.Operator = (Operator)operators.SelectedItem;
                _whereColumn.Value1 = v1;
                _whereColumn.Value2 = v2;
            }

            Close();
        }

        private void operators_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void columns_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectColumn selectColumn = (SelectColumn)columns.SelectedItem;

            if (selectColumn != null)
            {
                if (selectColumn.OptionsSelectStatement != null)
                {
                    value1.Visible = false;
                    options.Visible = true;
                    value2.Enabled = false;

                    operators.DataSource = new[] {Operator.IsEqualTo};
                    operators.SelectedItem = Operator.IsEqualTo;

                    options.ValueMember = "Id";
                    options.DisplayMember = "Text";

                    DataTable dataTable = selectColumn.OptionsSelectStatement.Execute().Tables[0];

                    options.DataSource = dataTable.Rows.Cast<DataRow>().Select(item => new ComboBoxItem
                    {
                        Id = item[0].ToString(),
                        Text = item.ItemArray.Skip(1).Select(item2 => item2.ToString()).Aggregate((a, b) => a + " - " + b)
                    }).ToList();
                }
                else
                {
                    operators.DataSource = new List<object>(Enum.GetValues(typeof (Operator)).Cast<object>());
                    value1.Visible = true;
                    options.Visible = false;
                    value2.Enabled = false;
                }
            }
        }

        class ComboBoxItem
        {
            public string Id { get; set; }
            public string Text { get; set; }
        }
    }
}
