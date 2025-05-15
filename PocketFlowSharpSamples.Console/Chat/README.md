# Chat Application

## Overview
This is a console-based chat application that interacts with a language model via an API. The application allows users to have a conversation with an AI assistant, with the ability to exit the conversation by typing 'exit'.

## Purpose
The purpose of this project is to demonstrate how to integrate a language model API into a .NET console application using the PocketFlowSharp framework. It serves as a simple example for developers who want to build chat applications or integrate AI capabilities into their .NET projects.

## Setup
1. **Clone the Repository:**
   
   ```sh
   git clone https://github.com/your-repo-url/PocketFlowSharp.git
   cd PocketFlowSharp/PocketFlowSharpSamples.Console/Chat
   ```
   
2. **Install Dependencies:**
   Ensure you have .NET 8.0 installed on your system. You can install it from the official .NET website: https://dotnet.microsoft.com/download/dotnet/8.0

   Install the required NuGet packages:
   ```sh
   dotnet restore
   ```

3. **Configure Environment Variables:**
   Copy the `.env.example` file to `.env`:
   ```sh
   copy .env.example .env
   ```

   Edit the `.env` file to include your API credentials:
   ```ini
   ModelName=Qwen/Qwen2.5-72B-Instruct
   EndPoint=https://api.siliconflow.cn/v1
   ApiKey=sk-xxx
   BraveSearchApiKey=BSA9xxx
   ```

## Usage
1. **Run the Application:**
   ```sh
   dotnet run
   ```

2. **Start a Conversation:**
   - The application will start and display a welcome message.
   - Type your messages to interact with the AI assistant.
   - Type 'exit' to end the conversation.

## Key Components
- **Program.cs:**
  - Entry point of the application.
  - Loads environment variables from the `.env` file.
  - Initializes the chat flow and runs it.

- **ChatNode.cs:**
  - Implements the `Node` interface from the PocketFlowSharp framework.
  - Handles user input, calls the language model API, and processes the response.

- **Utils.cs:**
  - Contains utility methods for calling the language model API.
  - Manages API credentials and configuration.

## Dependencies
- **dotenv.net:**
  - Used to load environment variables from a `.env` file.
  - Version: 3.2.1

- **Microsoft.Extensions.AI.OpenAI:**
  - Provides client libraries for interacting with the OpenAI API.
  - Version: 9.5.0-preview.1.25262.9

- **PocketFlowSharp:**
  - Custom framework for building AI-powered applications.
  - Project reference: `..\..\PocketFlowSharp\PocketFlowSharp.csproj`
