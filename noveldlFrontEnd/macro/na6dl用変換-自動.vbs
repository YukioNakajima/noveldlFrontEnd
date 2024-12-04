Option Explicit

'na6dl32�Ń_�E�����[�h�����e�L�X�g�̑}�G�����N��u��������
'"�m�������N�̐}�i//[����5���̃��[�U�[�R�[�h].mitemin.net/userpageimage/viewimagebig/icode/[i+����6���̉摜ID]/�j����n"
'"�m���}�G�i[�摜�t�@�C����]�j����n"
'�t�@�C���ŏ�����i���A��Җ��ŁA���͉��s2���󏑎��̂͂�
'�r���݂͍͂�҃R�����g�Ȃ̂ō폜����
'

Private AUTO_FLAG 'As Boolean

'********************************************************************************
'TXT�t�@�C���̕����Z�b�g�̔���
'NonSoft���̃T���v���\�[�X���Q�l��VBScript�œ��삷��悤���ς��Ă��܂��B
'http://homepage2.nifty.com/nonnon/index.html
'http://homepage2.nifty.com/nonnon/SoftSample/SampleModJUDG.html
'
'****************************************************************************
' �@�\��    : Module1.bas
' �@�\����  : �����R�[�h����
' ���l      :
' ���쌠    : Copyright(C) 2008 - 2009 �̂� All rights reserved
' ---------------------------------------------------------------------------
' �g�p����  : ���̃T�C�g�̓��e���g�p(���p/����/�]��/���S��)�������ʕ���s����
'           : �����Ɍ��J/�z�z����ꍇ�́A���̃T�C�g���Q�l�ɂ����|���L�q���Ă�
'           : �������B(��)WEB�y�[�W��ReadMe�Ƀ����N��\���Ă�������
' ---------------------------------------------------------------------------
'****************************************************************************

Private Const JUDGEFIX = 9999       '�����R�[�h���聓
Private Const JUDGESIZEMAX = 1000   '�����R�[�h����o�C�g��
Private Const SingleByteWeight = 1  '�P�o�C�g�����R�[�h�̈�v�d��
Private Const Multi_ByteWeight = 2  '�����o�C�g�����R�[�h�̈�v�d��

Private Const ctrl = 0		'����R�[�h
Private Const asci = 1		'ASCII
Private Const roma = 2		'JIS���[�}��
Private Const kana = 3		'JIS�J�i�i���p�J�i�j
Private Const kanO = 4		'��JIS���� (1978)
Private Const kanN = 5		'�VJIS���� (1983/1990)
Private Const kanH = 6		'JIS�⏕����

'----�����R�[�h����
' �֐���    : JudgeCode
' �Ԃ�l    : ���茋�ʕ����R�[�h���iADODB.Stream��Charset�p�j
' ������    : FilePath : ���蕶���e�L�X�g�t�@�C��
' �@�\����  : �����R�[�h�𔻒肷��
' ���l      :
Public Function JudgeCode(FilePath,ByRef probability)
	JudgeCode = "Shift_JIS"
	Dim lngSJIS 'As Long
	Dim lngJIS 'As Long
	Dim lngEUC 'As Long
	Dim lngUNI 'As Long
	Dim lngUTF7 'As Long
	Dim lngUTF8 'As Long

	Dim sIn
	Dim bytCode(1000)
	Dim CodeMax

	Set sIn = CreateObject("ADODB.Stream")
	sIn.Type = 1 ' �o�C�i���[���[�h
	sIn.Open
	sIn.LoadFromFile(FilePath)
	For CodeMax = 0 to JUDGESIZEMAX -1
		bytCode(CodeMax) = AscB(sIn.Read(1))
		If sIn.EOS Then
			Exit For
		End If
    Next
	Set sIn = Nothing
'	Call Editor.InfoMsg("CodeMax=" & CodeMax )
	lngJIS = JudgeJIS(bytCode,CodeMax, True)
'	Call Editor.InfoMsg("lngJIS=" & lngJIS )
	If lngJIS >= JUDGEFIX Then
		probability = lngJIS
		JudgeCode = "ISO-2022-JP"		'JIS
		Exit Function
	End If

	lngUNI = JudgeUNI(bytCode,CodeMax, True)
'	Call Editor.InfoMsg("lngUNI=" & lngUNI )
	If lngUNI >= JUDGEFIX Then
		probability = lngUNI
		JudgeCode = "UNICODE"
		Exit Function
	End If

	lngUTF8 = JudgeUTF8(bytCode,CodeMax, True)
'	Call Editor.InfoMsg("lngUTF8=" & lngUTF8 )
	If lngUTF8 >= JUDGEFIX Then
		probability = lngUTF8
		JudgeCode = "UTF-8"
		Exit Function
	End If

	lngUTF7 = JudgeUTF7(bytCode,CodeMax, True)
'	Call Editor.InfoMsg("lngUTF7=" & lngUTF7 )
	If lngUTF7 >= JUDGEFIX Then
		probability = lngUTF7
		JudgeCode = "UTF-7"
		Exit Function
	End If

	lngSJIS = JudgeSJIS(bytCode,CodeMax, True)
'	Call Editor.InfoMsg("lngSJIS=" & lngSJIS )
	If lngSJIS >= JUDGEFIX Then
		probability = lngUNI
		JudgeCode = "Shift_JIS"
		Exit Function
	End If

	lngEUC = JudgeEUC(bytCode,CodeMax, True)
'	Call Editor.InfoMsg("lngEUC=" & lngEUC )
	If lngEUC >= JUDGEFIX Then
		probability = lngEUC
		JudgeCode = "EUC-JP"
		Exit Function
	End If

	If lngSJIS >= lngSJIS And lngSJIS >= lngUNI And lngSJIS >= lngJIS And _
	lngSJIS >= lngUTF7 And lngSJIS >= lngUTF8 And lngSJIS >= lngEUC Then
		probability = lngSJIS
		JudgeCode = "Shift_JIS"
		Exit Function
	End If

	If lngUTF8 >= lngSJIS And lngUTF8 >= lngUNI And lngUTF8 >= lngJIS And _
	lngUTF8 >= lngUTF7 And lngUTF8 >= lngUTF8 And lngUTF8 >= lngEUC Then
		probability = lngUTF8
		JudgeCode = "UTF-8"
		Exit Function
	End If

	If lngUNI >= lngSJIS And lngUNI >= lngUNI And lngUNI >= lngJIS And _
	lngUNI >= lngUTF7 And lngUNI >= lngUTF8 And lngUNI >= lngEUC Then
		probability = lngUNI
		JudgeCode = "UNICODE"
		Exit Function
	End If

	If lngJIS >= lngSJIS And lngJIS >= lngUNI And lngJIS >= lngJIS And _
	lngJIS >= lngUTF7 And lngJIS >= lngUTF8 And lngJIS >= lngEUC Then
		probability = lngJIS
		JudgeCode = "ISO-2022-JP"		'JIS
		Exit Function
	End If

	If lngUTF7 >= lngSJIS And lngUTF7 >= lngUNI And lngUTF7 >= lngJIS And _
	lngUTF7 >= lngUTF7 And lngUTF7 >= lngUTF8 And lngUTF7 >= lngEUC Then
		probability = lngUTF7
		JudgeCode = "UTF-7"
		Exit Function
	End If

	If lngEUC >= lngSJIS And lngEUC >= lngUNI And lngEUC >= lngJIS And _
	lngEUC >= lngUTF7 And lngEUC >= lngUTF8 And lngEUC >= lngEUC Then
		probability = lngEUC
		JudgeCode = "EUC-JP"
		Exit Function
	End If

End Function

'----SJIS�֌W
' �֐���    : JudgeSJIS
' �Ԃ�l    : ���茋�ʊm���i���j
' ������    : bytCode : ���蕶���f�[�^
'           : fixFlag : �m�蔻�f�L��
' �@�\����  : SJIS�̕����R�[�h����(�\��)�m�����v�Z����
' ���l      :
Private Function JudgeSJIS(ByRef bytCode(),CodeMax, fixFlag )
	Dim i 'As Long
	Dim lngFit 'As Long
	Dim lngUB 'As Long

	lngUB = JUDGESIZEMAX - 1
	If lngUB > CodeMax Then
		lngUB = CodeMax
	End If
	For i = 0 To lngUB
		'81-9F,E0-EF(1�o�C�g��)
		If (bytCode(i) >= &H81 And bytCode(i) <= &H9F) Or _
			(bytCode(i) >= &HE0 And bytCode(i) <= &HEF) Then
			If i <= UBound(bytCode) - 1 Then
				'40-7E,80-FC(2�o�C�g��)
				If (bytCode(i + 1) >= &H40 And bytCode(i + 1) <= &H7E) Or _
					(bytCode(i + 1) >= &H80 And bytCode(i + 1) <= &HFC) Then
					lngFit = lngFit + (2 * Multi_ByteWeight)
					i = i + 1
				End If
			End If

			'A1-DF(1�o�C�g��)
			ElseIf (bytCode(i) >= &HA1 And bytCode(i) <= &HDF) Then
				lngFit = lngFit + (1 * SingleByteWeight)

			'20-7E(1�o�C�g��)
			ElseIf (bytCode(i) >= &H20 And bytCode(i) <= &H7E) Then
				lngFit = lngFit + (1 * SingleByteWeight)

			'00-1F, 7F(1�o�C�g��)
			ElseIf (bytCode(i) >= &H0 And bytCode(i) <= &H1F) Or _
				bytCode(i) = &H7F Then
				lngFit = lngFit + (1 * SingleByteWeight)
		End If
	Next 
	JudgeSJIS = (lngFit * 100) / ((lngUB + 1) * Multi_ByteWeight)
End Function

'----JIS�֌W
' �֐���    : JudgeJIS
' �Ԃ�l    : ���茋�ʊm���i���j
' ������    : bytCode : ���蕶���f�[�^
'           : fixFlag : �m�蔻�f�L��
' �@�\����  : JIS�̕����R�[�h����(�\��)�m�����v�Z����
' ���l      :
Private Function JudgeJIS(ByRef bytCode(),CodeMax, fixFlag )
	Dim i 'As Long
	Dim lngFit 'As Long
	Dim lngMode 'As JISMODE
	Dim lngUB 'As Long

	lngUB = JUDGESIZEMAX - 1
	If lngUB > CodeMax Then
		lngUB = CodeMax
	End If
	For i = 0 To lngUB
		'1B(1�o�C�g��)
		If bytCode(i) = &H1B Then
			If i <= UBound(bytCode) - 2 Then
				'28 42(2�E3�o�C�g��)
				If bytCode(i + 1) = &H28 And bytCode(i + 1) <= &H42 Then
					lngMode = asci
					lngFit = lngFit + (3 * Multi_ByteWeight)
					i = i + 2
					If fixFlag Then
						JudgeJIS = JUDGEFIX
						Exit Function
					End If
				End If
				'28 4A(2�E3�o�C�g��)
				If bytCode(i + 1) = &H28 And bytCode(i + 1) <= &H4A Then
					lngMode = roma
					lngFit = lngFit + (3 * Multi_ByteWeight)
					i = i + 2
					If fixFlag Then
						JudgeJIS = JUDGEFIX
						Exit Function
					End If
				End If
				'28 49(2�E3�o�C�g��)
				If bytCode(i + 1) = &H28 And bytCode(i + 1) <= &H49 Then
					lngMode = kana
					lngFit = lngFit + (3 * Multi_ByteWeight)
					i = i + 2
					If fixFlag Then
						JudgeJIS = JUDGEFIX
						Exit Function
					End If
				End If
				'24 40(2�E3�o�C�g��)
				If bytCode(i + 1) = &H24 And bytCode(i + 1) <= &H40 Then
					lngMode = kanO
					lngFit = lngFit + (3 * Multi_ByteWeight)
					i = i + 2
					If fixFlag Then
						JudgeJIS = JUDGEFIX
						Exit Function
					End If
				End If
				'24 42(2�E3�o�C�g��)
				If bytCode(i + 1) = &H24 And bytCode(i + 1) <= &H42 Then
					lngMode = kanN
					lngFit = lngFit + (3 * Multi_ByteWeight)
					i = i + 2
					If fixFlag Then
						JudgeJIS = JUDGEFIX
						Exit Function
					End If
				End If
				'24 44(2�E3�o�C�g��)
				If bytCode(i + 1) = &H24 And bytCode(i + 1) <= &H44 Then
					lngMode = kanH
					lngFit = lngFit + (3 * Multi_ByteWeight)
					i = i + 2
					If fixFlag Then
						JudgeJIS = JUDGEFIX
						Exit Function
					End If
				End If
			End If
		Else
			Select Case lngMode
			Case ctrl, asci, roma
				'00-1F,7F
				If (bytCode(i) >= &H0 And bytCode(i) <= &H1F) Or _
					bytCode(i) = &H7F Then
					lngFit = lngFit + (1 * SingleByteWeight)
				End If
				'20-7E
				If (bytCode(i) >= &H20 And bytCode(i) <= &H7E) Then
					lngFit = lngFit + (1 * SingleByteWeight)
				End If
			Case kana
				'21-5F
				If (bytCode(i) >= &H21 And bytCode(i) <= &H5F) Then
					lngFit = lngFit + (1 * SingleByteWeight)
				End If
			Case kanO, kanN, kanH
				If i <= UBound(bytCode) - 1 Then
					'21-7E
					If (bytCode(i) >= &H21 And bytCode(i) <= &H7E) And _
						(bytCode(i - 1) >= &H21 And bytCode(i - 1) <= &H7E) Then
						lngFit = lngFit + (2 * Multi_ByteWeight)
						i = i + 1
					End If
				End If
			End Select
		End If
	Next 
	JudgeJIS = (lngFit * 100) / ((lngUB + 1) * Multi_ByteWeight)
End Function

'----EUC�֌W
' �֐���    : JudgeEUC
' �Ԃ�l    : ���茋�ʊm���i���j
' ������    : bytCode : ���蕶���f�[�^
'           : fixFlag : �m�蔻�f�L��
' �@�\����  : EUC�̕����R�[�h����(�\��)�m�����v�Z����
' ���l      :
Private Function JudgeEUC(ByRef bytCode(),CodeMax, fixFlag) 
	Dim i 'As Long
	Dim lngFit 'As Long
	Dim lngUB 'As Long

	lngUB = JUDGESIZEMAX - 1
	If lngUB > CodeMax Then
		lngUB = CodeMax
	End If
	For i = 0 To lngUB
		'8E(1�o�C�g��) + A1-DF(2�o�C�g��)
		If bytCode(i) = &H8E Then
			If i <= UBound(bytCode) - 1 Then
				If bytCode(i + 1) >= &HA1 And bytCode(i + 1) <= &HDF Then
					lngFit = lngFit + (2 * Multi_ByteWeight)
					i = i + 1
				End If
			End If

		'8F(1�o�C�g��) + A1-0xFE(2�E3�o�C�g��)
		ElseIf bytCode(i) = &H8F Then
			If i <= UBound(bytCode) - 2 Then
				If (bytCode(i + 1) >= &HA1 And bytCode(i + 1) <= &HFE) And _
				(bytCode(i + 2) >= &HA1 And bytCode(i + 2) <= &HFE) Then
					lngFit = lngFit + (3 * Multi_ByteWeight)
					i = i + 2
				End If
			End If

		'A1-FE(1�o�C�g��) + A1-FE(2�o�C�g��)
		ElseIf bytCode(i) >= &HA1 And bytCode(i) <= &HFE Then
			If i <= UBound(bytCode) - 1 Then
				If bytCode(i + 1) >= &HA1 And bytCode(i + 1) <= &HFE Then
					lngFit = lngFit + (2 * Multi_ByteWeight)
					i = i + 1
				End If
			End If

		'20-7E(1�o�C�g��)
		ElseIf (bytCode(i) >= &H20 And bytCode(i) <= &H7E) Then
			lngFit = lngFit + (1 * SingleByteWeight)

		'00-1F, 7F(1�o�C�g��)
		ElseIf (bytCode(i) >= &H0 And bytCode(i) <= &H1F) Or _
			bytCode(i) = &H7F Then
			lngFit = lngFit + (1 * SingleByteWeight)
		End If
	Next 
	JudgeEUC = (lngFit * 100) / ((lngUB + 1) * Multi_ByteWeight)
End Function

'----UNICODE�֌W
' �֐���    : JudgeUNI
' �Ԃ�l    : ���茋�ʊm���i���j
' ������    : bytCode : ���蕶���f�[�^
'           : fixFlag : �m�蔻�f�L��
' �@�\����  : UTF16�̕����R�[�h����(�\��)�m�����v�Z����
' ���l      :
Private Function JudgeUNI(ByRef bytCode(),CodeMax, fixFlag )
	Dim i 'As Long
	Dim lngFit 'As Long
	Dim lngUB 'As Long

	lngUB = JUDGESIZEMAX - 1
	If lngUB > CodeMax Then
		lngUB = CodeMax
	End If
	For i = 0 To lngUB
		If fixFlag Then
			'BOM
			If bytCode(i) = &HFF Then
				If i <= UBound(bytCode) - 1 Then
					If bytCode(i + 1) = &HFE Then
						JudgeUNI = JUDGEFIX
						Exit Function
					End If
				End If
			End If
'			'���p�̏�
'			If bytCode(i) = &H0 Then
'				JudgeUNI = JUDGEFIX
'				Exit Function
'			End If
		End If

		If i <= UBound(bytCode) - 1 Then
			'00(2�o�C�g��)
			If (bytCode(i + 1) = &H0) Then
				'00-FF(1�o�C�g��)
				lngFit = lngFit + (2 * Multi_ByteWeight)

			'01-33(2�o�C�g��)
			ElseIf (bytCode(i + 1) >= &H1 And bytCode(i + 1) <= &H33) Then
				'00-FF(1�o�C�g��)
				lngFit = lngFit + (2 * Multi_ByteWeight)

			'34-4D(2�o�C�g��)
			ElseIf (bytCode(i + 1) >= &H34 And bytCode(i + 1) <= &H4D) Then
				'00-FF(1�o�C�g��)----��----
				lngFit = 0
				Exit For

			'4E-9F(2�o�C�g��)
			ElseIf (bytCode(i + 1) >= &H4E And bytCode(i + 1) <= &H9F) Then
				'00-FF(1�o�C�g��)
				lngFit = lngFit + (2 * Multi_ByteWeight)

			'A0-AB(2�o�C�g��)
			ElseIf (bytCode(i + 1) >= &HA0 And bytCode(i + 1) <= &HAB) Then
				'00-FF(1�o�C�g��)----��----
				lngFit = 0
				Exit For

			'AC-D7(2�o�C�g��)
			ElseIf (bytCode(i + 1) >= &HAC And bytCode(i + 1) <= &HD7) Then
				'00-FF(1�o�C�g��)----�n���O��----
				lngFit = 0
				Exit For

			'D8-DF(2�o�C�g��)
			ElseIf (bytCode(i + 1) >= &HD8 And bytCode(i + 1) <= &HDF) Then
				'00-FF(1�o�C�g��)
				lngFit = lngFit + (2 * Multi_ByteWeight)

			'E0-F7(2�o�C�g��)
			ElseIf (bytCode(i + 1) >= &HE0 And bytCode(i + 1) <= &HF7) Then
				'00-FF(1�o�C�g��)----�O��----
				lngFit = 0
				Exit For

			'F8-FF(2�o�C�g��)
			ElseIf (bytCode(i + 1) >= &HF8 And bytCode(i + 1) <= &HFF) Then
				'00-FF(1�o�C�g��)
				lngFit = lngFit + (2 * Multi_ByteWeight)

			End If
			i = i + 1
		End If
	Next 
	JudgeUNI = (lngFit * 100) / ((lngUB + 1) * Multi_ByteWeight)
End Function

'----UTF7�֌W
' �֐���    : JudgeUTF7
' �Ԃ�l    : ���茋�ʊm���i���j
' ������    : bytCode : ���蕶���f�[�^
'           : fixFlag : �m�蔻�f�L��
' �@�\����  : UTF7�̕����R�[�h����(�\��)�m�����v�Z����
' ���l      :
Private Function JudgeUTF7(ByRef bytCode(),CodeMax, fixFlag ) 
	Dim i 'As Long
	Dim lngFit 'As Long
	Dim lngWrk 'As Long
	Dim str64 'As String
	Dim bln64 'As Boolean
	str64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
	Dim lngUB 'As Long
	Dim lngBY 'As Long
	Dim lngXB 'As Long
	Dim lngXX 'As Long

	lngUB = JUDGESIZEMAX - 1
	If lngUB > CodeMax Then
		lngUB = CodeMax
	End If
	lngWrk = 0

	For i = 0 To lngUB
		'+�`-�܂ł�BASE64ENCODE
		If bytCode(i) = Asc("+") And bln64 = False Then
			lngWrk = 1
			bln64 = True
		ElseIf bytCode(i) = Asc("-") Then
			If lngWrk <= 0 Then
				lngWrk = lngWrk + 1
				lngFit = lngFit + (lngWrk * SingleByteWeight)
			ElseIf lngWrk = 1 Then
				lngWrk = lngWrk + 1
				lngFit = lngFit + (lngWrk * Multi_ByteWeight)
			ElseIf lngWrk >= 4 And lngXB < 6 And _
				((InStr(str64, Chr(bytCode(i - 1))) - 1) And lngXX) = 0 Then
				lngWrk = lngWrk + 1
				lngFit = lngFit + (lngWrk * Multi_ByteWeight)
			End If
			lngWrk = 0
			bln64 = False
		Else
			If bln64 = True Then
				'BASE64ENCODE��
				If InStr(str64, Chr(bytCode(i))) > 0 Then
					lngBY = Int((lngWrk * 6) / 8)
					lngXB = (lngWrk * 6) - (lngBY * 8)
					lngXX = (2 ^ lngXB) - 1
					lngWrk = lngWrk + 1
				Else
					lngWrk = 0
					bln64 = False
				End If
			Else
				'20-7E(1�o�C�g��)
				If (bytCode(i) >= &H20 And bytCode(i) <= &H7E) Then
					lngFit = lngFit + (1 * SingleByteWeight)

				'00-1F, 7F(1�o�C�g��)
				ElseIf (bytCode(i) >= &H0 And bytCode(i) <= &H1F) Or _
					bytCode(i) = &H7F Then
					lngFit = lngFit + (1 * SingleByteWeight)
				End If
			End If
		End If
	Next 
	JudgeUTF7 = (lngFit * 100) / ((lngUB + 1) * Multi_ByteWeight)
End Function

'----UTF8�֌W
' �֐���    : JudgeUTF8
' �Ԃ�l    : ���茋�ʊm���i���j
' ������    : bytCode : ���蕶���f�[�^
'           : fixFlag : �m�蔻�f�L��
' �@�\����  : UTF8�̕����R�[�h����(�\��)�m�����v�Z����
' ���l      :
Private Function JudgeUTF8(ByRef bytCode(),CodeMax, fixFlag )
	Dim i 
	Dim lngFit
	Dim lngUB

	lngUB = JUDGESIZEMAX - 1
	If lngUB > CodeMax Then
		lngUB = CodeMax
	End If
	For i = 0 To lngUB
		If fixFlag Then
		'BOM
			If bytCode(i) = &HEF Then
				If i <= UBound(bytCode) - 2 Then
					If bytCode(i + 1) = &HBB And _
						bytCode(i + 2) = &HBF Then
						JudgeUTF8 = JUDGEFIX
						Exit Function
					End If
				End If
			End If
		End If

		'AND FC(1�o�C�g��) + 80-BF(2-6�o�C�g��)
		If (bytCode(i) And &HFC) = &HFC Then
			If i <= UBound(bytCode) - 5 Then
				If (bytCode(i + 1) >= &H80 And bytCode(i + 1) <= &HBF) And _
					(bytCode(i + 2) >= &H80 And bytCode(i + 2) <= &HBF) And _
					(bytCode(i + 3) >= &H80 And bytCode(i + 3) <= &HBF) And _
					(bytCode(i + 4) >= &H80 And bytCode(i + 4) <= &HBF) And _
					(bytCode(i + 5) >= &H80 And bytCode(i + 5) <= &HBF) Then
					lngFit = lngFit + (6 * Multi_ByteWeight)
					i = i + 5
				End If
			End If

		'AND F8(1�o�C�g��) + 80-BF(2-5�o�C�g��)
		ElseIf (bytCode(i) And &HF8) = &HF8 Then
			If i <= UBound(bytCode) - 4 Then
				If (bytCode(i + 1) >= &H80 And bytCode(i + 1) <= &HBF) And _
					(bytCode(i + 2) >= &H80 And bytCode(i + 2) <= &HBF) And _
					(bytCode(i + 3) >= &H80 And bytCode(i + 3) <= &HBF) And _
					(bytCode(i + 4) >= &H80 And bytCode(i + 4) <= &HBF) Then
					lngFit = lngFit + (5 * Multi_ByteWeight)
					i = i + 4
				End If
			End If

		'AND F0(1�o�C�g��) + 80-BF(2-4�o�C�g��)
		ElseIf (bytCode(i) And &HF0) = &HF0 Then
			If i <= UBound(bytCode) - 3 Then
				If (bytCode(i + 1) >= &H80 And bytCode(i + 1) <= &HBF) And _
					(bytCode(i + 2) >= &H80 And bytCode(i + 2) <= &HBF) And _
					(bytCode(i + 3) >= &H80 And bytCode(i + 3) <= &HBF) Then
					lngFit = lngFit + (4 * Multi_ByteWeight)
					i = i + 3
				End If
			End If

		'AND E0(1�o�C�g��) + 80-BF(2-3�o�C�g��)
		ElseIf (bytCode(i) And &HE0) = &HE0 Then
			If i <= UBound(bytCode) - 2 Then
				If (bytCode(i + 1) >= &H80 And bytCode(i + 1) <= &HBF) And _
					(bytCode(i + 2) >= &H80 And bytCode(i + 2) <= &HBF) Then
					lngFit = lngFit + (3 * Multi_ByteWeight)
					i = i + 2
				End If
			End If

		'AND C0(1�o�C�g��) + 80-BF(2�o�C�g��)
		ElseIf (bytCode(i) And &HC0) = &HC0 Then
			If i <= UBound(bytCode) - 1 Then
				If (bytCode(i + 1) >= &H80 And bytCode(i + 1) <= &HBF) Then
					lngFit = lngFit + (2 * Multi_ByteWeight)
					i = i + 1
				End If
			End If

		'20-7E(1�o�C�g��)
		ElseIf (bytCode(i) >= &H20 And bytCode(i) <= &H7E) Then
			lngFit = lngFit + (1 * SingleByteWeight)

		'00-1F, 7F(1�o�C�g��)
		ElseIf (bytCode(i) >= &H0 And bytCode(i) <= &H1F) Or _
			bytCode(i) = &H7F Then
			lngFit = lngFit + (1 * SingleByteWeight)
		End If
	Next 
	JudgeUTF8 = (lngFit * 100) / ((lngUB + 1) * Multi_ByteWeight)
End Function

'********************************************************************************

'********************************************************************************
'�󕶌Ɍ`���̃��r��U��֐�
'�����̏����́A"������s���r�t"�܂���"�b������s���r�t"
'"������s���r�t"�̏ꍇ�̏���
'�܂����r�̕t���Ă��Ȃ�"������"���������āA"������s���r�t"�ɒu������B
'���r�t���̔����"�s"���t���Ă��鎖�Ŕ��f����B
'"������"�̒��O�̕����������̏ꍇ�A"������"�̑O��"�b"��t���Ēu������B
'"�b������s���r�t"�̏ꍇ�̏���
'�܂����r�̕t���Ă��Ȃ�"������"���������āA"�b������s���r�t"�ɒu������B
'�@��@���r��U�镶�����"�b�����_"�A���r��"���܂���"�Ƃ���ƁA"�����_"�Ō�����"�b�����_�s���܂����t"�ɒu������B
'
'"���́s�}�i�t"��"���͒����ʁs�}�i��v�[���t"�̗l�Ɉ���������̈ꕔ�ł���ꍇ�A���X�g���̏��Ԃ��l�����Ȃ��ƁA
'"���́s�}�i�t�����ʁs�}�i��v�[���t"�̂悤�ɒu�������ꍇ������B
'�������u������ɂ́A���r��U�镶����̒����������������X�g�̑O�ɓo�^���A�Z��������ɓo�^����B
'��̗�ł͖��͒����ʁs�}�i��v�[���t���ɁA���́s�}�i�t����ɒu������l�A���X�g�ɋL�q����B
'********************************************************************************
Private Function AddRubiAll(ByVal strLine)
	Dim str1	'As String
	Dim str2	'As String
	Dim str3	'As String
	Dim pos1 	'As Integer
	Dim pos2 	'As Integer

	AddRubiAll = false
	'�w�蕶����̏���"������s���r�t"�ŕ�����A���r����1�����ȏ゠�邩�𔻒�
	pos1 = InStr(strLine,"�s")
	pos2 = InStr(strLine,"�t")
	If 1 < pos1 And  pos1 + 1 < pos2 Then
		str1 = left(strLine,pos1 -1 )					'���r��U�銿���̎擾
		str2 = mid(strLine,pos1 + 1,pos2 - pos1 - 1)	'���r�̎擾
		str3 = left(str1,1)
		If str3 = "�b" or str3 = "|" Then
			str1 = mid(str1,2)
			'���r��U�镶����̐擪��"�b"�̏ꍇ�i���p|��������S�p�Œu������j
			' ���オ�s�Ŗ���"������"���������A"�b������s���r�t"�ɒu������B
			Call Editor.ReplaceAll(str1 & "(\p{Han}*+[^�s])", "�b" & str1 & "�s" & str2 & "�t$1", 20)	' ���ׂĒu��
		else
			' ���O�̕����������Œ��オ�s�Ŗ���"������"���������A"�b������s���r�t"�ɒu������B
			Call Editor.ReplaceAll("(\p{Han})" & str1 & "(\p{Han}*+[^�s])", "$1�b" & str1 & "�s" & str2 & "�t$2", 20)	' ���ׂĒu��
			'���オ�s�Ŗ���"������"���������A"������s���r�t"�ɒu������B
			Call Editor.ReplaceAll(str1 & "(\p{Han}*+[^�s])", str1 & "�s" & str2 & "�t$1", 20)	' ���ׂĒu��
		End If
		AddRubiAll = true
	End If
End Function

'********************************************************************************
'�w�肳�ꂽ�I�v�V�����t���O��
'�w�肳�ꂽ�����������
'�w�肳�ꂽ�u��������
'�m�F���Ȃ���S�Ēu������
'�߂�l�F�u���񐔁i�L�����Z�����ɂ�1�̕␔�ɂȂ�j
'********************************************************************************
Private Function ReplaceAllWithConfirmation(strSrc, strDst, intOpt)
	Dim posx 	'As Integer
	Dim posy 	'As Integer
	Dim repCnt	'As Integer
	Dim str1	'As String

	ReplaceAllWithConfirmation = 0
	Call Editor.GoFileTop			'�t�@�C���擪�Ɉړ�
	Call Editor.SearchNext(strSrc, CLng(intOpt))	'��������
	posy = Editor.GetSelectLineTo()	'���݂̑I���I���s�ʒu���擾����
	posx = Editor.GetSelectColmTo()	'���݂̑I���I�����ʒu���擾����
	Call Editor.SetDrawSwitch(1)	' �`��ĊJ
	Call Editor.ReDraw(0)			' �ĕ`��
	repCnt = 0
	Do While posy <> 0 And posx <> 0 		'������̑I���I���ʒu��(0,0)�Ŗ������i���������������j
		str1 = Editor.GetSelectedString()	'�I������Ă��镶����
		Select Case Editor.MessageBox("""" & str1 & """" & "��u�����܂����H", 35)	'Yes/No/Cancel + �m�F�A�C�R��
		Case 6			'YES
			Call Editor.Replace(strSrc, strDst, CLng(intOpt))	'�u���i���������Ɠ����������s���j
			repCnt = repCnt + 1
		Case 7			'No
			Call Editor.SearchNext(strSrc, CLng(intOpt))	'��������
		Case 2			'�L�����Z��
			repCnt = (repCnt + 1) * -1 
			Exit Do				'�I��
		End Select
		posy = Editor.GetSelectLineTo()	'���݂̑I���I���s�ʒu���擾����
		posx = Editor.GetSelectColmTo()	'���݂̑I���I�����ʒu���擾����
		Call Editor.ReDraw(0)			' �ĕ`��
	Loop
	Call Editor.SetDrawSwitch(0)	' �`���~
	ReplaceAllWithConfirmation = repCnt
End Function

'********************************************************************************
'Speeeeed�̕ϊ����X�g�̏������g�p���Ēu������
'lst�t�@�C���̏��� : "����������\t�u��������\t�u���I�v�V����"�i\t�̓^�u Chr(9)�j
'�u���I�v�V�����͈ȉ��̑g�ݍ��킹
'"S":�G�X�P�[�v�V�[�P���X���g�p���Ȃ�
'"C":�p���̑啶������������ʂ��Ȃ�
'"�m":1��1�ɂ��Ēu�����邩���m�F����i���̃X�N���v�g�ׂ̈̊g���u���I�v�V�����j
'"E":���K�\���A�ȉ��̃I�v�V�����͐��K�\�����̃I�v�V����
'"k":���{��i�S�p�����j���Ώہi���̃X�N���v�g�ł͏�ɓ��{����Ώہj
'"m":�����s�i"."�����s���܂ޗl�ɂȂ�j�i���̃X�N���v�g�ł͖����j
'"i":�p���̑啶������������ʂ��Ȃ�
'********************************************************************************
Private Function ReplaceAllSpe5d(strLine)
	Dim str1	'As String
	Dim str2	'As String
	Dim str3	'As String
	Dim pos1 	'As Integer
	Dim pos2 	'As Integer
	Dim opt		'As Long

	ReplaceAllSpe5d = false
	pos1 = InStr(strLine,Chr(9))		'�^�u��؂�
	If pos1 > 0 Then
		str1 = Left(strLine,pos1-1)
		pos2 = InStr(pos1 + 1,strLine,Chr(9))		'�^�u��؂�
		If pos1 < pos2 Then
			str2 = Mid(strLine,pos1 + 1,pos2 - pos1 - 1)
			str3 = Mid(StrLine,pos2 + 1)
			opt = 16		'�_�C�A���O�����
			If InStr(str3,"E") > 0 Then
				'���K�\���̃I�v�V����
				opt = opt + 4								'���K�\���i�T�N���G�f�B�^�ł͐��K�\�����̂�"\n"����F������j
'				If InStr(str3,"k") > 0 Then opt = opt + 0	'���{��iWindowsXP�ȍ~��Unicode�Ȃ̂ŏ�ɑΉ��j
'				If InStr(str3,"m") > 0 Then opt = opt + 0	'�����s�i�T�N���G�f�B�^�ł͎w��ł��Ȃ��j
				If InStr(str3,"i") <= 0 Then opt = opt + 2 	'"i"���������A�p�啶���Ə���������ʂ���
			Else
				'���K�\���Ŗ������̃I�v�V����
'				If InStr(str3,"S") > 0 Then opt = opt + 0	'�G�X�P�[�v�V�[�P���X���g�p���Ȃ��B�i�T�N���G�f�B�^�ł͐��K�\���Ŗ����ꍇ�A��Ɏg�p�ł��Ȃ��j
				If InStr(str3,"C") <= 0 Then opt = opt + 2 	'"C"���������A�p�啶���Ə���������ʂ���
			End If
			If Instr(str3,"�m") > 0 Then
				Call ReplaceAllWithConfirmation(str1, str2, opt)	' ���ׂĒu��
			Else
				Call Editor.ReplaceAll(str1, str2, opt)	' ���ׂĒu��
			End If
			ReplaceAllSpe5d = true
		End If
	End If
End Function

'********************************************************************************
'������𔻒肵�āA���s���鏈����U�蕪����
'********************************************************************************
Private Function SelectOperation(strLine)
	Dim pos1 	'As Integer
	Dim pos2 	'As Integer

	SelectOperation = false
	If Trim(strLine) = "" Or left(strLine,1) = "'" Then
		'���������s���A�s����"'"�Ȃ�R�����g�s�Ȃ̂ŉ������Ȃ��Ő���I��
		SelectOperation = true
		Exit Function
	End If

	pos1 = InStr(strLine,Chr(9))			'�^�u����
	pos2 = InStr(pos1 + 1,strLine,Chr(9))	'�^�u����
	If pos1 > 1 And pos2 >= 3 And  pos1 < pos2  Then		'�^�u��؂�A��Q������""���L�肤��
		'Speeeed�u�����X�g�݊�
		SelectOperation = ReplaceAllSpe5d(strLine)
		Exit Function
	End If
	'���r�U��
	SelectOperation = AddRubiAll(strLine)

End Function

'********************************************************************************
'�u�����X�g���g�p�����u��
'********************************************************************************
Private Function ReplaceByList(ByVal FilePath)
	Dim intCnt			'As Long
	Dim LineNo			'As Long
	Dim strRead			'As String
	Dim CharSetOfText	'As String
	Dim fProBability	'As Single
	Dim objFso			'As Object

	If FilePath = "" Then		'�t�@�C���p�X��������
		Exit Function
	End If
	Set objFso = CreateObject("Scripting.FileSystemObject")
	if objFso.FileExists(FilePath) = false Then
		Set objFso = Nothing
		Exit Function
	End If
	Set objFso = Nothing

	'���X�g�t�@�C���̕����Z�b�g�𔻒�AVB�̃f�t�H���g�����Z�b�g��SJIS
	CharSetOfText = JudgeCode(FilePath, fProBability)

	'�t�@�C���I�[�v������
	With CreateObject("ADODB.Stream")
		.Charset = CharSetOfText
		.Open
		.LoadFromFile(FilePath)
		If Err.Number > 0 Then		'�G���[������
			'�G���[��\������
			Call Editor.MessageBox("Open Error : " & FilePath )
		Else
'			' ������h�~
			LineNo = CLng(Editor.ExpandParameter("$y"))
			intCnt = 1
			Do While Not .EOS
		    	'�����ł��Ȃ��s���L��ΏI��
		    	strRead = .ReadText(-2)
		        If SelectOperation( strRead ) = false Then
					Call Editor.ErrorMsg("Replace Error. Line = " & intCnt & ":""" & strRead & """" )
		        	Exit Do
		        End If
				intCnt = intCnt + 1
			Loop
			Call Editor.MoveCursor( LineNo, 1, 0)
			Call Editor.SearchClearMark()	' �����}�[�N���N���A����
		End If
		'�t�@�C���N���[�Y
		.Close
	End With

End Function

''*******************************************************************************
''�摜�t�@�C���̊g���q��ς��āA�L�����m�F
''���J���Ă���t�@�C���̂���t�H���_����������
''��������g���q�F".png","jpeg",".jpg"
''���́F	FigName:�摜�t�@�C�����A��F1234_i5678.jpeg
''�߂�l�F�t�@�C�������������̃t�@�C���g���q�A������Ȃ��������ɂ�""
''*******************************************************************************
'Private Function JuadgeFigType(FolderPath, FigName)
'	Dim fname
'	Dim fext
'	Dim strTmpFilePath	'As String
'	Dim FigExts
'	Dim objFso			'As Object
'
'	JuadgeFigType = ""
'	Set objFso = CreateObject("Scripting.FileSystemObject")
'	'��΃p�X���쐬
'	strTmpFilePath = FolderPath & FigName
''	MsgBox("�t�@�C���p�X:" & strTmpFilePath )
'	'�t�@�C���L���̔���
'	If objFso.FileExists(strTmpFilePath) = FALSE then
'		'����
'		FigExts = Array(".png","jpeg",".jpg")
'		fname = Left(FigName, InStr(FigName,".") - 1)
'		For Each fext in FigExts
'			'�t�@�C�����L�邩
'			strTmpFilePath = FolderPath & fname & fext
''			MsgBox("�t�@�C���p�X:" & strTmpFilePath )
'			If objFso.FileExists(strTmpFilePath) then
'				JuadgeFigType = fext
'				Exit For
'			End If
'		Next
'	End If
'	Set objFso = Nothing
'End Function
'
''*******************************************************************************
''�}�G�t�@�C���̎�ނ��m�F���A�t�@�C���g���q�������ւ���
''*******************************************************************************
'Private Function ReplaceFigFileExt()
'	Const FigString = "(�m��[^�i]*�i[a-zA-Z_0-9]+)(\.jpe?g|\.png)([^�j]*�j����n)"
'	Dim posx 	'As Integer
'	Dim posy 	'As Integer
'	Dim str1	'As String
'	Dim FigName
'	Dim FigExt
'	Dim Pos1
'	Dim Pos2
'	Dim strTmpFilePath	'As String
'	Dim FolderPath
'
'With Editor
'	strTmpFilePath = .ExpandParameter("$F")
'	FolderPath = Left( strTmpFilePath, InStrRev( strTmpFilePath, "\"))
'	Call .GoFileTop				'�t�@�C���擪�Ɉړ�
'	Call .SearchNext(FigString, CLng(20))	'��������
'	posy = .GetSelectLineTo()	'���݂̑I���I���s�ʒu���擾����
'	posx = .GetSelectColmTo()	'���݂̑I���I�����ʒu���擾����
'	Do While posy <> 0 And posx <> 0 		'������̑I���I���ʒu��(0,0)�Ŗ������i���������������j
'		'�I�𕶎�����}�̃t�@�C�����𒊏o
'		'��F"�m���}�̐����ifig0123_01.png�A��321�~�c123�j����n"
'		'�t�@�C�����͔��p�́A�A���t�@�x�b�g�A�����A�A���_�[�o�[�A�s���I�h�����ł��邱��
'		str1 = .GetSelectedString()			'�I������Ă��镶����
'		'�i�j���̕����؂�o��
'		pos1 = InStr(str1,"�i")
'		pos2 = InStr(pos1,str1,"�j")
'		FigName = Mid(str1,pos1 + 1,pos2 - (pos1 + 1))
'		' "�A"�ȍ~�͕s�v
'		pos1 = InStr(FigName,"�A")
'		If pos1 <> 0 THEN FigName= Left(FigName,pos1-1)
'		'�t�@�C���g���q��ς��Ăǂ̉摜�t�@�C�������邩�m�F
'		FigExt = JuadgeFigType(FolderPath, FigName)
'		If FigExt <> "" Then	'�قȂ�g���q�̃t�@�C�����L��ꍇ
'			Call .Replace(FigString, "$1" & FigExt & "$3" , CLng(20))	'�u��
'		Else
'			Call .SearchNext(FigString, CLng(20))	'��������
'		End If
'		posy = .GetSelectLineTo()	'���݂̑I���I���s�ʒu���擾����
'		posx = .GetSelectColmTo()	'���݂̑I���I�����ʒu���擾����
'	Loop
'End With
'
'End Function


'*******************************************************************************
'�摜�̃T�C�Y�擾
'*******************************************************************************
Private Function GetFigureSize(imgFilePath, ByRef sizeX, ByRef sizeY)
	Dim objImg 'As Object
	Dim strMsg 'As String

	GetFigureSize = False
	' �C���X�^���X�쐬
	Set objImg = CreateObject("WIA.ImageFile")
	If Not objImg Is Nothing Then
		' �t�@�C���p�X�w��
		objImg.LoadFile(imgFilePath)
'		Call Editor.MessageBox(imgFilePath , 32)
		' ��x�����ŕ\��
		'WScript.Echo(objImg.Width & "x" & objImg.Height & "(�s�N�Z��)")
		sizeX = objImg.Width
		sizeY = objImg.Height
		' �C���X�^���X������
		Set objImg = Nothing

'		'### �܂��� ###
'		With CreateObject("WIA.imagefile")
'		    .LoadFile(imgFilePath)
'		'    WScript.Echo(.Width & "x" & .Height & "(�s�N�Z��)")
'			sizeX = .Width
'			sizeY = .Height
'		End With

		GetFigureSize = True
	End If

End Function

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
	Dim objFso			'As Object
	Dim FSCol

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

	'�����N�̐}��"�m���}�G�i�摜�t�@�C�����A��Xsize�~�cYsize�j����n"�ɕϊ�
'	.GoFileTop
'	LineNo = CLng(.ExpandParameter("$y"))
'	Call .SearchNext("�m����������r�͂݁n", 20)	'����
'	On Error Resume Next
'	do Until (CLng(Editor.ExpandParameter("$y")) = LineNo)
'		LineNo = CLng(.ExpandParameter("$y"))
'		Call .GoLineTop(8)
'		Call .BeginSelect
'		Call .SearchNext("�m�������Ōr�͂ݏI���n", 20)	'����
'		Call .GoLineEnd_Sel(8)
''		strLine = .GetSelectedString(0) '�I�𕶎�����擾
''		Call .MessageBox(strLine, 32)
'		Call .Delete
'		Call .SearchNext("�m����������r�͂݁n", 20)	'����
''		Call .MessageBox(CStr(Editor.ExpandParameter("$y")) & ";"& CStr(LineNo), 32)
'	Loop
'	On Error GoTo 0
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

	ReplaceFile  = dirPath & "_Replace.lst"
	if AUTO_FLAG = False Then
		Set objFso = CreateObject("Scripting.FileSystemObject")
		If objFso.FileExists(ReplaceFile) = False then
			'ReplaceFile = shell.ExpandEnvironmentStrings(%APPDATA%)
			Set FSCol = CreateObject("WScript.Shell").SpecialFolders
			ReplaceFile = FSCol("AppData") & "\Roaming\sakura\MACRO\ReplaceList\_Replace.lst"	'AppData�t�H���_�ȉ�sakura�̃}�N���t�H���_
			Set FSCol = Nothing
		End If
		Set objFso = Nothing
		ReplaceFile = Editor.FileOpenDialog(ReplaceFile,"*.lst")
		Call Editor.ReDraw(0)			' �ĕ`��A�t�@�C���_�C�A���O����������
	End If
	if ReplaceFile <> "" Then
		Call ReplaceByList(ReplaceFile)		'���X�g�t�@�C���ɂ��u�����������s
	End If

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

