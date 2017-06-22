using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyboard : MonoBehaviour {
	public int octave = 5;
	Instrument instrument;
	string[] keyMapping = {"a","w","s","e","d","f","t","g","y","h","u","j"};

	// Use this for initialization
	void Start () {
		instrument = GetComponent<Instrument> ();

	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 12; i++) {
			if (Input.GetKeyDown (keyMapping [i])) {
				instrument.PlayNote (octave*12 + i);
			}
		}
	}
}
