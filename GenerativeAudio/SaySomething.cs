using UnityEngine;

namespace GenerativeAudio
{
    public class SaySomething : MonoBehaviour
    {
        public static SaySomething Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
                DontDestroyOnLoad(gameObject);
            } 
        }
        
        public ElevenlabsAPI tts;
        public GenerateDialogue engine;
        
        void Start()
        {
            tts.AudioReceived.AddListener(PlayClip);
        }

        public async void Speak(DialogueParameters dialogueParameters)
        {
            var elephantLine = await engine.GetDialogue(dialogueParameters);
            // Maybe error check and retry here?
            tts.GetAudio(elephantLine);
        }
        
        void PlayClip(AudioClip clip)
        {
            // Or error check and retry here...
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }
}