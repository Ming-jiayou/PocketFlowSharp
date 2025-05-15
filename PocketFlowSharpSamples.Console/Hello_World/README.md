# Hello_World Sample Project

## Overview
This is a simple console application that demonstrates the use of the PocketFlowSharp library to interact with a language model (LLM) and retrieve answers to questions. The application is designed to load environment variables from a `.env` file, call the LLM with a predefined question, and display the answer.

## Project Structure
- **.env**: Contains environment variables for the LLM API.
- **.env.example**: Example file for setting up environment variables.
- **AnswerNode.cs**: Defines the `AnswerNode` class, which is responsible for preparing, executing, and post-processing the LLM call.
- **Hello_World.csproj**: Project file for the console application.
- **Program.cs**: Entry point of the application, where the flow is created and executed.
- **Utils.cs**: Utility class containing methods for interacting with the LLM.

## Environment Variables
- **ModelName**: The name of the LLM model to use.
- **EndPoint**: The endpoint URL for the LLM API.
- **ApiKey**: The API key for authenticating with the LLM API.
- **BraveSearchApiKey**: (Optional) API key for Brave Search, if needed.

## Dependencies
- **dotenv.net**: For loading environment variables from the `.env` file.
- **Microsoft.Extensions.AI.OpenAI**: For interacting with the OpenAI API.

## Running the Application
1. Ensure you have the necessary environment variables set in the `.env` file.
2. Open a terminal and navigate to the project directory:
   ```sh
   cd d:/Learning/MyProject/PocketFlowSharp/PocketFlowSharpSamples.Console/Hello_World
   ```
3. Build the project:
   ```sh
   dotnet build
   ```
4. Run the application:
   ```sh
   dotnet run
   ```

## Example Usage
The application is configured to ask the LLM a predefined question: "你是谁？" (Who are you?).

### Output
```
Question: 你是谁？
Answer: [LLM response will be displayed here]
```
