namespace SelectBuilder.Designer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.selects = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.selectsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showHideMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orderByMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wheres = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.wheresMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addWhereMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editWhereMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeWhereMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.selectStatements = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.statement = new System.Windows.Forms.RichTextBox();
            this.data = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.isPaged = new System.Windows.Forms.CheckBox();
            this.pageNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.requestId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.userName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pageSize = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.noRows = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.selectsMenu.SuspendLayout();
            this.wheresMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 33);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(1154, 667);
            this.splitContainer1.SplitterDistance = 365;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(200, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.selects);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.wheres);
            this.splitContainer2.Size = new System.Drawing.Size(954, 365);
            this.splitContainer2.SplitterDistance = 340;
            this.splitContainer2.TabIndex = 0;
            // 
            // selects
            // 
            this.selects.CheckBoxes = true;
            this.selects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.selects.ContextMenuStrip = this.selectsMenu;
            this.selects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selects.Location = new System.Drawing.Point(0, 0);
            this.selects.Name = "selects";
            this.selects.Size = new System.Drawing.Size(340, 365);
            this.selects.TabIndex = 0;
            this.selects.UseCompatibleStateImageBehavior = false;
            this.selects.View = System.Windows.Forms.View.Details;
            this.selects.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.selects_ItemChecked);
            this.selects.SelectedIndexChanged += new System.EventHandler(this.selects_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 174;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Is Visible";
            this.columnHeader3.Width = 76;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Order By";
            // 
            // selectsMenu
            // 
            this.selectsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHideMenuItem,
            this.orderByMenuItem});
            this.selectsMenu.Name = "selectsMenu";
            this.selectsMenu.Size = new System.Drawing.Size(134, 48);
            // 
            // showHideMenuItem
            // 
            this.showHideMenuItem.Name = "showHideMenuItem";
            this.showHideMenuItem.Size = new System.Drawing.Size(133, 22);
            this.showHideMenuItem.Text = "Show\\Hide";
            this.showHideMenuItem.Click += new System.EventHandler(this.showHideMenuItem_Click);
            // 
            // orderByMenuItem
            // 
            this.orderByMenuItem.Name = "orderByMenuItem";
            this.orderByMenuItem.Size = new System.Drawing.Size(133, 22);
            this.orderByMenuItem.Text = "Order By";
            this.orderByMenuItem.Click += new System.EventHandler(this.orderByMenuItem_Click);
            // 
            // wheres
            // 
            this.wheres.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.wheres.ContextMenuStrip = this.wheresMenu;
            this.wheres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wheres.Location = new System.Drawing.Point(0, 0);
            this.wheres.Name = "wheres";
            this.wheres.Size = new System.Drawing.Size(610, 365);
            this.wheres.TabIndex = 0;
            this.wheres.UseCompatibleStateImageBehavior = false;
            this.wheres.View = System.Windows.Forms.View.Details;
            this.wheres.SelectedIndexChanged += new System.EventHandler(this.wheres_SelectedIndexChanged);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Name";
            this.columnHeader5.Width = 116;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Operator";
            this.columnHeader6.Width = 129;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Value1";
            this.columnHeader7.Width = 180;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Value2";
            this.columnHeader8.Width = 139;
            // 
            // wheresMenu
            // 
            this.wheresMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addWhereMenuItem,
            this.editWhereMenuItem,
            this.removeWhereMenuItem});
            this.wheresMenu.Name = "wheresMenu";
            this.wheresMenu.Size = new System.Drawing.Size(118, 70);
            this.wheresMenu.Opening += new System.ComponentModel.CancelEventHandler(this.wheresMenu_Opening);
            // 
            // addWhereMenuItem
            // 
            this.addWhereMenuItem.Name = "addWhereMenuItem";
            this.addWhereMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addWhereMenuItem.Text = "Add";
            this.addWhereMenuItem.Click += new System.EventHandler(this.addWhereMenuItem_Click);
            // 
            // editWhereMenuItem
            // 
            this.editWhereMenuItem.Name = "editWhereMenuItem";
            this.editWhereMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editWhereMenuItem.Text = "Edit";
            this.editWhereMenuItem.Click += new System.EventHandler(this.editWhereMenuItem_Click);
            // 
            // removeWhereMenuItem
            // 
            this.removeWhereMenuItem.Name = "removeWhereMenuItem";
            this.removeWhereMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeWhereMenuItem.Text = "Remove";
            this.removeWhereMenuItem.Click += new System.EventHandler(this.removeWhereMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.selectStatements);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 365);
            this.panel1.TabIndex = 1;
            // 
            // selectStatements
            // 
            this.selectStatements.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.selectStatements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectStatements.HideSelection = false;
            this.selectStatements.Location = new System.Drawing.Point(0, 0);
            this.selectStatements.MultiSelect = false;
            this.selectStatements.Name = "selectStatements";
            this.selectStatements.Size = new System.Drawing.Size(200, 365);
            this.selectStatements.TabIndex = 0;
            this.selectStatements.UseCompatibleStateImageBehavior = false;
            this.selectStatements.View = System.Windows.Forms.View.Details;
            this.selectStatements.SelectedIndexChanged += new System.EventHandler(this.selectStatements_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 196;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.statement);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.data);
            this.splitContainer3.Size = new System.Drawing.Size(1154, 298);
            this.splitContainer3.SplitterDistance = 579;
            this.splitContainer3.TabIndex = 0;
            // 
            // statement
            // 
            this.statement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statement.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statement.Location = new System.Drawing.Point(0, 0);
            this.statement.Name = "statement";
            this.statement.Size = new System.Drawing.Size(579, 298);
            this.statement.TabIndex = 0;
            this.statement.Text = "";
            // 
            // data
            // 
            this.data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.data.Location = new System.Drawing.Point(0, 0);
            this.data.Name = "data";
            this.data.Size = new System.Drawing.Size(571, 298);
            this.data.TabIndex = 0;
            this.data.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.data_ColumnHeaderMouseClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.noRows);
            this.panel2.Controls.Add(this.isPaged);
            this.panel2.Controls.Add(this.pageNo);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.requestId);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.userName);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.pageSize);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1154, 33);
            this.panel2.TabIndex = 2;
            // 
            // isPaged
            // 
            this.isPaged.AutoSize = true;
            this.isPaged.Checked = true;
            this.isPaged.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isPaged.Location = new System.Drawing.Point(200, 7);
            this.isPaged.Name = "isPaged";
            this.isPaged.Size = new System.Drawing.Size(68, 17);
            this.isPaged.TabIndex = 8;
            this.isPaged.Text = "Is Paged";
            this.isPaged.UseVisualStyleBackColor = true;
            // 
            // pageNo
            // 
            this.pageNo.Location = new System.Drawing.Point(488, 7);
            this.pageNo.Name = "pageNo";
            this.pageNo.Size = new System.Drawing.Size(61, 20);
            this.pageNo.TabIndex = 7;
            this.pageNo.TextChanged += new System.EventHandler(this.pageNo_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(427, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Page No";
            // 
            // requestId
            // 
            this.requestId.Location = new System.Drawing.Point(692, 7);
            this.requestId.Name = "requestId";
            this.requestId.Size = new System.Drawing.Size(61, 20);
            this.requestId.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(631, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "RequestId";
            // 
            // userName
            // 
            this.userName.Location = new System.Drawing.Point(69, 7);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(110, 20);
            this.userName.TabIndex = 3;
            this.userName.Text = "exc01";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "User Name";
            // 
            // pageSize
            // 
            this.pageSize.Location = new System.Drawing.Point(359, 7);
            this.pageSize.Name = "pageSize";
            this.pageSize.Size = new System.Drawing.Size(61, 20);
            this.pageSize.TabIndex = 1;
            this.pageSize.TextChanged += new System.EventHandler(this.pageSize_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(298, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Page Size";
            // 
            // noRows
            // 
            this.noRows.AutoSize = true;
            this.noRows.Location = new System.Drawing.Point(555, 10);
            this.noRows.Name = "noRows";
            this.noRows.Size = new System.Drawing.Size(35, 13);
            this.noRows.TabIndex = 9;
            this.noRows.Text = "label5";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 700);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.selectsMenu.ResumeLayout(false);
            this.wheresMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView selects;
        private System.Windows.Forms.ListView wheres;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView selectStatements;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.RichTextBox statement;
        private System.Windows.Forms.DataGridView data;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ContextMenuStrip selectsMenu;
        private System.Windows.Forms.ToolStripMenuItem showHideMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderByMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ContextMenuStrip wheresMenu;
        private System.Windows.Forms.ToolStripMenuItem addWhereMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editWhereMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeWhereMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox pageSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox userName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox requestId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pageNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox isPaged;
        private System.Windows.Forms.Label noRows;
    }
}

