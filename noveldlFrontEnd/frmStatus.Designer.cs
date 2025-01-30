namespace noveldlFrontEnd
{
	partial class frmErrStatus
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
			this.lboxErrStatus = new System.Windows.Forms.ListBox();
			this.btnHide = new System.Windows.Forms.Button();
			this.btnUrlOpen = new System.Windows.Forms.Button();
			this.btnLogOut = new System.Windows.Forms.Button();
			this.btnLogDelAll = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lboxErrStatus
			// 
			this.lboxErrStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lboxErrStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lboxErrStatus.FormattingEnabled = true;
			this.lboxErrStatus.ItemHeight = 12;
			this.lboxErrStatus.Location = new System.Drawing.Point(2, 2);
			this.lboxErrStatus.Name = "lboxErrStatus";
			this.lboxErrStatus.ScrollAlwaysVisible = true;
			this.lboxErrStatus.Size = new System.Drawing.Size(461, 160);
			this.lboxErrStatus.TabIndex = 0;
			this.lboxErrStatus.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lboxErrStatus_DrawItem);
			this.lboxErrStatus.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lboxErrStatus_MouseDoubleClick);
			// 
			// btnHide
			// 
			this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHide.Location = new System.Drawing.Point(380, 168);
			this.btnHide.Name = "btnHide";
			this.btnHide.Size = new System.Drawing.Size(83, 28);
			this.btnHide.TabIndex = 1;
			this.btnHide.Text = "Hide";
			this.btnHide.UseVisualStyleBackColor = true;
			this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
			// 
			// btnUrlOpen
			// 
			this.btnUrlOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnUrlOpen.Location = new System.Drawing.Point(2, 168);
			this.btnUrlOpen.Name = "btnUrlOpen";
			this.btnUrlOpen.Size = new System.Drawing.Size(81, 28);
			this.btnUrlOpen.TabIndex = 2;
			this.btnUrlOpen.Text = "ブラウザで開く";
			this.btnUrlOpen.UseVisualStyleBackColor = true;
			this.btnUrlOpen.Click += new System.EventHandler(this.btnUrlOpen_Click);
			// 
			// btnLogOut
			// 
			this.btnLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnLogOut.Location = new System.Drawing.Point(114, 168);
			this.btnLogOut.Name = "btnLogOut";
			this.btnLogOut.Size = new System.Drawing.Size(75, 28);
			this.btnLogOut.TabIndex = 3;
			this.btnLogOut.Text = "ログ出力";
			this.btnLogOut.UseVisualStyleBackColor = true;
			this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
			// 
			// btnLogDelAll
			// 
			this.btnLogDelAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnLogDelAll.Location = new System.Drawing.Point(205, 168);
			this.btnLogDelAll.Name = "btnLogDelAll";
			this.btnLogDelAll.Size = new System.Drawing.Size(75, 28);
			this.btnLogDelAll.TabIndex = 3;
			this.btnLogDelAll.Text = "ログ全削除";
			this.btnLogDelAll.UseVisualStyleBackColor = true;
			this.btnLogDelAll.Click += new System.EventHandler(this.btnLogDelAll_Click);
			// 
			// frmErrStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(467, 199);
			this.Controls.Add(this.btnLogDelAll);
			this.Controls.Add(this.btnLogOut);
			this.Controls.Add(this.btnUrlOpen);
			this.Controls.Add(this.btnHide);
			this.Controls.Add(this.lboxErrStatus);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(483, 238);
			this.Name = "frmErrStatus";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "エラー表示";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmErrStatus_FormClosing);
			this.Load += new System.EventHandler(this.frmErrStatus_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button btnHide;
		private System.Windows.Forms.Button btnUrlOpen;
		public System.Windows.Forms.ListBox lboxErrStatus;
		private System.Windows.Forms.Button btnLogOut;
		private System.Windows.Forms.Button btnLogDelAll;
	}
}