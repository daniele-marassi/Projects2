VERSION 5.00
Begin VB.Form mute 
   Caption         =   "mute"
   ClientHeight    =   3135
   ClientLeft      =   60
   ClientTop       =   405
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   ScaleHeight     =   3135
   ScaleWidth      =   4680
   Visible         =   0   'False
   WindowState     =   1  'Minimized
End
Attribute VB_Name = "mute"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()
    Set audio = New CAudio
    If (audio.GetMute = 0) Then
        audio.SetMute 1
    Else
        audio.SetMute 0
    End If
    End
End Sub
