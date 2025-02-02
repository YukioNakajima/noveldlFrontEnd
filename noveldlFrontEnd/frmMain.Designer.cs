namespace noveldlFrontEnd
{
	partial class frmMain
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.chkVanish = new System.Windows.Forms.CheckBox();
			this.btnfrmErrStShow = new System.Windows.Forms.Button();
			this.btnItemDn = new System.Windows.Forms.Button();
			this.btnItemUp = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.chkListChg = new System.Windows.Forms.CheckBox();
			this.lblStatusApp = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblListProgress = new System.Windows.Forms.Label();
			this.btnDownload = new System.Windows.Forms.Button();
			this.lblTimeCount = new System.Windows.Forms.Label();
			this.lbUrlList = new System.Windows.Forms.ListBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.コピーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.ペーストToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlBtn = new System.Windows.Forms.Panel();
			this.btnNovelAdd = new System.Windows.Forms.Button();
			this.btnAddList = new System.Windows.Forms.Button();
			this.btnDelList = new System.Windows.Forms.Button();
			this.uC_NovelDL = new UC.UC_DownloadNovel();
			this.contextMenuStrip1.SuspendLayout();
			this.pnlBtn.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkVanish
			// 
			this.chkVanish.AutoSize = true;
			this.chkVanish.Location = new System.Drawing.Point(356, 239);
			this.chkVanish.Name = "chkVanish";
			this.chkVanish.Size = new System.Drawing.Size(72, 16);
			this.chkVanish.TabIndex = 12;
			this.chkVanish.Text = "消失確認";
			this.chkVanish.UseVisualStyleBackColor = true;
			this.chkVanish.CheckedChanged += new System.EventHandler(this.chkVanish_CheckedChanged);
			// 
			// btnfrmErrStShow
			// 
			this.btnfrmErrStShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnfrmErrStShow.Location = new System.Drawing.Point(412, 190);
			this.btnfrmErrStShow.Name = "btnfrmErrStShow";
			this.btnfrmErrStShow.Size = new System.Drawing.Size(106, 23);
			this.btnfrmErrStShow.TabIndex = 21;
			this.btnfrmErrStShow.Text = "エラー画面表示";
			this.btnfrmErrStShow.UseVisualStyleBackColor = true;
			this.btnfrmErrStShow.Click += new System.EventHandler(this.btnfrmErrStShow_Click);
			// 
			// btnItemDn
			// 
			this.btnItemDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnItemDn.Location = new System.Drawing.Point(58, 29);
			this.btnItemDn.Name = "btnItemDn";
			this.btnItemDn.Size = new System.Drawing.Size(50, 23);
			this.btnItemDn.TabIndex = 1;
			this.btnItemDn.Text = "▼";
			this.btnItemDn.UseVisualStyleBackColor = true;
			this.btnItemDn.Click += new System.EventHandler(this.btnItemDn_Click);
			// 
			// btnItemUp
			// 
			this.btnItemUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnItemUp.Location = new System.Drawing.Point(2, 29);
			this.btnItemUp.Name = "btnItemUp";
			this.btnItemUp.Size = new System.Drawing.Size(50, 23);
			this.btnItemUp.TabIndex = 1;
			this.btnItemUp.Text = "▲";
			this.btnItemUp.UseVisualStyleBackColor = true;
			this.btnItemUp.Click += new System.EventHandler(this.btnItemUp_Click);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// chkListChg
			// 
			this.chkListChg.AutoSize = true;
			this.chkListChg.Enabled = false;
			this.chkListChg.Location = new System.Drawing.Point(434, 239);
			this.chkListChg.Name = "chkListChg";
			this.chkListChg.Size = new System.Drawing.Size(72, 16);
			this.chkListChg.TabIndex = 13;
			this.chkListChg.Text = "リスト変更";
			this.chkListChg.UseVisualStyleBackColor = true;
			// 
			// lblStatusApp
			// 
			this.lblStatusApp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblStatusApp.Location = new System.Drawing.Point(9, 223);
			this.lblStatusApp.Name = "lblStatusApp";
			this.lblStatusApp.Size = new System.Drawing.Size(293, 14);
			this.lblStatusApp.TabIndex = 15;
			this.lblStatusApp.Text = "AppStatus";
			this.lblStatusApp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(361, 224);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 12);
			this.label1.TabIndex = 17;
			this.label1.Text = "リスト進行状況";
			// 
			// lblListProgress
			// 
			this.lblListProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblListProgress.AutoSize = true;
			this.lblListProgress.Location = new System.Drawing.Point(444, 224);
			this.lblListProgress.Name = "lblListProgress";
			this.lblListProgress.Size = new System.Drawing.Size(23, 12);
			this.lblListProgress.TabIndex = 18;
			this.lblListProgress.Text = "0/0";
			// 
			// btnDownload
			// 
			this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDownload.Location = new System.Drawing.Point(412, 159);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new System.Drawing.Size(106, 23);
			this.btnDownload.TabIndex = 14;
			this.btnDownload.Text = "ダウンロード開始";
			this.btnDownload.UseVisualStyleBackColor = true;
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// lblTimeCount
			// 
			this.lblTimeCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTimeCount.Location = new System.Drawing.Point(308, 224);
			this.lblTimeCount.Name = "lblTimeCount";
			this.lblTimeCount.Size = new System.Drawing.Size(47, 15);
			this.lblTimeCount.TabIndex = 16;
			this.lblTimeCount.Text = "00:00:00";
			// 
			// lbUrlList
			// 
			this.lbUrlList.AllowDrop = true;
			this.lbUrlList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbUrlList.ContextMenuStrip = this.contextMenuStrip1;
			this.lbUrlList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lbUrlList.FormattingEnabled = true;
			this.lbUrlList.ItemHeight = 12;
			this.lbUrlList.Location = new System.Drawing.Point(3, 10);
			this.lbUrlList.Name = "lbUrlList";
			this.lbUrlList.ScrollAlwaysVisible = true;
			this.lbUrlList.Size = new System.Drawing.Size(404, 208);
			this.lbUrlList.TabIndex = 11;
			this.lbUrlList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbUrlList_DrawItem);
			this.lbUrlList.SelectedIndexChanged += new System.EventHandler(this.lbUrlList_SelectedIndexChanged);
			this.lbUrlList.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbUrlList_DragDrop);
			this.lbUrlList.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbUrlList_DragEnter);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.コピーToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ペーストToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(150, 54);
			// 
			// コピーToolStripMenuItem
			// 
			this.コピーToolStripMenuItem.Name = "コピーToolStripMenuItem";
			this.コピーToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.コピーToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.コピーToolStripMenuItem.Text = "コピー";
			this.コピーToolStripMenuItem.Click += new System.EventHandler(this.コピーToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 6);
			// 
			// ペーストToolStripMenuItem
			// 
			this.ペーストToolStripMenuItem.Name = "ペーストToolStripMenuItem";
			this.ペーストToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.ペーストToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.ペーストToolStripMenuItem.Text = "ペースト";
			this.ペーストToolStripMenuItem.Click += new System.EventHandler(this.ペーストToolStripMenuItem_Click);
			// 
			// pnlBtn
			// 
			this.pnlBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlBtn.Controls.Add(this.btnNovelAdd);
			this.pnlBtn.Controls.Add(this.btnItemDn);
			this.pnlBtn.Controls.Add(this.btnItemUp);
			this.pnlBtn.Controls.Add(this.btnAddList);
			this.pnlBtn.Controls.Add(this.btnDelList);
			this.pnlBtn.Location = new System.Drawing.Point(410, 10);
			this.pnlBtn.Name = "pnlBtn";
			this.pnlBtn.Size = new System.Drawing.Size(110, 143);
			this.pnlBtn.TabIndex = 20;
			// 
			// btnNovelAdd
			// 
			this.btnNovelAdd.Location = new System.Drawing.Point(2, 120);
			this.btnNovelAdd.Name = "btnNovelAdd";
			this.btnNovelAdd.Size = new System.Drawing.Size(106, 23);
			this.btnNovelAdd.TabIndex = 3;
			this.btnNovelAdd.Text = "小説を追加";
			this.btnNovelAdd.UseVisualStyleBackColor = true;
			this.btnNovelAdd.Click += new System.EventHandler(this.btnNovelAdd_Click);
			// 
			// btnAddList
			// 
			this.btnAddList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddList.Location = new System.Drawing.Point(2, 0);
			this.btnAddList.Name = "btnAddList";
			this.btnAddList.Size = new System.Drawing.Size(106, 23);
			this.btnAddList.TabIndex = 1;
			this.btnAddList.Text = "リストに追加...";
			this.btnAddList.UseVisualStyleBackColor = true;
			this.btnAddList.Click += new System.EventHandler(this.btnAddList_Click);
			// 
			// btnDelList
			// 
			this.btnDelList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelList.BackColor = System.Drawing.Color.Yellow;
			this.btnDelList.Location = new System.Drawing.Point(2, 66);
			this.btnDelList.Name = "btnDelList";
			this.btnDelList.Size = new System.Drawing.Size(106, 23);
			this.btnDelList.TabIndex = 2;
			this.btnDelList.Text = "リストから削除";
			this.btnDelList.UseVisualStyleBackColor = false;
			this.btnDelList.Click += new System.EventHandler(this.btnDelList_Click);
			// 
			// uC_NovelDL
			// 
			this.uC_NovelDL.Location = new System.Drawing.Point(1, 254);
			this.uC_NovelDL.Name = "uC_NovelDL";
			this.uC_NovelDL.Size = new System.Drawing.Size(523, 63);
			this.uC_NovelDL.TabIndex = 22;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(524, 321);
			this.Controls.Add(this.uC_NovelDL);
			this.Controls.Add(this.chkVanish);
			this.Controls.Add(this.chkListChg);
			this.Controls.Add(this.btnfrmErrStShow);
			this.Controls.Add(this.lblStatusApp);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblListProgress);
			this.Controls.Add(this.btnDownload);
			this.Controls.Add(this.lblTimeCount);
			this.Controls.Add(this.lbUrlList);
			this.Controls.Add(this.pnlBtn);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(535, 355);
			this.Name = "frmMain";
			this.Text = "小説ダウンローダーFrontEnd";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.pnlBtn.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox chkVanish;
		private System.Windows.Forms.Button btnfrmErrStShow;
		private System.Windows.Forms.Button btnItemDn;
		private System.Windows.Forms.Button btnItemUp;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.CheckBox chkListChg;
		private System.Windows.Forms.Label lblStatusApp;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblListProgress;
		private System.Windows.Forms.Button btnDownload;
		private System.Windows.Forms.Label lblTimeCount;
		private System.Windows.Forms.ListBox lbUrlList;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem コピーToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem ペーストToolStripMenuItem;
		private System.Windows.Forms.Panel pnlBtn;
		private System.Windows.Forms.Button btnAddList;
		private System.Windows.Forms.Button btnDelList;
		private System.Windows.Forms.Button btnNovelAdd;
		private UC.UC_DownloadNovel uC_NovelDL;
	}
}

