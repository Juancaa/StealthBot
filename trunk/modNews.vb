Option Strict Off
Option Explicit On
Module modNews
	
	Public Function GetNewsURL() As String
		' Changed link back to original one until everything gets moved. (8/20/09) -Pyro
		' Updated to new file for 2.7 release -andy
		GetNewsURL = "http://www.stealthbot.net/sb/version.php?cv=" & My.Application.Info.Version.Revision & "&lv=" & lLauncherVersion
	End Function
	
	Public Sub HandleNews(ByVal Buffer As String, ByVal ResponseCode As Integer)
		On Error Resume Next
		
		Dim Splt() As String
		Dim SubSplt() As String
		Dim i As Short
		Dim OldValue As Boolean
		
		Splt = Split(Buffer, "|")
		
		' New format 2.7+ -at
		'Beta Build 0 | Regular Build 1 | Launcher Build 2 | Regular News 3 | Beta News 4
		If ResponseCode <> 0 Then
			frmChat.AddChat(RTBColors.ErrorMessageText, Buffer & ". Error retrieving news from http://www.stealthbot.net.")
		ElseIf UBound(Splt) <> 4 Then 
			frmChat.AddChat(RTBColors.ErrorMessageText, "Format not understood. Error retrieving news from http://www.stealthbot.net.")
		Else
			OldValue = frmChat.mnuUTF8.Checked ' old value of UTF8 encoding setting
			frmChat.mnuUTF8.Checked = False
			
			If StrictIsNumeric(Splt(0)) Then
				'############# Beta only
#If BETA Then
				frmChat.AddChat(RTBColors.ServerInfoText, "->> ")
				frmChat.AddChat(RTBColors.ServerInfoText, "->> �cbStealthBot Beta News")
				
				If InStr(1, Splt(4), "\n") > 0 Then
					SubSplt = Split(Splt(4), "\n")
					
					For i = 0 To UBound(SubSplt)
						frmChat.AddChat(RTBColors.ServerInfoText, "->> " & SubSplt(i))
					Next i
				Else
					frmChat.AddChat(RTBColors.ServerInfoText, "->> " & Splt(4))
				End If
				
				frmChat.AddChat(RTBColors.ServerInfoText, " ")
				frmChat.AddChat(RTBColors.ServerInfoText, "The current StealthBot Beta version is build " & Splt(0) & ".")
#End If
				'##############
				
				If Val(Splt(1)) <> My.Application.Info.Version.Revision Or (lLauncherVersion > 0 And Val(Splt(2)) <> lLauncherVersion) Then '// old version
					If (Val(Splt(0)) <= My.Application.Info.Version.Revision) Then
						frmChat.AddChat(RTBColors.InformationText, "�cbYou are running a development release of StealthBot, visit http://www.stealthbot.net/wiki/BuildLog for more information")
					Else
						frmChat.AddChat(RTBColors.ErrorMessageText, "�cbYou are running an outdated version of StealthBot.")
						frmChat.AddChat(RTBColors.ErrorMessageText, "To download an updated version or for more information, visit http://www.stealthbot.net.")
						frmChat.AddChat(RTBColors.ErrorMessageText, "To disable version checking, add the line " & Chr(34) & "DisableSBNews=Y" & Chr(34) & " under the [Main] section of your config.ini file.")
					End If
				End If
				
				If Len(Splt(3)) > 1 Then
					frmChat.AddChat(RTBColors.ServerInfoText, ">> ")
					frmChat.AddChat(RTBColors.ServerInfoText, ">> �cbStealthBot News")
					
					If InStr(1, Splt(3), "\n") > 0 Then
						SubSplt = Split(Splt(3), "\n")
						
						For i = 0 To UBound(SubSplt)
							frmChat.AddChat(RTBColors.ServerInfoText, ">> " & SubSplt(i))
						Next i
					Else
						frmChat.AddChat(RTBColors.ServerInfoText, ">> " & Splt(3))
					End If
					
					frmChat.AddChat(RTBColors.ServerInfoText, ">> ")
				End If
			End If
			
			frmChat.mnuUTF8.Checked = OldValue
		End If
	End Sub
End Module