# LLM 流式输出示例

这是一个使用 PocketFlowSharp 框架实现的大语言模型(LLM)流式输出示例项目。该项目展示了如何通过简单的方式实现与 LLM 的流式交互，让 AI 生成的内容能够实时、流畅地显示在控制台中。

## 功能特点

- ✨ 实时流式输出：AI 生成的内容会逐字显示，提供更好的交互体验
- 🎮 用户控制：支持随时通过回车键中断输出
- 🎨 彩色输出：使用不同颜色区分系统消息和 AI 响应
- 🔄 异步处理：采用现代的异步编程模式
- 🛠 模块化设计：使用 PocketFlowSharp 的节点系统实现功能

## 环境要求

- .NET 8.0 或更高版本
- Visual Studio 2022 或其他支持 .NET 8.0 的 IDE
- 有效的 OpenAI API 密钥

## 必需的包

```xml
<PackageReference Include="dotenv.net" Version="3.2.1" />
<PackageReference Include="Microsoft.Extensions.AI.OpenAI" Version="9.5.0-preview.1.25262.9" />
```

## 使用说明

1. 克隆或下载项目到本地

2. 在项目根目录创建 `.env` 文件，添加以下配置：
   ```
   ModelName=你的模型名称
   EndPoint=API端点
   ApiKey=你的OpenAI API密钥
   ```

3. 使用 Visual Studio 或命令行打开项目

4. 运行项目，默认会要求 AI 创作一首关于春天的诗歌

5. 观察控制台输出，AI 的回答会以绿色文字流式显示

6. 如果想要中断输出，随时按下回车键

## 自定义提示词

你可以通过修改 `Program.cs` 中的 `prompt` 值来更改提问内容：

```csharp
var shared = new Dictionary<string, object>
{
    { "prompt", "你的自定义提示词" }
};
```

## 注意事项

- 确保 `.env` 文件中的配置信息正确
- API 调用可能会产生费用，请注意控制使用频率
- 建议在开发环境中测试完善后再部署到生产环境

## 贡献

欢迎提交 Issue 和 Pull Request 来帮助改进这个项目！

## 许可证

本项目采用 MIT 许可证 