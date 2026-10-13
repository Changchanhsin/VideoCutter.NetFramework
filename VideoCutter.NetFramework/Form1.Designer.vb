<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.txtInput = New System.Windows.Forms.TextBox()
        Me.btnBrowseInput = New System.Windows.Forms.Button()
        Me.btnBrowseFFmpeg = New System.Windows.Forms.Button()
        Me.txtFFmpeg = New System.Windows.Forms.TextBox()
        Me.lblCurrent = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.btnSetStart = New System.Windows.Forms.Button()
        Me.btnSetEnd = New System.Windows.Forms.Button()
        Me.txtStart = New System.Windows.Forms.TextBox()
        Me.txtEnd = New System.Windows.Forms.TextBox()
        Me.btnCut = New System.Windows.Forms.Button()
        Me.progressBar = New System.Windows.Forms.ProgressBar()
        Me.timerProgress = New System.Windows.Forms.Timer(Me.components)
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.AxWindowsMediaPlayer1 = New AxWMPLib.AxWindowsMediaPlayer()
        Me.btnPlayFromStart = New System.Windows.Forms.Button()
        Me.btnPlayFromEnd = New System.Windows.Forms.Button()
        Me.timerPos = New System.Windows.Forms.Timer(Me.components)
        Me.chkSnapKeyframe = New System.Windows.Forms.CheckBox()
        CType(Me.AxWindowsMediaPlayer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtInput
        '
        Me.txtInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtInput.Location = New System.Drawing.Point(12, 12)
        Me.txtInput.Name = "txtInput"
        Me.txtInput.Size = New System.Drawing.Size(427, 21)
        Me.txtInput.TabIndex = 1
        '
        'btnBrowseInput
        '
        Me.btnBrowseInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseInput.Location = New System.Drawing.Point(444, 10)
        Me.btnBrowseInput.Name = "btnBrowseInput"
        Me.btnBrowseInput.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseInput.TabIndex = 2
        Me.btnBrowseInput.Text = "浏览"
        Me.btnBrowseInput.UseVisualStyleBackColor = True
        '
        'btnBrowseFFmpeg
        '
        Me.btnBrowseFFmpeg.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseFFmpeg.Location = New System.Drawing.Point(444, 39)
        Me.btnBrowseFFmpeg.Name = "btnBrowseFFmpeg"
        Me.btnBrowseFFmpeg.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseFFmpeg.TabIndex = 4
        Me.btnBrowseFFmpeg.Text = "浏览ffmpeg"
        Me.btnBrowseFFmpeg.UseVisualStyleBackColor = True
        '
        'txtFFmpeg
        '
        Me.txtFFmpeg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFFmpeg.Location = New System.Drawing.Point(12, 41)
        Me.txtFFmpeg.Name = "txtFFmpeg"
        Me.txtFFmpeg.Size = New System.Drawing.Size(426, 21)
        Me.txtFFmpeg.TabIndex = 3
        '
        'lblCurrent
        '
        Me.lblCurrent.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCurrent.AutoSize = True
        Me.lblCurrent.Location = New System.Drawing.Point(12, 365)
        Me.lblCurrent.Name = "lblCurrent"
        Me.lblCurrent.Size = New System.Drawing.Size(53, 12)
        Me.lblCurrent.TabIndex = 7
        Me.lblCurrent.Text = "当前时间"
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(12, 344)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(41, 12)
        Me.lblTotal.TabIndex = 8
        Me.lblTotal.Text = "总时长"
        '
        'btnSetStart
        '
        Me.btnSetStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSetStart.Location = New System.Drawing.Point(14, 392)
        Me.btnSetStart.Name = "btnSetStart"
        Me.btnSetStart.Size = New System.Drawing.Size(99, 23)
        Me.btnSetStart.TabIndex = 9
        Me.btnSetStart.Text = "设为开始"
        Me.btnSetStart.UseVisualStyleBackColor = True
        '
        'btnSetEnd
        '
        Me.btnSetEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSetEnd.Location = New System.Drawing.Point(14, 421)
        Me.btnSetEnd.Name = "btnSetEnd"
        Me.btnSetEnd.Size = New System.Drawing.Size(100, 23)
        Me.btnSetEnd.TabIndex = 9
        Me.btnSetEnd.Text = "设为结束"
        Me.btnSetEnd.UseVisualStyleBackColor = True
        '
        'txtStart
        '
        Me.txtStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtStart.Location = New System.Drawing.Point(119, 394)
        Me.txtStart.Name = "txtStart"
        Me.txtStart.Size = New System.Drawing.Size(100, 21)
        Me.txtStart.TabIndex = 10
        '
        'txtEnd
        '
        Me.txtEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtEnd.Location = New System.Drawing.Point(119, 423)
        Me.txtEnd.Name = "txtEnd"
        Me.txtEnd.Size = New System.Drawing.Size(100, 21)
        Me.txtEnd.TabIndex = 10
        '
        'btnCut
        '
        Me.btnCut.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCut.Location = New System.Drawing.Point(13, 474)
        Me.btnCut.Name = "btnCut"
        Me.btnCut.Size = New System.Drawing.Size(100, 23)
        Me.btnCut.TabIndex = 11
        Me.btnCut.Text = "保存剪切"
        Me.btnCut.UseVisualStyleBackColor = True
        '
        'progressBar
        '
        Me.progressBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.progressBar.Location = New System.Drawing.Point(119, 474)
        Me.progressBar.Name = "progressBar"
        Me.progressBar.Size = New System.Drawing.Size(400, 23)
        Me.progressBar.TabIndex = 12
        '
        'timerProgress
        '
        Me.timerProgress.Interval = 200
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.Location = New System.Drawing.Point(12, 509)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(507, 17)
        Me.lblStatus.TabIndex = 13
        Me.lblStatus.Text = "状态显示"
        '
        'AxWindowsMediaPlayer1
        '
        Me.AxWindowsMediaPlayer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AxWindowsMediaPlayer1.Enabled = True
        Me.AxWindowsMediaPlayer1.Location = New System.Drawing.Point(12, 71)
        Me.AxWindowsMediaPlayer1.Name = "AxWindowsMediaPlayer1"
        Me.AxWindowsMediaPlayer1.OcxState = CType(resources.GetObject("AxWindowsMediaPlayer1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxWindowsMediaPlayer1.Size = New System.Drawing.Size(507, 261)
        Me.AxWindowsMediaPlayer1.TabIndex = 0
        '
        'btnPlayFromStart
        '
        Me.btnPlayFromStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPlayFromStart.Location = New System.Drawing.Point(225, 392)
        Me.btnPlayFromStart.Name = "btnPlayFromStart"
        Me.btnPlayFromStart.Size = New System.Drawing.Size(294, 23)
        Me.btnPlayFromStart.TabIndex = 14
        Me.btnPlayFromStart.Text = "从开始时间播放"
        Me.btnPlayFromStart.UseVisualStyleBackColor = True
        '
        'btnPlayFromEnd
        '
        Me.btnPlayFromEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPlayFromEnd.Location = New System.Drawing.Point(223, 421)
        Me.btnPlayFromEnd.Name = "btnPlayFromEnd"
        Me.btnPlayFromEnd.Size = New System.Drawing.Size(296, 23)
        Me.btnPlayFromEnd.TabIndex = 15
        Me.btnPlayFromEnd.Text = "从结束时间播放"
        Me.btnPlayFromEnd.UseVisualStyleBackColor = True
        '
        'timerPos
        '
        Me.timerPos.Interval = 200
        '
        'chkSnapKeyframe
        '
        Me.chkSnapKeyframe.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSnapKeyframe.AutoSize = True
        Me.chkSnapKeyframe.Location = New System.Drawing.Point(119, 450)
        Me.chkSnapKeyframe.Name = "chkSnapKeyframe"
        Me.chkSnapKeyframe.Size = New System.Drawing.Size(156, 16)
        Me.chkSnapKeyframe.TabIndex = 16
        Me.chkSnapKeyframe.Text = "如果可以，吸附到关键帧"
        Me.chkSnapKeyframe.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(531, 535)
        Me.Controls.Add(Me.chkSnapKeyframe)
        Me.Controls.Add(Me.btnPlayFromEnd)
        Me.Controls.Add(Me.btnPlayFromStart)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.progressBar)
        Me.Controls.Add(Me.btnCut)
        Me.Controls.Add(Me.txtEnd)
        Me.Controls.Add(Me.txtStart)
        Me.Controls.Add(Me.btnSetEnd)
        Me.Controls.Add(Me.btnSetStart)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.lblCurrent)
        Me.Controls.Add(Me.btnBrowseFFmpeg)
        Me.Controls.Add(Me.txtFFmpeg)
        Me.Controls.Add(Me.btnBrowseInput)
        Me.Controls.Add(Me.txtInput)
        Me.Controls.Add(Me.AxWindowsMediaPlayer1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.AxWindowsMediaPlayer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents AxWindowsMediaPlayer1 As AxWMPLib.AxWindowsMediaPlayer
    Friend WithEvents txtInput As TextBox
    Friend WithEvents btnBrowseInput As Button
    Friend WithEvents btnBrowseFFmpeg As Button
    Friend WithEvents txtFFmpeg As TextBox
    Friend WithEvents lblCurrent As Label
    Friend WithEvents lblTotal As Label
    Friend WithEvents btnSetStart As Button
    Friend WithEvents btnSetEnd As Button
    Friend WithEvents txtStart As TextBox
    Friend WithEvents txtEnd As TextBox
    Friend WithEvents btnCut As Button
    Friend WithEvents progressBar As ProgressBar
    Friend WithEvents timerProgress As Timer
    Friend WithEvents lblStatus As Label
    Friend WithEvents btnPlayFromStart As Button
    Friend WithEvents btnPlayFromEnd As Button
    Friend WithEvents timerPos As Timer
    Friend WithEvents chkSnapKeyframe As CheckBox
End Class
