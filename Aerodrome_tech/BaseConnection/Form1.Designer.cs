namespace BaseConnection {
	partial class FMain {
		/// <summary>
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent() {
			this.DGrid = new System.Windows.Forms.DataGridView();
			this.BSave = new System.Windows.Forms.Button();
			this.BCancel = new System.Windows.Forms.Button();
			this.BAdd = new System.Windows.Forms.Button();
			this.BDelete = new System.Windows.Forms.Button();
			this.bMaintTypes = new System.Windows.Forms.Button();
			this.BPlaneModels = new System.Windows.Forms.Button();
			this.bGroupsLeaders = new System.Windows.Forms.Button();
			this.bWorkloads = new System.Windows.Forms.Button();
			this.bTechGroups = new System.Windows.Forms.Button();
			this.bPlanes = new System.Windows.Forms.Button();
			this.bWorksHistory = new System.Windows.Forms.Button();
			this.bPlanesStatus = new System.Windows.Forms.Button();
			this.bUsers = new System.Windows.Forms.Button();
			this.bLogout = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.DGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// DGrid
			// 
			this.DGrid.AllowUserToAddRows = false;
			this.DGrid.AllowUserToDeleteRows = false;
			this.DGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGrid.Location = new System.Drawing.Point(12, 12);
			this.DGrid.Name = "DGrid";
			this.DGrid.ShowCellErrors = false;
			this.DGrid.Size = new System.Drawing.Size(587, 332);
			this.DGrid.TabIndex = 0;
			this.DGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGrid_DataError);
			// 
			// BSave
			// 
			this.BSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.BSave.Location = new System.Drawing.Point(12, 350);
			this.BSave.Name = "BSave";
			this.BSave.Size = new System.Drawing.Size(75, 23);
			this.BSave.TabIndex = 1;
			this.BSave.Text = "Сохранить";
			this.BSave.UseVisualStyleBackColor = true;
			this.BSave.Click += new System.EventHandler(this.BSave_Click);
			// 
			// BCancel
			// 
			this.BCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.BCancel.Location = new System.Drawing.Point(93, 350);
			this.BCancel.Name = "BCancel";
			this.BCancel.Size = new System.Drawing.Size(75, 23);
			this.BCancel.TabIndex = 2;
			this.BCancel.Text = "Отменить";
			this.BCancel.UseVisualStyleBackColor = true;
			this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
			// 
			// BAdd
			// 
			this.BAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.BAdd.Location = new System.Drawing.Point(174, 350);
			this.BAdd.Name = "BAdd";
			this.BAdd.Size = new System.Drawing.Size(75, 23);
			this.BAdd.TabIndex = 3;
			this.BAdd.Text = "Добавить";
			this.BAdd.UseVisualStyleBackColor = true;
			this.BAdd.Click += new System.EventHandler(this.BAdd_Click);
			// 
			// BDelete
			// 
			this.BDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.BDelete.Location = new System.Drawing.Point(255, 350);
			this.BDelete.Name = "BDelete";
			this.BDelete.Size = new System.Drawing.Size(75, 23);
			this.BDelete.TabIndex = 4;
			this.BDelete.Text = "Удалить";
			this.BDelete.UseVisualStyleBackColor = true;
			this.BDelete.Click += new System.EventHandler(this.BDelete_Click);
			// 
			// bMaintTypes
			// 
			this.bMaintTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bMaintTypes.Location = new System.Drawing.Point(609, 12);
			this.bMaintTypes.Name = "bMaintTypes";
			this.bMaintTypes.Size = new System.Drawing.Size(114, 23);
			this.bMaintTypes.TabIndex = 5;
			this.bMaintTypes.Text = "Типы ТО";
			this.bMaintTypes.UseVisualStyleBackColor = true;
			this.bMaintTypes.Click += new System.EventHandler(this.bMaintTypes_Click);
			// 
			// BPlaneModels
			// 
			this.BPlaneModels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BPlaneModels.Location = new System.Drawing.Point(609, 146);
			this.BPlaneModels.Name = "BPlaneModels";
			this.BPlaneModels.Size = new System.Drawing.Size(114, 23);
			this.BPlaneModels.TabIndex = 6;
			this.BPlaneModels.Text = "Модели самолётов";
			this.BPlaneModels.UseVisualStyleBackColor = true;
			this.BPlaneModels.Click += new System.EventHandler(this.BPlaneModels_Click);
			// 
			// bGroupsLeaders
			// 
			this.bGroupsLeaders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bGroupsLeaders.Location = new System.Drawing.Point(609, 70);
			this.bGroupsLeaders.Name = "bGroupsLeaders";
			this.bGroupsLeaders.Size = new System.Drawing.Size(114, 41);
			this.bGroupsLeaders.TabIndex = 6;
			this.bGroupsLeaders.Text = "Руководители групп техников";
			this.bGroupsLeaders.UseVisualStyleBackColor = true;
			this.bGroupsLeaders.Click += new System.EventHandler(this.bGroupsLeaders_Click);
			// 
			// bWorkloads
			// 
			this.bWorkloads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bWorkloads.Location = new System.Drawing.Point(609, 41);
			this.bWorkloads.Name = "bWorkloads";
			this.bWorkloads.Size = new System.Drawing.Size(114, 23);
			this.bWorkloads.TabIndex = 7;
			this.bWorkloads.Text = "Типы объемов раб.";
			this.bWorkloads.UseVisualStyleBackColor = true;
			this.bWorkloads.Click += new System.EventHandler(this.bWorkloads_Click);
			// 
			// bTechGroups
			// 
			this.bTechGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bTechGroups.Location = new System.Drawing.Point(609, 117);
			this.bTechGroups.Name = "bTechGroups";
			this.bTechGroups.Size = new System.Drawing.Size(114, 23);
			this.bTechGroups.TabIndex = 8;
			this.bTechGroups.Text = "Группы техников";
			this.bTechGroups.UseVisualStyleBackColor = true;
			this.bTechGroups.Click += new System.EventHandler(this.bTechGroups_Click);
			// 
			// bPlanes
			// 
			this.bPlanes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bPlanes.Location = new System.Drawing.Point(609, 175);
			this.bPlanes.Name = "bPlanes";
			this.bPlanes.Size = new System.Drawing.Size(114, 23);
			this.bPlanes.TabIndex = 9;
			this.bPlanes.Text = "Самолёты";
			this.bPlanes.UseVisualStyleBackColor = true;
			this.bPlanes.Click += new System.EventHandler(this.bPlanes_Click);
			// 
			// bWorksHistory
			// 
			this.bWorksHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bWorksHistory.Location = new System.Drawing.Point(609, 204);
			this.bWorksHistory.Name = "bWorksHistory";
			this.bWorksHistory.Size = new System.Drawing.Size(114, 23);
			this.bWorksHistory.TabIndex = 10;
			this.bWorksHistory.Text = "История работ";
			this.bWorksHistory.UseVisualStyleBackColor = true;
			this.bWorksHistory.Click += new System.EventHandler(this.bWorksHistory_Click);
			// 
			// bPlanesStatus
			// 
			this.bPlanesStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bPlanesStatus.Location = new System.Drawing.Point(609, 304);
			this.bPlanesStatus.Name = "bPlanesStatus";
			this.bPlanesStatus.Size = new System.Drawing.Size(114, 40);
			this.bPlanesStatus.TabIndex = 11;
			this.bPlanesStatus.Text = "Состояние самолётов";
			this.bPlanesStatus.UseVisualStyleBackColor = true;
			this.bPlanesStatus.Click += new System.EventHandler(this.bPlanesStatus_Click);
			// 
			// bUsers
			// 
			this.bUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bUsers.Enabled = false;
			this.bUsers.Location = new System.Drawing.Point(609, 275);
			this.bUsers.Name = "bUsers";
			this.bUsers.Size = new System.Drawing.Size(114, 23);
			this.bUsers.TabIndex = 12;
			this.bUsers.Text = "Пользователи";
			this.bUsers.UseVisualStyleBackColor = true;
			this.bUsers.Visible = false;
			this.bUsers.Click += new System.EventHandler(this.bUsers_Click);
			// 
			// bLogout
			// 
			this.bLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bLogout.Location = new System.Drawing.Point(488, 350);
			this.bLogout.Name = "bLogout";
			this.bLogout.Size = new System.Drawing.Size(111, 23);
			this.bLogout.TabIndex = 13;
			this.bLogout.Text = "Выход";
			this.bLogout.UseVisualStyleBackColor = true;
			this.bLogout.Click += new System.EventHandler(this.bLogout_Click);
			// 
			// FMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(731, 382);
			this.Controls.Add(this.bLogout);
			this.Controls.Add(this.bUsers);
			this.Controls.Add(this.bPlanesStatus);
			this.Controls.Add(this.bWorksHistory);
			this.Controls.Add(this.bPlanes);
			this.Controls.Add(this.bTechGroups);
			this.Controls.Add(this.bWorkloads);
			this.Controls.Add(this.bGroupsLeaders);
			this.Controls.Add(this.BPlaneModels);
			this.Controls.Add(this.bMaintTypes);
			this.Controls.Add(this.BDelete);
			this.Controls.Add(this.BAdd);
			this.Controls.Add(this.BCancel);
			this.Controls.Add(this.BSave);
			this.Controls.Add(this.DGrid);
			this.MinimumSize = new System.Drawing.Size(739, 409);
			this.Name = "FMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Аэродром";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMain_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.DGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView DGrid;
		private System.Windows.Forms.Button BSave;
		private System.Windows.Forms.Button BCancel;
		private System.Windows.Forms.Button BAdd;
		private System.Windows.Forms.Button BDelete;
		private System.Windows.Forms.Button bMaintTypes;
		private System.Windows.Forms.Button BPlaneModels;
		private System.Windows.Forms.Button bGroupsLeaders;
		private System.Windows.Forms.Button bWorkloads;
		private System.Windows.Forms.Button bTechGroups;
		private System.Windows.Forms.Button bPlanes;
		private System.Windows.Forms.Button bWorksHistory;
		private System.Windows.Forms.Button bPlanesStatus;
		private System.Windows.Forms.Button bUsers;
		private System.Windows.Forms.Button bLogout;
	}
}

