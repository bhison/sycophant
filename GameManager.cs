using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using GenerativeAudio;
using UnityEngine;

[RequireComponent(typeof(GameFlow))]
[RequireComponent(typeof(ElephantActions))]
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

    public float patience = 20f;
    public float patienceLeftForTask = 0;

    public GenerateDialogue generateDialogue;

    public bool waitingToEnd = false;
    
    public int Tips { get; private set; }
    public bool GameInSession{ get; private set; }
    public bool TaskInSession{ get; private set; }
    public float RapportPercent { get; private set; }
    public float GameRunTime { get; set; }

    public string PlayerName = "Assistant";

    private GameFlow _gameFlow;

    [SerializeField] private float GraceForMusicChange = 5f;
    public float MusicChangeGraceCountdown = 0;
    private MusicType internalMusicType = MusicType.EasyListening;

    public MusicType currentMusicType
    {
        get
        {
            return internalMusicType;
        }
        set
        {
            internalMusicType = value;
            MusicChangeGraceCountdown = GraceForMusicChange;
        }
    }

    public bool GameLengthMinExceeded => GameInSession && GameRunTime > GameLengthMinimum;

    // Start is called before the first frame update
    private void Awake()
    {
        InitSingleton();
        _gameFlow = GetComponent<GameFlow>();
    }
    
    
    private void Update()
    {
        if (GameInSession)
        {
            GameRunTime += Time.deltaTime;
        }

        _gameFlow.DoGameStuff();
        
        if (waitingToEnd && !ElephantController.Instance.isBusy && !ElephantController.Instance.isSpeaking)
        {
            QuitToLobby();
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
        TaskInSession = false;
        patienceLeftForTask = 0;
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
        AudioController.Instance.PlayTipSFX();
    }

    /**
     * Amount should be -1 to 1 as represents %change
     */
    public void ChangeRapport(float amount)
    {
        RapportPercent = Math.Clamp(RapportPercent + amount, -1, 1);
        if (amount < 0)
        {
            AudioController.Instance.PlayTrumpet();
        }
    }

    public void EndGame()
    {
        waitingToEnd = true;
    }

    private void QuitToLobby()
    {
        //something here
    }
}
