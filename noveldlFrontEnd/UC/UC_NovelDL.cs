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

namespace UC
{
	public enum NOVEL_STATUS
	{
		Vanishment = -1,
		None = 0,
		Running = 1,
		Stopped = 2,
		complete = 9,
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct COPYDATASTRUCT32
	{
		[FieldOffset(0)] public UInt32 dwData;
		[FieldOffset(4)] public UInt32 cbData;
		[FieldOffset(8)] public IntPtr lpData;
	}

	public partial class UC_DownloadNovel : UserControl
	{
		// WM_COPYDATAのメッセージID
		private const int WM_COPYDATA = 0x004A;
		private const int WM_USER = 0x400;
		private const int WM_DLINFO = WM_USER + 30;

		private IntPtr hWnd = IntPtr.Zero;
		private UInt32 TotalChap = 0;
		private UInt32 ChapCount = 0;
		private string UrlType = "";
		private string sStatus = "";
		//public bool DlAbort = false;
		public NOVEL_STATUS novelSt = NOVEL_STATUS.None;

		public string NovelTitle
		{
			set { lblText(lblTitle, value); }
			get { return lblTitle.Text; }
		}

		public UInt32 ChapNum
		{
			get { return TotalChap; }
		}

		public UC_DownloadNovel()
		{
			InitializeComponent();
		}

		private void UC_DownloadNovel_Load(object sender, EventArgs e)
		{
			hWnd = this.Handle;
			lblTitle.Text =
			lblStatus.Text = "";
			lblProgress.Text = "";

		}

		private NOVEL_STATUS chkNovelSt(string strTitle)
		{
			int spos = strTitle.IndexOf('【');
			int epos = strTitle.IndexOf('】');
			if ((spos >= 0) && (epos >= 0))
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

		/// <summary>
		/// C#による仮想WndProc()、WM_COPYDATAとWM_USER+30(WM_DLINFO)をハンドルする
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			switch (m.Msg)
			{
				case WM_COPYDATA:
					{
						COPYDATASTRUCT32 cds = (COPYDATASTRUCT32)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT32));
						TotalChap = cds.dwData;
						string novelname = Marshal.PtrToStringAuto(cds.lpData);
						lblTitle.Text = novelname;
						novelSt = chkNovelSt(novelname);
					}
					break;
				case WM_DLINFO:
					{
						ChapCount = (UInt32)m.WParam;
						lblProgress.Text = $"{(int)(ChapCount * 100 / TotalChap)}".PadLeft(3) + $@"% ({ChapCount}/{TotalChap})";
						lblProgress.BackColor = ((ChapCount & 1) == 0) ? SystemColors.Control : Color.AliceBlue;
					}
					break;
			}
		}

		/// <summary>
		/// ダウンローダーを使用して小説をダウンロードする
		/// </summary>
		/// <param name="downloaderName">ダウンローダー名</param>
		/// <param name="URL">ダウンロードするURL</param>
		/// <param name="filePath">保存する小説ファイルのパス</param>
		/// <param name="startChap">ダウンロード開始する章番号</param>
		/// <returns>ダウンロード処理のプロセス</returns>
		public Process novelDownload(string downloaderName, string URL, string filePath = null, int startChap = 1)
		{
			lblText(lblStatus, "ダウンロード中");
			lblText(lblProgress, "");
			Process proc = new Process();
			proc.StartInfo.FileName = downloaderName; // @"na6dl.exe";
			string arg = $" \"-h {hWnd}\"";
			if (string.IsNullOrEmpty(filePath) == false)
			{
				arg += $" \"{filePath}\"";
			}
			if (startChap > 0)
			{
				arg += $" \"-s {startChap}\"";
			}
			arg += $" {URL}";
			proc.StartInfo.Arguments = arg;

			proc.StartInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
			proc.StartInfo.UseShellExecute = false; // シェル機能を使用しない
			proc.SynchronizingObject = this;
			proc.Exited += new EventHandler(proc_Exited);//終了イベントを登録
			proc.EnableRaisingEvents = true;
			//起動する
			proc.Start();
			return proc;
		}

		/// <summary>
		/// プロセスが終了したときに実行される
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void proc_Exited(object sender, EventArgs e)
		{
			if (sStatus == "")
			{
				lblText(lblStatus, "ダウンロード終了");
				if (TotalChap != 0) lblText(lblProgress, $"100% ({TotalChap}/{TotalChap})");
			}
			else
			{
				lblText(lblStatus, sStatus);
			}
			lblBkCol(lblProgress, SystemColors.Control);
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

		public void statusClear()
		{
			lblText(lblTitle, "");
			lblText(lblStatus, "");
			lblText(lblProgress, "");
		}

	}
}
