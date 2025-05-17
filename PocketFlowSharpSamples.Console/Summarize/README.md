# 文本摘要示例 (Text Summarization Example)

这是一个使用 PocketFlowSharp 框架实现的简单文本摘要应用示例。该示例展示了如何使用 PocketFlowSharp 框架构建一个基本的文本处理流程，将长文本转换为简短的摘要。

## 功能特点

- 使用 LLM (大语言模型) 生成文本摘要
- 简单的流程设计，展示 PocketFlowSharp 的基本用法
- 包含错误处理和回退机制
- 支持环境变量配置

## 项目结构

```
Summarize/
├── Program.cs          # 主程序入口
├── SummarizeNode.cs    # 摘要节点实现
├── Utils.cs            # 工具类
└── Summarize.csproj    # 项目文件
```

## 使用方法

1. 首先确保你已经安装了 .NET SDK

2. 在项目根目录创建 `.env` 文件，并配置以下环境变量：
   ```
   ModelName=你的模型名称
   EndPoint=API端点地址
   ApiKey=你的API密钥
   ```

3. 运行程序：
   ```bash
   dotnet run
   ```

## 代码示例

这是一个基本的使用示例：

```csharp
// 创建共享数据字典
var shared = new Dictionary<string, object>();

// 设置要摘要的文本
shared["data"] = "你的长文本...";

// 创建并运行摘要流程
var summarizeFlow = CreateFlow();
summarizeFlow.Run(shared);

// 获取摘要结果
string summary = (string)shared["summary"];
```

## 注意事项

- 确保正确配置环境变量
- API密钥请妥善保管，不要提交到版本控制系统
- 建议在使用前测试API连接是否正常

## 依赖项

- PocketFlowSharp
- dotenv.net

## 贡献

欢迎提交 Issue 和 Pull Request 来帮助改进这个示例项目。

## 许可证

MIT License 