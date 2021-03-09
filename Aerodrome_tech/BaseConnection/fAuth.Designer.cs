namespace BaseConnection {
	partial class fAuth {
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
			this.label1 = new System.Windows.Forms.Label();
			this.tbLogin = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.bSignIn = new System.Windows.Forms.Button();
			this.bRegister = new System.Windows.Forms.Button();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(29, 57);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Логин";
			// 
			// tbLogin
			// 
			this.tbLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tbLogin.Location = new System.Drawing.Point(32, 76);
			this.tbLogin.Name = "tbLogin";
			this.tbLogin.Size = new System.Drawing.Size(196, 22);
			this.tbLogin.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(100, 19);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 24);
			this.label3.TabIndex = 4;
			this.label3.Text = "Вход";
			// 
			// bSignIn
			// 
			this.bSignIn.Location = new System.Drawing.Point(161, 169);
			this.bSignIn.Name = "bSignIn";
			this.bSignIn.Size = new System.Drawing.Size(75, 23);
			this.bSignIn.TabIndex = 5;
			this.bSignIn.Text = "Вход";
			this.bSignIn.UseVisualStyleBackColor = true;
			this.bSignIn.Click += new System.EventHandler(this.bSignIn_Click);
			// 
			// bRegister
			// 
			this.bRegister.Location = new System.Drawing.Point(70, 169);
			this.bRegister.Name = "bRegister";
			this.bRegister.Size = new System.Drawing.Size(85, 23);
			this.bRegister.TabIndex = 6;
			this.bRegister.Text = "Регистрация";
			this.bRegister.UseVisualStyleBackColor = true;
			this.bRegister.Click += new System.EventHandler(this.bRegister_Click);
			// 
			// tbPassword
			// 
			this.tbPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tbPassword.Location = new System.Drawing.Point(32, 122);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(196, 22);
			this.tbPassword.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(29, 103);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Пароль";
			// 
			// fAuth
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(261, 223);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.bRegister);
			this.Controls.Add(this.bSignIn);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbLogin);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(267, 248);
			this.Name = "fAuth";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Вход";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fAuth_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbLogin;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button bSignIn;
		private System.Windows.Forms.Button bRegister;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label label2;
	}
}