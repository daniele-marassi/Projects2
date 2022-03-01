VERSION 5.00
Begin VB.Form volume_with_percentage 
   Caption         =   "volume_with_percentage"
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
Attribute VB_Name = "volume_with_percentage"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()
    Set Audio = New CAudio
    Dim volume_prec As Double
    Dim parm As String
    parm = Val(Command$) / 100
    
    
    Audio.SetMasterVolumeLevelScalar parm

    End
End Sub
