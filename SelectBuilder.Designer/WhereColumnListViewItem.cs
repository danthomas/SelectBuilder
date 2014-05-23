using System.Windows.Forms;

namespace SelectBuilder.Designer
{
    class WhereColumnListViewItem : ListViewItem
    {
        public WhereColumn WhereColumn { get; set; }

        public WhereColumnListViewItem(string text, WhereColumn whereColumn)
        {
            WhereColumn = whereColumn;
            Text = text;
            SubItems.Add("");
            SubItems.Add("");
            SubItems.Add("");
            RefreshText();
        }

        public void RefreshText()
        {
            SubItems[1].Text = WhereColumn.Operator.ToString();
            SubItems[2].Text = WhereColumn.Value1;
            SubItems[3].Text = WhereColumn.Value2;
        }
    }
}
