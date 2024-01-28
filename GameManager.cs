using System;
using System.Collections;
using System.Collections.Generic;
using GenerativeAudio;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void InitSingleton()
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

    [Range(0,1)]
    public float StartingRapport = 0.3f;

    public int GameLengthMinimum = 240;

    public GenerateDialogue generateDialogue;
    
    public int Tips { get; private set; }
    public bool GameInSession{ get; private set; }
    public float RapportPercent { get; private set; }
    public float GameRunTime { get; private set; }

    public string PlayerName = "Assistant";

    public bool GameLengthMinExceeded => GameInSession && GameRunTime > GameLengthMinimum;

    // Start is called before the first frame update
    private void Awake()
    {
        InitSingleton();
    }
    
    
    private void Update()
    {
        if (GameInSession)
        {
            GameRunTime += Time.deltaTime;
        }
    }

    // This should be triggered when the game scene starts
    public void StartGameInSession()
    {
        GameInSession = true;
        ResetPlayerState();
    }
    
    public void ResetPlayerState()
    {
        Tips = 0;
        RapportPercent = StartingRapport;
        GameInSession = false;
        GameRunTime = 0;
        generateDialogue.ResetMessages();
    }
    
    public void AddTip(int amount)
    {
        Tips += amount;
    }

    public void AddTip(float amount)
    {
        Tips += Mathf.RoundToInt(amount);
    }

    /**
     * Amount should be -1 to 1 as represents %change
     */
    public void ChangeRapport(float amount)
    {
        RapportPercent = Math.Clamp(RapportPercent + amount, -1, 1);
    }


}
