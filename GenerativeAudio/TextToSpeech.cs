using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextToSpeech : MonoBehaviour
{
    public ElevenlabsAPI tts;
    public Button sendButton;
    public TMP_InputField inputField;
    
    void Start()
    {
        // Add the PlayClip handler to the ElevenLabsAPI script
        tts.AudioReceived.AddListener(PlayClip);
        
        // Add the Button's onClick handler 
        if (sendButton == null) return;
        sendButton.onClick.AddListener( () => {
            tts.GetAudio(inputField.text);
        });
    }
    
    public void PlayClip(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
}
