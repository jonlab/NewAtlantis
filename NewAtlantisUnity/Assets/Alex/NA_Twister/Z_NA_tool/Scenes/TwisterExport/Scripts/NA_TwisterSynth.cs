using UnityEngine;
using System.Collections;

public class NA_TwisterSynth : MonoBehaviour {


    public float gain = 0.1F;
    public int position = 0;
    public int samplerate = 44100;
    public float frequency = 440;

    float[] bufferAudio;
    bool started = false;


    public void StartBuffer(float[] b)
    {

        AudioClip myClip = AudioClip.Create("MySinusoid", b.Length, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        AudioSource aud = GetComponent<AudioSource>();
        aud.clip = myClip;
        aud.Play();
        aud.loop = true;

        started = true;
        bufferAudio = b;
    }

    public void SetBuffer(float[] b)
    {

        bufferAudio = b;
    }


    void OnAudioRead(float[] data)
    {
        if (!started) return;


        int count = 0;
        while (count < data.Length)
        {
            data[count] = bufferAudio[count] * gain;
            position++;
            count++;
        }
    }
    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }
}
