# **“微软工业元宇宙”动手实验指导（v1.0）**

## **实验目标**

本实验旨在让使用者了解 “微软元宇宙技术栈“中的主要技术和工具，以及Web3.0的概念，并结合一个能源行业的场景，给使用者初步的元宇宙应用体验。

## **实验贡献者**

本动手实验基于 [Build mixed reality digital twins with Azure Digital Twins and Unity - Training | Microsoft Learn](https://learn.microsoft.com/en-us/training/paths/build-mixed-reality-azure-digital-twins-unity/) 在线课程，并由微软中国的多位专家 Alex Zhao, Yingguang Mei, Wayne Wang 和 Warren Zhou 修改和完善而来。

---

## **实验一：创建、验证一个DTDL模型，并发布到Azure Digital Twins**
在实验一里，您将会使用Digital Twin Definition Language(DTDL)创建一个简单的风力发电机模型，验证它的语法正确性，并在Azure Digital Twins服务中创建它的数字孪生实例。您还将通过Azure Resource Manager（ARM）模板，批量地部署其他的 Azure 基础服务，用于后续的实验步骤。

### **实验环境与准备**
- 虽然使用任何一种文本编辑器都可以编写DTDL模型文件，但我们推荐先安装 [Visual Studio Code](https://code.visualstudio.com/Download)，再安装和使用它的扩展插件 [DTDL Editor for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=vsciot-vscode.vscode-dtdl) 来编写DTDL模型。后者可以使用预定义的模板和代码片段，以及语法检查，帮助您更高效地编写DTDL文件。
- **（可选）** 如果您不是在带有DTDL语法检查功能的环境下编辑的模型文件，在导入到Azure Digital Twins服务之前，我们建议使用 [DTDL Validator](https://github.com/Azure-Samples/DTDL-Validator) 这个开源工具来验证一下您的模型定义是否符合DTDL的语法。编译和运行该工具需要先安装 [.Net SDK 6.0 或 .Net Core 3.1](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) 。
- 您需要一个国际版的Azure订阅来创建Azure Digital Twins服务。如果您还没有，请在 [这里](https://azure.microsoft.com/en-us/offers/ms-azr-0044p/) 免费申请一个试用账号。
- 您需要 [Azure CLI](https://learn.microsoft.com/zh-cn/cli/azure/install-azure-cli) 以在 Windows PowerShell 中运行命令行命令执行跟 Azure 有关的操作。无论您的操作系统是 Windows、Linux 或是 macOS，您都可以选择相应的平台安装。

### **实验流程**
1. 打开Visual Studio Code。在”View”菜单中选择”Command Palette…”菜单项。

    ![Command Palette](/pics/lab1-palette.png "Lab 1 - Step 1")

2. 在命令行中输入`dtdl`，并在候选项中选择”DTDL: Create Interface”。

    ![dtdl create](/pics/lab1-create.png "Lab 1 - Step 2")

3. 依照向导，依次输入Interface Name，本例中为"ADT_Turbine"；如果您没指定过模型文件夹，需要指定一个文件夹用以存放编写的模型；一个根据basic template创建出来的默认模型如下图。这其中，`@context`代表了使用的是DTDL v2的语法，`@id`是根据您输入的 Interface Name 自动创建的唯一的模型名称。
   
    ![basic model](/pics/lab1-basicmodel.png "Lab 1 - Step 3")

4. 模型可以包含 Property, Telemetry 和 Command 三种元素。本例中只会使用到前两种。请您修改contents部分，使得这个模型具有：
    - 一个名叫TurbineID的Property，类型为string，值可修改；
    - 一个名叫Alert的Property, 类型为boolean，值可修改；
    - 一个名叫TimeInterval的Telemetry，类型为string；
    - 一个名叫Description的Telemetry，类型为string；
    - 一个名叫Code的Telemetry，类型为integer；
    - 一个名叫WindSpeed的Telemetry，类型为double；
    - 一个名叫Ambient的Telemetry，类型为double;
    - 一个名叫Rotor的Telemetry，类型为double;
    - 一个名叫Power的Telemetry，类型为double。

    您可以键入`dtp`和`dtt`来导入Property和Telemetry的代码片段，帮助您快速编写这些元素。可以看到DTDL Editor这个插件具有语法校验功能，不符合DTDL语法的会以红色提示。
    
    ![code snippet](/pics/lab1-snippet.png "Lab 1 - Step 4")

5. 最终您的DTDL模型应该是如下这样。保存并退出。
```json
{
  "@context": "dtmi:dtdl:context;2",
  "@id": "dtmi:com:example:ADT_Turbine;1",
  "@type": "Interface",
  "displayName": "ADT Wind Turbine -v1",
  "description": "ADT Wind Turbine data",
  "contents": [
    {
      "@type": "Property",
      "name": "TurbineID",
      "displayName": "TurbineID",
      "description": "The unique id of the turbine",
      "writable": true,
      "schema": "string"
    },
    {
      "@type": "Property",
      "name": "Alert",
      "displayName": "Alert",
      "description": "whether or not this turbine needs maintenance",
      "writable": true,
      "schema": "boolean"
    },
    {
      "@type": "Telemetry",
      "name": "TimeInterval",
      "description":  "The time interval of this data",
      "schema": "string"
    },
    {
      "@type": "Telemetry",
      "name": "Description",
      "schema": "string"
    },
    {
      "@type": "Telemetry",
      "name": "Code",
      "schema": "integer"
    },
    {
      "@type": "Telemetry",
      "name": "WindSpeed",
      "schema": "double"
    },
    {
      "@type": "Telemetry",
      "name": "Ambient",
      "schema": "double"
    },
    {
      "@type": "Telemetry",
      "name": "Rotor",
      "schema": "double"
    },
    {
      "@type": "Telemetry",
      "name": "Power",
      "schema": "double"
    }
  ]
}
```

6. **（可选）** 如果您是在一个不带有语法检查的编辑环境下编写的DTDL模型的json文件，在导入到云端服务并创建数字孪生体之前，需要先对它验证是否符合DTDL的语法。一个简单的方法是，使用.Net Core编译一个使用了“Azure Digital Twins Parser”的开源工具DTDL Validator，并用命令行验证我们的模型文件的正确性。请参考“实验环境与准备”安装.Net Core，并从Github获得DTDL Validator的工程源码。

7. **（可选）** 请使用如下语句，在DTDL Validator项目的根目录生成命令行工具：
    ```console
    dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true
    ```
    ![compile validator](/pics/lab1-compile.png "Lab 1 - Step 7")

8. **（可选）** 请在Release目录之下，找到对应的DTDLValidator.exe所在的目录，并在命令行下用 `-d` 参数指定DTDL模型所在的目录（本例中是"d:\models\"）进行语法验证。下图展示了模型文件成功通过语法验证。
![validate model](/pics/lab1-validate.png "Lab 1 - Step 8")

9. 接下来在Azure上创建数字孪生服务。登录Azure门户，选择“创建资源”，在搜索框中输入`Azure Digital Twins`并点击候选项；或者也可以在“类别”-“物联网”下，找到“Azure Digital Twins”并点击“创建” 。
![search ADT](/pics/lab1-searchadt.png "Lab 1 - Step 9")

10. 在创建Azure数字孪生向导页面，填入各项信息并创建一个服务实例：
    - 如果有多个活跃的订阅，选择一个合适的**订阅**。
    - 新建，或者选择一个已有的**资源组**。新建的资源组便于您组织本实验的相关资源，并在实验结束后删除资源。本实验中为“RG-Metaverse” . 
    - **资源名称**，也就是创建的Azure数字孪生服务实例的名称。本实验中为“ADT-Turbine” .
    - **区域**，建议选择“East US”，这是本实验所需各种Azure服务都可用的区域之一。
    - 勾选下面的**分配“Azure 数字孪生数据所有者”角色**。
    
    
    ![create ADT](/pics/lab1-createadt.png "Lab 1 - Step 10")  
    
11.  创建完成后，转到创建的资源的“概述”页面，点击上方的 **“打开Azure Digital Twins Explorer（预览）”** 。
![ADT overview](/pics/lab1-adtoverview.png "Lab 1 - Step 11")

12. 在新打开的“Azure Digital Twins Explorer”窗口，点击左侧的 **“Upload a model”** 按钮，选择之前的实验步骤所创建的ADT_Turbine.json模型文件上传。点击中间的 **“MODEL GRAPH”**，可以看见已经创建了一个模型图。展开右侧的 **“MODEL DETAIL”**，可以看到模型的定义，一如我们之前所编写的。
![ADT explorer](/pics/lab1-adtexplorer.png "Lab 1 - Step 12")

13. 点击中间的 **“TWIN GRAPH”**，然后在左侧模型的“...”菜单中选择 **“Create a Twin”**，在对话框中输入数字孪生体的名称，例如本实验的“Turbine1”。点击中间画布区域的孪生体，并展开右侧的 **“TWIN PROPERTIES”**，可以看到之前模型定义时的两个属性TurbineID和Alert可以被赋值。
![ADT twins](/pics/lab1-adttwins.png "Lab 1 - Step 13")

14. 本实验中定义的DTDL模型并不复杂，仅仅是一个单本体的场景。您也可以参考和采用更多的 [工业场景的本体](https://learn.microsoft.com/en-us/azure/digital-twins/concepts-ontologies-adopt) 作为您创建更复杂的模型的起点。
    

您已经了解了如何通过DTDL语言编写对象的模型、在Azure数字孪生服务中导入模型并创建数字孪生体实例。但要让这个数字孪生实例能真正映射物理世界对象的遥测数据，还需要诸如 IoT Hub、SignalR、Event Grid、Function app 等多个Azure基础服务共同作用。为了节约时间，我们提供了一个ARM模板，帮助您批量化的部署它们以及Azure数字孪生服务。在开始部署之前，您可以删除您刚才创建的“ADT-Turbine”数字孪生服务和“RG-Metaverse”资源组。
    
15. 请您在Windows桌面的搜索框内输入 `Windows PowerShell` ，并运行该程序。在命令行中，输入下面的命令：
  
    ```console
    cd <path for azuredeploy.bicep>
    ```

    <>的内容请替换为您电脑上的 AzureDeploy.bicep 文件所在的路径。如果您下载了本代码库，AzureDeploy.bicep 应该在根目录下的 code/ARM-Template 目录下。bicep是ARM模板所支持的格式之一。
    
16. 在命令行中输入如下命令以登录Azure：
    
    ```console
    az login
    ```

    一个浏览器窗口会启动，您只需要根据引导完成验证即可登录到Azure。登录成功后命令行窗口会显示您的订阅。
    ![PS Az Login](/pics/lab1-azlogin.png "Lab 1 - Step 16")

    如果命令行无法启动浏览器窗口，您可以尝试使用设备代码的方式来完成验证。在命令行中输入以下命令：
    ```console
    az login --use-device-code
    ```
    
    命令行中将会返回一个唯一性代码。然后您打开浏览器访问 [设备登录页面](https://aka.ms/devicelogin)，并输入命令行中返回的设备代码，也可以完成Azure登录流程。

17. 然后我们在PS的命令行中定义两个变量projectname和appreg，请输入如下命令：
    ```console
    $projectname="myproj"
    $appreg="myappreg"
    ```
    PS中，变量名以$为前缀。您也可以自定义变量值为您希望的字符串。但请注意，字符串不能超过14个字符，而且只能使用小写字母和数字。

18. 输入下列命令来创建一个服务主体（Service Principle）以及它访问 Azure 资源的权限角色：
    ```console
    az ad sp create-for-rbac --name ${appreg} --role Contributor --scopes /subscriptions/<SUBSCRIPTION-ID> > AppCredentials.txt
    ```
    把其中的 "\<SUBSCRIPTION-ID>" 替换为您的有效的订阅ID。命令执行结果的屏幕输出，被重定向到了当前目录下一个 AppCredentials.txt 文件中。请保存好这个文件，因为其中包含了后续配置遥测数据模拟程序步骤要用到的安全信息。

    ![PS Create SP](/pics/lab1-createsp.png "Lab 1 - Step 18")

19. 在命令行中使用下面的命令获取并显示应用注册的 ObjectID ：
    ```console
    $objectid=$(az ad sp show --id <YOUR-APPID> --query id --out tsv)
    echo $objectid
    ```

    其中的 "\<YOUR-APPID>" 请填入 AppCredentials.txt 文件中对应项的信息。然后使用下面的命令获取并显示 UserID ：

    ```console
    $userid=$(az ad signed-in-user show --query id -o tsv)
    echo $userid
    ```

    现在您应该看到命令行窗口中显示出的 GUID 格式的 ObjectID 和 UserID 。

20. 使用如下的命令创建资源组：
    ```console
    az group create --name ${projectname}-rg --location eastus
    ```
    
    这一步其实是以命令行的方式，做了和本实验中第10步类似的事情，即在 **eastus** 区域创建了一个名为 "您定义的ProjectName-rg"的资源组，用以容纳和管理所需的 Azure 基础服务。不要轻易去改变 **eastus** 这个参数，以免所需的服务在您指定的其他 Azure 区域不可用。

21. 运行如下命令把当前目录下的 azuredeploy.bicep 模板所定义的资源部署到上一步所创建的资源组中：
    ```console
    az deployment group create -f azuredeploy.bicep -g ${projectname}-rg --parameters projectName=${projectname} userId=${userid} appRegObjectId=${objectid} > ARM_deployment_out.txt
    ```
    
    命令运行过程中您将会收到若干条警告信息，可以忽略它们，不影响运行结果。这个部署过程可能会耗时20至30分钟才能完成。您可以离开休息一会儿再回来。

22. 继续在 PowerShell 命令行窗口中，为 Azure CLI 安装 **azure-iot** 扩展，这将会在后续步骤中用到。运行如下命令：
    ```console
    az extension add --name azure-iot
    ```
    网络情况不好可能会导致扩展搜索失败，此时尝试其他的网络连接或使用VPN应该会解决问题。

23. 接下来我们用两条命令把迄今部署的 Azure 资源的重要信息导出，用于后续的实验。请运行如下命令：
    ```console
    az deployment group show -n azuredeploy -g ${projectname}-rg --query properties.outputs.importantInfo.value > Azure_config_settings.txt
    ```
    这里用 **az deployment** 命令把部署好的 Azure 资源的关键配置参数重定向保存到名为 Azure_config_settings.txt 的文件中。然后继续运行下面的命令：
    ```console
    az iot hub connection-string show --resource-group ${projectname}-rg >> Azure_config_settings.txt
    ```
    这里用了刚安装的 **az-iot** 扩展中的 **az iot** 命令把资源组的连接字符串等安全信息追加保存到了 Azure_config_settings.txt 中。所以请妥善保存好这一文件。

24. 如果一切正常，您可以在资源管理器中找到 Azure_config_settings.txt 文件并打开观察其内容，或运行如下的命令在命令行环境下显示其内容：
    ```console
    get-content Azure_config_settings.txt
    ```
    其结果应类似下图。

    ![PS Get Content](/pics/lab1-getcontent.png "Lab 1 - Step 24")

    而如果您登录 Azure 门户，也可以找到由命令行所创建的资源组，并在下面的 **资源可视化工具** 看到通过模板所部署的服务及其相互的关系。

    ![Portal Resource Diagram](/pics/lab1-resourcediagram.png "Lab 1 - Step 24")


至此，您已经完成了实验一的所有步骤。Azure 的数字孪生服务以及其他基础服务都已就绪，等待您进入后续的实验了。

