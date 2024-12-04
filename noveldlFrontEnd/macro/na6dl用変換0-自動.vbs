Option Explicit

'na6dl32でダウンロードしたテキストの挿絵リンクを置き換える
'"［＃リンクの図（//[数字5桁のユーザーコード].mitemin.net/userpageimage/viewimagebig/icode/[i+数字6桁の画像ID]/）入る］"
'"［＃挿絵（[画像ファイル名]）入る］"
'ファイル最初が作品名、作者名で、次は改行2つが青空書式のはず
'罫線囲みは作者コメントなので削除する
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

'	'リンクの図を［＃挿絵（画像ファイル名）入る］"に変換
	Call .ReplaceAll("^[ 　\t]*［＃リンクの図（//([0-9]+)\.mitemin\..*icode/([i0-9]+)/(.*$)","［＃挿絵（$1_$2.jpg$3", 20) ' すべて置換

	'罫線囲みは作者コメントなので削除する
	.GoFileTop
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
	'1行目はタイトル、2行目は作者名なので3行目以降から最初の見出しまで削除して改めて3個改行を挿入する
	Call .Jump(3,1)
	'ここから最初の"［＃大見出し］"or"［＃中見出し］"までは不要
	Call .BeginSelect
	Call .SearchNext("［＃(中|大)見出し］", 20)	'検索
	Call .GoLineTop_Sel(8)
'	strLine = .GetSelectedString(0) '選択文字列を取得
'	Call .MessageBox(strLine, 32)
	Call .Delete

	Call Char(13)
	Call Char(13)
	Call Char(13)

	Call Editor.SetUndoBuffer()		' ここでまとめてUndoバッファのリストに登録される
	Call Editor.MoveCursor( LatestPos, 1, 0)
	Call .FileSave						' 上書き保存

	.SearchClearMark	' 検索マークをクリアする
	.SetDrawSwitch(1)	' 描画再開
	.ReDraw(0)			' 再描画
End With
End Function

Call main()
If AUTO_FLAG Then
	ExitAll()
End If

