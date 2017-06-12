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
        Dim remoteUri As String = "https://github.com/alexa-pi/AlexaPi-Windows/archive/"
        Dim fileName As String = "master.zip"
        Dim myStringWebResource As String = Nothing
        ' Create a new WebClient instance.
        Dim myWebClient As New WebClient()
        ' Concatenate the domain with the Web resource filename. Because DownloadFile 
        'requires a fully qualified resource name, concatenate the domain with the Web resource file name.
        myStringWebResource = remoteUri + fileName
        download.ReportProgress(25, "Downloading master.zip from github")

        ' The DownloadFile() method downloads the Web resource and saves it into the current file-system folder.
        myWebClient.DownloadFile(myStringWebResource, Application.StartupPath & "\master.zip")

        download.ReportProgress(50, "Unzipping")

        ZipFile.ExtractToDirectory(Application.StartupPath & "\master.zip", Application.StartupPath)

        download.ReportProgress(75, "Installing")

        Dim folderinfo As New ProcessStartInfo("explorer.exe", """" & Application.StartupPath & "\AlexaPi-Windows-master\src""")

        Process.Start(folderinfo)

        Thread.Sleep(1000)

        Dim startinfo As New ProcessStartInfo("cmd.exe", "/c """ & Application.StartupPath & "\AlexaPi-Windows-master\src\scripts\setup.bat""")

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