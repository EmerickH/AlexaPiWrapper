Imports System.ComponentModel
Imports System.Net
Imports System.IO
Imports System.IO.Compression
Imports System.Windows.Forms
Imports System.Threading

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        download.RunWorkerAsync()
    End Sub

    Private Sub download_DoWork(sender As Object, e As DoWorkEventArgs) Handles download.DoWork

        Dim myWebClient As New WebClient()

        Dim myStringWebResource = url + file
        download.ReportProgress(15, "Downloading AlexaPi (" & file & ") from github")
        myWebClient.DownloadFile(myStringWebResource, Application.StartupPath & "\alexapi.zip")



        myStringWebResource = swigurl + swigfile
        download.ReportProgress(30, "Downloading swigwin-AlexaPi (" & swigfile & ") from github")
        myWebClient.DownloadFile(myStringWebResource, Application.StartupPath & "\swig.zip")


        download.ReportProgress(45, "Unzipping AlexaPi")
        ZipFile.ExtractToDirectory(Application.StartupPath & "\alexapi.zip", Application.StartupPath)


        download.ReportProgress(60, "Unzipping swigwin-AlexaPi")
        ZipFile.ExtractToDirectory(Application.StartupPath & "\swig.zip", Application.StartupPath & "\" & Module1.name & "\src\scripts\")


        download.ReportProgress(75, "Installing")

        Dim folderinfo As New ProcessStartInfo("explorer.exe", """" & Application.StartupPath & "\" & Module1.name & "\src""")

        Process.Start(folderinfo)

        Thread.Sleep(1000)

        Dim startinfo As New ProcessStartInfo("cmd.exe", "/c """ & Application.StartupPath & "\" & Module1.name & "\src\scripts\setup.bat""")

        Dim pr = Process.Start(startinfo)
        pr.WaitForExit()
        download.ReportProgress(100, "Done!")
        Thread.Sleep(1000)
        Application.Restart()

    End Sub

    Private Sub download_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles download.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        Label1.Text = e.UserState
    End Sub

End Class