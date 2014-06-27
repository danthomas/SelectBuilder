using System;
using System.Windows.Forms;

namespace SelectBuilder.Designer
{
    class SelectColumnListViewItem : ListViewItem
    {
        public SelectColumn SelectColumn { get; set; }

        public SelectColumnListViewItem(string text, SelectColumn selectColumn)
        {
            SelectColumn = selectColumn;
            Text = text;
            SubItems.Add("");
            SubItems.Add("");
            RefreshText();
        }

        public void RefreshText()
        {
            SubItems[1].Text = SelectColumn.IsVisible ? "Yes" : "No";
            SubItems[2].Text = SelectColumn.OrderByIndex == 0 ? "" : String.Format("{0} {1}", Math.Abs(SelectColumn.OrderByIndex), SelectColumn.OrderByIndex > 0 ? "ASC" : "DESC");
            Checked = SelectColumn.IsVisible;
        }
    }
}
