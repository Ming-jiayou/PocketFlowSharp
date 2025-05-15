# Chat_Guardrail 项目 README

## 项目简介
`Chat_Guardrail` 是一个基于 PocketFlowSharp 的控制台应用程序，旨在提供一个旅行顾问聊天机器人。该聊天机器人通过与用户交互，确保用户的问题与旅行相关，并使用语言模型（LLM）生成旅行建议。

## 目录结构
- `.env`: 存储环境变量的文件，包含模型名称、API 端点和 API 密钥。
- `.env.example`: 示例环境变量文件，用于指导用户如何配置 `.env` 文件。
- `Chat_Guardrail.csproj`: 项目文件，定义了项目的依赖和配置。
- `Nodes.cs`: 包含用户输入节点、验证节点和 LLM 节点的定义。
- `Program.cs`: 项目的入口文件，负责加载环境变量并启动聊天流程。
- `Utils.cs`: 包含调用 LLM 和解析响应的工具函数。

## 环境变量
- `ModelName`: 语言模型的名称，例如 `Qwen/Qwen2.5-72B-Instruct`。
- `EndPoint`: 语言模型 API 的端点，例如 `https://api.siliconflow.cn/v1`。
- `ApiKey`: 用于访问语言模型 API 的密钥。
- `BraveSearchApiKey`: 用于 Brave 搜索 API 的密钥（如果需要）。

## 项目依赖
- `dotenv.net`: 用于加载环境变量。
- `Microsoft.Extensions.AI.OpenAI`: 用于与 OpenAI 语言模型进行交互。
- `PocketFlowSharp`: 项目的核心库，提供了节点和流程管理的功能。

## 项目功能
1. **用户输入节点 (`UserInputNode`)**:
   - 处理用户输入并初始化消息列表。
   - 欢迎用户并提示输入旅行相关的问题。
   - 检查用户是否输入了 `exit` 以结束对话。

2. **验证节点 (`GuardrailNode`)**:
   - 确保用户输入是旅行相关的问题。
   - 使用 LLM 进行旅行主题验证。
   - 如果验证失败，提示用户重新输入问题。

3. **LLM 节点 (`LLMNode`)**:
   - 处理与语言模型的交互。
   - 生成旅行建议并打印给用户。
   - 将助手的回复添加到消息历史中。

## 运行项目
1. 确保 `.env` 文件已正确配置。
2. 打开命令行，导航到项目目录：
   ```sh
   cd d:/Learning/MyProject/PocketFlowSharp/PocketFlowSharpSamples.Console/Chat_Guardrail
   ```
3. 运行项目：
   ```sh
   dotnet run
   ```
