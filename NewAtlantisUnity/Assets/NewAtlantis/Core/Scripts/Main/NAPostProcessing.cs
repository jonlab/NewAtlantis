using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class NAPostProcessing : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		PostProcessingBehaviour b = GetComponent<PostProcessingBehaviour> ();

		b.profile.chromaticAberration.enabled = true;

		//ChromaticAberrationModel.Settings chromaticSettings = b.profile.chromaticAberration.settings;
		//chromaticSettings.intensity = 10;
		//b.profile.chromaticAberration.settings = chromaticSettings;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
