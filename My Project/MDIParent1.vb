
Imports System.Windows.Forms

Public Class MDIParent1
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If terminate = False Then
            e.Cancel = True
            ShowWindow(windowhandle, SW_HIDE)
        End If
    End Sub
End Class
