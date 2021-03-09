namespace BaseConnection {
	partial class ChooseForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.DGrid = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.bSubmit = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.DGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// DGrid
			// 
			this.DGrid.AllowUserToAddRows = false;
			this.DGrid.AllowUserToDeleteRows = false;
			this.DGrid.AllowUserToOrderColumns = true;
			this.DGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.DGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGrid.Location = new System.Drawing.Point(10, 59);
			this.DGrid.MultiSelect = false;
			this.DGrid.Name = "DGrid";
			this.DGrid.ReadOnly = true;
			this.DGrid.Size = new System.Drawing.Size(427, 195);
			this.DGrid.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(10, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(427, 32);
			this.label1.TabIndex = 1;
			this.label1.Text = "Было найдено несколько записей в ответ на введенные данные, выберите пожалуйста к" +
    "акую из них Вы хотели выбрать:\r\n";
			// 
			// bSubmit
			// 
			this.bSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bSubmit.Location = new System.Drawing.Point(354, 275);
			this.bSubmit.Name = "bSubmit";
			this.bSubmit.Size = new System.Drawing.Size(85, 23);
			this.bSubmit.TabIndex = 2;
			this.bSubmit.Text = "Подтвердить";
			this.bSubmit.UseVisualStyleBackColor = true;
			this.bSubmit.Click += new System.EventHandler(this.bSubmit_Click);
			// 
			// bCancel
			// 
			this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(273, 275);
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new System.Drawing.Size(75, 23);
			this.bCancel.TabIndex = 3;
			this.bCancel.Text = "Отменить";
			this.bCancel.UseVisualStyleBackColor = true;
			this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
			// 
			// ChooseForm
			// 
			this.AcceptButton = this.bSubmit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(451, 310);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bSubmit);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.DGrid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MinimumSize = new System.Drawing.Size(459, 337);
			this.Name = "ChooseForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Выберите запись";
			((System.ComponentModel.ISupportInitialize)(this.DGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView DGrid;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button bSubmit;
		private System.Windows.Forms.Button bCancel;
	}
}