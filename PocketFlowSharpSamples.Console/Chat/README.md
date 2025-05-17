# Chat Sample - PocketFlowSharp 示例项目

这是一个基于 PocketFlowSharp 框架开发的简单聊天应用示例，展示了如何使用 PocketFlowSharp 构建一个交互式的 AI 聊天程序。

## 功能特点

- 💬 交互式命令行聊天界面
- 🤖 集成 OpenAI API 进行智能对话
- 📝 完整的对话历史记录
- 🔄 流程化的对话管理
- 🛠️ 基于 Node 的模块化设计

## 环境要求

- .NET 8.0 或更高版本
- Visual Studio 2022 或其他支持 .NET 的 IDE
- OpenAI API 密钥

## 安装步骤

1. 克隆项目到本地：
```bash
git clone <repository-url>
cd PocketFlowSharp/PocketFlowSharpSamples.Console/Chat
```

2. 创建 `.env` 文件并配置以下环境变量：
```
ModelName=<你的模型名称>  # 例如：gpt-4-turbo-preview
EndPoint=<API端点>       # OpenAI API 端点
ApiKey=<你的API密钥>     # OpenAI API 密钥
```

3. 还原项目依赖：
```bash
dotnet restore
```

4. 编译项目：
```bash
dotnet build
```

## 使用说明

1. 运行程序：
```bash
dotnet run
```

2. 开始对话：
   - 程序启动后，你可以直接输入文字与 AI 进行对话
   - 每次输入后按回车发送消息
   - 输入 "exit" 结束对话

## 项目结构

- `Program.cs`: 程序入口点和流程配置
- `ChatNode.cs`: 聊天节点实现，处理用户输入和 AI 响应
- `Utils.cs`: 工具类，包含 API 调用相关功能

## 依赖项

- dotenv.net (v3.2.1)
- Microsoft.Extensions.AI.OpenAI (v9.5.0-preview)
- PocketFlowSharp

## 注意事项

- 请确保 `.env` 文件中的 API 密钥安全保存，不要提交到版本控制系统
- 使用前请确认你有足够的 OpenAI API 额度
- 建议在开发环境中测试完成后再部署到生产环境

## 贡献

欢迎提交 Issue 和 Pull Request 来帮助改进这个示例项目！

## 许可证

[MIT License](LICENSE) 