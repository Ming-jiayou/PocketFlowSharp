# BatchNode Demo

这个项目展示了如何使用 PocketFlowSharp 框架中的 BatchNode 来处理批量数据。

This project demonstrates how to use BatchNode from the PocketFlowSharp framework to process batch data.

## 项目说明 | Project Description

这个示例演示了如何使用 BatchNode 来批量处理一组数字。在这个例子中，我们将一个数字列表中的每个数字乘以2。项目展示了 PocketFlowSharp 框架中批处理节点的三个主要阶段：

- Prep：准备要处理的数据
- Exec：执行实际的处理逻辑
- Post：处理完成后的后续操作

This example demonstrates how to use BatchNode to process a list of numbers. In this case, we multiply each number in a list by 2. The project showcases the three main phases of a batch processing node in the PocketFlowSharp framework:

- Prep: Prepare the data for processing
- Exec: Execute the actual processing logic
- Post: Perform post-processing operations

## 主要组件 | Main Components

- `Program.cs`: 主程序入口，创建示例数据并设置处理流程
- `NumberBatchProcessNode.cs`: 实现批处理逻辑的节点类

## 使用方法 | Usage

1. 创建要处理的数字列表
2. 设置共享数据字典
3. 创建并运行处理流程

示例代码 | Example:

```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var shared = new Dictionary<string, object>
{
    { "numbers", numbers }
};

var batchFlow = CreateFlow();
batchFlow.Run(shared);
```

## 运行结果 | Output

程序将显示原始数字列表和处理后的结果。每个数字都会被乘以2。

The program will display the original list of numbers and the processed results. Each number will be multiplied by 2.

示例输出 | Sample output:
```
原始数字：1, 2, 3, 4, 5
处理数字 1
处理数字 2
处理数字 3
处理数字 4
处理数字 5
处理后的数字：2, 4, 6, 8, 10
```

## 依赖项 | Dependencies

- PocketFlowSharp 框架
- .NET