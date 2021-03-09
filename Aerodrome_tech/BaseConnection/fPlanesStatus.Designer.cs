namespace BaseConnection {
	partial class fPlanesStatus {
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
			this.StatusGrid = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.StatusGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// StatusGrid
			// 
			this.StatusGrid.AllowUserToAddRows = false;
			this.StatusGrid.AllowUserToDeleteRows = false;
			this.StatusGrid.AllowUserToOrderColumns = true;
			this.StatusGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.StatusGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.StatusGrid.Location = new System.Drawing.Point(12, 12);
			this.StatusGrid.Name = "StatusGrid";
			this.StatusGrid.ReadOnly = true;
			this.StatusGrid.Size = new System.Drawing.Size(673, 415);
			this.StatusGrid.TabIndex = 0;
			// 
			// fPlanesStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(697, 439);
			this.Controls.Add(this.StatusGrid);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(705, 466);
			this.Name = "fPlanesStatus";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Состояние самолётов";
			((System.ComponentModel.ISupportInitialize)(this.StatusGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView StatusGrid;
	}
}