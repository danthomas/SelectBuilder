using System.Windows.Forms;

namespace SelectBuilder.Designer
{
    class SelectStatementListViewItem : ListViewItem
    {
        public SelectStatementListViewItem(string text, SelectStatement selectStatement)
        {
            Text = text;
            SelectStatement = selectStatement;
        }

        public SelectStatement SelectStatement { get; set; }

        public void RefreshText()
        {
        }
    }
}
