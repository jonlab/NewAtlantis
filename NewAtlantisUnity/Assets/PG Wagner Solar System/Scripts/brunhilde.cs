using UnityEngine;
using System.Collections;

public class brunhilde : MonoBehaviour {
	public AudioClip mysound; public float dopplerL;
	public float pitchL;
	private AudioSource audioSource;
	AudioSource pitchSource;
	int[] hexagram = new int[2];
	float[] hexflt = new float[2]; float[] hexpit = new float[2];
	GenaLib mycall = new GenaLib ();
	//Vector3 target;
	Vector3 startpos1;
	bool on = false;

    public GameObject myParent;
    GameObject currentParent;

    float pitch, doppler;
    public int duration = 100;
    int time = 0;

    // Use this for initialization
    void Start () {
		startpos1 = transform.position;
		GetComponent<Renderer>().material.color = Color.red;
		pitchSource=GetComponent<AudioSource> ();
		GameObject parent = GameObject.Find ("Vahalla");
		transform.parent = parent.transform;
        pitch = 1;
	}


    void OnTriggerEnter(Collider e)
    {

        seekParent(e.transform);

        if (currentParent != myParent) StartEvent();

    }


    void seekParent(Transform t)
    {

        if (t.parent != null)
        {
            if (t.gameObject == myParent)
            {
                currentParent = t.gameObject;
                return;
            }
            seekParent(t.parent);

        }
        else
        {
            currentParent = t.gameObject;
        }

    }
    void StartEvent()
    {
        time = 0;
        GetComponent<Renderer>().material.color = Color.white;
        on = !on;
        if (on)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            // change Doppler shift with iChing value
            hexagram = mycall.iChing();
            //Debug.Log ("hexagram[0]: " + hexagram[0]);
            //Debug.Log ("hexagram[1]: " + hexagram[1]);


            if (NA.isServer() || NA.isStandalone())
            {
              doppler = (hexagram[0] * 7.81f) * .01f;
             // GetComponent<NetworkView>().RPC("syncDoppler", RPCMode.All, doppler);



                if (hexagram[1] > 0)
                {
                    pitch = (hexagram[1] * .09375f) - 3.0f;
                  //  GetComponent<NetworkView>().RPC("syncPitch", RPCMode.All, pitch);
  
                }

                pitchSource.pitch = pitch;


            }

           // dopplerL = hexflt[0];
            audioSource.dopplerLevel = dopplerL;
            // change Pitch using hexagram 2, i.e. Hexagram 1 was unstable. (do nothing if 0)
           
        }
    }
	// Update is called once per frame
	void Update () {

        if(time > duration)
        {
            //on = false;
        }
		//return;
		if (on)
		{

			Vector3 centerrotation = transform.parent.position;
			transform.RotateAround(centerrotation, new Vector3(0.4f, 0.8f,0.1f), -Time.deltaTime*40f);
			//transform.Rotate(new Vector3(Time.deltaTime*1f, Time.deltaTime*0.3f, Time.deltaTime*2f));\
		}

		else
		{
			//transform.parent = null;
			transform.position = startpos1;
			//return pitch to 0
			pitchSource.pitch = 1.0f;
			GetComponent<AudioSource>().Stop();
		}
	}

    [RPC]
    void syncPitch(float _pitch)
    {
        pitch = _pitch;
    }

    [RPC]
    void syncDoppler(float _doppler)
    {
        doppler = _doppler;
    }

}	