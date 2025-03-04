using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using UC;

namespace noveldlFrontEnd
{

	public partial class frmMain : Form
	{
		[DllImport("KERNEL32.DLL")]
		public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);
		[DllImport("KERNEL32.DLL")]
		public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);
		[DllImport("kernel32.dll")]
		public static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpstring, string lpFileName);


		// WM_COPYDATAのメッセージID
		private const int WM_COPYDATA = 0x004A;
		private const int WM_USER = 0x400;
		private const int WM_DLINFO = WM_USER + 30;

		public string exePath = "";
		public string exeDirName = "";
		public string iniPath = "";
		private UInt32 TotalChap = 0;
		private UInt32 ChapCount = 0;
		private DateTime latestDLDateTime;
		private DateTime nextEveryDay;
		private DateTime nextEveryWeek;
		private DateTime nextEveryMon;
		private string sStatus = "";
		private int novelTotal = 0;
		private int novelCount = 0;
		public bool busy = false;
		private IntPtr hWnd = IntPtr.Zero;
		private string dlAfterOpeNovel1st = "";
		private string dlAfterOpeNovel1Later = "";
		private string[] dlAfterOpeProg = new string[3];

		private string[] dlAfterOpeProg_Type = new string[3];

		private string downloaderName = "";
		private string[] urlTopParts;
		private string UrlType = "";


		public bool DlAbort = false;
		public NOVEL_STATUS novelSt = NOVEL_STATUS.None;

		private Color lboxSelCol = Color.LightBlue;

		private frmErrStatus frmErrSt = null;

		public frmMain()
		{
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			Assembly myAssembly = Assembly.GetEntryAssembly();
			exePath = myAssembly.Location;
			exeDirName = Path.GetDirectoryName(exePath);
			iniPath = exeDirName + @"\" + Path.GetFileNameWithoutExtension(exePath) + ".ini";
			string logPath = exeDirName + @"\Log";
			if (Directory.Exists(logPath) == false)
			{
				Directory.CreateDirectory(logPath);
			}
			logPath += $@"\{DateTime.Now.ToString("yyyyMMdd")}.log";

			StringBuilder wk = new StringBuilder(512);
			GetPrivateProfileString("NextDownLoad", "毎日", "2000/01/01 00:00:00", wk, 512, iniPath);
			if(DateTime.TryParse(wk.ToString(),out nextEveryDay) == false)
			{ nextEveryDay = new DateTime(2000,01,01,00,00,00); }
			GetPrivateProfileString("NextDownLoad", "毎週", "2000/01/01 00:00:00", wk, 512, iniPath);
			if (DateTime.TryParse(wk.ToString(), out nextEveryWeek) == false)
			{ nextEveryWeek = new DateTime(2000, 01, 01, 00, 00, 00); }
			GetPrivateProfileString("NextDownLoad", "毎月", "2000/01/01 00:00:00", wk, 512, iniPath);
			if (DateTime.TryParse(wk.ToString(), out nextEveryMon) == false)
			{ nextEveryMon = new DateTime(2000, 01, 01, 00, 00, 00); }

			GetPrivateProfileString("DownloadAfterOperation", "Novel1st", "", wk, 512, iniPath);
			dlAfterOpeNovel1st = wk.ToString();
			GetPrivateProfileString("DownloadAfterOperation", "Novel1Later", "", wk, 512, iniPath);
			dlAfterOpeNovel1Later = wk.ToString();

			GetPrivateProfileString("DownloadAfterOperation", "ExtProgram1", "", wk, 512, iniPath);
			dlAfterOpeProg[0] = wk.ToString();
			GetPrivateProfileString("DownloadAfterOperation", "ExtProgram2", "", wk, 512, iniPath);
			dlAfterOpeProg[1] = wk.ToString();
			GetPrivateProfileString("DownloadAfterOperation", "ExtProgram3", "", wk, 512, iniPath);
			dlAfterOpeProg[2] = wk.ToString();

			frmErrSt = new frmErrStatus(this);

			hWnd = this.Handle;

			lblStatusApp.Text = "";

			frmErrSt.Location = new Point(this.Location.X, this.Location.Y + this.Size.Height);

			int num = (int)GetPrivateProfileInt("ListItems", "Count", -1, iniPath);
			for (int i = 0; i < num; i++)
			{
				GetPrivateProfileString("ListItems", $"Item{i + 1}", "", wk, 256, iniPath);
				string item = wk.ToString();
				if (item != "") lbUrlList.Items.Add(item);
			}
			lbUrlList_SelectedIndexChanged(null, null);

			if (nextEveryDay > DateTime.Now)
			{
				btnDownload.Enabled = false;
				if (MessageBox.Show($"最後にダウンロードしてから１２時間経過していません\nあと[{(nextEveryDay - DateTime.Now):hh\\:mm\\:ss}]\n実行しますか？(y/n)", "警告", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					Close();
				}
				lblStatusApp.Text = $"実行可能迄後[{(nextEveryDay - DateTime.Now):hh\\:mm\\:ss}]";
			}
			//toolTip1.SetToolTip(chkMoveComplete, "小説更新時に完結した小説を#完結セクションに移動する");
			toolTip1.SetToolTip(chkVanish, "小説更新時に消失していないか確認する");
			toolTip1.SetToolTip(chkListChg, "小説消失時にリストファイルの小説情報を移動します");
			//30日以上経過したログの消去
			try
			{
				string fpath = exeDirName;
				fpath += @"\Log\";
				if (Directory.Exists(fpath) == false)
				{
					Directory.CreateDirectory(fpath);
				}
				string[] files = System.IO.Directory.GetFiles(fpath, "*.log"); //, System.IO.SearchOption.AllDirectories);
				foreach (string path in files)
				{
					string fname = Path.GetFileNameWithoutExtension(path);
					int iy, im, id;
					if ((fname.Length == 6)
					&& (int.TryParse(fname.Substring(0, 2), out iy))
					&& (int.TryParse(fname.Substring(2, 2), out im))
					&& (int.TryParse(fname.Substring(4, 2), out id)))
					{
						DateTime fdate = new DateTime(iy + 2000, im, id);
						if ((DateTime.Now - fdate).TotalDays > 30)
						{
							System.IO.File.Delete(path);
						}
					}
				}
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
			}
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if ((e.CloseReason == CloseReason.UserClosing)
			&& (nextEveryDay < DateTime.Now))
			{
				if (MessageBox.Show(this, "終了しますか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				{
					e.Cancel = true;
					return;
				}
				if (busy && (MessageBox.Show(this, "ダウンロード中ですが本当に終了しますか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes))
				{
					e.Cancel = true;
					return;
				}
			}
		}

		private void lbUrlList_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.All;
		}

		private void lbUrlList_DragDrop(object sender, DragEventArgs e)
		{
			//ドラッグしてきたアイテムの文字列をstrに格納する
			string str = e.Data.GetData(DataFormats.Text).ToString();
			lbUrlList.Items.Add(str);
		}

		private void lbUrlList_DrawItem(object sender, DrawItemEventArgs e)
		{
			// 背景を描画する.
			e.DrawBackground(); // コメントアウトしても問題なさそう.
								// リストボックスの現在の選択インデックスを取得する.
			int idx = e.Index;
			Color testcol = Color.FromArgb(Color.Blue.ToArgb() | Color.Red.ToArgb());
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
					//txtBrush = new SolidBrush((((ListBox)sender).Enabled) ? Color.Black : e.ForeColor);
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

		private void lbUrlList_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnItemDn.Enabled = ((lbUrlList.Items.Count > (lbUrlList.SelectedIndex + 1)) && (lbUrlList.SelectedIndex != -1));
			btnItemUp.Enabled = ((lbUrlList.SelectedIndex != 0) && (lbUrlList.SelectedIndex != -1));
		}

		private void コピーToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int idx = lbUrlList.SelectedIndex;
			if (idx >= 0)
			{
				string str = (string)lbUrlList.Items[idx];
				IDataObject iData = Clipboard.GetDataObject(); // システムのクリップボードにあるデータを表す、IDataObjectを取得
				iData.SetData(str);
			}
		}

		private void ペーストToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IDataObject iData = Clipboard.GetDataObject(); // システムのクリップボードにあるデータを表す、IDataObjectを取得
			if (iData.GetDataPresent(DataFormats.UnicodeText))
			{
				string str = (string)iData.GetData(DataFormats.UnicodeText);
				lbUrlList.Items.Add(str);
			}
		}

		private void btnAddList_Click(object sender, EventArgs e)
		{
			lblStatusApp.Text = "";
			sStatus = "";

			string path = "";
			OpenFileDialog ofd = new OpenFileDialog();
			if (lbUrlList.Items.Count > 0)
			{
				path = Path.GetFullPath((string)lbUrlList.Items[lbUrlList.Items.Count - 1]);
				//int pos = path.LastIndexOf('\\');
				//ofd.InitialDirectory = path.Substring(0, pos);
				ofd.InitialDirectory = Path.GetDirectoryName(path);
			}
			ofd.FileName = "URL.txt";
			ofd.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
			ofd.FilterIndex = 1;
			ofd.Title = "追加するファイルを選択してください";
			ofd.RestoreDirectory = false;
			ofd.CheckFileExists = true;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				lbUrlList.Items.Add(getShortedPath(exePath, ofd.FileName));
				writeIniListItem();
			}
		}

		/// <summary>
		/// 対象のパスで絶対パス、相対パスの短い方を取得する
		/// </summary>
		/// <param name="basePath">相対パスの基準となる絶対パス</param>
		/// <param name="targetPath">対象の絶対パス</param>
		/// <returns>対象のより短いパス</returns>
		private string getShortedPath(string basePath, string targetPath)
		{
			string relpath = "";
			string[] baseItems = basePath.TrimEnd(' ', '\\').Split('\\');
			string[] tgtItems = targetPath.TrimEnd(' ', '\\').Split('\\');
			if (baseItems[0] != tgtItems[0])
			{
				return targetPath;
			}
			int idx = 1;
			for (; (idx < baseItems.Length) && (idx < tgtItems.Length); idx++)
			{
				if (baseItems[idx] != tgtItems[idx]) break;
			}
			for (int i = idx; i < (baseItems.Length - (File.Exists(basePath) ? 1 : 0)); i++)
			{
				relpath += @"..\";
			}
			for (; idx < (tgtItems.Length - 1); idx++)
			{
				relpath += tgtItems[idx] + @"\";
			}
			relpath += tgtItems[idx];
			return (relpath.Length < targetPath.Length) ? relpath : targetPath;
		}

		private void btnItemUp_Click(object sender, EventArgs e)
		{
			int idx = lbUrlList.SelectedIndex;
			if (idx > 0)
			{
				object tmpobj = lbUrlList.Items[idx - 1];
				lbUrlList.Items[idx - 1] = lbUrlList.Items[idx];
				lbUrlList.Items[idx] = tmpobj;
				lbUrlList.SelectedIndex--;
			}
		}

		private void btnItemDn_Click(object sender, EventArgs e)
		{
			int idx = lbUrlList.SelectedIndex;
			int maxno = lbUrlList.Items.Count - 1;
			if (idx < maxno)
			{
				object tmpobj = lbUrlList.Items[idx + 1];
				lbUrlList.Items[idx + 1] = lbUrlList.Items[idx];
				lbUrlList.Items[idx] = tmpobj;
				lbUrlList.SelectedIndex++;
			}
		}

		private void btnDelList_Click(object sender, EventArgs e)
		{
			lblStatusApp.Text = "";
			sStatus = "";
			if ((lbUrlList.SelectedIndex < 0) || (lbUrlList.SelectedIndex >= lbUrlList.Items.Count))
			{
				MessageBox.Show(this, "選択されていません", "Warning");
				return;
			}
			if (MessageBox.Show(this, $"{lbUrlList.Items[lbUrlList.SelectedIndex]} をリストから削除しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				lbUrlList.Items.RemoveAt(lbUrlList.SelectedIndex);
				writeIniListItem();
			}
		}

		private void btnfrmErrStShow_Click(object sender, EventArgs e)
		{
			if (frmErrSt.Visible == false)
			{
				frmErrSt.Show(this);
			}
			frmErrSt.Activate();
		}

		/// <summary>
		/// リストに表示している項目をINIファイルに書き込み
		/// </summary>
		private void writeIniListItem()
		{
			int num = lbUrlList.Items.Count;
			WritePrivateProfileString("ListItems", "Count", num.ToString(), iniPath);
			for (int i = 0; i < num; i++)
			{
				WritePrivateProfileString("ListItems", $"Item{i + 1}", (string)lbUrlList.Items[i], iniPath);
			}
		}

		private NOVEL_STATUS chkNovelSt(string strTitle)
		{
			int spos = strTitle.IndexOf('【');
			int epos = strTitle.IndexOf('】');
			if ((spos >= 0) && (epos > spos))
			{
				switch (strTitle.Substring(spos + 1, epos - spos - 1))
				{
					case "連載中": return NOVEL_STATUS.Running; break;
					case "中断": return NOVEL_STATUS.Stopped; break;
					case "短編":
					case "完結": return NOVEL_STATUS.complete; break;
				}
			}
			return NOVEL_STATUS.None;
		}

		private async void btnDownload_Click(object sender, EventArgs e)
		{
			if (busy)
			{
				DlAbort = true;
				lbUrlList.Enabled = false;
				btnDownload.Text = "中止中…";
				lblText(lblStatusApp, "この小説のダウンロード完了で停止します");
			}
			else
			{
				if (frmErrSt.Visible == false) frmErrSt.Show(this);
				lbUrlList.Enabled =
				pnlBtn.Enabled = false;
				btnDownload.Text = "中止";
				await DownloadAllAsync();
				lbUrlList.Enabled =
				pnlBtn.Enabled = true;
				btnDownload.Text = "ダウンロード開始";
			}
		}

		/// <summary>
		/// ダウンロードの非同期用
		/// </summary>
		/// <returns></returns>
		private async Task DownloadAllAsync()
		{
			timer1.Enabled = true;
			Task task = Task.Run(() => { DownloadAll(); });
			await task;
			timer1.Enabled = false;
		}

		/// <summary>
		/// lbUrlListのSelectedIndexを操作する
		/// 非同期スレッド／メインスレッド共用
		/// </summary>
		/// <param name="idx"></param>
		private delegate void dglbUrlListSelectedIndex(int idx);
		private void lbUrlListSelectedIndex(int idx)
		{
			if (lbUrlList.InvokeRequired)
			{
				this.Invoke(new dglbUrlListSelectedIndex(lbUrlListSelectedIndex), idx);
				return;
			}
			lbUrlList.SelectedIndex = idx;
		}

		/// <summary>
		/// lbUrlListの選択されているアイテムを取得する
		/// 非同期スレッド／メインスレッド共用
		/// </summary>
		/// <returns>選択されているアイテム</returns>
		private delegate object dglbUrlListGetItem();
		private object lbUrlListGetItem()
		{
			if (lbUrlList.InvokeRequired)
			{
				return this.Invoke(new dglbUrlListGetItem(lbUrlListGetItem));
			}
			return lbUrlList.Items[lbUrlList.SelectedIndex];
		}

		/// <summary>
		/// LabelのTextに文字列をセットする
		/// 非同期スレッド／メインスレッド共用
		/// </summary>
		/// <param name="label">対象ラベル</param>
		/// <param name="str">表示文字列</param>
		private delegate void dglblText(Label label, string str);
		private void lblText(Label label, string str)
		{
			if (label.InvokeRequired)
			{
				this.Invoke(new dglblText(lblText), label, str);
				return;
			}
			label.Text = str;
		}

		private delegate void dglblBkCol(Label label, Color col);
		private void lblBkCol(Label label, Color col)
		{
			if (label.InvokeRequired)
			{
				this.Invoke(new dglblBkCol(lblBkCol), label, col);
				return;
			}
			label.BackColor = col;
		}

		private delegate void dglistBoxAdd(ListBox lbox, string msg);
		private void listBoxAdd(ListBox lbox, string msg)
		{
			if (lbox.InvokeRequired)
			{
				this.Invoke(new dglistBoxAdd(listBoxAdd), lbox, msg);
				return;
			}
			((List<string>)((BindingSource)lbox.DataSource).DataSource).Add(msg);//リストボックスにバインディングされているデータソースに追加
			((BindingSource)lbox.DataSource).ResetBindings(false);//バインディングしているコントロールに再描画を通知
			lbox.SelectedIndex = lbox.Items.Count - 1;
		}

		private void LogOut(String logMsg)
		{
			string fpath = exeDirName + @"\Log\";
			if (!Directory.Exists(fpath))
			{
				Directory.CreateDirectory(fpath);
			}
			using (StreamWriter sw = new StreamWriter(fpath + DateTime.Now.ToString("yyMMdd") + @".log", true, Encoding.UTF8))
			{
				sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":" + logMsg);
			}
		}

		/// <summary>
		/// 表内の全てのリストファイルを利用してダウンロード
		/// </summary>
		private void DownloadAll()
		{
			try
			{
				lblText(lblStatusApp, "ダウンロード中");
				uC_NovelDL.statusClear();
				sStatus = "";

				busy = true;
				string section = CheckDateTime();

				if (section != "")
				{
					for (int idx = 0; idx < lbUrlList.Items.Count; idx++)
					{
						lbUrlListSelectedIndex(idx);
						if (DlAbort) break;

						string tmpPath = (string)lbUrlList.Items[idx];
						if (tmpPath.IndexOf(@"https://") == 0)
						{
							DownloadNovel(tmpPath);
							continue;
						}

						string listPath = Path.GetFullPath(tmpPath);
						if (File.Exists(listPath) == false)
						{
							MessageBox.Show(this, $"リスト[{tmpPath}]が有りません。スキップします", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							continue;
						}
						string[] linebuf = File.ReadAllLines(listPath);
						if (listNovelDL(linebuf, section, true) == false)
						{
							break;
						}
						if (DlAbort) break;
						novelTotal = novelCount;
						lblText(lblListProgress, "(" + "   0" + " / " + novelTotal.ToString().PadLeft(4) + ")");
						if (listNovelDL(linebuf, section) == false)
						{
							break;
						}
						if (DlAbort) break;
					}
					//lbUrlList.SelectedIndex = -1;
					lbUrlListSelectedIndex(-1);
					lblText(lblStatusApp, "ダウンロード終了");
					WriteNextDateTime(section);
				}
				else
				{
					MessageBox.Show(this, $"最後にダウンロードしてから１２時間経過していません\nあと[{nextEveryDay - DateTime.Now}]");
				}
				busy = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message,"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool chkUrl(string URL)
		{
			if ((urlTopParts == null) || (urlTopParts.Length <= 0)) return false;
			foreach (string str in urlTopParts)
			{
				if(URL.Contains(str))	return true;
			}
			return false;
		}

		private string getListFileInfo(string lineData)
		{
			//string DlBaseDir = "";
			string novelBaseDir = "";

			if (UrlType == "") UrlType = getSetting(lineData, "Type:");
			//if (DlBaseDir == "") DlBaseDir = getSetting(lineData, "DL Folder:");
			if (novelBaseDir == "") novelBaseDir = getSetting(lineData, "Novel Folder:");
			if ((UrlType != "")
			//&& (DlBaseDir != "")
			&& (novelBaseDir != ""))
			{
				StringBuilder wk = new StringBuilder(512);
				GetPrivateProfileString(UrlType, "Downloader", "", wk, 512, iniPath);
				downloaderName = wk.ToString();
				GetPrivateProfileString(UrlType, "URLTop", "", wk, 512, iniPath);
				urlTopParts = wk.ToString().Split(',');

				GetPrivateProfileString(UrlType, "ExtProgram1", "", wk, 512, iniPath);
				dlAfterOpeProg_Type[0] = wk.ToString();
				GetPrivateProfileString(UrlType, "ExtProgram2", "", wk, 512, iniPath);
				dlAfterOpeProg_Type[1] = wk.ToString();
				GetPrivateProfileString(UrlType, "ExtProgram3", "", wk, 512, iniPath);
				dlAfterOpeProg_Type[2] = wk.ToString();
				return novelBaseDir;
			}
			return "";
		}

		/// <summary>
		/// リストファイルの小説をダウンロードする
		/// </summary>
		/// <param name="linebuf">リストファイルの内容</param>
		/// <param name="section">読み込みセクション</param>
		/// <param name="countOnly">小説のカウントのみする時true</param>
		/// <returns>成功時true</returns>
		private bool listNovelDL(string[] linebuf, string section, bool countOnly = false)
		{
			bool result = false;

			//string DlBaseDir = "";
			string novelBaseDir = "";
			int seqno = 0;
			bool abortFlag = false;
			string filepath = "";
			//string infopath = "";
			string novelDir = "";

			novelCount = 0;
			UrlType = "";

			try
			{
				foreach (string linedata in linebuf)
				{
					if (DlAbort) break;
					string ldata = linedata.Trim();
					//文字が無いか、コメントなら読み飛ばし
					if ((ldata == "")
					|| (ldata[0] == '\''))
					{
						continue;
					}

					switch (seqno)
					{
						//小説を保存するベースフォルダと、ダウンロードした各章テキストを保存するベースフォルダを取得する
						case 0:
							//if(UrlType == "") UrlType = getSetting(ldata, "Type:");
							//if (DlBaseDir == "") DlBaseDir = getSetting(ldata, "DL Folder:");
							//if (novelBaseDir == "") novelBaseDir = getSetting(ldata, "Novel Folder:");
							//if ((UrlType != "") && (DlBaseDir != "") && (novelBaseDir != ""))
							//{
							//	StringBuilder wk = new StringBuilder(512);
							//	GetPrivateProfileString(UrlType, "Downloader", "", wk, 512, iniPath);
							//	downloaderName = wk.ToString();
							//	GetPrivateProfileString(UrlType, "URLTop", "", wk, 512, iniPath);
							//	urlTopParts = wk.ToString().Split(',');

							//	GetPrivateProfileString(UrlType, "ExtProgram1", "", wk, 512, iniPath);
							//	dlAfterOpeProg_Type[0] = wk.ToString();
							//	GetPrivateProfileString(UrlType, "ExtProgram2", "", wk, 512, iniPath);
							//	dlAfterOpeProg_Type[1] = wk.ToString();
							//	GetPrivateProfileString(UrlType, "ExtProgram3", "", wk, 512, iniPath);
							//	dlAfterOpeProg_Type[2] = wk.ToString();

							//	seqno++;
							//}
							novelBaseDir = getListFileInfo(ldata);
							if(novelBaseDir != "") seqno++;
							continue;
						//小説読み込みの最後のセクションを探す
						case 1:
							if ((ldata[0] == '#')
							&& (ldata.IndexOf(section) == 1))
							{
								seqno++;
							}
							break;
						//最後のセクションの次のセクションを示す"#"を見つけたら終了
						case 2:
							abortFlag = (ldata[0] == '#');
							break;
						default:
							seqno = 0;
							break;
					}
					if (DlAbort) break;
					if (abortFlag) break;

					if ((ldata.Length > 8)
					&& chkUrl(ldata)
					&& (string.IsNullOrEmpty(filepath) == false))
					{
						novelCount++;
						if (countOnly == false)
						{
							//string fullpath = novelBaseDir + @"\" + filepath;
							//string fname = Path.GetFileNameWithoutExtension(filepath);//.TrimEnd(new char[] { ' ', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
							//string fext = Path.GetExtension(filepath);
							//小説を格納するフォルダがなければ作成
							novelDir = Path.GetDirectoryName(novelBaseDir + @"\" + filepath);
							if (Directory.Exists(novelDir) == false)
							{
								Directory.CreateDirectory(novelDir);
							}
							//小説一つをダウンロード
							if ((DownloadNovel(ldata, novelBaseDir, filepath) == false)
							&& chkVanish.Checked && chkListChg.Checked)
							{
								chkListChg.Enabled = chkVanish.Checked;

								NovelListMove(filepath, ldata);
							}
							else if (novelSt == NOVEL_STATUS.complete)
							{
								NovelListMove(filepath, ldata, "#完結");
							}
							lblText(lblListProgress, "(" + novelCount.ToString().PadLeft(4) + " / " + novelTotal.ToString().PadLeft(4) + ")");
						}
						filepath = "";
					}
					else
					{
						filepath = ldata;
					}
				}
				result = true;
			}
			catch (Exception ex)
			{
				sStatus = $"ダウンロードエラー：{ex.Message}";
				lblText(lblStatusApp, sStatus);
			}
			return result;
		}

		/// <summary>
		/// 小説をダウンロード
		/// </summary>
		/// <param name="UrlAdr">URL</param>
		/// <param name="baseDir">ベースの格納するディレクトリ名</param>
		/// <param name="relfpath">ベースからの相対の格納するファイルパス</param>
		/// <returns>小説がある場合true</returns>
		private bool DownloadNovel(string UrlAdr, string baseDir = "", string relfpath = "")
		{
			bool result = true;
			//小説情報ファイルを読み込む
			string fname = Path.GetFileNameWithoutExtension(relfpath);
			string fext = Path.GetExtension(relfpath);
			//string fullpath = baseDir + @"\" + relfpath;
			string novelDir = Path.GetDirectoryName(baseDir + @"\" + relfpath);

			string dirname = ((string.IsNullOrEmpty(novelDir) ? exeDirName : novelDir));
			string infopath = $@"{dirname}\{fname}Info.txt";
			string logpath = $@"{dirname}\{fname}.log";
			string filepath = (string.IsNullOrEmpty(fname) ? "" : $@"{dirname}\{fname}{fext}");
			int latestChap = 0;
			ChapCount = 0;
			TotalChap = 0;
			DateTime latestDate = DateTime.Parse("2000/1/1");
			List<string> infoLines = null;
			try
			{
				if (File.Exists(infopath))
				{
					infoLines = File.ReadAllLines(infopath).ToList<string>();
					if (infoLines.Count > 0)
					{
						foreach (string ldata in infoLines)
						{
							string[] infos = ldata.Split(',');
							if (infos.Length >= 2)
							{
								if ((DateTime.TryParse(infos[0], out latestDate))
								&& (int.TryParse(infos[1], out latestChap)))
								{
									infoLines.Remove(ldata);
									break;
								}
							}
						}
					}
					if (DlAbort) return result;
				}

				int startChap = 0;
				//string tmppath = $@"{exeDirName}\tmp.txt";
				string tmppath = $@"{dirname}\__tmp.txt";
				Process proc = null;
				novelSt = NOVEL_STATUS.Vanishment;//WM_COPYDATAでダウンロード章数が０でも小説名を通知するかと期待したが通知はされない
												  //途中までダウンロードできていれば続きをダウンロードし、マージする
				if (latestChap > 0)
				{
					startChap = latestChap + 1;
					if (File.Exists(tmppath)) File.Delete(tmppath);
					uC_NovelDL.NovelTitle = filepath;
					//小説を続きの章から最新章までダウンロード
					proc = uC_NovelDL.novelDownload(downloaderName,UrlAdr, tmppath, startChap);
					proc.WaitForExit();

					//小説をダウンロードしたファイルが有る
					if (File.Exists(tmppath))
					{
						//小説ファイルをマージする
						//リンクの図を検索してリンクのみの文字列配列を取得し、情報ファイルの内容に追加・重複削除する
						int addFigCount = getFigLink(File.ReadAllLines(tmppath), ref infoLines);
						if (File.Exists(tmppath))
						{
							if (dlAfterOpeNovel1Later != "")
							{
								exeAfterOperation(dlAfterOpeNovel1Later, tmppath);
							}
							foreach (string str in dlAfterOpeProg_Type)
							{
								if (str == "") break;
								exeAfterOperation(str, tmppath);
							}
							foreach (string str in dlAfterOpeProg)
							{
								if (str == "") break;
								exeAfterOperation(str, tmppath);
							}
							//List<string> buff = File.ReadAllLines(tmppath).ToList<string>();
							string[] buff = File.ReadAllLines(tmppath);
							using (FileStream fs = File.Open(filepath, FileMode.Open))
							using (StreamWriter sw = new StreamWriter(fs, new UTF8Encoding()))
							{
								//int len = buff.Count;
								int len = buff.Length;
								fs.Seek(0, SeekOrigin.End);
								int idx = 0;
								for (; idx < len; idx++)
								{
									if (buff[idx].IndexOf("［＃中見出し］") >= 0) break;
								}
								for (; idx < len; idx++)
								{
									sw.WriteLine(buff[idx]);
								}
							}
							File.Delete(tmppath);
							File.Delete($@"{dirname}\__tmp.log");
						}
						ChapCount = TotalChap = uC_NovelDL.ChapNum;
						string msg = $"更新 #{startChap} + {ChapCount} Fig{addFigCount}:[{relfpath}], {UrlAdr}";
						listBoxAdd(frmErrSt.lboxErrStatus, msg);
						LogOut(msg);
					}
					else if (chkVanish.Checked)
					{
						//小説が消滅していないか確認する
						//lblText(lblStatusNovel, "小説存在確認中");
						startChap = latestChap - 1;
						//前回の最終章の１つ前の章からダウンロード
						tmppath = $@"{exeDirName}\tmp.txt";
						proc = uC_NovelDL.novelDownload(downloaderName, UrlAdr, tmppath, startChap);
						proc.WaitForExit();
						//ファイルができなければ小説が消滅している
						result = deleteTmpFiles();
						if (result == false)
						{
							//MessageBox.Show($"[{fname}]が消失しました", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							listBoxAdd(frmErrSt.lboxErrStatus, $"消滅:[{fname}], {UrlAdr}");
							LogOut($"{fname}、{UrlAdr}、消滅");
						}
					}
					//完結確認
					if (novelSt == NOVEL_STATUS.complete)
					{
						MessageBox.Show($"[{fname}]が完結しました", "通知", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						listBoxAdd(frmErrSt.lboxErrStatus, $"完結:[{fname}], {UrlAdr}");
						LogOut($"{fname}、{UrlAdr}、完結");
					}
				}
				else
				{
					//小説を最初から最新章までダウンロード
					proc = uC_NovelDL.novelDownload(downloaderName, UrlAdr, filepath);
					proc.WaitForExit();
					result = File.Exists(filepath);
					if (result)
					{
						infoLines = new List<string>();
						//リンクの図を検索してリンクのみの文字列配列を取得し、情報ファイルの内容に追加・重複削除する
						int addFigCount = getFigLink(File.ReadAllLines(filepath), ref infoLines);
						if (dlAfterOpeNovel1st != "")
						{
							exeAfterOperation(dlAfterOpeNovel1st, filepath);
						}
						foreach(string str in dlAfterOpeProg_Type)
						{
							if (str == "") break;
							exeAfterOperation(str, filepath);
						}
						foreach (string str in dlAfterOpeProg)
						{
							if (str == "") break;
							exeAfterOperation(str, filepath);
						}
						ChapCount = TotalChap = uC_NovelDL.ChapNum;
						string msg = $"追加 #1 + {ChapCount} Fig{addFigCount}:[{relfpath}], {UrlAdr}";
						listBoxAdd(frmErrSt.lboxErrStatus,msg);
						LogOut(msg);
					}
					else
					{
						//MessageBox.Show($"[{fname}]がダウンロードできませんでした", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						listBoxAdd(frmErrSt.lboxErrStatus, $"ロスト:[{fname}], {UrlAdr}");
						LogOut($"{fname}、{UrlAdr}、消滅");
					}
					//完結確認
					if (novelSt == NOVEL_STATUS.complete)
					{
						MessageBox.Show($"[{fname}]が完結しました", "通知", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						listBoxAdd(frmErrSt.lboxErrStatus, $"完結:[{fname}], {UrlAdr}");
						LogOut($"{fname}、{UrlAdr}、完結");
					}
				}
				if ((File.Exists(filepath)) && (ChapCount > 0))
				{
					//小説情報ファイルを書き込む
					using (StreamWriter sw = new StreamWriter(File.Create(infopath), new UTF8Encoding()))
					{
						sw.WriteLine($"{DateTime.Now}, {ChapCount + latestChap}");
						if (infoLines.Count > 0)
						{
							foreach (string str in infoLines)
							{
								sw.WriteLine(str);
							}
						}
					}
					//小説logファイルを読み込み、小説ステータスを取得する
					if(novelSt == NOVEL_STATUS.None)
					{
						string[] rlines = File.ReadAllLines(logpath);
						if (rlines.Length > 0)
						{
							foreach (string str in rlines)
							{
								if (str.IndexOf("タイトル:") >= 0)
								{
									novelSt = chkNovelSt(str);
									break;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				listBoxAdd(frmErrSt.lboxErrStatus, $"失敗:[{fname}], {UrlAdr}");
			}
			return result;
		}

		/// <summary>
		/// 外部プログラムの実行
		/// 
		/// iniファイルには以下が設定されている
		/// //初回ダウンロード後処理
		/// Novel1st=""C:\Program Files(x86)\sakura\sakura.exe" "-M=%A\na6dl用変換-自動.vbs" "%F""
		/// //2回目以降の差分ダウンロード後の差分ファイルに対しての処理
		/// Novel1Later=""C:\Program Files(x86)\sakura\sakura.exe" "-M=%A\na6dl用章追加変換-自動.vbs" "%F""
		/// </summary>
		/// <param name="cmdLine">コマンドライン、%Fは小説ファイル名に、%Aはexeフォルダパスに置き換え</param>
		/// <param name="filepath">小説のダウンロード保存先パス</param>
		private void exeAfterOperation(string cmdline, string filepath)
		{
			int pos = 0;
			string arg = "";
			string filename = "";
			cmdline = cmdline.Trim();
			if (cmdline[0] == '"')
			{
				pos = cmdline.IndexOf('\"', 1);
				filename = cmdline.Substring(1, pos - 1);//.Trim(new char[] { '"', ' ' });
			}
			else
			{
				pos = cmdline.IndexOf(' ', 1);
				filename = cmdline.Substring(0, pos);//.Trim(new char[] { '"', ' ' });
			}
			//引数を確定し、特定文字を置き換える
			arg = cmdline.Substring(pos + 1);//.Trim(new char[] { '"', ' ' });
			arg = arg.Replace("%F", filepath).Replace("%f", filepath);
			arg = arg.Replace("%A", exeDirName).Replace("%a", exeDirName);
			//プロセスを作成し、実行する
			ProcessStartInfo pInfo = new ProcessStartInfo();
			pInfo.FileName = filename;
			pInfo.Arguments = arg;
			//意味がない
			//pInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
			//pInfo.UseShellExecute = false; // シェル機能を使用しない
			Process p = Process.Start(pInfo);
			//終了待ち
			p.WaitForExit();
			//MessageBox.Show("終了しました");
		}

		/// <summary>
		/// ダウンロードした一時ファイルを削除する
		/// 一時ファイルがなければ小説が消滅している
		/// </summary>
		/// <returns>削除した時true</returns>
		bool deleteTmpFiles()
		{
			string tmppath = $@"{exeDirName}\tmp.txt";
			if (File.Exists(tmppath))
			{
				File.Delete(tmppath);
				tmppath = $@"{exeDirName}\tmp.log";
				if (File.Exists(tmppath)) File.Delete(tmppath);
				return true;
			}
			string[] fnames = Directory.GetFiles(exeDirName, $"tmp*.*");
			if (fnames.Length > 0)
			{
				foreach (string filename in fnames)
				{
					File.Delete(filename);
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// リフトファイルの指定小説情報を、指定セクションに移動する
		/// 毎回ファイルを読み込み
		/// リストファイルは
		/// Novel Folder:格納するフォルダの絶対パス
		/// #毎日
		/// #毎週
		/// #毎月
		/// #完結
		/// #消滅
		/// ##
		/// のような構造となる
		/// </summary>
		/// <param name="srcIndex">移動する小説情報位置</param>
		/// <param name="section">移動先のセクション</param>
		private void NovelListMove(string filepath, string url, string section = "#消滅")
		{
			try
			{
				//指定されたURLがリストファイルに有れば移動する
				string listfilepath = (string)lbUrlListGetItem();
				List<string> Lines = File.ReadAllLines(listfilepath).ToList<string>();
				int idx = Lines.IndexOf(section);
				if ((idx > 0) && Lines.Contains(filepath) && Lines.Remove(url))
				{
					Lines.Remove(filepath);
					//セクションが有って、情報削除が成功した時、リストの指定セクションの最後に追加する
					int pos = idx++;
					for (; idx < Lines.Count; idx++)
					{
						if (string.IsNullOrEmpty(Lines[idx].Trim()) == false)
						{
							if (Lines[idx].Trim().Substring(0, 1) == "#") break;
							pos = idx;
						}
					}
					Lines.InsertRange(pos + 1, new string[] { filepath, url });
					File.WriteAllLines(listfilepath, Lines, Encoding.UTF8);
				}
			}
			catch (Exception ex)
			{
				string errstr = ex.Message;
			}
		}

		/// <summary>
		/// 指定されたURLがリストファイルに無ければ指定セクションに追加する
		/// </summary>
		/// <param name="filePath">追加するファイルパス</param>
		/// <param name="URL">追加するURL</param>
		/// <param name="section">追加するセクション</param>
		private void NovelListAdd(string filePath, string URL,  string section = "#毎週")
		{
			string listfilepath = (string)lbUrlListGetItem();
			List<string> Lines = File.ReadAllLines(listfilepath).ToList<string>();
			int idx = Lines.IndexOf(section);
			if ((idx > 0) && (Lines.Contains(URL) == false))
			{
				//URLが無いのでリストの毎週の最後に追加する
				int pos = -1;
				for (int i = idx + 1; i < Lines.Count; i++)
				{
					if (string.IsNullOrEmpty(Lines[i].Trim()) == false)
					{
						if (Lines[i].Trim().Substring(0, 1) == "#") break;
						pos = i;
					}
				}
				if (pos > 0)
				{
					Lines.InsertRange(pos + 1, new List<string>() { filePath, URL });
					File.WriteAllLines(listfilepath, Lines, Encoding.UTF8);
				}
			}
		}

		/// <summary>
		/// リストファイル内の設定項目を取得
		/// </summary>
		/// <param name="lineData"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string getSetting(string lineData, string key)
		{
			int idx = lineData.IndexOf(key);
			if (idx >= 0)
			{
				string res = lineData.Substring(idx + key.Length);
				return res;
			}
			return "";
		}

		/// <summary>
		/// 前回ダウンロードからの時間でダウンロードするセクションを決定する
		/// </summary>
		/// <returns></returns>
		private string CheckDateTime()
		{
			String result = "";

			//DateTime tmpdt;
			latestDLDateTime = DateTime.Now;

			if (latestDLDateTime >= nextEveryMon)
			{
				int monInterval = DateTime.DaysInMonth(latestDLDateTime.Year, latestDLDateTime.Month) - latestDLDateTime.Day + 1;
				nextEveryMon = DateTime.Parse(latestDLDateTime.AddDays(monInterval).ToString("yyyy/MM/dd"));
				nextEveryWeek = getEveryWeekNext(latestDLDateTime);
				nextEveryDay = latestDLDateTime.AddHours(12);
				result = "毎月";
			}
			else if (latestDLDateTime >= nextEveryWeek)
			{
				nextEveryWeek = getEveryWeekNext(latestDLDateTime);
				nextEveryDay = latestDLDateTime.AddHours(12);
				result = "毎週";
			}
			else if (latestDLDateTime >= nextEveryDay)
			{
				nextEveryDay = latestDLDateTime.AddHours(12);
				result = "毎日";
			}
			return result;
		}

		/// <summary>
		/// 前回ダウンロードからの時間でダウンロードするセクションを決定する
		/// </summary>
		/// <returns></returns>
		private void WriteNextDateTime(string section)
		{
			switch (section)
			{
				case "毎月":
					WritePrivateProfileString("NextDownLoad", "毎月", nextEveryMon.ToString(), iniPath);
					WritePrivateProfileString("NextDownLoad", "毎週", nextEveryWeek.ToString(), iniPath);
					WritePrivateProfileString("NextDownLoad", "毎日", nextEveryDay.ToString(), iniPath);
					break;
				case "毎週":
					WritePrivateProfileString("NextDownLoad", "毎週", nextEveryWeek.ToString(), iniPath);
					WritePrivateProfileString("NextDownLoad", "毎日", nextEveryDay.ToString(), iniPath);
					break;
				case "毎日":
					WritePrivateProfileString("NextDownLoad", "毎日", nextEveryDay.ToString(), iniPath);
					break;
			}
			WritePrivateProfileString("NextDownLoad", "実行日時", latestDLDateTime.ToString(), iniPath);
		}

		/// <summary>
		/// 毎週の次回取得可能な最近の日時を取得する
		/// </summary>
		/// <param name="nowDateTime"></param>
		/// <returns></returns>
		private DateTime getEveryWeekNext(DateTime nowDateTime)
		{
			int weekInterval;
			StringBuilder wk = new StringBuilder(256);

			//毎週のINIで指定された曜日にダウンロードする
			GetPrivateProfileString("Setting", "曜日", "月", wk, 256, iniPath);
			switch (wk.ToString())
			{
				case "日": weekInterval = 7; break;
				default:
				case "月": weekInterval = 8; break;
				case "火": weekInterval = 9; break;
				case "水": weekInterval = 10; break;
				case "木": weekInterval = 11; break;
				case "金": weekInterval = 12; break;
				case "土": weekInterval = 13; break;
			}
			weekInterval -= (int)nowDateTime.DayOfWeek;
			if (weekInterval > 7) weekInterval -= 7;
			return DateTime.Parse(nowDateTime.AddDays(weekInterval).ToString("yyyy/MM/dd"));
		}

		/// <summary>
		/// "［＃リンクの図（//41743.mitemin.net/userpageimage/viewimagebig/icode/i813181/）入る］"と同様の文字列を含む行を抽出、
		/// 「https://41743.mitemin.net/i813181/」
		/// リンクのみにして指定のリストにマージ、
		/// 重複を削除する
		/// </summary>
		/// <param name="strSrray">抽出元の文字列配列￥</param>
		/// <param name="destlist">マージする文字列のリスト</param>
		private int getFigLink(string[] strSrray, ref List<string> destlist)
		{
			string[] strs = getFigLink(strSrray);
			destlist.AddRange(strs);
			destlist.Distinct();
			return destlist.Count;
		}

		private string[] getFigLink(string[] strSrray)
		{
			switch(UrlType)
			{
				case "カクヨム":
					//  https://kakuyomu.jp/users/mezukusugaki/news/16817330662721550689
					return strSrray.Where(str => str.Contains("https://kakuyomu.jp/users/")).Select(str => Regex.Replace(str, @"^.*https:", "https:")).ToArray();
				default:
					//［＃リンクの図（//41743.mitemin.net/userpageimage/viewimagebig/icode/i813181/）入る］
					//return strSrray.Where(str => str.Contains("リンクの図")).Select(str => Regex.Replace(str, @"^.*リンクの図（", "https:")).Select(str => Regex.Replace(str, @"）入る］.*", "")).ToArray();
					return strSrray.Where(str => str.Contains("リンクの図")).Select(str => Regex.Replace(str, @"^.*リンクの図（", "https:")).Select(str => Regex.Replace(str, @"/userpageimage/viewimagebig/icode/", @"/")).Select(str => Regex.Replace(str, @"）入る］.*", "")).ToArray();
			}
		}

		private string latesttime = "";
		private void timer1_Tick(object sender, EventArgs e)
		{
			string strtime = $"{DateTime.Now - latestDLDateTime:hh\\:mm\\:ss}";
			if (latesttime != strtime)
			{
				//lblTimeCount.Text = strtime;
				lblText(lblTimeCount, strtime);
				latesttime = strtime;
			}
		}

		private void chkVanish_CheckedChanged(object sender, EventArgs e)
		{
			chkListChg.Enabled = chkVanish.Checked;
		}

		private async void btnNovelAdd_Click(object sender, EventArgs e)
		{
			int idx = lbUrlList.SelectedIndex;
			if (idx < 0)
			{
				MessageBox.Show("小説を追加するリストファイルを選択してください");
				return;
			}

			string tmppath = (string)lbUrlList.Items[idx];
			string listPath = Path.GetFullPath(tmppath);
			if (File.Exists(listPath) == false)
			{
				MessageBox.Show(this, $"リスト[{tmppath}]が有りません。スキップします", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			bool btndlen = btnDownload.Enabled;
			btnDownload.Enabled = 
			lbUrlList.Enabled =
			pnlBtn.Enabled = false;
			string baseDir = "";
			using (StreamReader sr = new StreamReader(listPath))
			{
				for (; baseDir == "" && !sr.EndOfStream;)
				{
					baseDir = getListFileInfo(sr.ReadLine());
				}
			}
			frmNovelAdd frmNA = new frmNovelAdd(baseDir);
			if (frmNA.ShowDialog() == DialogResult.OK)
			{
				if (frmErrSt.Visible == false) frmErrSt.Show(this);
				string dirpath = Path.GetDirectoryName(baseDir + @"\" + frmNA.RelPath);
				Directory.CreateDirectory(dirpath);

				await DownloadOneAsync(frmNA.URL, baseDir, frmNA.RelPath);
			}
			btnDownload.Enabled = btndlen;
			lbUrlList.Enabled =
			pnlBtn.Enabled = true;
		}

		private async Task DownloadOneAsync(string url, string baseDir, string relPath)
		{
			latestDLDateTime = DateTime.Now;
			timer1.Enabled = true;
			Task task = Task.Run(() => { DownloadOne(url, baseDir, relPath); });
			await task;
			timer1.Enabled = false;
		}

		private void DownloadOne(string url, string baseDir, string relPath)
		{
			if (DownloadNovel(url, baseDir, relPath))
			{
				if (novelSt == NOVEL_STATUS.complete)
				{
					NovelListAdd(relPath, url, "#完結");
				}
				else
				{
					NovelListAdd(relPath, url, "#毎週");
				}
			}
		}

	}
}
