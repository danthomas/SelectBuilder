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

            foreach (SelectColumn selectColumn in _selectStatement.SelectColumns.Where(item => (item.ColumnType & ColumnType.Where) == ColumnType.Where))
            {
                columns.Items.Add(selectColumn.Alias);
            }

            foreach (string @operator in Enum.GetNames(typeof(Operators)))
            {
                operators.Items.Add(@operator);
            }

            if (whereColumn != null)
            {
                columns.Enabled = false;
                columns.SelectedItem = whereColumn.ColumnDef.Name;
                operators.SelectedItem = whereColumn.Operator.ToString();
                value1.Text = whereColumn.Value1;
                value2.Text = whereColumn.Value2;
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (_whereColumn == null)
            {
                SelectColumn selectColumn = _selectStatement.SelectColumns.Single(item => item.Alias == columns.SelectedItem.ToString());
                
                _selectStatement.Where(selectColumn, (Operators)Enum.Parse(typeof(Operators), operators.SelectedItem.ToString()), value1.Text, value2.Text);
            }
            else
            {
                _whereColumn.Operator = (Operators)Enum.Parse(typeof(Operators), operators.SelectedItem.ToString());
                _whereColumn.Value1 = value1.Text;
                _whereColumn.Value2 = value2.Text;
            }

            Close();
        }
    }
}
