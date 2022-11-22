# **“微软工业元宇宙”动手实验指导（v1.0）**

## **实验目标**

本实验旨在让使用者了解 “微软元宇宙技术栈“中的主要技术和工具，以及Web3.0的概念，并结合一个能源行业的场景，给使用者初步的元宇宙应用体验。


## **实验贡献者**
本动手实验基于 [Build mixed reality digital twins with Azure Digital Twins and Unity - Training | Microsoft Learn](https://learn.microsoft.com/en-us/training/paths/build-mixed-reality-azure-digital-twins-unity/) 在线课程，并由微软中国的多位专家 Alex Zhao, Yingguang Mei, Wayne Wang 和 Warren Zhou 修改和完善而来。

---

## **实验二：将Unity中的3D场景连接到IoT服务，并发布到MR设备**

### **实验环境与准备**

- HoloLens的开发没有单独的SDK，使用Visual Studio 和 Windows 10 SDK即可。
  - 下载并且安装 [Visual Studio 2022 社区版本](https://visualstudio.microsoft.com/zh-hans/downloads/)。请确保安装以下模块：**.NET 桌面开发、使用 C++ 的桌面开发、通用 Windows 平台 (UWP) 开发**
      ![](/pics/lab2-01.png)
  - 勾选 “**Game development with C++**”模块，检查一下右侧是否已经勾选了最新的 Windows 10 SDK
      ![](/pics/lab2-02.png)
  - 勾选 “**Game Development with Unity**”模块，确保右侧没有勾选“**Unity Hub**”
      ![](/pics/lab2-03.png)
- Unity 是市面上领先的实时开发引擎之一，它支持多种硬件平台的开发。本实验我们使用Unity来开发HoloLens应用 
  - 安装 [**Unity Hub**](https://unity3d.com/get-unity/download).
  - 打开 Unity Hub，选择 **Installs** 标签，然后选择 **Install Editor**
       ![](/pics/lab2-04.png)
  - 选择最新的 **Unity 2020.3** 版本（边上有 LTS 标记）然后点击 **Install** 按钮
       ![](/pics/lab2-05.png)
  - 选择 **“Universal Windows Platform Build Support”** 和 **“Windows Build Support(IL2CPP)”** 平台，然后点击 **Install** 按钮开始安装
       ![](/pics/lab2-06.png)
  - 安装完成后，回到Unity Hub的主界面，在 Installs 标签下会出现安装好的Unity Editor版本，请确保有 “UWP” 和 “Windows” 二个标记
      ![](/pics/lab2-07.png)
- **(可选)** 安装HoloLens仿真器，可在没有 HoloLens 的情况下在 HoloLens 虚拟机映像上运行应用程序
  - 下载仿真器的安装文件到PC并且安装：https://go.microsoft.com/fwlink/?linkid=2211620

### **实验流程**
1. [创建 Bing Map 的账号](https://learn.microsoft.com/en-us/bingmaps/getting-started/bing-maps-dev-center-help/creating-a-bing-maps-account)，[获取 Bing Map 的开发者 Key](https://learn.microsoft.com/en-us/bingmaps/getting-started/bing-maps-dev-center-help/getting-a-bing-maps-key)
2. 下载 Unity 项目课件：(https://github.com/alezhao/microsoft-devhack/tree/main/code)
3. 打开 Unity 工程
  - 在PC的根目录下建立一个目录，把课件全部解压到该目录（保证项目目录的层次越少越好）；本次实验会用到二个工程：**Unity-Project** 和 **Device-SImulator**
  - 打开 Unity Hub，选择 **Projects** 标签下的 **Open** 按钮
       ![](/pics/lab2-20.png)
    选择 **Unity-Project** 目录并打开这个工程
       ![](/pics/lab2-21.png)
    - 如果出现项目和 Unity 版本不统一的提示，可以把项目切换为已安装的 Unity 版本
    ![](/pics/lab2-22.png)
    - 切换版本的时候，出现警告框只需要选择 **Continue** 即可
         ![](/pics/lab2-23.png)
4. 配置 Unity 编辑器
  - 认识 Unity 编辑器的各个功能区窗口，可按自己的喜好改变功能区的分布
       ![](/pics/lab2-24.png)
       
  - 打开 **Edit/Preference** 菜单，进入 **Externals Tool**，设置外部脚本编辑器为已经安装的 **Visual Studio 2022 社区版**
       ![](/pics/lab2-25.png)
       
  - 打开 **File/Build Setting** 菜单，在 Build Settings 窗口选择 **Universal Windows Platform**
       ![](/pics/lab2-26.png)
    选择 **Switch Platform** 按钮切换至该平台。
    ![](/pics/lab2-27.png)    
    如下系统默认的选项保持不变：
    | 选项                | 数值          |
    | ------------------- | ------------- |
    | Target Device       | Any device    |
    | Architecture        | x64           |
    | Build Type          | D3D Project   |
    | Build and Run On    | Local Machine |
    | Build configuration | Release       | 

  - 在 Build Settings 窗口选择 **Player Settings**；打开 Player Settings 窗口后，选择最下端的 **XR-Plugin Management**，在 Universal Windows Platform 的 Tab下，选中 **OpenXR** （Microsoft HoloLens Feature Group 会默认选中）
    ![](/pics/lab2-28.png)
5. 配置 Unity 工程
  - 在 Project 窗口进入 Assets/Scenes, 双击 **Main Scene** 打开这个场景
    ![](/pics/lab2-29.png)
    这时，在 Hierarchy 窗口，可以看到已经加载了一些场景元素（Unity 称之为 “Game Object”）
    ![](/pics/lab2-30.png)
  -  在 Project 窗口进入 Assets/Art/Prefabs, 把 **Bing Maps Operate** 蓝色小方块 （Unity 称之为 “Prefab”）用鼠标拖拽到 Hierarchy 窗口下的 Main Scene 结点下
    ![](/pics/lab2-31.png)
    操作完成后，在 Hierarchy 窗口下的布局如下图所示：
    ![](/pics/lab2-32.png)
    在 Scene 窗口，你可以看到有风车、地图底座等3D模型被加载进来了；（**可选**：你可以在 Gizmos 下拉菜单里面拖动 3D Icons 边上的横条来调整 3D 图标的大小）
    ![](/pics/lab2-33.png)
  - 在 Hierarchy 窗口，选中 Bing Maps Operate 这个 Game Object，这时在 Inspector 窗口就会出现它的属性，在 **Developer Key** 的地方输入之前获得的 **Bing Map Key**
    ![](/pics/lab2-34.png)
  - 使用 Ctrl-S 组合键，保存场景。这个时候，Hierarchy 窗口里 MainScene 右侧的星号*消失了，代表场景已经被保存
  - **可选**：此时可以点击 Unity 编辑器上方的 **Play** 按钮，运行测试一下这个工程。在 Game 窗口里面，正常情况下已经可以看到地图和转动的风车都被正常加载出来了
    ![](/pics/lab2-35.png)
- 在 Project 窗口进入 Assets/UIPrefabs/Prefabs, 把 **OperateSceneUI** 用鼠标拖拽到 Hierarchy 窗口下的 Main Scene 结点下 

    ![](/pics/lab2-36.png)
- 在 Project 窗口进入 Assets/ADTPrefabs, 把 **ADTConnection** 和 **ADTTubineAlertController** 二个 prefab 用鼠标拖拽到 Hierarchy 窗口下的 Main Scene 结点下 

    ![](/pics/lab2-37.png)
- 完成后 Hierarchy 窗口下的场景布局如下图所示，记得使用 Ctrl+S 保存场景
    ![](/pics/lab2-38.png)
6. 在 Unity 工程里设置 ADT 参数
- 打开之前生成的 Azure_config_settings.txt 文件中，找到 **signalRNegotiatePath** 后面的字符串，拷贝这个字符串；

    **注意：** 只需要拷贝 “**/negotiate**” 之前的字符串；比如下面的 signalRNegotiatePath

    ```json
      "signalRNegotiatePath": "https://myprojfuncappxxxxxxxxxx.azurewebsites.net/api/negotiate"
    ```

    只需要拷贝 `https://myprojfuncappxxxxxxxxxx.azurewebsites.net/api`

- 在 Hierarchy 窗口，选中 ADTConnection，在 Inspector 窗口把上述字符串粘贴到 **url** 中；保存场景
    ![](/pics/lab2-39.png)
    
- 在 Project 窗口选中 Assets/ScriptableObjects/AzureDigitalTwin/Credentials；然后选择菜单 Assets/Create/Scriptable Objects/Credentials/ADT Rest API Credentials
    ![](/pics/lab2-40.png)
  在目录下会生成 **ADTRestAPICredentials** 文件
    ![](/pics/lab2-41.png)
  
- 在 Project 窗口选中刚才生成的 ADTRestAPICredentials 文件，在 Inspector 窗口中可以看到有4个参数需要输入
    ![](/pics/lab2-42.png)
  
  打开之前生成的 Azure_config_settings.txt 和 AppCredentials.txt 文件，按如下表格依次填入这4个参数

    | 文件名                    | 文件中的参数名 | Unity 中的参数名 |
    | ------------------------- | -------------- | ---------------- |
    | Azure_config_settings.txt | adtHostName    | Adt Instance URL |
    | AppCredentials.txt        | appId          | Client Id        |
    | AppCredentials.txt        | password       | Client Secret    |
    | AppCredentials.txt        | tenant         | Tenant Id        |
- 在 Hierarchy 窗口，选中 ADTTurbineAlertController 这个 Game Object, 确保在 Inspector窗口里看到 **ADT Turbine Alert Controller (Script)** 这个属性；把刚才的 ADTRestAPICredentials 文件从 Project 窗口拖拽到 ADT Turbine Alert Controller (Script) 下面的 **Adt Connection Info** 项右侧的空格里面
    ![](/pics/lab2-43.png)
  成功后，界面如下所示: 

    ![](/pics/lab2-44.png)
- Ctrl+S 保存场景；Unity 工程部分已经全部配置完成！

  **提示：** 
    - 如果在配置过程中遇到困难无法完成，可以在 Project 窗口进入 Assets/Scenes, 双击 **MainSceneFinished** 打开这个场景。这个是完成的本实验场景文件，只需要添加绑定 ADT 数据即可！
    - 目录下还有另外一个场景文件 **CompletedScene**，这个是包含实验三全部内容的最终场景
7. 启动风车模拟数据
- 用 Visual Studio 2022 社区版，打开风车模拟器工程 “**DeviceSimulator**”
  ![](/pics/lab2-50.png)
  
- 打开 "AzureIoTHub.cs" 文件，我们需要指定此处的 **iotHubConnectionString** 和 **adtInstanceUrl** 2个参数
  ```csharp
      public static class AzureIoTHub
      {
          /// <summary>
          /// Please replace with correct connection string value
          /// The connection string could be got from Azure IoT Hub -> Shared access policies -> iothubowner -> Connection String:
          /// </summary>
          private const string iotHubConnectionString = ""; //Need data
          private const string adtInstanceUrl = ""; //Need data
          private const string alertTurbineId = "T102";
          private const string alertVariableName = "Alert";
      ...
  ```

  此处的对应关系如下：

  | 文件名                      | 文件中的参数名 | 项目中的参数名 |
  | ------------------------- | -------------- | ---------------- |
  | Azure_config_settings.txt | connectionString    | iotHubConnectionString |
  | Azure_config_settings.txt        | adtHostName          | adtInstanceUrl        |
  
- 打开 "PropUpdater.cs"文件，我们需要指定此处的 **clientId** 、**clientSecret** 和 **tenantId** 3个参数

  ```csharp
      public class PropUpdater
      {
          private string accessToken;
          private readonly Uri adtInstanceUri;
          private readonly string azureLoginUrl = "https://login.microsoftonline.com/{0}/oauth2/token"; //{0}: tenantId
  
          private string clientId = ""; //Need data
          private string clientSecret = ""; //Need data
          private string tenantId = ""; //Need data
       ...
  ```
  此处的对应关系如下：

  | 文件名             | 文件中的参数名 | 项目中的参数名 |
  | ------------------ | -------------- | -------------- |
  | AppCredentials.txt | appId          | clientId       |
  | AppCredentials.txt | password       | clientSecret   |
  | AppCredentials.txt | tenant         | tenantId       |
- 在 Visual Studio 里面按 F5，启动模拟器
- 在命令行窗口，按任意键开始模拟
  ![](/pics/lab2-51.png)
- 模拟器保持开启状态，返回 Unity
  ![](/pics/lab2-52.png)
8. 在 Unity 编辑器里测试
  - 点击 Unity 编辑器上方的 Play 按钮，在 Game 窗口里面体验这个项目的运行结果
  - 参考 [MRTK 输入模拟](https://learn.microsoft.com/zh-cn/windows/mixed-reality/mrtk-unity/mrtk2/features/input-simulation/input-simulation-service?view=mrtkunity-2022-05#how-to-use-mrtk-input-simulation) 在 Game 窗口里面进行交互操作
    ![](/pics/lab2-60.png) 
9. （**可选**）输出应用并且部署到 HoloLens 上
  - 在 Unity 编辑器里，选择 **File/Build Setting** 菜单，打开 Build Settings 窗口
  - 确保当前场景 **MainScene** 是选中状态；点击 **Build** 按钮
    ![](/pics/lab2-70.png) 
  - 在弹出的文件浏览对话框中，鼠标右键创建一个新的目录夹 **App** （与 Assets 目录是同级目录）；选中这个目录夹后点击 **Select Folder** 按钮；Unity开始构建项目
    ![](/pics/lab2-71.png) 
  - Unity 构建完成后，进入这个目录夹；此时这个目录夹内已经生成了一个 Win10 的 UWP 项目。双击 solution 文件，在 Visual Studio 2022 社区版中打开这个项目 
  
    ![](/pics/lab2-72.png)  
  - 在  Visual Studio 里选择菜单 **Project/Publish/Create App Package**，在 Package 的设置选项里（如下图），选择 **ARM64（Release(ARM64))**, 按 **Create** 按钮开始生成安装包
    ![](/pics/lab2-73.png)  
  - 安装包（appx后缀的文件）完成后，打开 HoloLens 的[设备门户](https://learn.microsoft.com/zh-cn/windows/mixed-reality/develop/advanced-concepts/using-the-windows-device-portal)，[旁加载这个安装包](https://learn.microsoft.com/zh-cn/windows/mixed-reality/develop/advanced-concepts/using-the-windows-device-portal#sideloading-applications)即可
  - 如果想使用仿真器，在 Visual Studio 里需要把平台设置为 **x64**，选择用 **Emulator** 启动项目就可以了
    ![](/pics/lab2-74.png)  

至此，您已经完成了实验二的所有步骤。将Unity中的3D场景连接到IoT服务，并发布到MR设备，等待您进入后续的实验了。
