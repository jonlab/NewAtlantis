using UnityEngine;
using System.Collections;

public class NA_TwisterAudio : MonoBehaviour {

    public AudioSource[] sources;
    
	// Use this for initialization
	void Start () {
       

	}

    public void InstantiateAudio(int nbSources,AudioClip clip)
    {
        sources = new AudioSource[nbSources];

        for(int i = 0; i<nbSources; i++)
        {
            sources[i] = transform.gameObject.AddComponent<AudioSource>();

           sources[i].clip = clip;
            sources[i].volume = 0.5f;
            sources[i].loop = true;
            sources[i].spatialBlend = 0.993f;
            sources[i].minDistance = 4.2f;
            sources[i].Play();
            
        }

    }

    public void SetAudioPitches(float[] pitches)
    {
        for (int i = 0; i < pitches.Length; i++)
        {
            sources[i].pitch = pitches[i];
        }
        
      }
    // Update is called once per frame
    void Update()
    {

    }
}
