using Microsoft.Extensions.AI;
using PocketFlowSharp;
using System;
using System.Collections.Generic;
using OpenAI.Chat;
using ChatMessage = OpenAI.Chat.ChatMessage;

namespace Chat
{
    public class ChatNode : Node
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            // Initialize messages if this is the first run
            if (!shared.ContainsKey("messages"))
            {
                shared["messages"] = new List<ChatMessage>();
                Console.WriteLine("Welcome to the chat! Type 'exit' to end the conversation.");
            }
            
            // Get user input
            Console.Write("\nYou: ");
            string userInput = Console.ReadLine();
            
            // Check if user wants to exit
            if (userInput.ToLower() == "exit")
            {
                return null;
            }

            // Add user message to history
            var c = (List<ChatMessage>)shared["messages"];
            c.Add(new UserChatMessage(userInput));

            // Return all messages for the LLM
            return shared["messages"];
        }
        
        public override object Exec(object prepResult)
        {
            if (prepResult == null)
            {
                return null;
            }
            
            // Call LLM with the entire conversation history
            var messages = (List<ChatMessage>)prepResult;
            return Utils.CallLLM(messages);
        }
        
        public override object Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            if (prepResult == null || execResult == null)
            {
                Console.WriteLine("\nGoodbye!");
                return null;  // End the conversation
            }
            
            // Print the assistant's response
            string response = (string)execResult;
            Console.WriteLine($"\nAssistant: {response}");

            // Add assistant message to history
            var c = (List<ChatMessage>)shared["messages"];
            c.Add(new AssistantChatMessage(response));
            
            // Loop back to continue the conversation
            return "continue";
        }
    }
} 