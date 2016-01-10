using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class NA_AnimateSynthTwister : MonoBehaviour {

    List<float[]> allValues, interpolatePositions;
    int time;
    int nbFrames;
    int currentKey = 0;
    int nbKeys = 0;
    NA_Simple_CurveMaker cmk;
    NA_TwisterSynth naTwisterAudio;

   // public AudioClip clip;

    public void SetValuesAndInterpolation(List<float[]> val, List<float[]> inter, int nbFr, int nbK)
    {
        allValues = val;
        interpolatePositions = inter;
        nbFrames = nbFr;
        nbKeys = nbK;

    }
	// Use this for initialization
	void Start () {

        cmk = GetComponent<NA_Simple_CurveMaker>();
        naTwisterAudio = GetComponent<NA_TwisterSynth>();
        naTwisterAudio.StartBuffer(allValues[0]);
        
	}
	
	// Update is called once per frame
	void Update () {

        time++;
        if (time % nbFrames == 0) currentKey++;
        if (currentKey == nbKeys) currentKey = 0;

        float[] currentInterpolate = currentInterpolation(time);
    
        cmk.SetRadiusValues(currentInterpolate);

        naTwisterAudio.SetBuffer(currentInterpolate);
    }


    float[] currentInterpolation(int timer)
    {
        if (allValues.Count == 0) return new float[0];

        float[] current = new float[allValues[0].Length];

        int modulateTime = timer % nbFrames;

        for (int i = 0; i < current.Length; i++)
        {
            current[i] = allValues[currentKey][i] + (interpolatePositions[currentKey][i] * ((float)modulateTime / (float)nbFrames));
        }

        return current;
    }

}
