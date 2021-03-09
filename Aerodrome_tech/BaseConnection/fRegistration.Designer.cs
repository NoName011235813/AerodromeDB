namespace BaseConnection {
	partial class fRegistration {
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
			this.label3 = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbLogin = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.bSubmit = new System.Windows.Forms.Button();
			this.tbRepeatPassword = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.bBack = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(69, 19);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(126, 24);
			this.label3.TabIndex = 5;
			this.label3.Text = "Регистрация";
			// 
			// tbPassword
			// 
			this.tbPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tbPassword.Location = new System.Drawing.Point(32, 122);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(196, 22);
			this.tbPassword.TabIndex = 12;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(29, 103);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 16);
			this.label2.TabIndex = 11;
			this.label2.Text = "Пароль";
			// 
			// tbLogin
			// 
			this.tbLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tbLogin.Location = new System.Drawing.Point(32, 76);
			this.tbLogin.Name = "tbLogin";
			this.tbLogin.Size = new System.Drawing.Size(196, 22);
			this.tbLogin.TabIndex = 10;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(29, 57);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 16);
			this.label1.TabIndex = 9;
			this.label1.Text = "Логин";
			// 
			// bSubmit
			// 
			this.bSubmit.Location = new System.Drawing.Point(155, 215);
			this.bSubmit.Name = "bSubmit";
			this.bSubmit.Size = new System.Drawing.Size(82, 23);
			this.bSubmit.TabIndex = 13;
			this.bSubmit.Text = "Подтвердить";
			this.bSubmit.UseVisualStyleBackColor = true;
			this.bSubmit.Click += new System.EventHandler(this.bSubmit_Click);
			// 
			// tbRepeatPassword
			// 
			this.tbRepeatPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tbRepeatPassword.Location = new System.Drawing.Point(32, 169);
			this.tbRepeatPassword.Name = "tbRepeatPassword";
			this.tbRepeatPassword.PasswordChar = '*';
			this.tbRepeatPassword.Size = new System.Drawing.Size(196, 22);
			this.tbRepeatPassword.TabIndex = 15;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label4.Location = new System.Drawing.Point(29, 150);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(130, 16);
			this.label4.TabIndex = 14;
			this.label4.Text = "Повторите пароль";
			// 
			// bBack
			// 
			this.bBack.Location = new System.Drawing.Point(74, 215);
			this.bBack.Name = "bBack";
			this.bBack.Size = new System.Drawing.Size(75, 23);
			this.bBack.TabIndex = 16;
			this.bBack.Text = "Назад";
			this.bBack.UseVisualStyleBackColor = true;
			this.bBack.Click += new System.EventHandler(this.bBack_Click);
			// 
			// fRegistration
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(261, 259);
			this.Controls.Add(this.bBack);
			this.Controls.Add(this.tbRepeatPassword);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.bSubmit);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbLogin);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(267, 248);
			this.Name = "fRegistration";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Регистрация";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fRegistration_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbLogin;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button bSubmit;
		private System.Windows.Forms.TextBox tbRepeatPassword;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button bBack;
	}
}