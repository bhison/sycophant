using System.Collections.Generic;
using UnityEngine;

namespace GenerativeAudio
{
    public class TestEndToEndPromptGeneration: MonoBehaviour
    {
        [SerializeField] private ClothesType reqClothesType;
        [SerializeField] private ClothesType givenClothesType;
        [SerializeField] private Describer reqDescriber;
        [SerializeField] private Describer givenDescriber;
        
        public ElevenlabsAPI tts;
        public GenerateDialogue engine;
        
        void Start()
        {
            // Add the PlayClip handler to the ElevenLabsAPI script
            tts.AudioReceived.AddListener(PlayClip);
        }

        [ContextMenu("Demo Prompt")]
        public async void DemoPrompt()
        {
            var contextStrings = new List<string>
            {
                // ContextPrompts.AskedFor(reqClothesType, reqDescriber),
                // ContextPrompts.BeenGiven((reqClothesType == givenClothesType) && (reqDescriber == givenDescriber), givenClothesType, givenDescriber),
                ContextPrompts.LaughingAt(true, false, true)
            };
            var dialogueParameters = new DialogueParameters
            {
                Context = contextStrings.ToArray(),
                Guidance = Guidance.ExpressYourself(),
                LookingForALaugh = true
            };
            var elephantLine = await engine.GetDialogue(dialogueParameters);
            tts.GetAudio(elephantLine);
        }
        
        void PlayClip(AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }
}