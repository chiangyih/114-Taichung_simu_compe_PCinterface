# 114-中區模擬技藝競賽 - 電腦修護第二站 PC 端控制程式

## 專案簡介
本專案為 114 學年度工業類科學生技藝競賽 - 電腦修護第二站 PC 端控制程式，使用 VB.NET 與 .NET 8.0 開發，提供與 MCU（微控制器）進行序列通訊的完整功能。

## 技術規格
- **開發框架**: .NET 8.0 Windows Forms
- **程式語言**: Visual Basic .NET
- **通訊協定**: Serial Port (UART, 9600 baud)
- **目標平台**: Windows

## 主要功能

### 1. 序列埠通訊
- 自動掃描並連接藍芽序列埠（優先選擇號碼最小的埠）
- Open/Close 連線控制
- HELLO 初始化握手
- 完整的錯誤處理與超時機制（2 秒）

### 2. 時間同步
- 每秒自動更新系統時間顯示（格式：yyyy/MM/dd HH:mm:ss）
- 連線狀態下每秒自動發送時間至 MCU
- 手動時間同步按鈕

### 3. 系統資訊監控
- **CPU 使用率**: 使用 PerformanceCounter 即時監控
- **RAM 使用量**: 顯示已用/總量（GB）
- 連線狀態下每秒自動傳送至 MCU

### 4. 模式切換
- **模式 1 - 鎖定螢幕**: 發送 M1 命令，MCU 切換至初始狀態
- **模式 2 - 顯示設置**: 發送 M2 命令並自動同步時間
- **模式 3 - 電腦資訊**: 發送 M3 命令並自動傳送系統資訊

### 5. EEPROM 讀寫
- **Read**: 讀取 MCU EEPROM 中的 LED 腳位設定值
- **Write**: 寫入 1-254 範圍內的值至 EEPROM
- 自動驗證輸入合法性
- 寫入成功後顯示確認訊息

### 6. 狀態管理
- 裝置狀態顯示（Online/Offline，綠色/紅色）
- Close 後自動保存當前狀態（模式選擇、LED 值）
- 重新 Open 時自動恢復所有設定
- 根據連線狀態動態啟用/停用控制項

## UI 物件對應說明

### Form1 介面配置

#### 1. 時間與模式控制區（上方）
| 物件名稱 | 類型 | 功能說明 |
|---------|------|---------|
| `lblCurrentTime` | Label | 顯示當前系統時間，每秒自動更新 |
| `btnTimeSync` | Button | 手動觸發時間同步至 MCU |
| `lblMode` | Label | 標籤文字「模式選擇:」 |
| `cmbMode` | ComboBox | 下拉選單，選項：1-鎖定螢幕、2-顯示設置、3-電腦資訊 |

#### 2. 介面卡狀態區（左側）
| 物件名稱 | 類型 | 功能說明 |
|---------|------|---------|
| `lblInterfaceStatus` | Label | 區塊標題「介面卡狀態」 |
| `cmbCOMPort` | ComboBox | COM 埠選擇下拉選單，自動掃描可用埠 |
| `btnOpen` | Button | 開啟序列埠連線，發送 HELLO 握手 |
| `btnClose` | Button | 關閉序列埠連線，保存當前狀態 |
| `lblDeviceStatus` | Label | 標籤文字「Device Status :」 |
| `txtDeviceStatus` | TextBox | 顯示連線狀態（Online=綠色/Offline=紅色），唯讀 |
| `btnExit` | Button | 關閉連線並結束程式 |

#### 3. 系統資訊區（右側）
| 物件名稱 | 類型 | 功能說明 |
|---------|------|---------|
| `lblSystemInfo` | Label | 區塊標題「系統資訊」 |
| `lblCPUUsage` | Label | 顯示 CPU 使用率，格式：CPU 使用率: XX% |
| `lblRAMUsage` | Label | 顯示 RAM 使用量，格式：RAM 使用量: X.XGB / XX.XGB |

#### 4. LED 輸出讀寫區（右下）
| 物件名稱 | 類型 | 功能說明 |
|---------|------|---------|
| `lblLEDOutput` | Label | 區塊標題「LED輸出讀寫」 |
| `txtLEDOutput` | TextBox | 顯示/輸入 LED 腳位值（1-254） |
| `btnRead` | Button | 讀取 MCU EEPROM 的 LED 腳位值 |
| `btnWrite` | Button | 將輸入值寫入 MCU EEPROM（驗證範圍 1-254） |

## 通訊協定

### PC → MCU 命令格式
| 命令 | 格式 | 說明 |
|------|------|------|
| 初始化 | `HELLO` | 連線握手 |
| 時間同步 | `Tyyyy/MM/dd HH:mm:ss` | 傳送系統時間 |
| 系統資訊 | `Scpu=XX;ram=X.X/XX.X` | 傳送 CPU 與 RAM 資訊 |
| 模式切換 1 | `M1` | 切換至鎖定螢幕模式 |
| 模式切換 2 | `M2` | 切換至顯示設置模式 |
| 模式切換 3 | `M3` | 切換至電腦資訊模式 |
| 讀取 EEPROM | `R` | 讀取 LED 腳位值 |
| 寫入 EEPROM | `Lxxx` | 寫入 LED 腳位值（例如：L230） |

### MCU → PC 回應格式
| 回應 | 說明 |
|------|------|
| `OK` | 命令執行成功 |
| `VAL=xxx` | EEPROM 讀取值（例如：VAL=255） |
| `WRITE OK` | EEPROM 寫入成功 |

## 程式架構

### 核心類別與元件
```vb
Public Class Form1
    ' 序列埠通訊
    Private WithEvents serialPort As SerialPort
    
    ' 計時器
    Private WithEvents timerClock As Timer          ' 時鐘更新（1 秒間隔）
    Private WithEvents timerSystemInfo As Timer     ' 系統資訊更新（1 秒間隔）
    
    ' 系統監控
    Private cpuCounter As PerformanceCounter        ' CPU 使用率計數器
    
    ' 狀態管理
    Private responseBuffer As String                ' 接收緩衝區
    Private waitingForResponse As Boolean           ' 等待回應旗標
    Private lastCommand As String                   ' 最後發送的命令
    Private savedModeIndex As Integer               ' 保存的模式索引
    Private savedLEDValue As String                 ' 保存的 LED 值
    Private isFirstRead As Boolean                  ' 首次讀取旗標
End Class
```

### 主要方法說明

#### 初始化方法
- `InitializeSerialPort()`: 設定序列埠參數（9600 baud, 8N1）
- `InitializeTimers()`: 初始化兩個計時器
- `InitializeCPUCounter()`: 初始化 CPU 效能計數器

#### 連線控制方法
- `btnOpen_Click()`: 開啟連線，發送 HELLO，啟動計時器
- `btnClose_Click()`: 關閉連線，保存狀態，停止計時器
- `AutoConnectBluetoothPort()`: 自動掃描並連接藍芽序列埠

#### 資料傳送方法
- `SendCommand(command As String)`: 通用命令發送方法
- `SendTimeSync()`: 發送時間同步資料
- `SendSystemInfo()`: 發送系統資訊（CPU + RAM）

#### 資料接收方法
- `serialPort_DataReceived()`: 序列埠接收事件處理
- `ProcessResponse(response As String)`: 解析並處理 MCU 回應

#### UI 更新方法
- `UpdateCurrentTime()`: 更新時間顯示
- `UpdateSystemInfo()`: 更新 CPU/RAM 顯示
- `UpdateUIState(isConnected As Boolean)`: 根據連線狀態更新所有控制項
- `SetDeviceOnline()` / `SetDeviceOffline()`: 更新裝置狀態顯示

#### 狀態管理方法
- `SaveCurrentState()`: 保存當前模式與 LED 值
- `RestorePreviousState()`: 恢復之前保存的狀態

## 安裝與執行

### 系統需求
- Windows 10/11
- .NET 8.0 Runtime
- 可用的 COM Port（藍芽或 USB 轉序列埠）

### NuGet 套件依賴
```xml
<PackageReference Include="System.IO.Ports" Version="8.0.0" />
<PackageReference Include="System.Management" Version="8.0.0" />
```

### 編譯方式
```bash
# 使用 Visual Studio 2022 開啟專案後直接建置
# 或使用命令列：
dotnet build 114-Taichung_simu_compe_PCinterface.vbproj
```

### 執行方式
1. 確保 MCU 裝置已透過藍芽或 USB 連接至電腦
2. 執行程式，程式會自動掃描並嘗試連接藍芽序列埠
3. 如未自動連接，可手動從下拉選單選擇 COM Port 並按 Open
4. 連線成功後，所有功能自動啟用

## 使用流程

### 標準操作流程
1. **啟動程式** → 自動掃描 COM Port
2. **自動連線** → 程式嘗試連接藍芽序列埠
3. **驗證連線** → 檢查 Device Status 是否顯示 Online（綠色）
4. **選擇模式** → 從下拉選單選擇 1-3 的顯示模式
5. **監控資訊** → 觀察 CPU/RAM 即時資訊
6. **讀寫 EEPROM** → 使用 Read/Write 按鈕管理 LED 設定值
7. **關閉連線** → 按 Close 按鈕（狀態自動保存）
8. **結束程式** → 按 EXIT 按鈕

### 重新連線流程
1. 按 Close 關閉連線（自動保存狀態）
2. 按 Open 重新連線
3. 所有設定自動恢復（模式選擇、LED 值等）
4. 時間同步與系統資訊自動開始傳送

## 錯誤處理

### 已實作的錯誤處理機制
- **COM Port 未選擇**: 彈出警告訊息
- **COM Port 被占用**: 顯示錯誤訊息
- **輸入值不合法**: 驗證數字格式與範圍（1-254）
- **連線逾時**: 2 秒超時設定
- **序列埠異常**: Try-Catch 例外處理
- **跨執行緒 UI 更新**: 使用 Invoke 確保執行緒安全

## 測試建議

### 功能測試檢查清單
- [ ] 程式啟動時自動掃描 COM Port
- [ ] 自動連接藍芽序列埠（號碼最小）
- [ ] Open 按鈕開啟連線，Device Status 變為 Online
- [ ] 時鐘每秒更新顯示
- [ ] 時間同步每秒自動發送至 MCU
- [ ] CPU 使用率正確顯示並即時更新
- [ ] RAM 使用量正確顯示並即時更新
- [ ] 模式切換下拉選單功能正常（M1/M2/M3）
- [ ] 模式 2 切換時自動發送時間同步
- [ ] 模式 3 切換時自動發送系統資訊
- [ ] Read 按鈕讀取 EEPROM 值並顯示
- [ ] Write 按鈕驗證輸入範圍（1-254）
- [ ] Write 成功後顯示 EEPROM 寫入成功訊息
- [ ] Close 按鈕關閉連線，Device Status 變為 Offline
- [ ] Close 後所有控制項正確停用
- [ ] 重新 Open 後自動恢復所有狀態
- [ ] EXIT 按鈕正確關閉連線並結束程式

## 開發者資訊

### 專案結構
```
114-Taichung_simu_compe_PCinterface/
├── 114-Taichung_simu_compe_PCinterface.vbproj  # 專案檔
├── Form1.vb                                    # 主表單程式碼
├── Form1.Designer.vb                           # 表單設計程式碼
├── Form1.resx                                  # 表單資源檔
├── My Project/                                 # VB.NET 專案設定
│   ├── Application.myapp
│   └── Application.Designer.vb
├── PC_FunctionSpec.md                          # 功能規格書
└── README.md                                   # 本檔案
```

### 修改建議
- 如需修改通訊協定，請同步更新 `SendCommand()` 與 `ProcessResponse()` 方法
- 如需調整 Timer 間隔，修改 `InitializeTimers()` 中的 Interval 值
- 如需更換序列埠設定，修改 `InitializeSerialPort()` 中的參數

## 授權與版權
本專案為 114 學年度工業類科學生技藝競賽競賽用程式，僅供教學與競賽使用。

## 參考文件
- [PC_FunctionSpec.md](PC_FunctionSpec.md) - 完整功能規格書
- [.NET Serial Port 文件](https://learn.microsoft.com/zh-tw/dotnet/api/system.io.ports.serialport)
- [PerformanceCounter 文件](https://learn.microsoft.com/zh-tw/dotnet/api/system.diagnostics.performancecounter)

---

**最後更新日期**: 2025-01-XX  
**專案版本**: v1.0  
**開發框架**: .NET 8.0