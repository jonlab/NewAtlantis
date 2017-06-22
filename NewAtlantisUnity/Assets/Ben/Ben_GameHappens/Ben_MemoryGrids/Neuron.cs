using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Synapse {
	Neuron neuron1;
	Neuron neuron2;
	float weight;

	public Synapse (Neuron n1, Neuron n2, float w)
	{
		neuron1=n1;
		neuron2=n2;
		weight=w;
	}

	public void Fire(float signal)
	{
		float newSignal = signal * weight;
		if (newSignal>.01f)	
			neuron2.Fire(signal * weight);

		// increase and cap weight
		weight+=.5f;
		if (weight>.95f)
			weight=.95f;
	}

}


public class Neuron : MonoBehaviour {
	List<Synapse> synapses;

	Vector3 baseScale; 
	NeuronCollection neuronCollection;
	public float distanceThreshold = 7.0f;

	 Instrument instrument;
	 AudioSource audioSource;
	public int midiNote=30;	// note to play when triggered

	public void SetMidiNote (int note)
	{
		midiNote = note;
	}
	void Start () {
		baseScale = transform.localScale;
		synapses = new List<Synapse>();
		neuronCollection = GameObject.FindObjectOfType<NeuronCollection> ();

		instrument = GetComponent<Instrument> ();
		audioSource = GetComponent<AudioSource> ();

		if (!NA.isClient())
		{
			foreach (GameObject g in neuronCollection.Nodes())
			{
				float d = Vector3.Distance (g.transform.position, transform.position);
				if (d < distanceThreshold)
				{
					Synapse s = new Synapse (this, g.GetComponent<Neuron>(), 0.2f);
					synapses.Add(s);
				}

			}
		}

	}

	void Update () {

	}

	void restartLoop()
	{

	}
	void OnTriggerEnter(Collider other) 
	{
		if (!NA.isClient())
		{
			//if (other.gameObject.tag == "Player")
			{
				// trigger other nearby neurons through synapses
				Fire(0.9f);
			}
		}
	}






	[RPC]
	public void Play(float signal)
	{
		if (instrument != null)
		{
			audioSource.volume=signal;
			instrument.PlayNote(midiNote);

		}
		StartCoroutine(DoScaleAnimation(signal));
	}



	public void Fire(float signal)
	{
		//only on server
		// play sound
		// play sound
		if (NA.isServer())
		{
			GetComponent<NetworkView>().RPC("Play", RPCMode.Others, signal);
		}
		Play(signal);

		if (synapses.Count>0)
			{
			int r = Random.Range(0, synapses.Count);
			StartCoroutine ("FireSynapse", signal);
			}

		StartCoroutine(DoScaleAnimation(signal));
	}

	IEnumerator FireSynapse(float signal)
	{
		yield return new WaitForSeconds (0.5f);

		if (synapses.Count>0)
		{
			int r = Random.Range(0, synapses.Count);
			synapses[r].Fire(signal);
		}

	}


	IEnumerator DoScaleAnimation(float signal)
	{
		// play sound
		// trigger animation or whatever

		Vector3 newScale = baseScale * (1.0f+signal);

		float duration = 0.3f;
		float startTime = Time.time;
		float endTime =startTime+duration;

		while (Time.time<=endTime)
		{
			float t=(Time.time-startTime)/duration; 
			transform.localScale = Vector3.Lerp(newScale, baseScale,t);
			yield return null;
		}

	}
}
