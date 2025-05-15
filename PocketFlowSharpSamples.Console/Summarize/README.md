# PocketFlowSharp Summarize Console Application

## Overview
This is a console application that uses the PocketFlowSharp framework to summarize text using a large language model (LLM).

## Purpose
The purpose of this project is to demonstrate how PocketFlowSharp can be used to create a simple workflow for text summarization. It reads environment variables from a `.env` file, processes a given text, and outputs a summary.

## Setup
1. **Clone the Repository:**
   ```sh
	git clone https://github.com/your-repo/PocketFlowSharpSamples.Console.git
	cd PocketFlowSharpSamples.Console/Summarize
	```

2. **Install Dependencies:**
   ```sh
	dotnet restore
	```

3. **Configure Environment Variables:**
   - Copy the `.env.example` file to `.env`:
     ```sh
	   copy .env.example .env
	 ```
   - Edit the `.env` file to include your API keys and endpoint:
     ```ini
	 ModelName=Qwen/Qwen2.5-72B-Instruct
	 EndPoint=https://api.siliconflow.cn/v1
	 ApiKey=sk-xxx
	 BraveSearchApiKey=BSA9xxx
	 ```

## Usage
**Run the Application:**

```sh
dotnet run
```

**Example Output:**

The application will output the input text and its summary:

```sh
 输入文本:
 PocketFlow is a minimalist LLM framework that models workflows as a Nested Directed Graph.
 Nodes handle simple LLM tasks, connecting through Actions for Agents.
 Flows orchestrate these nodes for Task Decomposition, and can be nested.
 It also supports Batch processing and Async execution.
```

```sh
 摘要:
 PocketFlow: Minimalist LLM framework for task decomposition.
```

## Project Structure

- **.env:** Contains environment variables for the application.
- **.env.example:** Example file for environment variables.
- **Program.cs:** Main entry point of the application.
- **SummarizeNode.cs:** Node class that handles the summarization task.
- **Utils.cs:** Utility functions for interacting with the LLM.
- **Summarize.csproj:** Project configuration file.

## Dependencies
- **dotenv.net:** For loading environment variables.
- **Microsoft.Extensions.AI.OpenAI:** For interacting with the LLM API.
