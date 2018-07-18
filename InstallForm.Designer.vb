<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InstallForm
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InstallForm))
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.download = New System.ComponentModel.BackgroundWorker()
        Me.LogBox = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(9, 10)
        Me.ProgressBar1.Margin = New System.Windows.Forms.Padding(2)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(684, 19)
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar1.TabIndex = 0
        '
        'download
        '
        Me.download.WorkerReportsProgress = True
        '
        'LogBox
        '
        Me.LogBox.BackColor = System.Drawing.Color.Black
        Me.LogBox.ForeColor = System.Drawing.Color.White
        Me.LogBox.FormattingEnabled = True
        Me.LogBox.Items.AddRange(New Object() {"Connecting to server..."})
        Me.LogBox.Location = New System.Drawing.Point(9, 42)
        Me.LogBox.Name = "LogBox"
        Me.LogBox.Size = New System.Drawing.Size(684, 251)
        Me.LogBox.TabIndex = 2
        '
        'InstallForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(705, 301)
        Me.Controls.Add(Me.LogBox)
        Me.Controls.Add(Me.ProgressBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.Name = "InstallForm"
        Me.Text = "Downloading AlexaPi-Windows"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents download As ComponentModel.BackgroundWorker
    Friend WithEvents LogBox As System.Windows.Forms.ListBox
End Class
