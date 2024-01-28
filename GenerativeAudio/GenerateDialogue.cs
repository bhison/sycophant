using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using UnityEngine;

namespace GenerativeAudio
{
    public struct DialogueParameters
    {
        public string[] Context;
        public string Guidance;
        public bool LookingForALaugh;
    }

    public class GenerateDialogue : MonoBehaviour
    {
        public bool useChatHistory = true;
        [SerializeField] private string openAiKey;
        [SerializeField] private string organisationId;

        private List<Message> _messages;

        public void ResetMessages()
        {
            _messages.Clear();
        }
        
        private OpenAIClient api = null;
        private void Awake()
        {
            api = new OpenAIClient(new OpenAIAuthentication(openAiKey, organisationId));
        }

        [ContextMenu("Run Test")]
        private async Task TestOpenAi()
        {
            if (api == null)
            {
                Debug.LogError("OpenAI client is not initialized!");
                return;
            }
            
            _messages = new List<Message>
            {
                new Message(Role.System, ConfigStatements.Setup),
                new Message(Role.User, ConfigStatements.ExampleSituationPrompt),
            };
            var chatRequest = new ChatRequest(_messages, Model.GPT3_5_Turbo);
            Debug.Log(chatRequest);
            var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            var choice = response.FirstChoice;
            Debug.Log(
                $"[{choice.Index}] {choice.Message.Role}: {choice.Message} | Finish Reason: {choice.FinishReason}");
        }
        
        public async Task<string> GetDialogue(DialogueParameters parameters)
        {
            var concatContext = string.Join(". ", parameters.Context) + ". ";
            var requestString = "Context:" + concatContext + "/Guidance:" + parameters.Guidance +
                "/LookingForALaugh:" + parameters.LookingForALaugh;
            _messages = _messages == null || !useChatHistory
                ? new List<Message>
                {
                    new Message(Role.System, ConfigStatements.Setup),
                    new Message(Role.System, ConfigStatements.ExampleSituationPrompt),
                    new Message(Role.User, requestString)
                }
                : _messages;
            var chatRequest = new ChatRequest(_messages, Model.GPT3_5_Turbo);
            Debug.Log(chatRequest);
            var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            var choice = response.FirstChoice;
            _messages.Add(new Message(Role.User, requestString));
            _messages.Add(choice.Message);
            Debug.Log(
                $"[{choice.Index}] {choice.Message.Role}: {choice.Message} | Finish Reason: {choice.FinishReason}");
            return choice.Message.ToString();
        }
    }
}
