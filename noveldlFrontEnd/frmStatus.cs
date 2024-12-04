using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace noveldlFrontEnd
{
	public partial class frmErrStatus : Form
	{
		private BindingSource bindingSource = new BindingSource();
		private List<string> lbSource = new List<string>();

		private frmMain fmain = null;
		public frmErrStatus(frmMain mainForm)
		{
			InitializeComponent();
			fmain = mainForm;
		}

		private void frmErrStatus_Load(object sender, EventArgs e)
		{
			bindingSource.DataSource = lbSource;
			lboxErrStatus.DataSource = bindingSource;
		}

		private void frmErrStatus_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
		}

		private void btnHide_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void btnUrlOpen_Click(object sender, EventArgs e)
		{
			int idx = lboxErrStatus.SelectedIndex;
			if (idx < 0) return;
			string url = ((string)lboxErrStatus.Items[idx]).Split(',')[1].Trim();
			System.Diagnostics.Process.Start(url);
		}

		private void lboxErrStatus_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			int idx = lboxErrStatus.IndexFromPoint(e.Location);
			if (idx < 0) return;
			string url = ((string)lboxErrStatus.Items[idx]).Split(',')[1].Trim();
			System.Diagnostics.Process.Start(url);
		}

		private Color lboxSelCol = Color.LightBlue;
		private void lboxErrStatus_DrawItem(object sender, DrawItemEventArgs e)
		{
			// 背景を描画する.
			e.DrawBackground(); // コメントアウトしても問題なさそう.
			// リストボックスの現在の選択インデックスを取得する.
			int idx = e.Index;
			if (idx > -1)
			{
				SolidBrush bkBrush = null; // 背景の描画色.
				SolidBrush txtBrush = null; // 文字の描画色.
				if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
				{
					// 選択されていない場合.
					bkBrush = new SolidBrush(e.BackColor);
					txtBrush = new SolidBrush(e.ForeColor);
				}
				else
				{
					// 選択されている場合.
					bkBrush = new SolidBrush(lboxSelCol);
					//txtBrush = new SolidBrush(e.ForeColor);
					txtBrush = new SolidBrush(Color.Black);
				}
				//描画する文字列の取得
				ListBox listbox = (ListBox)(sender);
				String str = listbox.Items[idx].ToString();
				// 背景を塗りつぶす.
				Rectangle rct = e.Bounds;
				e.Graphics.FillRectangle(bkBrush, rct);
				// 背景の上に文字列を描画する.
				Font font = e.Font;
				e.Graphics.DrawString(str, font, txtBrush, rct);
				// ソリッドブラシを破棄する.
				if (bkBrush != null) { bkBrush.Dispose(); bkBrush = null; }
				if (txtBrush != null) { txtBrush.Dispose(); txtBrush = null; }
			}
			// 選択された項目の外枠を示す矩形を描画する.
			e.DrawFocusRectangle(); // コメントアウトしても問題なさそう.
		}

		public void WriteErrStLogFile(bool criete = true)
		{
			string fpath = fmain.exeDirName + @"\Log\";
			if(!Directory.Exists(fpath))
			{
				Directory.CreateDirectory(fpath);
			}
			using (StreamWriter sw = new StreamWriter(fpath + DateTime.Now.ToString("yyMMdd") + @"_ErrSt.log", !criete, Encoding.UTF8))
			{
				foreach(string line in lboxErrStatus.Items)
				{
					sw.WriteLine(line);
				}
			}
		}

		public void DeleteErrStLogFileAll()
		{
			string folderpath = fmain.exeDirName + @"\Log\";
			foreach (string fpath in Directory.EnumerateFiles(folderpath, "*_ErrSt.log"))
			{
				//１ファイルの削除実行。
				File.Delete(fpath);
			}
		}

		private void btnLogOut_Click(object sender, EventArgs e)
		{
			WriteErrStLogFile();
		}

		private void btnLogDelAll_Click(object sender, EventArgs e)
		{
			DeleteErrStLogFileAll();
		}
	}
}
