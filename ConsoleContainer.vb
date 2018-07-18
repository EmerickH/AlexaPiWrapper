
Imports System.Windows.Forms

Public Class ConsoleContainer
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If terminate = False Then
            e.Cancel = True
            ShowWindow(windowhandle, SW_HIDE)
        End If
    End Sub

    Private Sub ConsoleContainer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AlexaPiMod.Main()
    End Sub
End Class
