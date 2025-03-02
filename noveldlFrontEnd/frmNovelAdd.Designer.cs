
namespace noveldlFrontEnd
{
	partial class frmNovelAdd
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
			this.tbNovelPath = new System.Windows.Forms.TextBox();
			this.btnDL = new System.Windows.Forms.Button();
			this.btnNovelOpen = new System.Windows.Forms.Button();
			this.tbURL = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbNovelPath
			// 
			this.tbNovelPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbNovelPath.Location = new System.Drawing.Point(69, 37);
			this.tbNovelPath.Name = "tbNovelPath";
			this.tbNovelPath.Size = new System.Drawing.Size(513, 19);
			this.tbNovelPath.TabIndex = 39;
			this.tbNovelPath.TextChanged += new System.EventHandler(this.tbNovelPath_TextChanged);
			// 
			// btnDL
			// 
			this.btnDL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDL.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnDL.Enabled = false;
			this.btnDL.Location = new System.Drawing.Point(507, 65);
			this.btnDL.Name = "btnDL";
			this.btnDL.Size = new System.Drawing.Size(75, 24);
			this.btnDL.TabIndex = 35;
			this.btnDL.Text = "追加";
			this.btnDL.UseVisualStyleBackColor = true;
			// 
			// btnNovelOpen
			// 
			this.btnNovelOpen.Location = new System.Drawing.Point(7, 65);
			this.btnNovelOpen.Name = "btnNovelOpen";
			this.btnNovelOpen.Size = new System.Drawing.Size(88, 24);
			this.btnNovelOpen.TabIndex = 40;
			this.btnNovelOpen.Text = "小説File選択...";
			this.btnNovelOpen.UseVisualStyleBackColor = true;
			this.btnNovelOpen.Click += new System.EventHandler(this.btnNovelOpen_Click);
			// 
			// tbURL
			// 
			this.tbURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbURL.Location = new System.Drawing.Point(69, 12);
			this.tbURL.Name = "tbURL";
			this.tbURL.Size = new System.Drawing.Size(513, 19);
			this.tbURL.TabIndex = 41;
			this.tbURL.TextChanged += new System.EventHandler(this.tbURL_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 12);
			this.label1.TabIndex = 42;
			this.label1.Text = "URL";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 12);
			this.label2.TabIndex = 42;
			this.label2.Text = "小説Path";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(414, 66);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 24);
			this.btnCancel.TabIndex = 35;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// frmNovelAdd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(594, 97);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbURL);
			this.Controls.Add(this.tbNovelPath);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnDL);
			this.Controls.Add(this.btnNovelOpen);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmNovelAdd";
			this.Text = "リストファイルに小説追加";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNovelAdd_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbNovelPath;
		private System.Windows.Forms.Button btnDL;
		private System.Windows.Forms.Button btnNovelOpen;
		private System.Windows.Forms.TextBox tbURL;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnCancel;
	}
}