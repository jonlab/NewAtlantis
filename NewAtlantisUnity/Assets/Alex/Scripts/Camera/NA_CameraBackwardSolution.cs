using UnityEngine;
using System.Collections;

public class NA_CameraBackwardSolution : MonoBehaviour {

    bool backwardSolution = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider e)
    {
      //  Debug.LogError("Trigger enter");

        backwardSolution = false;
    }

    void OnTriggerExit(Collider e)
    {
        backwardSolution = true;
    }

    public bool returnBackwardSolution()
    {
        return backwardSolution;
    }

}
