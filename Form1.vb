Imports System.IO.Ports
Imports System.Diagnostics
Imports System.Management

Public Class Form1
    ' SerialPort 連線物件
    Private serialPort As SerialPort
    Private isConnected As Boolean = False
    
    ' Timer 計時器
    Private updateTimer As System.Windows.Forms.Timer
    Private comPortMonitorTimer As System.Windows.Forms.Timer
    Private lastComPorts As String() = {}
    
    ' 系統資訊監控
    Private cpuCounter As PerformanceCounter
    
    ' 岗位号码
    Private positionNumber As String = "XX"
    
    ' 超時控制
    Private readTimeout As System.Timers.Timer
    Private isWaitingForResponse As Boolean = False
    Private lastCommand As String = ""
    
    ' LED 值存儲
    Private ledValue As Integer = 255
    Private isFirstRead As Boolean = True
    
    ' 接收緩衝區
    Private receivedData As String = ""
    
    ' Form Load 事件
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' 設定視窗標題
            Me.Text = String.Format("113學年度工業類科學生技藝競賽暨競賽檔案操做崗位位置設定: {0}", positionNumber)
            
            ' 初始化 lblCurrentTime 樣式和內容
            lblCurrentTime.BackColor = System.Drawing.Color.White
            lblCurrentTime.Text = String.Format("Current Time: {0:yyyy/MM/dd HH:mm:ss}", DateTime.Now)
            
            ' 初始化 SerialPort
            serialPort = New SerialPort() With {
                .BaudRate = 9600,
                .Parity = Parity.None,
                .DataBits = 8,
                .StopBits = StopBits.One,
                .ReadTimeout = 2000,
                .WriteTimeout = 2000
            }
            
            ' 綁定資料接收事件
            AddHandler serialPort.DataReceived, AddressOf SerialPort_DataReceived
            
            ' 初始化主 Timer（每秒更新）
            updateTimer = New System.Windows.Forms.Timer() With {
                .Interval = 1000,
                .Enabled = False
            }
            AddHandler updateTimer.Tick, AddressOf UpdateTimer_Tick
            
            ' 初始化 COM 埠監控 Timer
            comPortMonitorTimer = New System.Windows.Forms.Timer() With {
                .Interval = 2000,
                .Enabled = True
            }
            AddHandler comPortMonitorTimer.Tick, AddressOf ComPortMonitor_Tick
            
            ' 初始化 CPU Counter
            Try
                cpuCounter = New PerformanceCounter("Processor", "% Processor Time", "_Total", True)
                cpuCounter.NextValue() ' 預先讀取一次
            Catch ex As Exception
                ' CPU Counter 初始化失敗，使用替代方案
            End Try
            
            ' 初始化超時計時器
            readTimeout = New System.Timers.Timer(3000) With {
                .AutoReset = False
            }
            AddHandler readTimeout.Elapsed, AddressOf ReadTimeout_Elapsed
            
            ' 掃描並更新 COM 埠列表
            RefreshComPorts()
            
            ' 初始化 UI 狀態（預設為離線）
            UpdateUIState(False)
            UpdateDeviceStatus(False)
            
            ' 嘗試自動連線
            AutoConnectSerialPort()
            
        Catch ex As Exception
            MessageBox.Show(String.Format("初始化失敗: {0}", ex.Message), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    ' COM 埠監控 Timer Tick
    Private Sub ComPortMonitor_Tick(sender As Object, e As EventArgs)
        RefreshComPorts()
    End Sub
    
    ' 自動連線 SerialPort
    Private Sub AutoConnectSerialPort()
        Try
            Dim ports As String() = SerialPort.GetPortNames()
            
            If ports.Length = 0 Then
                Return
            End If
            
            ' 排序埠號，選擇號碼最小的藍芽序列埠
            Array.Sort(ports)
            Dim selectedPort As String = ports(0)
            
            ' 設定選擇的埠
            If cmbCOMPort.Items.Contains(selectedPort) Then
                cmbCOMPort.SelectedItem = selectedPort
            End If
            
            ' 嘗試自動打開連線
            ' OpenConnection(selectedPort)
            
        Catch ex As Exception
            ' 自動連線失敗，等待使用者手動連線
        End Try
    End Sub
    
    ' 刷新 COM 埠列表
    Private Sub RefreshComPorts()
        Try
            Dim currentPorts As String() = SerialPort.GetPortNames()
            
            ' 檢查是否有變化
            If currentPorts.Length <> lastComPorts.Length OrElse
               Not currentPorts.SequenceEqual(lastComPorts) Then
                
                lastComPorts = currentPorts
                
                Dim selectedPort As String = If(cmbCOMPort.SelectedItem IsNot Nothing, cmbCOMPort.SelectedItem.ToString(), "")
                
                ' 更新下拉菜單
                cmbCOMPort.Items.Clear()
                For Each port In currentPorts
                    cmbCOMPort.Items.Add(port)
                Next
                
                ' 保持之前選擇的埠
                If Not String.IsNullOrEmpty(selectedPort) AndAlso cmbCOMPort.Items.Contains(selectedPort) Then
                    cmbCOMPort.SelectedItem = selectedPort
                ElseIf cmbCOMPort.Items.Count > 0 Then
                    cmbCOMPort.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            ' 忽略刷新錯誤
        End Try
    End Sub
    
    ' Time Sync 按鈕點擊事件
    Private Sub btnTimeSync_Click(sender As Object, e As EventArgs) Handles btnTimeSync.Click
        Try
            If Not isConnected Then
                MessageBox.Show("請先連線到設備", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            
            ' 手動發送時間同步
            SendTimeSync()
            MessageBox.Show("時間同步命令已發送", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            
        Catch ex As Exception
            MessageBox.Show(String.Format("時間同步失敗: {0}", ex.Message), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    ' Open 按鈕點擊事件
    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        Try
            If cmbCOMPort.SelectedItem Is Nothing Then
                MessageBox.Show("請先選擇 COM 埠", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            
            Dim selectedPort As String = cmbCOMPort.SelectedItem.ToString()
            OpenConnection(selectedPort)
            
        Catch ex As Exception
            MessageBox.Show(String.Format("打開 COM 埠失敗: {0}", ex.Message), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            UpdateDeviceStatus(False)
        End Try
    End Sub
    
    ' 打開連線
    Private Sub OpenConnection(portName As String)
        Try
            If serialPort.IsOpen Then
                serialPort.Close()
                System.Threading.Thread.Sleep(200)
            End If
            
            ' 打開 SerialPort
            serialPort.PortName = portName
            serialPort.Open()
            
            ' 清空接收緩衝區
            receivedData = ""
            
            ' 傳送初始化封包
            System.Threading.Thread.Sleep(100)
            SendCommand("HELLO")
            
            ' 等待回應
            System.Threading.Thread.Sleep(500)
            
            ' 暫時假設連線成功
            isConnected = True
            UpdateDeviceStatus(True)
            UpdateUIState(True)
            
            ' 啟動定時器
            updateTimer.Start()
            
        Catch ex As Exception
            isConnected = False
            UpdateDeviceStatus(False)
            Throw ex
        End Try
    End Sub
    
    ' Close 按鈕點擊事件
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            CloseConnection()
            
        Catch ex As Exception
            MessageBox.Show(String.Format("關閉 COM 埠失敗: {0}", ex.Message), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    ' 關閉連線
    Private Sub CloseConnection()
        Try
            ' 停止計時器
            updateTimer.Stop()
            
            ' 關閉 SerialPort
            If serialPort.IsOpen Then
                serialPort.Close()
            End If
            
            ' 更新狀態
            isConnected = False
            UpdateDeviceStatus(False)
            UpdateUIState(False)
            
            ' 重置 LED 值狀態
            ledValue = 255
            isFirstRead = True
            txtLEDOutput.Text = ""
            
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    
    ' 更新 Device Status 狀態
    Private Sub UpdateDeviceStatus(isOnline As Boolean)
        Try
            If Me.InvokeRequired Then
                Me.Invoke(Sub() UpdateDeviceStatus(isOnline))
                Return
            End If
            
            If isOnline Then
                txtDeviceStatus.Text = "Online"
                txtDeviceStatus.BackColor = System.Drawing.Color.Green
                txtDeviceStatus.ForeColor = System.Drawing.Color.White
            Else
                txtDeviceStatus.Text = "Offline"
                txtDeviceStatus.BackColor = System.Drawing.Color.Red
                txtDeviceStatus.ForeColor = System.Drawing.Color.White
            End If
        Catch
        End Try
    End Sub
    
    ' 更新 UI 狀態
    Private Sub UpdateUIState(isOnline As Boolean)
        Try
            If Me.InvokeRequired Then
                Me.Invoke(Sub() UpdateUIState(isOnline))
                Return
            End If
            
            btnOpen.Enabled = Not isOnline
            btnClose.Enabled = isOnline
            btnTimeSync.Enabled = isOnline
            cmbMode.Enabled = isOnline
            btnRead.Enabled = isOnline
            btnWrite.Enabled = isOnline
            txtLEDOutput.Enabled = isOnline
            cmbCOMPort.Enabled = Not isOnline
            
        Catch
        End Try
    End Sub
    
    ' EXIT 按鈕點擊事件
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Try
            If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
                serialPort.Close()
            End If
            updateTimer.Stop()
            comPortMonitorTimer.Stop()
            Me.Close()
        Catch
        End Try
    End Sub
    
    ' Timer Tick 事件 - 每秒更新一次
    Private Sub UpdateTimer_Tick(sender As Object, e As EventArgs)
        Try
            If Not isConnected Then
                Return
            End If
            
            ' 更新系統時間顯示
            UpdateCurrentTime()
            
            ' 更新 CPU 和 RAM 資訊
            UpdateSystemInfo()
            
            ' 發送時間同步指令到 MCU
            SendTimeSync()
            
        Catch ex As Exception
            ' 忽略計時器錯誤
        End Try
    End Sub
    
    ' 更新當前系統時間
    Private Sub UpdateCurrentTime()
        Try
            If Me.InvokeRequired Then
                Me.Invoke(Sub() UpdateCurrentTime())
                Return
            End If
            
            Dim currentTime As DateTime = DateTime.Now
            lblCurrentTime.Text = String.Format("Current Time: {0:yyyy/MM/dd HH:mm:ss}", currentTime)
            
        Catch
        End Try
    End Sub
    
    ' 更新系統資訊（CPU 和 RAM）
    Private Sub UpdateSystemInfo()
        Try
            ' 獲取 CPU 使用率
            Dim cpuUsage As Single = 0
            If cpuCounter IsNot Nothing Then
                cpuUsage = cpuCounter.NextValue()
            End If
            
            ' 使用 WMI 獲取 RAM 資訊
            Try
                Dim searcher As New ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem")
                Dim collection As ManagementObjectCollection = searcher.Get()
                
                For Each obj As ManagementObject In collection
                    Dim totalMem As Long = Long.Parse(obj("TotalVisibleMemorySize").ToString()) * 1024
                    Dim freeMem As Long = Long.Parse(obj("FreePhysicalMemory").ToString()) * 1024
                    Dim usedMem As Long = totalMem - freeMem
                    
                    Dim usedGB As Double = usedMem / (1024.0 * 1024 * 1024)
                    Dim totalGB As Double = totalMem / (1024.0 * 1024 * 1024)
                    
                    If Me.InvokeRequired Then
                        Me.Invoke(Sub()
                            lblCPUUsage.Text = String.Format("CPU 使用率: {0:F0}%", cpuUsage)
                            lblRAMUsage.Text = String.Format("RAM 使用率: {0:F1}GB / {1:F1}GB", usedGB, totalGB)
                        End Sub)
                    Else
                        lblCPUUsage.Text = String.Format("CPU 使用率: {0:F0}%", cpuUsage)
                        lblRAMUsage.Text = String.Format("RAM 使用率: {0:F1}GB / {1:F1}GB", usedGB, totalGB)
                    End If
                    
                    ' 發送 CPU/RAM 資訊到 MCU（僅在模式 3 時需要）
                    If isConnected Then
                        SendSystemInfo(cpuUsage, usedGB, totalGB)
                    End If
                    
                    Exit For
                Next
                
            Catch
                ' WMI 失敗時的備用處理
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                        lblCPUUsage.Text = String.Format("CPU 使用率: {0:F0}%", cpuUsage)
                    End Sub)
                Else
                    lblCPUUsage.Text = String.Format("CPU 使用率: {0:F0}%", cpuUsage)
                End If
            End Try
            
        Catch ex As Exception
            ' 忽略系統資訊更新錯誤
        End Try
    End Sub
    
    ' 發送時間同步指令
    Private Sub SendTimeSync()
        Try
            If isConnected AndAlso serialPort.IsOpen Then
                Dim currentTime As DateTime = DateTime.Now
                Dim timeCommand As String = String.Format("T{0:yyyy/MM/dd HH:mm:ss}", currentTime)
                SendCommand(timeCommand)
            End If
        Catch
        End Try
    End Sub
    
    ' 發送系統資訊指令
    Private Sub SendSystemInfo(cpuUsage As Single, usedGB As Double, totalGB As Double)
        Try
            If isConnected AndAlso serialPort.IsOpen Then
                Dim infoCommand As String = String.Format("Scpu={0:F0};ram={1:F1}/{2:F1}", cpuUsage, usedGB, totalGB)
                SendCommand(infoCommand)
            End If
        Catch
        End Try
    End Sub
    
    ' 模式選擇下拉菜單事件
    Private Sub cmbMode_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbMode.SelectionChangeCommitted
        Try
            If Not isConnected Then
                MessageBox.Show("請先連線到設備", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            
            Dim selectedMode As String = cmbMode.SelectedItem.ToString()
            
            Select Case selectedMode
                Case "1-鎖定螢幕"
                    ' 模式 1: 傳送 M1
                    SendCommand("M1")
                    
                Case "2-顯示設置"
                    ' 模式 2: 傳送 M2 + 時間同步
                    SendCommand("M2")
                    System.Threading.Thread.Sleep(100)
                    SendTimeSync()
                    
                Case "3-電腦資訊"
                    ' 模式 3: 傳送 M3 + 系統資訊
                    SendCommand("M3")
                    System.Threading.Thread.Sleep(100)
                    
                    ' 獲取並發送系統資訊
                    Dim cpuUsage As Single = 0
                    If cpuCounter IsNot Nothing Then
                        cpuUsage = cpuCounter.NextValue()
                    End If
                    
                    Try
                        Dim searcher As New ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem")
                        Dim collection As ManagementObjectCollection = searcher.Get()
                        
                        For Each obj As ManagementObject In collection
                            Dim totalMem As Long = Long.Parse(obj("TotalVisibleMemorySize").ToString()) * 1024
                            Dim freeMem As Long = Long.Parse(obj("FreePhysicalMemory").ToString()) * 1024
                            Dim usedMem As Long = totalMem - freeMem
                            
                            Dim usedGB As Double = usedMem / (1024.0 * 1024 * 1024)
                            Dim totalGB As Double = totalMem / (1024.0 * 1024 * 1024)
                            
                            SendSystemInfo(cpuUsage, usedGB, totalGB)
                            Exit For
                        Next
                    Catch
                    End Try
            End Select
            
        Catch ex As Exception
            MessageBox.Show(String.Format("模式切換失敗: {0}", ex.Message), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    ' Read 按鈕點擊事件
    Private Sub btnRead_Click(sender As Object, e As EventArgs) Handles btnRead.Click
        Try
            If Not isConnected Then
                MessageBox.Show("請先連線到設備", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            
            ' 如果是第一次讀取，顯示預設值 255
            If isFirstRead Then
                txtLEDOutput.Text = "255"
                isFirstRead = False
                MessageBox.Show("首次讀取顯示預設值: 255", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            
            ' 發送讀取指令
            SendCommand("R")
            
            ' 設定超時
            isWaitingForResponse = True
            lastCommand = "READ"
            readTimeout.Start()
            
        Catch ex As Exception
            MessageBox.Show(String.Format("讀取失敗: {0}", ex.Message), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    ' Write 按鈕點擊事件
    Private Sub btnWrite_Click(sender As Object, e As EventArgs) Handles btnWrite.Click
        Try
            If Not isConnected Then
                MessageBox.Show("請先連線到設備", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            
            ' 獲取輸入框中的值
            Dim inputValue As String = txtLEDOutput.Text.Trim()
            
            ' 如果輸入框為空，使用 InputBox
            If String.IsNullOrEmpty(inputValue) Then
                inputValue = InputBox("請輸入要寫入的值 (1-254):", "寫入 LED 值", "")
            End If
            
            If String.IsNullOrEmpty(inputValue) Then
                Return
            End If
            
            ' 驗證輸入值
            Dim value As Integer
            If Not Integer.TryParse(inputValue, value) OrElse value < 1 OrElse value > 254 Then
                MessageBox.Show("輸入值無效，必須是 1 到 254 之間的數字", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            
            ' 發送寫入指令 (格式: Lxxx)
            Dim writeCommand As String = String.Format("L{0:D3}", value)
            SendCommand(writeCommand)
            
            ' 設定超時
            isWaitingForResponse = True
            lastCommand = "WRITE"
            readTimeout.Start()
            
        Catch ex As Exception
            MessageBox.Show(String.Format("寫入失敗: {0}", ex.Message), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    ' 超時事件處理
    Private Sub ReadTimeout_Elapsed(sender As Object, e As System.Timers.ElapsedEventArgs)
        Try
            isWaitingForResponse = False
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                    MessageBox.Show("MCU 無回應，操作超時", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End Sub)
            Else
                MessageBox.Show("MCU 無回應，操作超時", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch
        End Try
    End Sub
    
    ' SerialPort 資料接收事件
    Private Sub SerialPort_DataReceived(sender As Object, e As SerialDataReceivedEventArgs)
        Try
            If serialPort.BytesToRead = 0 Then Return
            
            ' 讀取可用數據
            Dim data As String = serialPort.ReadExisting()
            receivedData &= data
            
            ' 檢查是否有完整的命令（以換行符結尾）
            If receivedData.Contains(vbLf) OrElse receivedData.Contains(vbCr) Then
                Dim lines As String() = receivedData.Split(New String() {vbCrLf, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries)
                
                For Each line In lines
                    Dim trimmedLine As String = line.Trim()
                    If Not String.IsNullOrEmpty(trimmedLine) Then
                        ' 處理接收到的數據
                        If Me.InvokeRequired Then
                            Me.Invoke(Sub() ProcessReceivedData(trimmedLine))
                        Else
                            ProcessReceivedData(trimmedLine)
                        End If
                    End If
                Next
                
                ' 清空緩衝區
                receivedData = ""
            End If
            
        Catch ex As Exception
            ' 記錄錯誤
        End Try
    End Sub
    
    ' 處理接收到的數據
    Private Sub ProcessReceivedData(data As String)
        Try
            ' 檢查初始化回應
            If data.Contains("OK") AndAlso lastCommand.Contains("HELLO") Then
                isConnected = True
                UpdateDeviceStatus(True)
                UpdateUIState(True)
                lastCommand = ""
                Return
            End If
            
            ' 檢查讀取 EEPROM 回應 (VAL=xxx)
            If data.StartsWith("VAL=") Then
                Dim valueStr As String = data.Substring(4).Trim()
                If Integer.TryParse(valueStr, ledValue) Then
                    txtLEDOutput.Text = ledValue.ToString()
                End If
                isWaitingForResponse = False
                readTimeout.Stop()
                lastCommand = ""
                Return
            End If
            
            ' 檢查寫入 EEPROM 回應
            If data.Contains("WRITE OK") Then
                MessageBox.Show("寫入成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
                isWaitingForResponse = False
                readTimeout.Stop()
                lastCommand = ""
                
                ' 自動讀取剛寫入的值
                System.Threading.Thread.Sleep(500)
                SendCommand("R")
                Return
            End If
            
            ' 其他 OK 回應
            If data.Contains("OK") Then
                isWaitingForResponse = False
                readTimeout.Stop()
            End If
            
        Catch ex As Exception
            ' 忽略處理錯誤
        End Try
    End Sub
    
    ' 發送命令到 MCU
    Private Sub SendCommand(command As String)
        Try
            If serialPort.IsOpen Then
                ' 添加行尾符號 (CRLF)
                Dim fullCommand As String = command & vbCrLf
                serialPort.Write(fullCommand)
                lastCommand = command
            End If
        Catch ex As Exception
            ' 忽略發送錯誤
        End Try
    End Sub
    
    ' Form Closing 事件
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            ' 停止計時器
            If updateTimer IsNot Nothing Then
                updateTimer.Stop()
                updateTimer.Dispose()
            End If
            
            If comPortMonitorTimer IsNot Nothing Then
                comPortMonitorTimer.Stop()
                comPortMonitorTimer.Dispose()
            End If
            
            ' 停止超時計時器
            If readTimeout IsNot Nothing Then
                readTimeout.Stop()
                readTimeout.Dispose()
            End If
            
            ' 關閉 SerialPort
            If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
                serialPort.Close()
                serialPort.Dispose()
            End If
            
            ' 釋放 Performance Counter
            If cpuCounter IsNot Nothing Then
                cpuCounter.Dispose()
            End If
            
        Catch ex As Exception
            ' 忽略清理錯誤
        End Try
    End Sub

End Class
