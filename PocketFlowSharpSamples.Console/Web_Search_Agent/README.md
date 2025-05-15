# 手把手教你使用C#创建一个WebSearchAgent

## PocketFlowSharp介绍

最近我对PocketFlow比较感兴趣，不仅是因为它是一个极简的LLM框架，更加让我觉得很不错的地方在于作者提供了很多方便学习的例子，就算没有LLM应用开发经验，也可以快速上手。



我比较喜欢C#，也想为C#生态做一点小小的贡献，因此创建了PocketFlowSharp项目。

PocketFlowSharp项目的愿景是助力.NET开发者开发LLM应用。



在我个人在学习实践的过程中，我发现很多项目不是那么“新手友好的”，这也没有办法，开发者更关注的是代码实现，文档写起来确实也很费劲。



在PocketFlowSharp项目中，我希望可以做到足够的新手友好，提供一些只要简单配置即可跑起来的示例，并且每个示例是独立的。

PocketFlowSharp项目地址：https://github.com/Ming-jiayou/PocketFlowSharp

![image-20250515135028833](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515135028833.png)

## 构建Web_Search_Agent

今天介绍的是Web_Search_Agent。

代码在：https://github.com/Ming-jiayou/PocketFlowSharp/tree/main/PocketFlowSharpSamples.Console/Web_Search_Agent

### 效果

先来看下效果：

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/C%23%20WebSearchAgent%20%E6%95%88%E6%9E%9C.gif)

![image-20250515135915401](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515135915401.png)

![image-20250515135951723](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515135951723.png)

### 配置

运行这个示例非常简单，我提供了.env.example，如下所示：

![image-20250515140146258](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515140146258.png)

用于配置LLM与BraveSearchApi，目前BraveSearchApi的免费额度是一个月2000次。

将其重命名为.env，注意需要将其设置为嵌入的资源，如下所示：

![image-20250515140351370](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515140351370.png)

### 实现

在经过简单的配置之后，应该已经能够跑通了，为了让感兴趣的人更好的学习，我这里来介绍一下具体的实现。



Web_Search_Agent说是Agent其实我觉得更像是个工作流。PocketFlowSharp相当于一个简单的流程框架，将节点根据一个string类型的action进行连接。



Web_Search_Agent的整体流程如下所示：

![image-20250513150525245](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250513150525245.png)

首先创建一个Flow：

![image-20250515141310082](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515141310082.png)

将节点进行连接有两种方式。

一种是：

```csharp
decide.Next(search, "search");
```

另一种是：

```csharp
_ = search - "decide" - decide;
```

这是因为实现了运算符重载，具体可看此处：

![image-20250515141513915](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515141513915.png)

![image-20250515141545436](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515141545436.png)

运行Flow的时候，节点之间的编排在这里：

![image-20250515141752718](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515141752718.png)

每一个节点的运行流程在这里：

![image-20250515141848013](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515141848013.png)

首先会运行决定节点的prep：

![image-20250515142005794](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515142005794.png)

获取上下文（当前还没有上下文）与问题。

决定节点的exec：

![image-20250515142134681](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515142134681.png)

获取prep的问题与上下文，判断是搜索还是回答。

决定节点的post：

![image-20250515142256186](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515142256186.png)

根据LLM做出的决定选择行动。

这里LLM选择的是search。

根据返回的search寻找下一个节点也就是搜索节点，然后同样执行prep、exec与post。

Search节点的prep：

![image-20250515142827316](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515142827316.png)

从共享存储中获取要搜索的内容。

Search节点的exec：

![image-20250515142901028](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515142901028.png)

返回网络搜索结果：

![image-20250515142957989](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515142957989.png)

Search节点的post：

![image-20250515143035822](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515143035822.png)

将网络搜索的结果放到共享存储的context中。

然后返回"decide"又会回到决定节点。

决定节点这次选择的是answer：

![image-20250515143237521](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515143237521.png)

就会转到回答节点。

回答节点的prep：

![image-20250515143339979](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515143339979.png)

从共享存储中获取问题与上下文。

回答节点的exec：

![image-20250515143411477](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515143411477.png)

根据问题与上下文进行回答。

回答节点的post：

![image-20250515143536617](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515143536617.png)

将答案存入共享存储中。

最后从共享存储中提取出答案：

![image-20250515143702208](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250515143702208.png)

以上就是整个流程，希望能够让感兴趣的朋友快速理解。

## 最后

如果你还有什么不理解的地方，欢迎给我提issue。

如果对你有所帮助，点颗star⭐就是最大的鼓励。

欢迎感兴趣的朋友一起为爱发电。