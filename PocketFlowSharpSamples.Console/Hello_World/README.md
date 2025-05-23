# Hello World - PocketFlowSharp 示例项目

这是一个使用 PocketFlowSharp 框架的简单示例项目，展示了如何创建一个基本的对话流程。该示例实现了一个简单的问答系统，可以向大语言模型发送问题并获取回答。

## 项目结构

- `Program.cs`: 主程序入口，包含流程创建和执行逻辑
- `AnswerNode.cs`: 实现回答节点的核心逻辑
- `Utils.cs`: 工具类，包含与大语言模型通信的相关功能

## 环境要求

- .NET Core 6.0 或更高版本
- PocketFlowSharp 包
- dotenv.net 包

## 配置说明

1. 在项目根目录创建 `.env` 文件
2. 在 `.env` 文件中配置以下环境变量：
   ```
   ModelName=你的模型名称
   EndPoint=API端点地址
   ApiKey=你的API密钥
   ```

## 使用方法

1. 确保已安装所有必要的依赖包
2. 配置 `.env` 文件
3. 运行程序：
   ```bash
   dotnet run
   ```

## 示例流程说明

这个示例实现了一个简单的问答流程：

1. 程序启动时会从 `.env` 文件加载配置
2. 创建一个包含问题 "你是谁？" 的共享数据字典
3. 创建并执行问答流程
4. 通过 AnswerNode 处理问题并获取回答
5. 最后将问题和回答打印到控制台

## 扩展建议

你可以通过以下方式扩展这个示例：

1. 修改问题内容
2. 添加更多的处理节点
3. 增加错误处理机制
4. 实现更复杂的对话流程

## 注意事项

- 请确保 `.env` 文件中的配置信息正确
- API密钥请妥善保管，不要提交到版本控制系统
- 建议在使用前先测试API连接是否正常 