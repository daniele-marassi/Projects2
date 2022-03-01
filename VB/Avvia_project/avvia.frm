VERSION 5.00
Begin VB.Form Avvia 
   Caption         =   "Avvia"
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
Attribute VB_Name = "Avvia"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hwnd As Long, ByVal lpOperation As String, _
ByVal lpFile As String, ByVal lpParameters As String, _
ByVal lpDirectory As String, ByVal nShowCmd As Long) As Long

Const SW_SHOW = 5

Private Sub Form_Load()
    Dim testo As String
    
    Open App.Path & "\conf.csv" For Input As #1
        testo = Input(LOF(1), #1)
    Close #1
    Dim par() As String
    par = Split(testo, ";")
    Dim parametri As String
    If (UBound(par) = 1) Then
        parametri = par(1)
    Else
        parametri = ""
    End If
    On Error Resume Next
    ShellExecute Me.hwnd, "open", par(0), parametri, vbNullString, SW_SHOW
    End
End Sub
