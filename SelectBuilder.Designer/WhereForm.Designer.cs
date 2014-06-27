namespace SelectBuilder.Designer
{
    partial class WhereForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.columns = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.operators = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.value1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.value2 = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.options = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Column";
            // 
            // columns
            // 
            this.columns.FormattingEnabled = true;
            this.columns.Location = new System.Drawing.Point(130, 12);
            this.columns.Name = "columns";
            this.columns.Size = new System.Drawing.Size(230, 21);
            this.columns.TabIndex = 1;
            this.columns.SelectedIndexChanged += new System.EventHandler(this.columns_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Operator";
            // 
            // operators
            // 
            this.operators.FormattingEnabled = true;
            this.operators.Location = new System.Drawing.Point(130, 39);
            this.operators.Name = "operators";
            this.operators.Size = new System.Drawing.Size(230, 21);
            this.operators.TabIndex = 3;
            this.operators.SelectedIndexChanged += new System.EventHandler(this.operators_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Value1";
            // 
            // value1
            // 
            this.value1.Location = new System.Drawing.Point(130, 66);
            this.value1.Name = "value1";
            this.value1.Size = new System.Drawing.Size(230, 20);
            this.value1.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Value2";
            // 
            // value2
            // 
            this.value2.Location = new System.Drawing.Point(130, 94);
            this.value2.Name = "value2";
            this.value2.Size = new System.Drawing.Size(230, 20);
            this.value2.TabIndex = 7;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(204, 120);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 8;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(285, 120);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 9;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // options
            // 
            this.options.FormattingEnabled = true;
            this.options.Location = new System.Drawing.Point(130, 66);
            this.options.Name = "options";
            this.options.Size = new System.Drawing.Size(230, 21);
            this.options.TabIndex = 10;
            this.options.Visible = false;
            // 
            // WhereForm
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(370, 154);
            this.Controls.Add(this.options);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.value2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.value1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.operators);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.columns);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WhereForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WhereForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox columns;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox operators;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox value1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox value2;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ComboBox options;
    }
}