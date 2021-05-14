Public Class Form1
    'Dim f(2)
    'Dim proc() As Process = Process.GetProcesses
    'Dim d As Byte
    Dim y As Integer
    Dim y1 As Integer
    Dim y2 As Integer
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        'Dim PrcProcesso As System.Diagnostics.Process()
        Me.Visible = False
        'PrcProcesso = Process.GetProcessesByName("eShut")
        'd = 0
        'If (PrcProcesso.Length > 0) Then
        'For i As Integer = 0 To proc.GetUpperBound(0)
        'If proc(i).ProcessName = "eShut" Then
        'f(d) = i
        'd = d + 1
        'End If

        'If d > 1 Then MsgBox(d)
        'If d > 1 Then
        'Timer3.Enabled = True
        'End If
        'Next
        'Else
        'y1=2
        'Timer2.Enabled = True
        'End If

        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Me.BackColor = Color.Transparent
        Button1.BackgroundImage = My.Resources.Shutdown1
        Button2.BackgroundImage = My.Resources.restart1
        Button3.BackgroundImage = My.Resources.Log_Off1
        Button4.BackgroundImage = My.Resources.hybrid1
        Button5.BackgroundImage = My.Resources.exit1
        Button6.BackgroundImage = My.Resources.lock1
        Button1.FlatStyle = FlatStyle.Flat
        Button2.FlatStyle = FlatStyle.Flat
        Button3.FlatStyle = FlatStyle.Flat
        Button4.FlatStyle = FlatStyle.Flat
        Button5.FlatStyle = FlatStyle.Flat
        Button6.FlatStyle = FlatStyle.Flat
        y = ScreenResolution()
        Me.Location = New Point(155, 1080)
        Form2.Location = New Point(155, 1080)
        Timer1.Enabled = True
        y1 = 2
        y2 = 10
        Timer2.Enabled = True
    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Shell("C:\Windows\System32\shutdown.exe /s /t 0")
        Timer3.Enabled = True
    End Sub

    Private Sub Button1_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseDown
        Button1.BackgroundImage = My.Resources.Shutdown3
    End Sub

    Private Sub Button1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.MouseEnter
        Button1.BackgroundImage = My.Resources.Shutdown2
    End Sub
    Private Sub Button1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.MouseLeave
        Button1.BackgroundImage = My.Resources.Shutdown1
    End Sub

    Private Sub Button1_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseUp
        Button1.BackgroundImage = My.Resources.Shutdown2
    End Sub

    Private Sub Button1_Move(sender As Object, e As System.EventArgs) Handles Button2.Move
        'Button1.BackgroundImage = My.Resources.Shutdown2
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Shell("C:\Windows\System32\shutdown.exe /r /t 0")
        Timer3.Enabled = True
    End Sub

    Private Sub Button2_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button2.MouseDown
        Button2.BackgroundImage = My.Resources.restart3
    End Sub

    Private Sub Button2_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.MouseEnter
        Button2.BackgroundImage = My.Resources.restart2
    End Sub
    Private Sub Button2_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.MouseLeave
        Button2.BackgroundImage = My.Resources.restart1
    End Sub

    Private Sub Button2_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button2.MouseUp
        Button2.BackgroundImage = My.Resources.restart2
    End Sub

    Private Sub Button2_Move(sender As Object, e As System.EventArgs) Handles Button2.Move
        'Button2.BackgroundImage = My.Resources.restart2
    End Sub
    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Shell("C:\Windows\System32\shutdown.exe /l")
        Timer3.Enabled = True
    End Sub

    Private Sub Button3_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button3.MouseDown
        Button3.BackgroundImage = My.Resources.Log_Off3
    End Sub

    Private Sub Button3_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.MouseEnter
        Button3.BackgroundImage = My.Resources.Log_Off2
    End Sub
    Private Sub Button3_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.MouseLeave
        Button3.BackgroundImage = My.Resources.Log_Off1
    End Sub

    Private Sub Button3_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button3.MouseUp
        Button3.BackgroundImage = My.Resources.Log_Off2
    End Sub

    Private Sub Button3_Move(sender As Object, e As System.EventArgs) Handles Button3.Move
        'Button3.BackgroundImage = My.Resources.Log_Off2
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        'Shell("C:\Windows\System32\shutdown.exe /hybrid")
        Shell("C:\Windows\System32\Rundll32.exe powrprof.dll, SetSuspendState")
        Timer3.Enabled = True
    End Sub

    Private Sub Button4_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button4.MouseDown
        Button4.BackgroundImage = My.Resources.hybrid3
    End Sub

    Private Sub Button4_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.MouseEnter
        Button4.BackgroundImage = My.Resources.hybrid2
    End Sub
    Private Sub Button4_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.MouseLeave
        Button4.BackgroundImage = My.Resources.hybrid1
    End Sub

    Private Sub Button4_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button4.MouseUp
        Button4.BackgroundImage = My.Resources.hybrid2
    End Sub

    Private Sub Button4_Move(sender As Object, e As System.EventArgs) Handles Button4.Move
        'Button4.BackgroundImage = My.Resources.hybrid2
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        Timer3.Enabled = True
    End Sub

    Private Sub Button5_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button5.MouseDown
        Button5.BackgroundImage = My.Resources.exit3
    End Sub

    Private Sub Button5_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.MouseEnter
        Button5.BackgroundImage = My.Resources.exit2
    End Sub
    Private Sub Button5_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.MouseLeave
        Button5.BackgroundImage = My.Resources.exit1
    End Sub

    Private Sub Button5_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button5.MouseUp
        Button5.BackgroundImage = My.Resources.exit2
    End Sub

    Private Sub Button5_Move(sender As Object, e As System.EventArgs) Handles Button5.Move
        'Button5.BackgroundImage = My.Resources.exit2
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        LockWorkStation()
        Timer3.Enabled = True
    End Sub

    Private Sub Button6_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button6.MouseDown
        Button6.BackgroundImage = My.Resources.lock3
    End Sub

    Private Sub Button6_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button6.MouseEnter
        Button6.BackgroundImage = My.Resources.lock2
    End Sub
    Private Sub Button6_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button6.MouseLeave
        Button6.BackgroundImage = My.Resources.lock1
    End Sub

    Private Sub Button6_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Button6.MouseUp
        Button6.BackgroundImage = My.Resources.lock2
    End Sub

    Private Sub Button6_Move(sender As Object, e As System.EventArgs) Handles Button6.Move
        'Button6.BackgroundImage = My.Resources.lock2
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Button1.Location = New Point(0 + (Rnd() * 10), 792 + (Rnd() * 10))
        Button6.Location = New Point(0 + (Rnd() * 10), 636 + (Rnd() * 10))
        Button2.Location = New Point(0 + (Rnd() * 10), 480 + (Rnd() * 10))
        Button3.Location = New Point(0 + (Rnd() * 10), 324 + (Rnd() * 10))
        Button4.Location = New Point(0 + (Rnd() * 10), 168 + (Rnd() * 10))
        Button5.Location = New Point(0 + (Rnd() * 10), 12 + (Rnd() * 10))
    End Sub

    Private Sub Timer2_Tick(sender As System.Object, e As System.EventArgs) Handles Timer2.Tick
        If y1 < 11 Then
            If y1 = 0 Then Me.Visible = True
            y = (ScreenResolution() - (((Me.Height + 70) / 10) * y1))
            Me.Location = New Point(155, y + 40)
            Form2.Location = New Point(155, y + 40)
        ElseIf y1 = 11 Then
            Timer2.Enabled = False
            y1 = 2
        End If
        y1 = y1 + 1
    End Sub

    Private Sub Timer3_Tick(sender As System.Object, e As System.EventArgs) Handles Timer3.Tick
        If y2 > 2 Then
            y = (ScreenResolution() - (((Me.Height + 70) / 10) * y2))
            Me.Location = New Point(155, y + 40)
            Form2.Location = New Point(155, y + 40)
        ElseIf y2 = 2 Then
            'proc(f(0)).Kill()
            'proc(f(1)).Kill()
            Timer3.Enabled = False
            y2 = 10
            End
        End If
        y2 = y2 - 1
    End Sub

    Private Sub Timer4_Tick(sender As System.Object, e As System.EventArgs) Handles Timer4.Tick
        Dim AppPath As String
        AppPath = Environment.CurrentDirectory()
        Dim testo As String = My.Computer.FileSystem.ReadAllText(AppPath & "\eShut.ini")
        If UCase(Trim(testo)) = "END" Then
            Timer4.Enabled = False
            'My.Computer.FileSystem.WriteAllText(AppPath & "\eShut.ini", "START", False)
            Timer3.Enabled = True
        End If
    End Sub

End Class
