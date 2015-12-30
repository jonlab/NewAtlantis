using UnityEngine;
using System.Collections;

public class RadioScript : MonoBehaviour {

    // public AudioClip clip;
    AudioSource aud;
    int timer = 0;
    int timeMax = 100;

    public int minDuration = 100;
    public int maxDuration = 500;

    int nbSamples;

    public bool audioProcess = false;
    // Use this for initialization
    void Start () {
        aud = GetComponent<AudioSource>();
        
       nbSamples = aud.clip.samples * aud.clip.channels;
        print(nbSamples);
    }

    void Frag(int startPoint)
    {
        float[] samples = new float[nbSamples];
        aud.clip.GetData(samples, 0);

        int i = 0;
        while (i < samples.Length)
        {
            samples[i] = samples[i] * 1.0F;
            ++i;
        }

        aud.clip.SetData(samples, startPoint);
        aud.Play();

        audioProcess = true;
        timer = 0;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.X))
        {
            timeMax = (int)Random.Range(minDuration, maxDuration);
            int startPoint = (int)Random.Range(0, nbSamples - timeMax);

            Frag(startPoint);
            print(timeMax + " (timeMax)");
        }
        if (audioProcess) timer++;

        if (timeMax == timer)
        {
            timer = 0;
            aud.Stop();
            audioProcess = false;

        }


	}
}
