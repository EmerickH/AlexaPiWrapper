Imports System.Drawing
Imports System.Threading
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.IO

Module Module1

    Public Const name = "AlexaPi-dev"
    Public Const url = "https://github.com/alexa-pi/AlexaPi/archive/"
    Public Const file = "dev.zip"
    Public Const swigurl = "https://github.com/EmerickH/swigwin-AlexaPi/archive/"
    Public Const swigfile = "master.zip"

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

    WithEvents icon As New NotifyIcon()
    WithEvents menu As New ContextMenuStrip()
    WithEvents back As New BackgroundWorker()

    WithEvents exit_menu As New ToolStripMenuItem("Exit")
    WithEvents show_menu As New ToolStripMenuItem("Show log")
    WithEvents restart_menu As New ToolStripMenuItem("Restart")

    Dim window As New MDIParent1
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
        icon.ContextMenuStrip = menu

        icon.Visible = True
        icon.ShowBalloonTip(1000)

        If Directory.Exists(Application.StartupPath & "\" & name) Then
            back.RunWorkerAsync()

            window.Show()
            windowhandle = window.Handle
        Else
            Dim download As New Form1
            download.ShowDialog()
        End If



        Application.Run()
    End Sub

    Private Sub exit_menu_Click(sender As Object, e As EventArgs) Handles exit_menu.Click
        stop_console()
        terminate = True
        Application.Exit()
    End Sub

    Private Sub show_menu_Click(sender As Object, e As EventArgs) Handles show_menu.Click
        ShowWindow(window.Handle, SW_SHOW)
    End Sub

    Private Sub back_DoWork(sender As Object, e As DoWorkEventArgs) Handles back.DoWork
        Dim command = "python main.py -d"
        Dim folder = Application.StartupPath & "\" & name & "\src\"

        Dim exitCode As Integer
        Dim ProcessInfo As ProcessStartInfo
        Dim Proc As Process

        ProcessInfo = New ProcessStartInfo("cmd.exe", "/c " + command + " & pause")
        ProcessInfo.WorkingDirectory = folder

        Proc = Process.Start(ProcessInfo)
        Thread.Sleep(1000)
        consolehandle = Proc.MainWindowHandle
        consoleid = Proc.Id

        SetParent(consolehandle, windowhandle)

        setborder.HideCaption(consolehandle)
        setborder.ApplyStyleChanges(consolehandle)

        SendMessage(consolehandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0)
        ShowWindow(windowhandle, SW_HIDE)

        Proc.WaitForExit()

        exitCode = Proc.ExitCode

        'Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand")
        Proc.Close()
    End Sub

    Public Sub stop_console()
        Try
            Process.GetProcessById(consoleid).Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub restart_menu_Click(sender As Object, e As EventArgs) Handles restart_menu.Click
        Application.Restart()
    End Sub
End Module
