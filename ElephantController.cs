using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JudgeForLaughs))]
public class ElephantController : MonoBehaviour
{
    public static ElephantController Instance { get; private set; }

    public JudgeForLaughs judgeForLaughs;

    public bool isSpeaking = false;

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
    
    public bool Speaking { get; private set; }
    public ClothesType? wantedClothesType;
    public Describer? wantedDescriber;
    public MusicType? wantedMusicType;
    public Temperature? wantedTemperature;
    public ResponseExpectation wantedResponseExpectation = ResponseExpectation.Neutral;

    void Start()
    {
        ResetWants();
        judgeForLaughs = GetComponent<JudgeForLaughs>();
    }

    void ResetWants()
    {
        wantedClothesType = null;
        wantedDescriber = null;
        wantedMusicType = null;
        wantedTemperature = null;
        wantedResponseExpectation = ResponseExpectation.Neutral;
    }

    public void SetStartSpeaking()
    {
        isSpeaking = true;
    }

    public void OnStopSpeaking()
    {
        isSpeaking = false;
        judgeForLaughs.StartRecording();
    }
    
}
