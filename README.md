# VRMultiplayer

基于 Unity 6 的多人 VR 地学交互学习项目，围绕火山与地震两个主题组织教学场景。参与者可以在共享空间中观察地学模型、操作实验装置，并记录学习内容。

## 场景与功能

| 主题 | 主要内容 | 场景文件 |
| --- | --- | --- |
| 火山 | 火山形态、内部结构与岩石展示，以及多人触发的模型渐变切换 | [Volcano.unity](Assets/Scenes/Volcano.unity) |
| 地震 | 地震波强度控制、天平称量与幕布展示 | [Earthquake.unity](Assets/Scenes/Earthquake.unity) |

两个场景均提供多人连接、房间内位置切换和文字记录。记录保存在当前设备的 `Application.persistentDataPath/output.csv`，实际路径会打印到 Unity Console。CSV = 逗号分隔值文本格式。

## 运行方式

**导入项目 → 配置云服务 → 打开教学场景 → 创建或加入多人会话。**

1. 克隆仓库，在 Unity Hub 中添加仓库根目录，并使用 **Unity Editor 6000.0.37f1** 打开，等待资源导入与依赖解析完成。

   ```bash
   git clone https://github.com/jimi-heng/VRMultiplayer.git
   cd VRMultiplayer
   ```

2. 将项目关联到你有访问权限的 Unity 云项目，配置 **Unity Gaming Services = Unity 云端游戏服务（简称 UGS）**。多人连接使用 **Authentication = 玩家身份认证服务**进行匿名登录，并通过 **Relay = 云端连接转发服务**建立连接。

3. 打开 `Volcano.unity` 或 `Earthquake.unity`，连接头显后进入 Play Mode。

构建时，在 Unity 的构建场景列表中将目标教学场景移到首位。现有配置见 [EditorBuildSettings.asset](ProjectSettings/EditorBuildSettings.asset)。

### 多人连接

**主机 CREATE → 获取连接码 → 客户端填写 joinCode → JOIN。**

以下为编辑器中的连接流程，主机与客户端需打开同一教学场景。Inspector = Unity 编辑器中查看和修改组件参数的面板。

1. 主机进入 Play Mode，等待 UGS 初始化完成后点击 **CREATE**，从 Console 复制生成的 Relay 连接码。
2. 在另一台开发机的编辑器中运行同一场景，等待初始化完成后，在 `NetworkManager` 对象的 `NetworkConnect` 组件中填写 `joinCode`。
3. 客户端点击 **JOIN**，加入主机的网络会话。

JOIN 读取 `NetworkConnect.joinCode`。若需在独立构建中输入连接码，需要将输入界面绑定到该字段。

<details>
<summary>依赖版本与无头显调试</summary>

OpenXR = 跨设备 XR 接口。项目已为 Standalone 和 Android 配置 OpenXR，Android 配置启用了 Meta Quest 功能；运行时需使用对应设备的 OpenXR 环境。

主要依赖版本来自 [manifest.json](Packages/manifest.json)，完整解析结果见 [packages-lock.json](Packages/packages-lock.json)。

| 组件 | 版本 |
| --- | --- |
| OpenXR | 1.14.0 |
| XR Interaction Toolkit = VR 交互组件集 | 3.0.7 |
| XR Hands = 手部追踪数据接口 | 1.5.0 |
| Netcode for GameObjects = Unity 网络对象同步框架 | 2.4.3 |
| Unity Transport = 网络传输层 | 2.4.0 |
| Multiplayer Services = 多人服务依赖包 | 1.1.4 |
| Authentication | 3.5.1 |
| Universal Render Pipeline = 通用渲染管线 | 17.0.4 |

XR Device Simulator = 用键鼠模拟头显和控制器的编辑器工具。无头显调试时，可手动配置仓库附带的 [XR Device Simulator 预制体](Assets/Samples/XR%20Interaction%20Toolkit/3.0.7/XR%20Device%20Simulator/XR%20Device%20Simulator.prefab)；[当前设置](Assets/XRI/Settings/Resources/XRDeviceSimulatorSettings.asset)未启用自动实例化。

</details>

## 代码导航

自定义功能集中在 [Assets/Scripts](Assets/Scripts)，可按功能查找入口：

| 功能 | 入口脚本 |
| --- | --- |
| 多人连接与玩家姿态 | [NetworkConnect.cs](Assets/Scripts/NetworkConnect.cs)、[NetworkPlayer.cs](Assets/Scripts/NetworkPlayer.cs) |
| 模型切换与视频播放 | [NetworkedModelFader.cs](Assets/Scripts/NetworkedModelFader.cs)、[PlayVideo.cs](Assets/Scripts/PlayVideo.cs) |
| 地震波与展示装置 | [WaveButtonTrigger.cs](Assets/Scripts/WaveButtonTrigger.cs)、[ShowCurtain.cs](Assets/Scripts/ShowCurtain.cs) |
| 天平与浮力交互 | [NetworkedBalance.cs](Assets/Scripts/NetworkedBalance.cs)、[Floating.cs](Assets/Scripts/Floating.cs) |
| 场景内位置切换 | [RoomTransport.cs](Assets/Scripts/RoomTransport.cs) |
| 文字记录与保存 | [SaveToCSV.cs](Assets/Scripts/SaveToCSV.cs) |
