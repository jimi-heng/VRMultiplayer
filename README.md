# VRMultiplayer

基于 Unity 6 的多人 VR 地学交互学习项目，包含火山、地震两个主题场景。参与者可以在共享空间中观察模型、操作教学装置，并记录文字。

## 主要内容

- **火山学习**：火山形态、内部结构与岩石展示，配有多人触发的模型渐变切换。
- **地震交互**：包含地震波强度控制、天平称量和幕布展示等交互组件。
- **共享操作与记录**：包含玩家头部与双手姿态映射、视频播放/暂停广播、房间内位置切换，以及文字输入和本地 CSV 保存。CSV = 逗号分隔值文本格式。

## 开发环境

使用 **Unity Editor 6000.0.37f1** 打开项目。主要依赖版本来自 [Packages/manifest.json](Packages/manifest.json)，完整解析结果见 [Packages/packages-lock.json](Packages/packages-lock.json)。

| 组件 | 版本 |
| --- | --- |
| OpenXR = 跨设备 XR 接口 | 1.14.0 |
| XR Interaction Toolkit = VR 交互组件集 | 3.0.7 |
| XR Hands = 手部追踪数据接口 | 1.5.0 |
| Netcode for GameObjects = Unity 网络对象同步框架 | 2.4.3 |
| Unity Transport = 网络传输层 | 2.4.0 |
| Multiplayer Services = 多人服务依赖包 | 1.1.4 |
| Authentication = 玩家身份认证服务 | 3.5.1 |
| Vivox = 实时语音与文字通信服务 | 16.5.4 |
| Universal Render Pipeline = 通用渲染管线 | 17.0.4 |

项目已为 Standalone 和 Android 配置 OpenXR；Android 配置启用了 Meta Quest 功能。连接头显时，需要对应设备的 OpenXR 运行环境。

## 快速开始

**克隆项目 → 用指定 Unity 版本导入 → 配置云服务 → 打开场景运行。**

1. 克隆仓库，在 Unity Hub 中添加仓库根目录，并使用 **6000.0.37f1** 打开，等待资源导入与依赖解析完成。

   ```bash
   git clone https://github.com/jimi-heng/VRMultiplayer.git
   cd VRMultiplayer
   ```

2. 将项目关联到你有访问权限的 Unity 云项目，配置 **Unity Gaming Services = Unity 云端游戏服务（简称 UGS）**。主要场景的联网入口使用匿名身份认证和 **Relay = 云端连接转发服务**；运行多人功能前，确认对应服务已配置。如果使用仓库中的大厅或 Vivox 功能，再配置相应服务。

3. 打开 [Assets/Scenes/Volcano.unity](Assets/Scenes/Volcano.unity)，连接头显后进入 Play Mode。查看地震教学内容时，打开 [Earthquake.unity](Assets/Scenes/Earthquake.unity)。

无头显调试可手动配置仓库附带的 [XR Device Simulator 预制体](Assets/Samples/XR%20Interaction%20Toolkit/3.0.7/XR%20Device%20Simulator/XR%20Device%20Simulator.prefab)。XR Device Simulator = 用键鼠模拟头显和控制器的编辑器工具；[当前设置](Assets/XRI/Settings/Resources/XRDeviceSimulatorSettings.asset)未启用自动实例化。

## 多人连接

**主机 CREATE → 获取 Relay 连接码 → 客户端填写 joinCode → JOIN。**

两个教学场景中的按钮均绑定到 [NetworkConnect.cs](Assets/Scripts/NetworkConnect.cs)，使用 Relay 创建或加入连接：

1. 在主机实例中等待 UGS 初始化完成，点击 **CREATE**。程序创建 Relay 分配并启动主机，连接码会出现在 Unity Console 中；当前脚本通过 `Debug.LogError(newJoinCode)` 输出该码。
2. 在另一台开发机的编辑器中打开同一场景，进入 Play Mode 并等待初始化完成。在客户端运行中的 `NetworkManager` 对象的 `NetworkConnect` 组件中，将 `joinCode` 字段设为主机刚生成的连接码。
3. 在客户端点击 **JOIN**，加入主机的网络会话。

当前 JOIN 直接读取 Inspector 中的 `joinCode`，场景内的文字输入框用于文字记录，未连接到该字段。独立客户端构建若需要在运行时输入连接码，需要先为 `NetworkConnect.joinCode` 接入输入界面。

## 场景与构建入口

| 场景 | 用途 | 当前构建列表 |
| --- | --- | --- |
| [Volcano.unity](Assets/Scenes/Volcano.unity) | 火山主题展示与模型切换 | 启用 |
| [Earthquake.unity](Assets/Scenes/Earthquake.unity) | 地震主题展示与交互装置 | 启用 |

构建顺序记录在 [EditorBuildSettings.asset](ProjectSettings/EditorBuildSettings.asset)。若希望构建后直接进入火山或地震场景，在 Unity 的构建场景列表中将目标场景移到首位。`RoomTransport` 负责同一场景内的位置切换。

## 核心代码

自定义交互集中在 [Assets/Scripts](Assets/Scripts)，多人基础组件集中在 [Assets/VRMPAssets/Scripts](Assets/VRMPAssets/Scripts)。

| 文件 | 职责 |
| --- | --- |
| [NetworkConnect.cs](Assets/Scripts/NetworkConnect.cs) | 匿名认证、Relay 分配、主机创建与客户端加入 |
| [NetworkPlayer.cs](Assets/Scripts/NetworkPlayer.cs) | 将本地 VR 设备姿态映射到所属玩家的根节点、头部和双手 |
| [NetworkedModelFader.cs](Assets/Scripts/NetworkedModelFader.cs) | 经主机广播模型渐隐与渐显 |
| [WaveButtonTrigger.cs](Assets/Scripts/WaveButtonTrigger.cs) | 广播三档地震波强度与停止指令 |
| [NetworkedBalance.cs](Assets/Scripts/NetworkedBalance.cs) | 服务端计算托盘质量差并同步天平角度 |
| [Floating.cs](Assets/Scripts/Floating.cs) | 浮力处理及网络位置、旋转更新 |
| [PlayVideo.cs](Assets/Scripts/PlayVideo.cs) | 广播视频播放、暂停与对应播放时间 |
| [RoomTransport.cs](Assets/Scripts/RoomTransport.cs) | 根据头显与原点的偏移切换房间位置 |
| [SaveToCSV.cs](Assets/Scripts/SaveToCSV.cs) | 将输入框文字逐行追加到本地文件 |

`SaveToCSV` 默认保存到 `Application.persistentDataPath/output.csv`，实际路径会在启动时打印到 Console。文字记录由当前实例本地保存。
