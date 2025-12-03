<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        
        ' 頂部時間顯示標籤
        Me.lblCurrentTime = New System.Windows.Forms.Label()
        Me.lblCurrentTime.Text = "Current Time: 2024/10/23 13:20:32"
        Me.lblCurrentTime.Location = New System.Drawing.Point(20, 15)
        Me.lblCurrentTime.Size = New System.Drawing.Size(330, 30)
        Me.lblCurrentTime.Font = New System.Drawing.Font("Arial", 11, System.Drawing.FontStyle.Regular)
        Me.lblCurrentTime.BackColor = System.Drawing.Color.White
        Me.lblCurrentTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCurrentTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        
        ' Time Sync 按鈕
        Me.btnTimeSync = New System.Windows.Forms.Button()
        Me.btnTimeSync.Text = "Time Sync"
        Me.btnTimeSync.Location = New System.Drawing.Point(360, 15)
        Me.btnTimeSync.Size = New System.Drawing.Size(90, 30)
        Me.btnTimeSync.Font = New System.Drawing.Font("Arial", 10)
        
        ' 模式選擇標籤
        Me.lblMode = New System.Windows.Forms.Label()
        Me.lblMode.Text = "模式選擇:"
        Me.lblMode.Location = New System.Drawing.Point(465, 15)
        Me.lblMode.Size = New System.Drawing.Size(70, 30)
        Me.lblMode.Font = New System.Drawing.Font("Arial", 10)
        Me.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        
        ' 模式選擇下拉菜單
        Me.cmbMode = New System.Windows.Forms.ComboBox()
        Me.cmbMode.Items.AddRange(New String() {"1-鎖定螢幕", "2-顯示設置", "3-電腦資訊"})
        Me.cmbMode.Location = New System.Drawing.Point(540, 15)
        Me.cmbMode.Size = New System.Drawing.Size(210, 30)
        Me.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMode.Font = New System.Drawing.Font("Arial", 10)
        Me.cmbMode.SelectedIndex = 0
        
        ' 介面卡狀態標籤
        Me.lblInterfaceStatus = New System.Windows.Forms.Label()
        Me.lblInterfaceStatus.Text = "介面卡狀態"
        Me.lblInterfaceStatus.Location = New System.Drawing.Point(20, 60)
        Me.lblInterfaceStatus.Size = New System.Drawing.Size(200, 25)
        Me.lblInterfaceStatus.Font = New System.Drawing.Font("Arial", 11, System.Drawing.FontStyle.Regular)
        Me.lblInterfaceStatus.ForeColor = System.Drawing.Color.Gray
        
        ' COM 埠選擇下拉菜單
        Me.cmbCOMPort = New System.Windows.Forms.ComboBox()
        Me.cmbCOMPort.Items.AddRange(New String() {"COM3"})
        Me.cmbCOMPort.Location = New System.Drawing.Point(30, 95)
        Me.cmbCOMPort.Size = New System.Drawing.Size(135, 28)
        Me.cmbCOMPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
        Me.cmbCOMPort.Font = New System.Drawing.Font("Arial", 10)
        Me.cmbCOMPort.SelectedIndex = 0
        
        ' Open 按鈕
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnOpen.Text = "Open"
        Me.btnOpen.Location = New System.Drawing.Point(180, 95)
        Me.btnOpen.Size = New System.Drawing.Size(70, 28)
        Me.btnOpen.Font = New System.Drawing.Font("Arial", 10)
        
        ' Close 按鈕
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnClose.Text = "Close"
        Me.btnClose.Location = New System.Drawing.Point(260, 95)
        Me.btnClose.Size = New System.Drawing.Size(70, 28)
        Me.btnClose.Font = New System.Drawing.Font("Arial", 10)
        
        ' Device Status 標籤
        Me.lblDeviceStatus = New System.Windows.Forms.Label()
        Me.lblDeviceStatus.Text = "Device Status :"
        Me.lblDeviceStatus.Location = New System.Drawing.Point(20, 140)
        Me.lblDeviceStatus.Size = New System.Drawing.Size(120, 28)
        Me.lblDeviceStatus.Font = New System.Drawing.Font("Arial", 10)
        Me.lblDeviceStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        
        ' Device Status 狀態指示框
        Me.txtDeviceStatus = New System.Windows.Forms.TextBox()
        Me.txtDeviceStatus.Text = "Online"
        Me.txtDeviceStatus.Location = New System.Drawing.Point(150, 140)
        Me.txtDeviceStatus.Size = New System.Drawing.Size(180, 28)
        Me.txtDeviceStatus.BackColor = System.Drawing.Color.Green
        Me.txtDeviceStatus.ForeColor = System.Drawing.Color.White
        Me.txtDeviceStatus.ReadOnly = True
        Me.txtDeviceStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDeviceStatus.Font = New System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold)
        Me.txtDeviceStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
        
        ' EXIT 按鈕
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnExit.Text = "EXIT"
        Me.btnExit.Location = New System.Drawing.Point(30, 190)
        Me.btnExit.Size = New System.Drawing.Size(300, 40)
        Me.btnExit.Font = New System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold)
        
        ' 系統資訊標籤
        Me.lblSystemInfo = New System.Windows.Forms.Label()
        Me.lblSystemInfo.Text = "系統資訊"
        Me.lblSystemInfo.Location = New System.Drawing.Point(400, 60)
        Me.lblSystemInfo.Size = New System.Drawing.Size(200, 25)
        Me.lblSystemInfo.Font = New System.Drawing.Font("Arial", 11, System.Drawing.FontStyle.Regular)
        Me.lblSystemInfo.ForeColor = System.Drawing.Color.Gray
        
        ' CPU 使用率標籤
        Me.lblCPUUsage = New System.Windows.Forms.Label()
        Me.lblCPUUsage.Text = "CPU 使用率: 58%"
        Me.lblCPUUsage.Location = New System.Drawing.Point(400, 95)
        Me.lblCPUUsage.Size = New System.Drawing.Size(250, 25)
        Me.lblCPUUsage.Font = New System.Drawing.Font("Arial", 10)
        
        ' RAM 使用率標籤
        Me.lblRAMUsage = New System.Windows.Forms.Label()
        Me.lblRAMUsage.Text = "RAM 使用率: 8.3GB / 15.9GB"
        Me.lblRAMUsage.Location = New System.Drawing.Point(400, 125)
        Me.lblRAMUsage.Size = New System.Drawing.Size(300, 25)
        Me.lblRAMUsage.Font = New System.Drawing.Font("Arial", 10)
        
        ' LED 輸出讀寫標籤
        Me.lblLEDOutput = New System.Windows.Forms.Label()
        Me.lblLEDOutput.Text = "LED輸出讀寫"
        Me.lblLEDOutput.Location = New System.Drawing.Point(400, 160)
        Me.lblLEDOutput.Size = New System.Drawing.Size(200, 25)
        Me.lblLEDOutput.Font = New System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular)
        Me.lblLEDOutput.ForeColor = System.Drawing.Color.Black
        
        ' LED 輸出區域（顯示框）
        Me.txtLEDOutput = New System.Windows.Forms.TextBox()
        Me.txtLEDOutput.Location = New System.Drawing.Point(410, 190)
        Me.txtLEDOutput.Size = New System.Drawing.Size(130, 60)
        Me.txtLEDOutput.Multiline = True
        Me.txtLEDOutput.Font = New System.Drawing.Font("Arial", 10)
        Me.txtLEDOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        
        ' Write 按鈕
        Me.btnWrite = New System.Windows.Forms.Button()
        Me.btnWrite.Text = "Write"
        Me.btnWrite.Location = New System.Drawing.Point(560, 195)
        Me.btnWrite.Size = New System.Drawing.Size(70, 30)
        Me.btnWrite.Font = New System.Drawing.Font("Arial", 10)
        
        ' Read 按鈕
        Me.btnRead = New System.Windows.Forms.Button()
        Me.btnRead.Text = "Read"
        Me.btnRead.Location = New System.Drawing.Point(640, 195)
        Me.btnRead.Size = New System.Drawing.Size(70, 30)
        Me.btnRead.Font = New System.Drawing.Font("Arial", 10)
        
        ' Form1
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(765, 330)
        Me.Text = "113專年工三機械科單生技競賽暨競賽檔案操做系統位置設定: XX"
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.lblCurrentTime)
        Me.Controls.Add(Me.btnTimeSync)
        Me.Controls.Add(Me.lblMode)
        Me.Controls.Add(Me.cmbMode)
        Me.Controls.Add(Me.lblInterfaceStatus)
        Me.Controls.Add(Me.cmbCOMPort)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblDeviceStatus)
        Me.Controls.Add(Me.txtDeviceStatus)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.lblSystemInfo)
        Me.Controls.Add(Me.lblCPUUsage)
        Me.Controls.Add(Me.lblRAMUsage)
        Me.Controls.Add(Me.lblLEDOutput)
        Me.Controls.Add(Me.txtLEDOutput)
        Me.Controls.Add(Me.btnWrite)
        Me.Controls.Add(Me.btnRead)
    End Sub

    Friend WithEvents lblCurrentTime As System.Windows.Forms.Label
    Friend WithEvents btnTimeSync As System.Windows.Forms.Button
    Friend WithEvents lblMode As System.Windows.Forms.Label
    Friend WithEvents cmbMode As System.Windows.Forms.ComboBox
    Friend WithEvents lblInterfaceStatus As System.Windows.Forms.Label
    Friend WithEvents cmbCOMPort As System.Windows.Forms.ComboBox
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblDeviceStatus As System.Windows.Forms.Label
    Friend WithEvents txtDeviceStatus As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents lblSystemInfo As System.Windows.Forms.Label
    Friend WithEvents lblCPUUsage As System.Windows.Forms.Label
    Friend WithEvents lblRAMUsage As System.Windows.Forms.Label
    Friend WithEvents lblLEDOutput As System.Windows.Forms.Label
    Friend WithEvents txtLEDOutput As System.Windows.Forms.TextBox
    Friend WithEvents btnWrite As System.Windows.Forms.Button
    Friend WithEvents btnRead As System.Windows.Forms.Button

End Class
