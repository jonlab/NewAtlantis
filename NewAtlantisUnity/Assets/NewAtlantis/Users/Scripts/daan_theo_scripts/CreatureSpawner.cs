using UnityEngine;
using System.Collections;

public class CreatureSpawner : MonoBehaviour {
	public GameObject creaturePrefab;
	public float domeDiameter=5;
	public float spaceBetweenCreatures;
	public float emptySpaceDiameter;
	
	// Use this for initialization
	void Start () {
		float creatureWidth = creaturePrefab.GetComponent<Renderer>().bounds.size.x;
		
		int nb=0;
		for(float d=emptySpaceDiameter; d<(domeDiameter/2f); d+=creatureWidth*(1+spaceBetweenCreatures) ){
			// spawn as many creatures as possible as long as there's only 1 per 'trajectory' in the dome
			GameObject myCreature = Instantiate( creaturePrefab, Vector3.zero, Quaternion.identity  ) as GameObject;
			// set parent
			myCreature.transform.parent = this.transform;
			// set position
			//myCreature.transform.localPosition = 
			CreatureBehaviour creatureBehaviour = myCreature.GetComponent<CreatureBehaviour>();
			creatureBehaviour.distFromCenter = d;
			//print ("Instantiated: "+d);
			
			// set unique number
			creatureBehaviour.uniqueNb = nb;
			
			nb++;
		}
		
		// add empty object for tail parts (less messy)
		GameObject creatureTailParent = new GameObject("creatureTailParent");
		creatureTailParent.transform.position = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
