Module Module1
    Public Function ScreenResolution() As Integer
        Dim intX As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim intY As Integer = Screen.PrimaryScreen.Bounds.Height
        Return intY
    End Function
End Module
