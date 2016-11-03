using UnityEngine;
using System.Collections;

public class NADisplayTutoStep : MonoBehaviour {

    TextMesh tm;
	// Use this for initialization
	void Start () {
        tm = GetComponent<TextMesh>();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setCurrentStep(int step)
    {
        if(tm == null) tm = GetComponent<TextMesh>();
        tm.text = (step+1) + "/6";

    }
}
