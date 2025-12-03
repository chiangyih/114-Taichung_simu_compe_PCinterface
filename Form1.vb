Imports System.IO.Ports
Imports System.Diagnostics
Imports System.Management

Public Class Form1
    Private WithEvents serialPort As SerialPort
    Private WithEvents timerClock As New Timer()
    Private WithEvents timerSystemInfo As New Timer()
    Private cpuCounter As PerformanceCounter
    Private responseBuffer As String = ""
    Private waitingForResponse As Boolean = False
    Private lastCommand As String = ""

    Private savedModeIndex As Integer = 0
    Private savedLEDValue As String = ""
    Private isFirstRead As Boolean = True

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeSerialPort()
        InitializeTimers()
        InitializeCPUCounter()
        RefreshCOMPorts()
        UpdateUIState(False)

        StartClockTimer()

        AutoConnectBluetoothPort()
    End Sub

    Private Sub InitializeSerialPort()
        serialPort = New SerialPort()
        serialPort.BaudRate = 9600
        serialPort.DataBits = 8
        serialPort.Parity = Parity.None
        serialPort.StopBits = StopBits.One
        serialPort.ReadTimeout = 2000
        serialPort.WriteTimeout = 2000
    End Sub

    Private Sub InitializeTimers()
        timerClock.Interval = 1000

        timerSystemInfo.Interval = 1000
    End Sub

    Private Sub InitializeCPUCounter()
        Try
            cpuCounter = New PerformanceCounter("Processor", "% Processor Time", "_Total")
            cpuCounter.NextValue()
        Catch ex As Exception
            Debug.WriteLine("CPU Counter initialization failed: " & ex.Message)
        End Try
    End Sub

    Private Sub StartClockTimer()
        timerClock.Start()
        UpdateCurrentTime()
    End Sub

    Private Sub timerClock_Tick(sender As Object, e As EventArgs) Handles timerClock.Tick
        UpdateCurrentTime()

        If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
            SendTimeSync()
        End If
    End Sub

    Private Sub UpdateCurrentTime()
        lblCurrentTime.Text = "Current Time: " & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
    End Sub

    Private Sub timerSystemInfo_Tick(sender As Object, e As EventArgs) Handles timerSystemInfo.Tick
        UpdateSystemInfo()

        If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
            SendSystemInfo()
        End If
    End Sub

    Private Sub RefreshCOMPorts()
        Dim currentSelection As String = If(cmbCOMPort.SelectedItem?.ToString(), "")
        cmbCOMPort.Items.Clear()

        Dim ports As String() = SerialPort.GetPortNames()
        For Each port As String In ports
            cmbCOMPort.Items.Add(port)
        Next

        If cmbCOMPort.Items.Count > 0 Then
            If Not String.IsNullOrEmpty(currentSelection) AndAlso cmbCOMPort.Items.Contains(currentSelection) Then
                cmbCOMPort.SelectedItem = currentSelection
            Else
                cmbCOMPort.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub AutoConnectBluetoothPort()
        Try
            Dim bluetoothPorts As New List(Of String)()
            Dim ports As String() = SerialPort.GetPortNames()

            For Each port As String In ports
                Dim portNumber As Integer
                If Integer.TryParse(port.Replace("COM", ""), portNumber) Then
                    bluetoothPorts.Add(port)
                End If
            Next

            If bluetoothPorts.Count > 0 Then
                bluetoothPorts.Sort()
                Dim selectedPort = bluetoothPorts(0)
                cmbCOMPort.SelectedItem = selectedPort

                System.Threading.Thread.Sleep(500)
                btnOpen.PerformClick()
            End If
        Catch ex As Exception
            Debug.WriteLine("Auto-connect failed: " & ex.Message)
        End Try
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        If cmbCOMPort.SelectedItem Is Nothing Then
            MessageBox.Show("請選擇 COM Port", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            If serialPort.IsOpen Then
                serialPort.Close()
            End If

            serialPort.PortName = cmbCOMPort.SelectedItem.ToString()
            serialPort.Open()

            SendCommand("HELLO")
            System.Threading.Thread.Sleep(200)

            SetDeviceOnline()
            UpdateUIState(True)

            timerSystemInfo.Start()

            RestorePreviousState()

        Catch ex As Exception
            MessageBox.Show("無法開啟 COM Port: " & ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            SetDeviceOffline()
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseConnection()
    End Sub

    Private Sub CloseConnection()
        Try
            SaveCurrentState()

            timerSystemInfo.Stop()

            If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
                serialPort.Close()
            End If

            SetDeviceOffline()
            UpdateUIState(False)

        Catch ex As Exception
            Debug.WriteLine("Close error: " & ex.Message)
        End Try
    End Sub

    Private Sub UpdateUIState(isConnected As Boolean)
        btnOpen.Enabled = Not isConnected
        btnClose.Enabled = isConnected
        cmbCOMPort.Enabled = Not isConnected

        btnTimeSync.Enabled = isConnected
        cmbMode.Enabled = isConnected
        btnRead.Enabled = isConnected
        btnWrite.Enabled = isConnected
        txtLEDOutput.Enabled = isConnected
    End Sub

    Private Sub SetDeviceOnline()
        txtDeviceStatus.Text = "Online"
        txtDeviceStatus.BackColor = Color.Green
    End Sub

    Private Sub SetDeviceOffline()
        txtDeviceStatus.Text = "Offline"
        txtDeviceStatus.BackColor = Color.Red
    End Sub

    Private Sub SaveCurrentState()
        If cmbMode.SelectedIndex >= 0 Then
            savedModeIndex = cmbMode.SelectedIndex
        End If
        savedLEDValue = txtLEDOutput.Text
    End Sub

    Private Sub RestorePreviousState()
        If savedModeIndex >= 0 AndAlso savedModeIndex < cmbMode.Items.Count Then
            cmbMode.SelectedIndex = savedModeIndex
        End If

        If Not String.IsNullOrEmpty(savedLEDValue) Then
            txtLEDOutput.Text = savedLEDValue
        End If
    End Sub

    Private Sub btnTimeSync_Click(sender As Object, e As EventArgs) Handles btnTimeSync.Click
        If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
            SendTimeSync()
        End If
    End Sub

    Private Sub SendTimeSync()
        Try
            Dim timeString As String = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            SendCommand("T" & timeString)
        Catch ex As Exception
            Debug.WriteLine("Time sync error: " & ex.Message)
        End Try
    End Sub

    Private Sub cmbMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMode.SelectedIndexChanged
        If serialPort Is Nothing OrElse Not serialPort.IsOpen Then
            Return
        End If

        Dim modeCommand As String = ""
        Select Case cmbMode.SelectedIndex
            Case 0
                modeCommand = "M1"
            Case 1
                modeCommand = "M2"
                SendTimeSync()
            Case 2
                modeCommand = "M3"
                SendSystemInfo()
        End Select

        If Not String.IsNullOrEmpty(modeCommand) Then
            SendCommand(modeCommand)
        End If
    End Sub

    Private Sub UpdateSystemInfo()
        Try
            Dim cpuUsage As Single = 0
            If cpuCounter IsNot Nothing Then
                cpuUsage = cpuCounter.NextValue()
            End If
            lblCPUUsage.Text = $"CPU 使用率: {cpuUsage:F0}%"

            Dim totalRAM As Double = My.Computer.Info.TotalPhysicalMemory / (1024.0 ^ 3)
            Dim availableRAM As Double = My.Computer.Info.AvailablePhysicalMemory / (1024.0 ^ 3)
            Dim usedRAM As Double = totalRAM - availableRAM

            lblRAMUsage.Text = $"RAM 使用量: {usedRAM:F1}GB / {totalRAM:F1}GB"

        Catch ex As Exception
            Debug.WriteLine("System info update error: " & ex.Message)
        End Try
    End Sub

    Private Sub SendSystemInfo()
        Try
            Dim cpuUsage As Single = 0
            If cpuCounter IsNot Nothing Then
                cpuUsage = cpuCounter.NextValue()
            End If

            Dim totalRAM As Double = My.Computer.Info.TotalPhysicalMemory / (1024.0 ^ 3)
            Dim availableRAM As Double = My.Computer.Info.AvailablePhysicalMemory / (1024.0 ^ 3)
            Dim usedRAM As Double = totalRAM - availableRAM

            Dim infoString As String = $"Scpu={cpuUsage:F0};ram={usedRAM:F1}/{totalRAM:F1}"
            SendCommand(infoString)

        Catch ex As Exception
            Debug.WriteLine("System info send error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnRead_Click(sender As Object, e As EventArgs) Handles btnRead.Click
        If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
            SendCommand("R")
            lastCommand = "R"
            waitingForResponse = True
        End If
    End Sub

    Private Sub btnWrite_Click(sender As Object, e As EventArgs) Handles btnWrite.Click
        If serialPort Is Nothing OrElse Not serialPort.IsOpen Then
            MessageBox.Show("請先開啟連線", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim value As Integer
        If Not Integer.TryParse(txtLEDOutput.Text.Trim(), value) Then
            MessageBox.Show("請輸入有效的數字", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If value < 1 OrElse value > 254 Then
            MessageBox.Show("LED 腳位值必須在 1-254 之間", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        SendCommand("L" & value.ToString())
        lastCommand = "L"
        waitingForResponse = True
        isFirstRead = False
    End Sub

    Private Sub SendCommand(command As String)
        Try
            If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
                serialPort.WriteLine(command)
                Debug.WriteLine("Sent: " & command)
            End If
        Catch ex As Exception
            Debug.WriteLine("Send error: " & ex.Message)
            MessageBox.Show("傳送命令失敗: " & ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub serialPort_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles serialPort.DataReceived
        Try
            Dim data As String = serialPort.ReadExisting()
            responseBuffer &= data

            If responseBuffer.Contains(vbLf) OrElse responseBuffer.Contains(vbCr) Then
                Dim lines() As String = responseBuffer.Split(New String() {vbCrLf, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries)

                For Each line As String In lines
                    ProcessResponse(line.Trim())
                Next

                responseBuffer = ""
            End If

        Catch ex As Exception
            Debug.WriteLine("DataReceived error: " & ex.Message)
        End Try
    End Sub

    Private Sub ProcessResponse(response As String)
        Debug.WriteLine("Received: " & response)

        If response = "OK" Then
            waitingForResponse = False
            Return
        End If

        If response.StartsWith("VAL=") AndAlso lastCommand = "R" Then
            Dim value As String = response.Substring(4)
            Me.Invoke(Sub()
                          txtLEDOutput.Text = value
                      End Sub)
            waitingForResponse = False
            lastCommand = ""
        ElseIf response = "WRITE OK" AndAlso lastCommand = "L" Then
            Me.Invoke(Sub()
                          MessageBox.Show("EEPROM 寫入成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
                      End Sub)
            waitingForResponse = False
            lastCommand = ""
            isFirstRead = False
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        CloseConnection()
        Application.Exit()
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        CloseConnection()

        If cpuCounter IsNot Nothing Then
            cpuCounter.Dispose()
        End If

        MyBase.OnFormClosing(e)
    End Sub

End Class
