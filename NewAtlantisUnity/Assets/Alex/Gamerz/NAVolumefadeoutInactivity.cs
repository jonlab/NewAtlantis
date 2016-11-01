using UnityEngine;
using System.Collections;

public class NAVolumefadeoutInactivity : MonoBehaviour {
    public Vector3 difference;
    public float differenceMagnitude = 0;
    public float diffMin = 0.1f;
    //public Quaternion quaDiff;
    Vector3 prevPos;

    public float inactivityTime = 0;
    public float inactivityThreshold = 1500;

    float initVolume = 0;

   // public float fadeInValue = 0.5f;

    float ratioFadeOut = 2f;

    public float volumeMin = 0.05f;
    public float fadeValue = 0.01f;

    bool startFading = false;

	// Use this for initialization
	void Start () {
        prevPos = transform.position;
        initVolume = AudioListener.volume;
	}
	
	// Update is called once per frame
	void Update () {

        difference = prevPos - transform.position;
        differenceMagnitude = Vector3.Magnitude(difference);

        if (inactivityTime > inactivityThreshold)
        {
            fadeOut();
        }
        else
        {
            fadeIn();

        }

     

    

        if(differenceMagnitude < diffMin)
        {

            inactivityTime++;
        }
        else
        {
            inactivityTime = 0;
        }

        prevPos = transform.position;

    }


    void fadeOut()
    {
        //print("FADE OUT");

        // AudioListener.volume = initVolume/ratioFadeOut;
        if (AudioListener.volume > volumeMin) {  AudioListener.volume -= fadeValue;  } 
    }

    void fadeIn()
    {

        //print("FADE OUT"); 
        AudioListener.volume = initVolume;

    }
}
