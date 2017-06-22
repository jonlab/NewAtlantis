using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour {

	public GameObject spawnThing;

	public Vector3 count;
	public float spacing = 5.0f;
	public Vector3 center; 

	public List<GameObject> nodes;

	void Start () {
		nodes = new List<GameObject>();
		Generate();
	}
	
	void Update () {}

	void Generate()
	{
		float startX=center.x - (count.x/2) * spacing;
		float startY=center.y - (count.y/2) * spacing;
		float startZ=center.z - (count.z/2) * spacing;

		for (int i=0;i<count.z;i++)
		for (int j=0;j<count.y;j++)
		for (int k=0;k<count.x;k++)
		{
			Vector3 position = new Vector3 (k*spacing+startX, j*spacing+startY, i*spacing+startZ);
			GameObject g=GameObject.Instantiate(spawnThing,position,Quaternion.identity);
			SoundNodeWithDelay n = g.GetComponent<SoundNodeWithDelay> ();
			n.SetMidiNote (Random.Range(30,100));
			

			nodes.Add(g);

		}



	}



}
