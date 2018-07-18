Imports System.ComponentModel
Imports System.Net
Imports System.IO
Imports System.IO.Compression
Imports System.Windows.Forms
Imports System.Threading
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security

Public Class InstallForm
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        download.RunWorkerAsync()
    End Sub

    Private Sub download_DoWork(sender As Object, e As DoWorkEventArgs) Handles download.DoWork

        download.ReportProgress(0, "Creating config file")

        Try
            Dim definepath = Application.StartupPath & "\defines.bat"
            If Not File.Exists(definepath) Then
                Using sw As StreamWriter = File.CreateText(definepath)
                    'sw.WriteLine("set gitdir=" & Application.StartupPath)
                    sw.WriteLine("set alexapipath=""" & alexapipath & """")
                    sw.WriteLine("set documentpath=""" & documentspath & """")
                    sw.WriteLine("set origin=" & url)
                End Using
            End If
        Catch ex As Exception
            If MsgBox("Error: AlexaPiWrapper needs Administrator rights to install, click Retry to restart with Admin rights", MsgBoxStyle.RetryCancel + MsgBoxStyle.Critical, "AlexaPiWrapper") = MsgBoxResult.Retry Then
                RestartElevated()
            End If
        End Try

        Try
            download.ReportProgress(10, "Downloading AlexaPi from github")
            Dim gitinfo As New ProcessStartInfo("cmd.exe", "/c """ & Application.StartupPath & "\git.bat""")
            'gitinfo.WorkingDirectory = documentspath
            gitinfo.FileName = "cmd"
            gitinfo.UseShellExecute = False
            gitinfo.CreateNoWindow = True
            gitinfo.RedirectStandardInput = True
            gitinfo.RedirectStandardOutput = True
            gitinfo.RedirectStandardError = True
            Dim git = Process.Start(gitinfo)
            git.BeginOutputReadLine()

            Dim prog = 10

            AddHandler git.OutputDataReceived, Sub(snd As Object, LineOut As DataReceivedEventArgs)
                                                   If Not LineOut.Data = "" AndAlso Not LineOut.Data.Contains("echo off") Then
                                                       prog += 5
                                                       download.ReportProgress(prog, LineOut.Data)
                                                   End If
                                               End Sub

            git.WaitForExit()

            Thread.Sleep(1000)


            download.ReportProgress(50, "Installing")

            Dim folderinfo As New ProcessStartInfo("explorer.exe", """" & alexapipath & "\src""")

            Process.Start(folderinfo)

            Thread.Sleep(1000)

            Dim startinfo As New ProcessStartInfo("cmd.exe", "/c """ & alexapipath & "\src\scripts\setup.bat""")

            Dim pr = Process.Start(startinfo)
            pr.WaitForExit()
            download.ReportProgress(100, "Done! Close this window to continue and relaunch AlexapiWrapper")
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "AlexaPiWrapper")
        End Try
    End Sub

    Private Sub download_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles download.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        LogBox.Items.Add(e.UserState)
    End Sub

    Private Function ValidateRemoteCertificate(sender As Object, cert As X509Certificate, chain As X509Chain, err As SslPolicyErrors) As Boolean
        Return True
    End Function

    Private Sub InstallForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        terminate = True
        Application.Exit()
    End Sub
End Class