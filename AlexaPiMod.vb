Imports System.Drawing
Imports System.Threading
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography.X509Certificates

Module AlexaPiMod

    Public Const name = "AlexaPi"
    Public Const url = "https://github.com/alexa-pi/AlexaPi.git"

    <DllImport("kernel32.dll")>
    Private Function GetConsoleWindow() As IntPtr
    End Function

    <DllImport("user32.dll")>
    Public Function ShowWindow(hWnd As IntPtr, nCmdShow As Integer) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Function SetParent(ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function SendMessage(hWnd As IntPtr, Msg As UInt32, wParam As Integer, lParam As Integer) As IntPtr
    End Function


    Public Const SW_HIDE As Integer = 0
    Public Const SW_SHOW As Integer = 5
    Const WM_SYSCOMMAND As Integer = 274
    Const SC_MAXIMIZE As Integer = 61488

    Const GWL_EXSTYLE = -20
    Const WS_EX_MDICHILD = 40L

    Dim handle = GetConsoleWindow()

    Dim consolehandle = 0
    Dim consoleid = 0
    Dim ConsoleProc As Process

    Public documentspath = My.Computer.FileSystem.SpecialDirectories.MyDocuments
    Public alexapipath = documentspath & "\" & name

    WithEvents icon As New NotifyIcon()
    WithEvents menu As New ContextMenuStrip()
    WithEvents back As New BackgroundWorker()

    WithEvents exit_menu As New ToolStripMenuItem("Exit")
    WithEvents show_menu As New ToolStripMenuItem("Show log")
    WithEvents restart_menu As New ToolStripMenuItem("Restart")
    WithEvents update_menu As New ToolStripMenuItem("Update")

    Public windowhandle = 0

    Public terminate As Boolean = False

    Sub Main()
        ShowWindow(handle, SW_HIDE)

        icon.BalloonTipText = "AlexaPi has just started!"
        icon.BalloonTipTitle = "AlexaPi-Windows"
        icon.Text = "AlexaPi-Windows"
        icon.Icon = New Icon(My.Resources.icon, 40, 40)

        menu.Items.Add(show_menu)
        menu.Items.Add(restart_menu)
        menu.Items.Add(exit_menu)
        menu.Items.Add(update_menu)
        icon.ContextMenuStrip = menu

        icon.Visible = True
        icon.ShowBalloonTip(1000)

        windowhandle = ConsoleContainer.Handle

        If Directory.Exists(alexapipath) Then
            back.RunWorkerAsync()
            'Application.Run()
        Else
            ShowWindow(windowhandle, SW_HIDE)
            InstallForm.ShowDialog()
        End If
    End Sub

    Private Sub exit_menu_Click(sender As Object, e As EventArgs) Handles exit_menu.Click
        stop_app()
    End Sub

    Private Sub show_menu_Click(sender As Object, e As EventArgs) Handles show_menu.Click
        ShowWindow(windowhandle, SW_SHOW)
    End Sub

    Private Sub back_DoWork(sender As Object, e As DoWorkEventArgs) Handles back.DoWork
        Dim command = "python main.py -d"
        Dim folder = alexapipath & "\src\"

        Dim exitCode As Integer
        Dim ProcessInfo As ProcessStartInfo

        ProcessInfo = New ProcessStartInfo("cmd.exe", "/c " + command + " & pause")
        ProcessInfo.WorkingDirectory = folder

        ConsoleProc = Process.Start(ProcessInfo)
        Thread.Sleep(1000)
        consolehandle = ConsoleProc.MainWindowHandle
        consoleid = ConsoleProc.Id

        SetParent(consolehandle, windowhandle)

        setborder.HideCaption(consolehandle)
        setborder.ApplyStyleChanges(consolehandle)

        SendMessage(consolehandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0)
        ShowWindow(windowhandle, SW_HIDE)

        ConsoleProc.WaitForExit()

        exitCode = ConsoleProc.ExitCode

        'Console.WriteLine("ExitCode:  " + exitCode.ToString(), "ExecuteCommand")
        ConsoleProc.Close()
    End Sub

    Public Sub stop_console()
        Try
            ConsoleProc.Kill()
            If Not ConsoleProc.HasExited Then
                ConsoleProc.WaitForExit()
            End If
        Catch ex As Exception
            'MsgBox("Can't close AlexaPi: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Sub stop_app()
        stop_console()
        icon.BalloonTipText = "AlexaPi closed!"
        icon.ShowBalloonTip(1000)
        terminate = True
        Thread.Sleep(1000)
        icon.Visible = False
        Application.Exit()
    End Sub

    Public Sub RestartElevated()
        Dim startInfo As ProcessStartInfo = New ProcessStartInfo()
        startInfo.UseShellExecute = True
        startInfo.WorkingDirectory = Environment.CurrentDirectory
        startInfo.FileName = Application.ExecutablePath
        startInfo.Verb = "runas"
        Try
            Dim p As Process = Process.Start(startInfo)
        Catch ex As Exception
            Return 'If cancelled, do nothing
        End Try
        terminate = True
        icon.Visible = False
        Application.Exit()
    End Sub

    Private Sub restart_menu_Click(sender As Object, e As EventArgs) Handles restart_menu.Click
        Application.Restart()
    End Sub

    Private Sub update_menu_Click(sender As Object, e As EventArgs) Handles update_menu.Click
        Dim gitinfo As New ProcessStartInfo("cmd.exe", "/c ""Update.bat """ & alexapipath & " " & Application.StartupPath & """""")
        'gitinfo.WorkingDirectory = alexapipath
        Dim git = Process.Start(gitinfo)
        stop_app()
    End Sub

End Module

Class MyWebClient
    Inherits WebClient

    Protected Overrides Function GetWebRequest(ByVal address As Uri) As WebRequest
        Dim request As HttpWebRequest = CType(MyBase.GetWebRequest(address), HttpWebRequest)
        request.ClientCertificates.Add(New X509Certificate())
        Return request
    End Function
End Class