# 简历资格评估系统 (Resume Qualification System)

[English](#english) | [中文](#chinese)

<a name="chinese"></a>
## 中文说明

### 项目简介
这是一个基于 PocketFlowSharp 框架开发的简历资格评估系统，能够自动化处理和评估求职者简历，帮助筛选合格的候选人。系统使用 AI 技术对简历进行分析，根据预设的标准评估候选人是否符合高级技术职位的要求。

### 主要功能
- 批量读取和处理简历文件
- 自动评估候选人资格
- 生成详细的评估报告
- 提供合格/不合格候选人统计
- 显示每位候选人的具体评估原因

### 评估标准
系统根据以下标准评估候选人：
- 至少具有相关领域的学士学位
- 至少3年相关工作经验
- 与职位相关的强大技术技能

### 使用方法
1. 确保已配置环境变量（在 `.env` 文件中）：
   - `ModelName`：使用的AI模型名称
   - `EndPoint`：API端点
   - `ApiKey`：API密钥

2. 在 `data` 目录中放置简历文件（.txt格式）

3. 运行程序，系统将自动：
   - 读取所有简历文件
   - 进行资格评估
   - 生成评估报告

### 输出结果
系统将显示：
- 评估的总候选人数
- 合格候选人数及比例
- 合格/不合格候选人名单
- 每位候选人的详细评估结果和原因

---

<a name="english"></a>
## English

### Project Overview
This is a Resume Qualification Assessment System developed using the PocketFlowSharp framework. It automates the process of reviewing and evaluating job applicants' resumes, helping to identify qualified candidates. The system uses AI technology to analyze resumes and assess candidates based on predefined criteria for senior technical positions.

### Key Features
- Batch processing of resume files
- Automated candidate qualification assessment
- Detailed evaluation reporting
- Qualified/unqualified candidate statistics
- Specific evaluation reasons for each candidate

### Assessment Criteria
The system evaluates candidates based on:
- Minimum of a bachelor's degree in a relevant field
- At least 3 years of relevant work experience
- Strong technical skills relevant to the position

### How to Use
1. Ensure environment variables are configured (in `.env` file):
   - `ModelName`: AI model name to use
   - `EndPoint`: API endpoint
   - `ApiKey`: API key

2. Place resume files (in .txt format) in the `data` directory

3. Run the program, and it will automatically:
   - Read all resume files
   - Perform qualification assessment
   - Generate evaluation reports

### Output
The system will display:
- Total number of candidates evaluated
- Number and percentage of qualified candidates
- List of qualified/unqualified candidates
- Detailed evaluation results and reasons for each candidate 