using UnityEngine;
using System.Collections;

public class NADopplerSpawner : MonoBehaviour {
    public int interval = 4;
    public GameObject doppler;
    public AudioClip clip;

    public float lifeDuration = 2000;
    float time = 0;

    // Use this for initialization
    void Start () {
	
	}

    public void Init(AudioClip a)
    {
        clip = a;


    }
	
	// Update is called once per frame
	void Update () {

	if(Time.frameCount % interval == 0)
        {
            GameObject nd = GameObject.Instantiate(doppler);
            nd.transform.position = transform.position;
            AudioSource audio = nd.GetComponent<AudioSource>();
            audio.clip = clip;
            audio.Play();
        }

        time++;
        if (time > lifeDuration) Destroy(this.gameObject);
    }
}
