using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    public GameObject elephant;
    public GameObject[] speakers;

    public AudioClip money;
    public AudioClip trumpet;

    public AudioClip easyMusic;
    public AudioClip tranceMusic;
    public AudioClip popMusic;

    [Range(0,1)]
    public float shopAudioVolume;
    
    public AudioClip[] elephantIncidentalSounds;

    private Coroutine shopAudioCoroutine;

    private Vector3 atElephant => elephant.transform.position;
    private Vector3 atCamera => Camera.main.transform.position;

    // Added line
    Dictionary<GameObject, AudioSource> speakersAudioSource = new Dictionary<GameObject, AudioSource>(); 

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

    private void Start()
    {
        // Added loop
        foreach (var speaker in speakers)
        {
            var audioSource = speaker.AddComponent<AudioSource>();
            speakersAudioSource.Add(speaker, audioSource);
        }
        StopMusic();
    }

    private void OnDestroy()
    {
        StopMusic();
    }

    public void StopMusic()
    {
        if (shopAudioCoroutine != null)
        {
            StopCoroutine(shopAudioCoroutine);
            // Added loop
            foreach (var speaker in speakersAudioSource)
            {
                speaker.Value.Stop();
            }
        }
    }

    public void PlayTipSFX()
    {
        AudioSource.PlayClipAtPoint(money, atCamera);
    }

    public void PlayTrumpet()
    {
        AudioSource.PlayClipAtPoint(trumpet, atElephant);
    }

    public void PlayRandomElephantSound()
    {
        AudioClip randomSound = elephantIncidentalSounds[Random.Range(0, elephantIncidentalSounds.Length)];
        AudioSource.PlayClipAtPoint(randomSound, atElephant);
    }

    public void PlayMusic(MusicType musicType)
    {
        if (shopAudioCoroutine != null)
        {
            StopCoroutine(shopAudioCoroutine);
        }        
        AudioClip musicClip;
        switch (musicType)
        {
            case MusicType.EasyListening:
                musicClip = easyMusic;
                break;
            case MusicType.Trance:
                musicClip = tranceMusic;
                break;
            case MusicType.PopBanger:
                musicClip = popMusic;
                break;
            default:
                musicClip = null;
                break;
        }
        shopAudioCoroutine = StartCoroutine(LoopShopAudio(musicClip));
    }
    
    IEnumerator LoopShopAudio(AudioClip clip)
    {
        while (true)
        {
            foreach (var speaker in speakers)
            {
                speakersAudioSource[speaker].clip = clip;
                speakersAudioSource[speaker].volume = shopAudioVolume;
                speakersAudioSource[speaker].Play();
            }
            yield return new WaitForSeconds(clip.length + 0.5f);
        }
    }
}