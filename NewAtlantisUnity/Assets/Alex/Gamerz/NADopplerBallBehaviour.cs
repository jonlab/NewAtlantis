using UnityEngine;
using System.Collections;

public class NADopplerBallBehaviour : MonoBehaviour {
    public float lifeDuration = 200;
    float time = 0;
    public float amplitude = 0.1f;
    public float forwardIntensity = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //transform.Translate(new Vector3(Random.Range(-amplitude,amplitude),Random.Range(-amplitude,amplitude),Random.Range(-amplitude,amplitude)));
        transform.Rotate(new Vector3(Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude)));
        transform.Translate(transform.forward * forwardIntensity);

        time++;
        if (time > lifeDuration) Destroy(this.gameObject);
	}
}
