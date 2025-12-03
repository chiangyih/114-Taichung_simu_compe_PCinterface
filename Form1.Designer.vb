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
        lblCurrentTime = New Label()
        btnTimeSync = New Button()
        lblMode = New Label()
        cmbMode = New ComboBox()
        lblInterfaceStatus = New Label()
        cmbCOMPort = New ComboBox()
        btnOpen = New Button()
        btnClose = New Button()
        lblDeviceStatus = New Label()
        txtDeviceStatus = New TextBox()
        btnExit = New Button()
        lblSystemInfo = New Label()
        lblCPUUsage = New Label()
        lblRAMUsage = New Label()
        lblLEDOutput = New Label()
        txtLEDOutput = New TextBox()
        btnWrite = New Button()
        btnRead = New Button()
        SuspendLayout()
        ' 
        ' lblCurrentTime
        ' 
        lblCurrentTime.BorderStyle = BorderStyle.FixedSingle
        lblCurrentTime.Font = New Font("Arial", 11F)
        lblCurrentTime.Location = New Point(20, 15)
        lblCurrentTime.Name = "lblCurrentTime"
        lblCurrentTime.Size = New Size(340, 25)
        lblCurrentTime.TabIndex = 0
        lblCurrentTime.Text = "Current Time: 2024/10/23 13:20:32"
        ' 
        ' btnTimeSync
        ' 
        btnTimeSync.Font = New Font("Arial", 9F)
        btnTimeSync.Location = New Point(370, 15)
        btnTimeSync.Name = "btnTimeSync"
        btnTimeSync.Size = New Size(75, 25)
        btnTimeSync.TabIndex = 1
        btnTimeSync.Text = "Time Sync"
        ' 
        ' lblMode
        ' 
        lblMode.Font = New Font("Arial", 9F)
        lblMode.Location = New Point(460, 15)
        lblMode.Name = "lblMode"
        lblMode.Size = New Size(70, 25)
        lblMode.TabIndex = 2
        lblMode.Text = "模式選擇:"
        lblMode.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' cmbMode
        ' 
        cmbMode.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMode.Items.AddRange(New Object() {"1-鎖定螢幕"})
        cmbMode.Location = New Point(535, 15)
        cmbMode.Name = "cmbMode"
        cmbMode.Size = New Size(240, 23)
        cmbMode.TabIndex = 3
        ' 
        ' lblInterfaceStatus
        ' 
        lblInterfaceStatus.Font = New Font("Arial", 10F, FontStyle.Bold)
        lblInterfaceStatus.Location = New Point(20, 55)
        lblInterfaceStatus.Name = "lblInterfaceStatus"
        lblInterfaceStatus.Size = New Size(200, 20)
        lblInterfaceStatus.TabIndex = 4
        lblInterfaceStatus.Text = "介面卡狀態"
        ' 
        ' cmbCOMPort
        ' 
        cmbCOMPort.Items.AddRange(New Object() {"COM3"})
        cmbCOMPort.Location = New Point(30, 80)
        cmbCOMPort.Name = "cmbCOMPort"
        cmbCOMPort.Size = New Size(90, 23)
        cmbCOMPort.TabIndex = 5
        ' 
        ' btnOpen
        ' 
        btnOpen.Font = New Font("Arial", 9F)
        btnOpen.Location = New Point(130, 80)
        btnOpen.Name = "btnOpen"
        btnOpen.Size = New Size(65, 25)
        btnOpen.TabIndex = 6
        btnOpen.Text = "Open"
        ' 
        ' btnClose
        ' 
        btnClose.Font = New Font("Arial", 9F)
        btnClose.Location = New Point(205, 80)
        btnClose.Name = "btnClose"
        btnClose.Size = New Size(65, 25)
        btnClose.TabIndex = 7
        btnClose.Text = "Close"
        ' 
        ' lblDeviceStatus
        ' 
        lblDeviceStatus.Font = New Font("Arial", 9F)
        lblDeviceStatus.Location = New Point(20, 120)
        lblDeviceStatus.Name = "lblDeviceStatus"
        lblDeviceStatus.Size = New Size(110, 25)
        lblDeviceStatus.TabIndex = 8
        lblDeviceStatus.Text = "Device Status :"
        lblDeviceStatus.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' txtDeviceStatus
        ' 
        txtDeviceStatus.BackColor = Color.Green
        txtDeviceStatus.Font = New Font("Arial", 9F, FontStyle.Bold)
        txtDeviceStatus.ForeColor = Color.White
        txtDeviceStatus.Location = New Point(130, 120)
        txtDeviceStatus.Name = "txtDeviceStatus"
        txtDeviceStatus.ReadOnly = True
        txtDeviceStatus.Size = New Size(140, 21)
        txtDeviceStatus.TabIndex = 9
        txtDeviceStatus.Text = "Online"
        txtDeviceStatus.TextAlign = HorizontalAlignment.Center
        ' 
        ' btnExit
        ' 
        btnExit.Font = New Font("Arial", 10F, FontStyle.Bold)
        btnExit.Location = New Point(30, 170)
        btnExit.Name = "btnExit"
        btnExit.Size = New Size(240, 35)
        btnExit.TabIndex = 10
        btnExit.Text = "EXIT"
        ' 
        ' lblSystemInfo
        ' 
        lblSystemInfo.Font = New Font("Arial", 10F, FontStyle.Bold)
        lblSystemInfo.Location = New Point(400, 55)
        lblSystemInfo.Name = "lblSystemInfo"
        lblSystemInfo.Size = New Size(200, 20)
        lblSystemInfo.TabIndex = 11
        lblSystemInfo.Text = "系統資訊"
        ' 
        ' lblCPUUsage
        ' 
        lblCPUUsage.Font = New Font("Arial", 9F)
        lblCPUUsage.Location = New Point(400, 80)
        lblCPUUsage.Name = "lblCPUUsage"
        lblCPUUsage.Size = New Size(250, 20)
        lblCPUUsage.TabIndex = 12
        lblCPUUsage.Text = "CPU 使用率: 58%"
        ' 
        ' lblRAMUsage
        ' 
        lblRAMUsage.Font = New Font("Arial", 9F)
        lblRAMUsage.Location = New Point(400, 105)
        lblRAMUsage.Name = "lblRAMUsage"
        lblRAMUsage.Size = New Size(280, 20)
        lblRAMUsage.TabIndex = 13
        lblRAMUsage.Text = "RAM 使用率: 8.3GB / 15.9GB"
        ' 
        ' lblLEDOutput
        ' 
        lblLEDOutput.Font = New Font("Arial", 9F, FontStyle.Bold)
        lblLEDOutput.Location = New Point(400, 140)
        lblLEDOutput.Name = "lblLEDOutput"
        lblLEDOutput.Size = New Size(200, 20)
        lblLEDOutput.TabIndex = 14
        lblLEDOutput.Text = "LED輸出讀寫"
        ' 
        ' txtLEDOutput
        ' 
        txtLEDOutput.BorderStyle = BorderStyle.FixedSingle
        txtLEDOutput.Font = New Font("Arial", 9F)
        txtLEDOutput.Location = New Point(410, 165)
        txtLEDOutput.Multiline = True
        txtLEDOutput.Name = "txtLEDOutput"
        txtLEDOutput.Size = New Size(120, 50)
        txtLEDOutput.TabIndex = 15
        ' 
        ' btnWrite
        ' 
        btnWrite.Font = New Font("Arial", 9F)
        btnWrite.Location = New Point(555, 175)
        btnWrite.Name = "btnWrite"
        btnWrite.Size = New Size(60, 30)
        btnWrite.TabIndex = 16
        btnWrite.Text = "Write"
        ' 
        ' btnRead
        ' 
        btnRead.Font = New Font("Arial", 9F)
        btnRead.Location = New Point(625, 175)
        btnRead.Name = "btnRead"
        btnRead.Size = New Size(60, 30)
        btnRead.TabIndex = 17
        btnRead.Text = "Read"
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(803, 234)
        Controls.Add(lblCurrentTime)
        Controls.Add(btnTimeSync)
        Controls.Add(lblMode)
        Controls.Add(cmbMode)
        Controls.Add(lblInterfaceStatus)
        Controls.Add(cmbCOMPort)
        Controls.Add(btnOpen)
        Controls.Add(btnClose)
        Controls.Add(lblDeviceStatus)
        Controls.Add(txtDeviceStatus)
        Controls.Add(btnExit)
        Controls.Add(lblSystemInfo)
        Controls.Add(lblCPUUsage)
        Controls.Add(lblRAMUsage)
        Controls.Add(lblLEDOutput)
        Controls.Add(txtLEDOutput)
        Controls.Add(btnWrite)
        Controls.Add(btnRead)
        Name = "Form1"
        Text = "113專年工三機械科單生技競賽暨競賽檔案操做系統位置設定: XX"
        ResumeLayout(False)
        PerformLayout()
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
