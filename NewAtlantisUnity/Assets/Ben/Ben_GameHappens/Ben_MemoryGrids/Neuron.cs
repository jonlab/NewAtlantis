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

	NeuronCollection neuronCollection;
	public float distanceThreshold = 7.0f;

	Instrument instrument;
	AudioSource audioSource;

	GameObject pulsar;
	public GameObject pulsarPrefab;

	public int midiNote=30;	// note to play when triggered

	public void SetMidiNote (int note)
	{
		midiNote = note;
	}

	void Start () {
		synapses = new List<Synapse>();
		neuronCollection = GameObject.FindObjectOfType<NeuronCollection> ();

		instrument = GetComponent<Instrument> ();
		audioSource = GetComponent<AudioSource> ();

		pulsar=Instantiate(pulsarPrefab,transform.position,Quaternion.identity);
		pulsar.transform.parent = transform;

		if (!NA.isClient())
		{
			foreach (GameObject g in neuronCollection.Nodes())
			{
				float d = Vector3.Distance (g.transform.position, transform.position);
				if (d < distanceThreshold && g != this.gameObject)
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

	/*
		server authoritative 

		triggering is initiated on the server
		that calls Fire
		and fires the other random synapses

		RPC-call the Play() function on the clients


		trigger entry: this WILL get called with two non-RB colliders both set as triggers.  but it only gets called locally. 

		so it should be an RPC call to the server to the Fire function
		and the Play function should be local 

	*/

	void OnTriggerEnter(Collider other) 
	{
		if (NA.isClient())
		{
			GetComponent<NetworkView>().RPC("Server_Fire",RPCMode.Server,0.9f);
		}
		else
		{
			Fire(0.9f);
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
		pulsar.GetComponent<ScalePulse>().Pulse(signal);
	}


	[RPC]
	public void Server_Fire (float signal)
	{
		Fire(signal);
	}

	public void Fire(float signal)
	{
		//only on server

		if (!NA.isClient())
		{
			Play(signal);

			GetComponent<NetworkView>().RPC("Play", RPCMode.Others, signal);

			if (synapses.Count>0)
			{
				int r = Random.Range(0, synapses.Count);
				StartCoroutine ("FireSynapse", signal);
			}	

		}
	}

	IEnumerator FireSynapse(float signal)
	{
		yield return new WaitForSeconds (Random.Range(0.5f,2.0f));

		if (synapses.Count>0)
		{
			int r = Random.Range(0, synapses.Count);
			synapses[r].Fire(signal);
		}

	}
}
