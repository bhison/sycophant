using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

using System;
using System.IO;

[RequireComponent(typeof(APIInteraction))]
public class JudgeForLaughs : MonoBehaviour
{
    [SerializeField] private APIInteraction _apiInteraction;
    
    // The AudioSource component that will play the recorded audio
    private AudioSource audioSource;

    public AudioClip demoAudio;

    const int HEADER_SIZE = 44;
    
    // The duration of the audio clip
    [SerializeField] [Range(1,10)] int AudioLength = 3; // 3 seconds
    [SerializeField] private float LaughterStateLength = 5f;

    [SerializeField] private float _laughFactor = 0;
    public float LaughFactor
    {
        get => _laughFactor;
        set
        {
            _laughFactor = value;
            StartCoroutine(SetLaughFactorToZeroAfterSeconds(LaughterStateLength));
            Debug.Log("laugh factor set - " + value + "%");
            // could also trigger some event here??
        }
    }

    private void Awake()
    {
        if(_apiInteraction == null) _apiInteraction = GetComponent<APIInteraction>();
        _apiInteraction.SetJudge(this);
    }

    private void Start()
    {
        // Initialize the AudioSource
        audioSource = GetComponent<AudioSource>();
        // StartRecording();
        if(demoAudio != null)
        {
            var asWav = ConvertToWav(demoAudio);
            StartCoroutine(_apiInteraction.FetchLaughRating(asWav));
        }
    }
    
    private IEnumerator SetLaughFactorToZeroAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        LaughFactor = 0;
    }

    // Method to convert an AudioClip to a WAV byte array.
    public static byte[] ConvertToWav(AudioClip clip)
    {
        var samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];
        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767; 
        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        return Combine(WavHeader(clip, bytesData.Length), bytesData);
    }

    private static byte[] Combine(byte[] first, byte[] second)
    {
        byte[] ret = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, ret, 0, first.Length);
        Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
        return ret;
    }

    private static byte[] WavHeader(AudioClip clip, int length)
    {
        byte[] header = new byte[HEADER_SIZE];

        var hz = clip.frequency;
        var channels = clip.channels;
        var samples = clip.samples;

        Buffer.BlockCopy(System.Text.Encoding.UTF8.GetBytes("RIFF"), 0, header, 0, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(length + HEADER_SIZE - 8), 0, header, 4, 4);
        Buffer.BlockCopy(System.Text.Encoding.UTF8.GetBytes("WAVE"), 0, header, 8, 4);
        Buffer.BlockCopy(System.Text.Encoding.UTF8.GetBytes("fmt "), 0, header, 12, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(16), 0, header, 16, 4);
        Buffer.BlockCopy(BitConverter.GetBytes((ushort)1), 0, header, 20, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(channels), 0, header, 22, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(hz), 0, header, 24, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(hz * channels * 2), 0, header, 28, 4);
        Buffer.BlockCopy(BitConverter.GetBytes((ushort)(channels * 2)), 0, header, 32, 2);
        Buffer.BlockCopy(BitConverter.GetBytes((ushort)16), 0, header, 34, 2);
        Buffer.BlockCopy(System.Text.Encoding.UTF8.GetBytes("data"), 0, header, 36, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(length), 0, header, 40, 4);

        return header;
    }
    
   

    // Call this method to start recording
    [ContextMenu("Demo Record")]
    public void StartRecording()
    {
        StartCoroutine(RecordAudio());
    }

    private IEnumerator RecordAudio()
    {
        // Start recording from the default microphone
        AudioClip audioClip = Microphone.Start(null, false, AudioLength, 44100);
        
        // Wait for the duration of the audio clip
        yield return new WaitForSeconds(AudioLength);

        // Stop the recording
        Microphone.End(null);

        // Call your other function with the recorded audio clip
        var asWav = ConvertToWav(audioClip);
        StartCoroutine(_apiInteraction.FetchLaughRating(asWav));
    }
}