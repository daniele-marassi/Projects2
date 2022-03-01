VERSION 5.00
Begin VB.Form alzavolume 
   Caption         =   "alzavolume"
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
Attribute VB_Name = "alzavolume"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()
    Set Audio = New CAudio
    Dim volume_prec As Double
    volume_prec = (Audio.GetMasterVolumeLevelScalar)
    If ((volume_prec + 0.1) > 0.9) Then volume_prec = 0.9
    If (Audio.GetMute = 1) Then
        Audio.SetMute 0
    Else
        Audio.SetMasterVolumeLevelScalar volume_prec + 0.1
    End If
    End
End Sub
