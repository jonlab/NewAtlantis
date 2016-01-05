using UnityEngine;
using System.Collections;

public class NA_PG_EasyRotate2 : MonoBehaviour {

    public Vector3 rotationDirection;

    public float speed = 0.5f;
    public float duration = 80;
    float time = 0;
    public bool fadeMode = true;
    public bool fadeAudio = true;
    public bool materialAnimation = true;

    public int multiplyDuration = 4;
    AudioSource aud;

    Material m;
    Color rdC;
    Color white = new Color(1, 1, 1);

    // Use this for initialization
    void Start () {

        duration *= multiplyDuration;
        time = duration;
        aud = GetComponent<AudioSource>();
        m = GetComponent<MeshRenderer>().material;

    }
	
	// Update is called once per frame
	void Update () {

        float mapTime = (time / (float)duration) * speed;
        if (time < duration)
        {
            float normalizeMapTime = (speed - mapTime) * (1 / speed);
            if (fadeAudio) aud.volume = normalizeMapTime;
            if (!fadeMode) mapTime = 0;
            transform.Rotate(rotationDirection * speed);

            if (materialAnimation)
            {
                m.color = rdColor();
                m.color = mixColor(rdC, white, normalizeMapTime);

            }
           // print(m.color);
        }else
        {
            aud.volume = 0;
            m.color = white;
        }

        time++;

      // if (Input.GetKeyDown(KeyCode.Space)) StartTime();
	}

    Color mixColor(Color a ,Color b,float blend)
    {
     
        return new Color( a.r* blend + b.r*(1-blend),a.g*blend + b.g*(1-blend),b.r*blend+b.b*(1-blend), 1.0f);
    }

    Color rdColor()
    {

        return new Color(Random.Range(0,1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
    }

    void StartTime()
    {
        time = 0;
        if (!aud.isPlaying) aud.Play();
        aud.volume = 1;
   
        rdC = rdColor();
    }

    void OnTriggerStay(Collider e)
    {
       
        StartTime();
    }

    void OnCollisionEnter(Collision e)
    {
        StartTime();
    }
}
