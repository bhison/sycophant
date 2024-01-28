using UnityEngine;
using System;

public static class AudioClipExtensions
{
    public static byte[] ToByteArray(this AudioClip clip)
    {
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);

        var byteArray = new byte[samples.Length * 4];
        Buffer.BlockCopy(samples, 0, byteArray, 0, byteArray.Length);

        return byteArray;
    }
}