Public Class Form1
    Dim AppPath As String
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim proc() As Process = Process.GetProcesses
        Dim PrcProcesso As System.Diagnostics.Process()

        AppPath = Environment.CurrentDirectory()
        Me.Visible = False
        PrcProcesso = Process.GetProcessesByName("eShut")
        If (PrcProcesso.Length > 0) Then
            For i As Integer = 0 To proc.GetUpperBound(0)

                If proc(i).ProcessName = "eShut" Then
                    'MsgBox(proc(i).ProcessName)
                    end_eShut()
                End If
            Next
        Else
            start_eShut()
        End If
    End Sub

    Sub start_eShut()
        My.Computer.FileSystem.WriteAllText(AppPath & "\eShut.ini", "START", False)
        Timer1.Enabled = True
        Shell(AppPath & "\eShut.exe")
    End Sub

    Sub end_eShut()
        My.Computer.FileSystem.WriteAllText(AppPath & "\eShut.ini", "END", False)
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        End
    End Sub
End Class
