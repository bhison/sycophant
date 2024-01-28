using UnityEngine;

namespace DefaultNamespace
{
    public class ConfigOverrides : MonoBehaviour
    {

        public static ConfigOverrides Instance { get; private set; }
    
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
        
        public string openAiToken;
        public string openAiOrg;
        public string elevenLabsToken;
        public string elevenLabsVoiceId;
    }
}