Option Explicit

'na6dl32�Ń_�E�����[�h�����e�L�X�g�̑}�G�����N��u��������
'"�m�������N�̐}�i//[����5���̃��[�U�[�R�[�h].mitemin.net/userpageimage/viewimagebig/icode/[i+����6���̉摜ID]/�j����n"
'"�m���}�G�i[�摜�t�@�C����]�j����n"
'�t�@�C���ŏ�����i���A��Җ��ŁA���͉��s2���󏑎��̂͂�
'�r���݂͍͂�҃R�����g�Ȃ̂ō폜����
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
	' ������h�~
	Call Editor.SetDrawSwitch(0)	' �ȍ~�`���~(ReplaceAll�̃v���O���X�o�[�͕\�������)
	Call Editor.AddRefUndoBuffer()	' ����ȍ~�AUndo�o�b�t�@���܂Ƃ߂�

	'�}�N���̃t�@�C����������"-����"�Ȃ玩���X�N���v�g�p�Ƀt���O�Z�b�g����
	strFilePath = .ExpandParameter("$M")	'���̃}�N���̃t���p�X
	If Mid(strFilePath, InStrRev(strFilePath,".") -3, 3) = "-����" Then
		AUTO_FLAG = True
	Else
		AUTO_FLAG = False
	End If

	LatestPos = CLng(.ExpandParameter("$y"))
	tmpstr = Editor.ExpandParameter("$F")	'�J���Ă���t�@�C���̃t���p�X
	dirPath = Left(tmpstr, InStrRev(tmpstr,"\"))

'	'�����N�̐}���m���}�G�i�摜�t�@�C�����j����n"�ɕϊ�
	Call .ReplaceAll("^[ �@\t]*�m�������N�̐}�i//([0-9]+)\.mitemin\..*icode/([i0-9]+)/(.*$)","�m���}�G�i$1_$2.jpg$3", 20) ' ���ׂĒu��

	'�r���݂͍͂�҃R�����g�Ȃ̂ō폜����
	.GoFileTop
	LineNo = CLng(.ExpandParameter("$y"))
	Call .SearchNext("�m����������r�͂݁n", 20)	'����
	On Error Resume Next
	do Until (CLng(Editor.ExpandParameter("$y")) = LineNo)
		LineNo = CLng(.ExpandParameter("$y"))
		Call .GoLineTop(8)
		Call .BeginSelect
		Call .SearchNext("�m�������Ōr�͂ݏI���n", 20)	'����
		Call .GoLineEnd_Sel(8)
'		strLine = .GetSelectedString(0) '�I�𕶎�����擾
'		Call .MessageBox(strLine, 32)
		Call .Delete
		Call .SearchNext("�m����������r�͂݁n", 20)	'����
'		Call .MessageBox(CStr(Editor.ExpandParameter("$y")) & ";"& CStr(LineNo), 32)
	Loop
	On Error GoTo 0
	'1�s�ڂ̓^�C�g���A2�s�ڂ͍�Җ��Ȃ̂�3�s�ڈȍ~����ŏ��̌��o���܂ō폜���ĉ��߂�3���s��}������
	Call .Jump(3,1)
	'��������ŏ���"�m���匩�o���n"or"�m�������o���n"�܂ł͕s�v
	Call .BeginSelect
	Call .SearchNext("�m��(��|��)���o���n", 20)	'����
	Call .GoLineTop_Sel(8)
'	strLine = .GetSelectedString(0) '�I�𕶎�����擾
'	Call .MessageBox(strLine, 32)
	Call .Delete

	Call Char(13)
	Call Char(13)
	Call Char(13)

	Call Editor.SetUndoBuffer()		' �����ł܂Ƃ߂�Undo�o�b�t�@�̃��X�g�ɓo�^�����
	Call Editor.MoveCursor( LatestPos, 1, 0)
	Call .FileSave						' �㏑���ۑ�

	.SearchClearMark	' �����}�[�N���N���A����
	.SetDrawSwitch(1)	' �`��ĊJ
	.ReDraw(0)			' �ĕ`��
End With
End Function

Call main()
If AUTO_FLAG Then
	ExitAll()
End If

