Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Threading
Imports AxWMPLib

Public Class Form1
    Private cachedKeyframes As List(Of Double) = Nothing
    Private isDragging As Boolean = False
    Private isPlaying As Boolean = False
    Private totalDuration As Double = 0
    Private isCutting As Boolean = False

    ' ============ 初始化 ============
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 默认ffmpeg路径猜测
        Dim guess = "C:\ffmpeg\bin\ffmpeg.exe"
        If File.Exists(guess) Then txtFFmpeg.Text = guess

        Me.Text = "视频剪切工具"

        txtInput.AllowDrop = True
        txtFFmpeg.AllowDrop = True

        timerPos.Interval = 200
        timerPos.Start()
    End Sub

    ' ============ 浏览输入视频 ============
    Private Sub btnBrowseInput_Click(sender As Object, e As EventArgs) Handles btnBrowseInput.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "视频文件|*.mp4;*.avi;*.mkv;*.mov;*.flv;*.wmv;*.ts;*.m4v|所有文件|*.*"
            If ofd.ShowDialog() = DialogResult.OK Then
                txtInput.Text = ofd.FileName
                LoadVideo(ofd.FileName)
            End If
        End Using
    End Sub

    ' ============ 浏览ffmpeg ============
    Private Sub btnBrowseFFmpeg_Click(sender As Object, e As EventArgs) Handles btnBrowseFFmpeg.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "ffmpeg.exe|ffmpeg.exe|所有文件|*.*"
            If ofd.ShowDialog() = DialogResult.OK Then
                txtFFmpeg.Text = ofd.FileName
            End If
        End Using
    End Sub

    ' ============ 加载视频 ============
    Private Sub LoadVideo(filePath As String)
        Try
            totalDuration = 0
            AxWindowsMediaPlayer1.URL = filePath
            AxWindowsMediaPlayer1.Ctlcontrols.stop()

            ' 异步获取关键帧列表并缓存（不阻塞UI）
            Dim ffprobe As String = GetFFprobePath()
            If ffprobe IsNot Nothing Then
                Dim capturedPath As String = filePath
                Task.Run(Sub()
                             cachedKeyframes = GetKeyframeTimestamps(capturedPath, ffprobe)
                         End Sub)
            Else
                cachedKeyframes = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show("加载视频失败: " & ex.Message)
        End Try
    End Sub

    ' ============ 播放状态变化 ============
    Private Sub AxWindowsMediaPlayer1_PlayStateChange(sender As Object, e As AxWMPLib._WMPOCXEvents_PlayStateChangeEvent) Handles AxWindowsMediaPlayer1.PlayStateChange
        ' 3 = playing, 2 = paused, 1 = stopped, 8 = media ended
        If e.newState = 3 Then
            isPlaying = True
            If totalDuration <= 0 Then
                totalDuration = AxWindowsMediaPlayer1.currentMedia.duration
                lblTotal.Text = "总时长: " & FormatTimeHMS(totalDuration)
            End If
        ElseIf e.newState = 2 Or e.newState = 1 Then
            isPlaying = False
        ElseIf e.newState = 8 Then
            isPlaying = False
        End If
    End Sub

    ' ============ 定时器更新进度（用Timer控件，Name=timerProgress，Interval=200） ============
    Private Sub timerProgress_Tick(sender As Object, e As EventArgs) Handles timerProgress.Tick
        If AxWindowsMediaPlayer1.currentMedia Is Nothing Then Return

        If totalDuration <= 0 Then
            totalDuration = AxWindowsMediaPlayer1.currentMedia.duration
            lblTotal.Text = "总时长: " & FormatTimeHMS(totalDuration)
        End If

        If Not isDragging AndAlso totalDuration > 0 Then
            Dim cur As Double = AxWindowsMediaPlayer1.Ctlcontrols.currentPosition

            lblCurrent.Text = "当前: " & FormatTimeHMS(cur)
        End If
    End Sub

    ' ============ 拖动进度条 ============
    Private Sub trackBar_MouseDown(sender As Object, e As MouseEventArgs)
        isDragging = True
    End Sub



    ' ============ 设为开始 ============
    Private Sub btnSetStart_Click(sender As Object, e As EventArgs) Handles btnSetStart.Click
        If AxWindowsMediaPlayer1.currentMedia Is Nothing Then Return

        Dim cur As Double = AxWindowsMediaPlayer1.Ctlcontrols.currentPosition

        ' 若勾选了吸附，尝试对齐到最近的关键帧
        If chkSnapKeyframe.Checked Then
            Dim keyframes As List(Of Double) = cachedKeyframes
            If keyframes Is Nothing AndAlso GetFFprobePath() IsNot Nothing Then
                keyframes = GetKeyframeTimestamps(txtInput.Text, GetFFprobePath())
                cachedKeyframes = keyframes
            End If

            If keyframes IsNot Nothing AndAlso keyframes.Count > 0 Then
                cur = FindNearestKeyframe(keyframes, cur)
                lblStatus.Text = String.Format("已吸附到关键帧: {0}", FormatTimeHMS(cur))
            Else
                lblStatus.Text = "未获取到关键帧，使用原始时间"
            End If
        End If

        txtStart.Text = FormatTimeHMS(cur)
    End Sub

    ' ============ 设为结束 ============
    Private Sub btnSetEnd_Click(sender As Object, e As EventArgs) Handles btnSetEnd.Click
        If AxWindowsMediaPlayer1.currentMedia Is Nothing Then Return

        Dim cur As Double = AxWindowsMediaPlayer1.Ctlcontrols.currentPosition

        ' 若勾选了吸附，尝试对齐到最近的关键帧
        If chkSnapKeyframe.Checked Then
            Dim keyframes As List(Of Double) = cachedKeyframes
            If keyframes Is Nothing AndAlso GetFFprobePath() IsNot Nothing Then
                keyframes = GetKeyframeTimestamps(txtInput.Text, GetFFprobePath())
                cachedKeyframes = keyframes
            End If

            If keyframes IsNot Nothing AndAlso keyframes.Count > 0 Then
                cur = FindNearestKeyframe(keyframes, cur)
                lblStatus.Text = String.Format("已吸附到关键帧: {0}", FormatTimeHMS(cur))
            Else
                lblStatus.Text = "未获取到关键帧，使用原始时间"
            End If
        End If

        txtEnd.Text = FormatTimeHMS(cur)
    End Sub

    Private Function ParseTimeToSeconds(s As String) As Double
        Dim parts = s.Split(":"c)
        Dim sec As Double = 0
        If parts.Length = 3 Then
            sec = Double.Parse(parts(0), Globalization.CultureInfo.InvariantCulture) * 3600 +
              Double.Parse(parts(1), Globalization.CultureInfo.InvariantCulture) * 60 +
              Double.Parse(parts(2), Globalization.CultureInfo.InvariantCulture)
        ElseIf parts.Length = 2 Then
            sec = Double.Parse(parts(0), Globalization.CultureInfo.InvariantCulture) * 60 +
              Double.Parse(parts(1), Globalization.CultureInfo.InvariantCulture)
        Else
            sec = Double.Parse(s, Globalization.CultureInfo.InvariantCulture)
        End If
        Return sec
    End Function

    ' ============ 剪切 ============
    Private Async Sub btnCut_Click(sender As Object, e As EventArgs) Handles btnCut.Click
        If isCutting Then Return

        ' ---- 校验 ----
        If String.IsNullOrWhiteSpace(txtInput.Text) OrElse Not File.Exists(txtInput.Text) Then
            MessageBox.Show("请选择有效的输入视频文件")
            Return
        End If
        If String.IsNullOrWhiteSpace(txtFFmpeg.Text) OrElse Not File.Exists(txtFFmpeg.Text) Then
            MessageBox.Show("请选择有效的ffmpeg.exe路径")
            Return
        End If
        If String.IsNullOrWhiteSpace(txtStart.Text) OrElse String.IsNullOrWhiteSpace(txtEnd.Text) Then
            MessageBox.Show("请设置开始和结束时间")
            Return
        End If

        ' ---- 选输出 ----
        Dim outPath As String
        Using sfd As New SaveFileDialog()
            sfd.Filter = "MP4文件|*.mp4|所有文件|*.*"
            sfd.FileName = Path.GetFileNameWithoutExtension(txtInput.Text) & "_cut.mp4"
            If sfd.ShowDialog() <> DialogResult.OK Then Return
            outPath = sfd.FileName
        End Using

        ' ---- 计算时长 ----
        Dim startSec As Double = ParseTimeToSeconds(txtStart.Text.Trim())
        Dim endSec As Double = ParseTimeToSeconds(txtEnd.Text.Trim())
        Dim cutDuration As Double = endSec - startSec
        If cutDuration <= 0 Then
            MessageBox.Show("结束时间必须大于开始时间")
            Return
        End If

        If chkSnapKeyframe.Checked AndAlso cachedKeyframes IsNot Nothing AndAlso cachedKeyframes.Count > 0 Then
            startSec = FindNearestKeyframe(cachedKeyframes, startSec)
            endSec = FindNearestKeyframe(cachedKeyframes, endSec)

            ' 同步更新文本框显示
            txtStart.Text = FormatTimeHMS(startSec)
            txtEnd.Text = FormatTimeHMS(endSec)
        End If

        Dim ffmpeg As String = txtFFmpeg.Text
        Dim input As String = txtInput.Text
        Dim start As String = txtStart.Text.Trim()

        ' ---- 准备UI ----
        isCutting = True
        btnCut.Enabled = False
        progressBar.Style = ProgressBarStyle.Blocks
        progressBar.Minimum = 0
        progressBar.Maximum = 100
        progressBar.Value = 0
        lblStatus.Text = "正在剪切... 0%"

        Try
            Await Task.Run(Sub()
                               ' -progress pipe:2 让ffmpeg把进度键值对输出到stderr
                               ' -nostats 关掉传统的刷屏统计，避免干扰解析
                               Dim args As String = String.Format(
                "-y -ss {0} -i ""{1}"" -t {2} -c copy -progress pipe:2 -nostats ""{3}""",
                start, input,
                cutDuration.ToString("0.000", Globalization.CultureInfo.InvariantCulture),
                outPath)

                               Dim psi As New ProcessStartInfo()
                               psi.FileName = ffmpeg
                               psi.Arguments = args
                               psi.UseShellExecute = False
                               psi.CreateNoWindow = True
                               psi.RedirectStandardError = True
                               psi.RedirectStandardOutput = True

                               Using proc As Process = Process.Start(psi)
                                   Dim errLines As New List(Of String)()
                                   Dim line As String = proc.StandardError.ReadLine()
                                   While line IsNot Nothing
                                       ' 处理 line
                                       errLines.Add(line)

                                       ' 解析进度：新版本是 out_time_us，旧版本是 out_time_ms
                                       Dim curSec As Double = -1
                                       If line.StartsWith("out_time_us=") Then
                                           Dim v As Long
                                           If Long.TryParse(line.Substring(12), v) Then curSec = v / 1000000.0
                                       ElseIf line.StartsWith("out_time_ms=") Then
                                           ' 历史遗留：这里的值其实也是微秒
                                           Dim v As Long
                                           If Long.TryParse(line.Substring(12), v) Then curSec = v / 1000000.0
                                       End If

                                       If curSec >= 0 Then
                                           Dim pct As Integer = CInt(Math.Min(100, curSec / cutDuration * 100))
                                           If pct < 0 Then pct = 0
                                           Try
                                               Me.Invoke(Sub()
                                                             progressBar.Value = pct
                                                             lblStatus.Text = String.Format("正在剪切... {0}%", pct)
                                                         End Sub)
                                           Catch
                                               ' 窗体关闭时忽略
                                           End Try
                                       End If
                                       line = proc.StandardError.ReadLine()
                                   End While

                                   proc.WaitForExit()
                                   If proc.ExitCode <> 0 Then
                                       Throw New Exception("ffmpeg返回错误码 " & proc.ExitCode & vbCrLf &
                                        String.Join(vbCrLf, errLines))
                                   End If
                               End Using
                           End Sub)

            progressBar.Value = 100
            lblStatus.Text = "剪切完成: " & outPath
            'MessageBox.Show("剪切完成！" & vbCrLf & outPath, "成功")
        Catch ex As Exception
            Dim msg As String = ex.Message.Replace(vbCrLf, " ")
            If msg.Length > 120 Then msg = msg.Substring(0, 120) & "..."
            lblStatus.Text = "剪切失败: " & msg
            'MessageBox.Show("剪切失败: " & ex.Message, "错误")
        Finally
            isCutting = False
            btnCut.Enabled = True
        End Try
    End Sub

    ' ============ 工具方法 ============
    Private Function FormatTime(seconds As Double) As String
        If Double.IsNaN(seconds) OrElse Double.IsInfinity(seconds) OrElse seconds < 0 Then Return "00:00:00"
        Dim ts As TimeSpan = TimeSpan.FromSeconds(seconds)
        Return String.Format("{0:00}:{1:00}:{2:00}", CInt(ts.TotalHours), ts.Minutes, ts.Seconds)
    End Function

    Private Function FormatTimeHMS(seconds As Double) As String
        ' 输出HH:MM:SS.mmm格式，供ffmpeg使用
        If Double.IsNaN(seconds) OrElse Double.IsInfinity(seconds) OrElse seconds < 0 Then Return "00:00:00.000"
        Dim ts As TimeSpan = TimeSpan.FromSeconds(seconds)
        Return String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                             CInt(Math.Floor(ts.TotalHours)), ts.Minutes, ts.Seconds, ts.Milliseconds)
    End Function

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            AxWindowsMediaPlayer1.Ctlcontrols.stop()
            AxWindowsMediaPlayer1.close()
        Catch
        End Try
    End Sub

    Private Sub txtInput_DragEnter(sender As Object, e As DragEventArgs) Handles txtInput.DragEnter
        ' 判断拖进来的是文件
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If files IsNot Nothing AndAlso files.Length > 0 Then
                ' 只接受视频文件
                Dim ext As String = Path.GetExtension(files(0)).ToLower()
                Dim videoExts As String() = {".mp4", ".avi", ".mkv", ".mov", ".flv", ".wmv", ".ts", ".m4v", ".mpg", ".mpeg", ".webm", ".rmvb", ".3gp"}
                If videoExts.Contains(ext) Then
                    e.Effect = DragDropEffects.Copy
                    Return
                End If
            End If
        End If
        e.Effect = DragDropEffects.None
    End Sub

    Private Sub txtInput_DragDrop(sender As Object, e As DragEventArgs) Handles txtInput.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If files IsNot Nothing AndAlso files.Length > 0 Then
                Dim path As String = files(0)
                txtInput.Text = path
                LoadVideo(path)   ' 直接加载视频
            End If
        End If
    End Sub

    Private Sub txtFFmpeg_DragEnter(sender As Object, e As DragEventArgs) Handles txtFFmpeg.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If files IsNot Nothing AndAlso files.Length > 0 Then
                ' 只接受 exe 文件（通常是 ffmpeg.exe）
                If Path.GetExtension(files(0)).ToLower() = ".exe" Then
                    e.Effect = DragDropEffects.Copy
                    Return
                End If
            End If
        End If
        e.Effect = DragDropEffects.None
    End Sub

    Private Sub txtFFmpeg_DragDrop(sender As Object, e As DragEventArgs) Handles txtFFmpeg.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If files IsNot Nothing AndAlso files.Length > 0 Then
                Dim filepath As String = files(0)
                ' 简单校验：文件名里最好包含 ffmpeg
                If Path.GetFileNameWithoutExtension(filepath).ToLower().Contains("ffmpeg") Then
                    txtFFmpeg.Text = filepath
                Else
                    Dim r = MessageBox.Show(
                        "你拖入的不是 ffmpeg.exe，确定要使用吗？" & vbCrLf & filepath,
                        "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If r = DialogResult.Yes Then
                        txtFFmpeg.Text = filepath
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If isCutting Then
            e.Cancel = True
            MessageBox.Show("正在剪切中，请等待完成")
            Return
        End If
        Try
            AxWindowsMediaPlayer1.Ctlcontrols.stop()
            AxWindowsMediaPlayer1.close()
        Catch
        End Try
        timerPos.Stop()
    End Sub

    Private Function TryParseTimeToSeconds(s As String, ByRef result As Double) As Boolean
        result = 0
        If String.IsNullOrWhiteSpace(s) Then Return False
        Try
            Dim parts = s.Split(":"c)
            If parts.Length = 3 Then
                result = Double.Parse(parts(0), Globalization.CultureInfo.InvariantCulture) * 3600 +
                     Double.Parse(parts(1), Globalization.CultureInfo.InvariantCulture) * 60 +
                     Double.Parse(parts(2), Globalization.CultureInfo.InvariantCulture)
            ElseIf parts.Length = 2 Then
                result = Double.Parse(parts(0), Globalization.CultureInfo.InvariantCulture) * 60 +
                     Double.Parse(parts(1), Globalization.CultureInfo.InvariantCulture)
            Else
                result = Double.Parse(s, Globalization.CultureInfo.InvariantCulture)
            End If
            Return result >= 0
        Catch
            Return False
        End Try
    End Function

    Private Sub btnPlayFromStart_Click(sender As Object, e As EventArgs) Handles btnPlayFromStart.Click
        If AxWindowsMediaPlayer1.currentMedia Is Nothing Then
            MessageBox.Show("请先加载视频")
            Return
        End If
        If String.IsNullOrWhiteSpace(txtStart.Text) Then
            MessageBox.Show("请先设置开始时间")
            Return
        End If

        Dim sec As Double
        If Not TryParseTimeToSeconds(txtStart.Text.Trim(), sec) Then
            MessageBox.Show("开始时间格式不正确，应为 HH:MM:SS.mmm")
            Return
        End If

        Try
            AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = sec
            AxWindowsMediaPlayer1.Ctlcontrols.play()
        Catch ex As Exception
            MessageBox.Show("定位失败: " & ex.Message)
        End Try
    End Sub

    Private Sub btnPlayFromEnd_Click(sender As Object, e As EventArgs) Handles btnPlayFromEnd.Click
        If AxWindowsMediaPlayer1.currentMedia Is Nothing Then
            MessageBox.Show("请先加载视频")
            Return
        End If
        If String.IsNullOrWhiteSpace(txtEnd.Text) Then
            MessageBox.Show("请先设置结束时间")
            Return
        End If

        Dim sec As Double
        If Not TryParseTimeToSeconds(txtEnd.Text.Trim(), sec) Then
            MessageBox.Show("结束时间格式不正确，应为 HH:MM:SS.mmm")
            Return
        End If

        Try
            AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = sec
            AxWindowsMediaPlayer1.Ctlcontrols.play()
        Catch ex As Exception
            MessageBox.Show("定位失败: " & ex.Message)
        End Try
    End Sub

    Private Sub timerPos_Tick(sender As Object, e As EventArgs) Handles timerPos.Tick
        If AxWindowsMediaPlayer1.currentMedia Is Nothing Then
            lblCurrent.Text = "当前: 00:00:00.000"
            Return
        End If
        Try
            Dim cur As Double = AxWindowsMediaPlayer1.Ctlcontrols.currentPosition
            lblCurrent.Text = "当前: " & FormatTimeHMS(cur)
        Catch
        End Try
    End Sub

    Private Function GetKeyframeTimestamps(filePath As String, ffprobePath As String) As List(Of Double)
        Dim keyframes As New List(Of Double)()
        Try
            Dim psi As New ProcessStartInfo()
            psi.FileName = ffprobePath
            psi.Arguments = String.Format(
                "-loglevel error -select_streams v:0 -show_entries frame=key_frame,pkt_pts_time -of csv=print_section=0 ""{0}""",
                filePath)
            psi.UseShellExecute = False
            psi.CreateNoWindow = True
            psi.RedirectStandardOutput = True

            Using proc As Process = Process.Start(psi)
                Dim line As String = proc.StandardOutput.ReadLine()
                While line IsNot Nothing
                    ' 输出格式：1,0.000000 或 0,0.040000
                    Dim parts As String() = line.Split(","c)
                    If parts.Length >= 2 AndAlso parts(0).Trim() = "1" Then
                        Dim ts As Double
                        If Double.TryParse(parts(1), Globalization.NumberStyles.Float,
                                           Globalization.CultureInfo.InvariantCulture, ts) Then
                            keyframes.Add(ts)
                        End If
                    End If
                    line = proc.StandardOutput.ReadLine()
                End While
                proc.WaitForExit()
            End Using
        Catch
            ' 获取失败时返回空列表，调用方自行处理
        End Try
        Return keyframes
    End Function

    Private Function GetFFprobePath() As String
        If String.IsNullOrWhiteSpace(txtFFmpeg.Text) Then Return Nothing
        Dim dir As String = System.IO.Path.GetDirectoryName(txtFFmpeg.Text)
        Dim ffprobe As String = System.IO.Path.Combine(dir, "ffprobe.exe")
        If File.Exists(ffprobe) Then Return ffprobe
        Return Nothing
    End Function

    Private Function FindNearestKeyframe(keyframes As List(Of Double), targetTime As Double) As Double
        If keyframes Is Nothing OrElse keyframes.Count = 0 Then Return targetTime

        Dim nearest As Double = keyframes(0)
        Dim minDiff As Double = Math.Abs(keyframes(0) - targetTime)

        For Each kf In keyframes
            Dim diff As Double = Math.Abs(kf - targetTime)
            If diff < minDiff Then
                minDiff = diff
                nearest = kf
            End If
        Next

        Return nearest
    End Function

End Class