Option Explicit

'章を追加するためのファイルが対象
'na6dl32でダウンロードしたテキストの挿絵リンクを置き換える
'"［＃リンクの図（//[数字5桁のユーザーコード].mitemin.net/userpageimage/viewimagebig/icode/[i+数字6桁の画像ID]/）入る］"
'"［＃挿絵（[画像ファイル名]）入る］"
'ファイル先頭から最初の"［＃中見出し］"までは作品紹介なので不要
'罫線囲みは作者コメントなので削除する
' Ver.2
'

Private AUTO_FLAG 'As Boolean

'*******************************************************************************
'*******************************************************************************
Private Function main()
	Dim LineNo 'As Lng
	Dim LatestPos 'As Lng
	Dim strLine 'As String
	Dim tmpstr 'As String
	Dim fileName 'As String
	Dim dirPath 'As String
	Dim strSize 'As String
	Dim sx,sy
	Dim pos1,Pos2 ' As Integer
	Dim strFilePath 'As String
	Dim ReplaceFile

With Editor
	' ちらつき防止
	Call Editor.SetDrawSwitch(0)	' 以降描画停止(ReplaceAllのプログレスバーは表示される)
	Call Editor.AddRefUndoBuffer()	' これ以降、Undoバッファをまとめる

	'マクロのファイル名末尾が"-自動"なら自動スクリプト用にフラグセットする
	strFilePath = .ExpandParameter("$M")	'このマクロのフルパス
	If Mid(strFilePath, InStrRev(strFilePath,".") -3, 3) = "-自動" Then
		AUTO_FLAG = True
	Else
		AUTO_FLAG = False
	End If

	LatestPos = CLng(.ExpandParameter("$y"))
	tmpstr = Editor.ExpandParameter("$F")	'開いているファイルのフルパス
	dirPath = Left(tmpstr, InStrRev(tmpstr,"\"))
	'リンクの図を［＃挿絵（画像ファイル名）入る］"に変換
	Call .ReplaceAll("^[ 　\t]*［＃リンクの図（//([0-9]+)\.mitemin\..*icode/([i0-9]+)/(.*$)","［＃挿絵（$1_$2.jpg$3", 20) ' すべて置換
'	'カクヨム用"https://kakuyomu.jp/users/mezukusugaki/news/16817330660882288213"を［＃挿絵（画像ファイル名）入る］"に変換
	Call .ReplaceAll("^[ 　\t]*https://kakuyomu.jp/users/[^/]+/news/([i0-9]+)(.*$)","［＃挿絵（$1.jpg）入る］", 20) ' すべて置換

	'ファイル先頭から最初の"［＃中見出し］"までは作品紹介なので不要
	.GoFileTop
	Call .BeginSelect
	Call .SearchNext("［＃中見出し］", 20)	'検索
	Call .GoLineTop_Sel(8)
	Call .Delete

	'罫線囲みは作者コメントなので削除する
'	.GoFileTop
	LineNo = CLng(.ExpandParameter("$y"))
	Call .SearchNext("［＃ここから罫囲み］", 20)	'検索
	On Error Resume Next
	do Until (CLng(Editor.ExpandParameter("$y")) = LineNo)
		LineNo = CLng(.ExpandParameter("$y"))
		Call .GoLineTop(8)
		Call .BeginSelect
		Call .SearchNext("［＃ここで罫囲み終わり］", 20)	'検索
		Call .GoLineEnd_Sel(8)
'		strLine = .GetSelectedString(0) '選択文字列を取得
'		Call .MessageBox(strLine, 32)
		Call .Delete
		Call .SearchNext("［＃ここから罫囲み］", 20)	'検索
'		Call .MessageBox(CStr(Editor.ExpandParameter("$y")) & ";"& CStr(LineNo), 32)
	Loop
	On Error GoTo 0

	Call Editor.SetUndoBuffer()		' ここでまとめてUndoバッファのリストに登録される
	Call Editor.MoveCursor( LatestPos, 1, 0)
	.SearchClearMark	' 検索マークをクリアする
	.SetDrawSwitch(1)	' 描画再開
	.ReDraw(0)			' 再描画
End With
End Function

Call main()
If AUTO_FLAG Then
	Call Editor.FileSave					' 上書き保存
	ExitAll()
End If

