using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronCollection : MonoBehaviour {
	List<GameObject> _nodes;

	void Awake () {
		_nodes = new List<GameObject> ();

		Neuron[] neurons = GetComponentsInChildren<Neuron> ();
		foreach (Neuron n in neurons) {
			_nodes.Add (n.gameObject);
		}


	}

	public List<GameObject> Nodes()
	{
		return _nodes;
	}
}
