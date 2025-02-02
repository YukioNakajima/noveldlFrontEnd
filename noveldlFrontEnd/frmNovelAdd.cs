using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace noveldlFrontEnd
{
	public partial class frmNovelAdd : Form
	{
		private string baseDirPath = "";

		public string URL
		{
			get { return tbURL.Text; }
		}
		public string RelPath
		{
			get { return tbNovelPath.Text; }
		}

		public frmNovelAdd(string BaseDirPath)
		{
			InitializeComponent();

			baseDirPath = BaseDirPath;
		}

		private void frmNovelAdd_FormClosing(object sender, FormClosingEventArgs e)
		{
			switch(e.CloseReason)
			{
				case CloseReason.UserClosing:
					e.Cancel = true;
					break;
			}
		}

		private void btnNovelOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.InitialDirectory = baseDirPath;
			ofd.Filter = "テキストファイル(*.txt)|*.txt|全てのファイル(*.*)|*.*";
			ofd.FilterIndex = 1;
			ofd.Title = "小説を書き込むファイルを選択してください";
			ofd.RestoreDirectory = true;
			ofd.CheckFileExists = false;
			ofd.CheckPathExists = false;
			//sfd.CheckWriteAccess = true;
			//ダイアログを表示する
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				String fpath = ofd.FileName;
				if (fpath.Contains(baseDirPath))
				{
					tbNovelPath.Text = fpath.Substring(baseDirPath.Length + 1);
				}
				else
				{
					MessageBox.Show($"URLリストのベースフォルダ内にありません、無効なファイルパスです");
				}
			}
		}

		private void tbURL_TextChanged(object sender, EventArgs e)
		{
			btnDL.Enabled = tbURL.Text.Contains(@"http") && (string.Compare(StrLeft(tbNovelPath.Text, 4), ".txt", true) == 0);
		}

		private void tbNovelPath_TextChanged(object sender, EventArgs e)
		{
			btnDL.Enabled = tbURL.Text.Contains(@"http") && (string.Compare(StrLeft(tbNovelPath.Text,4),".txt", true) ==0);
		}
		private string StrLeft(string target, int number)
		{
			if (target.Length <= number) return "";
			return target.Substring(target.Length - number);
		}
	}
}
