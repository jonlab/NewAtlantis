using UnityEngine;
using System.Collections;

public class CreatureBehaviour : MonoBehaviour {
	//public float domeDiameter;
	GameObject player; // set player shape to this and make sure it's got a rigidbody. (for interections)
	public GameObject tailParticle;
	
	[HideInInspector] public float distFromCenter; // set by spawner
	[HideInInspector] public int uniqueNb;
	//[HideInInspector] public int emptySpaceDiameter;
	
	//bool onTrajectory = true;
	float defaultSpeed;
	float currentAngle;

	Vector3 movingDirection;
	Vector3 targetPosition;

	// Use this for initialization
	void Start () {

		// RANDOMIZE CREATURE PRESETS
		
		// set distance
		//float maxDistFromCenter = domeDiameter/2;
		//distFromCenter = Random.Range (maxDistFromCenter * .1f, maxDistFromCenter * .9f);
		// min dist
		//if( this.transform.localScale.x*2 > distFromCenter ) maxDistFromCenter = this.transform.localScale.x*2;
		// max dist
		//if (distFromCenter > maxDistFromCenter) distFromCenter = maxDistFromCenter;
		
		// new: contrain distance to increments of creature-diameter
		//float tmpSize = this.gameObject.renderer.bounds.size.x;
		//print (tmpSize + " - " +  distFromCenter/tmpSize );
		//distFromCenter = this.gameObject.renderer.bounds.size.x*1.1f * Mathf.Round( distFromCenter/tmpSize );
		
		// find player object
		player = GameObject.Find("Player");
		// fallback
		if(player == null) player = GameObject.Find("Main Camera");
		if(player == null) Debug.LogError("No active GameObject with the name «Player» has been found!");
		
		if(tailParticle==null) Debug.LogError("You must put attach tailParticle Prefab to the Creature Prefab!");
		
		// set speed
		//defaultSpeed = Random.Range(.2f,1.5f); // distance/second
		defaultSpeed = Mathf.Round( Random.Range (.51f,4.49f) );
		if(Random.Range(0f,1f) < .5f) defaultSpeed *= -1; // random direction
		//print(defaultSpeed);
		
		// random angle
		//currentAngle = Random.Range( 0, Mathf.PI*2 );
		currentAngle = 1;//Mathf.PI/2*uniqueNb;
		
		// place creature at initial spot
		Vector3 startPosition = new Vector3(Mathf.Cos(currentAngle)*distFromCenter, 0, Mathf.Sin(currentAngle)* distFromCenter );
		//this.transform.localPosition = startPosition;
		this.GetComponent<Rigidbody>().MovePosition(this.transform.parent.transform.position + startPosition);
		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		// recalc targetPosition
		CreatureSpawner tmp = this.GetComponentInParent<CreatureSpawner>();
		//print (Vector3.Distance( player.transform.position, this.transform.parent.transform.position ) +" - "+ tmp.domeDiameter/2);
		float tmpDistance = Vector3.Distance( player.transform.position, this.transform.parent.transform.position );
		if( tmpDistance > tmp.domeDiameter/2 || tmpDistance < 12 ){ // normal behaviour (circle around center)
			this.GetComponent<Rigidbody>().drag=50;
			//this.transform.localPosition = targetPosition;
			targetPosition = new Vector3(Mathf.Cos(currentAngle)*distFromCenter, this.transform.localPosition.y, Mathf.Sin(currentAngle)* distFromCenter );
			this.GetComponent<Rigidbody>().MovePosition(this.transform.position + (targetPosition-this.transform.localPosition)/5);
			
			currentAngle += defaultSpeed * Time.deltaTime; // deltaTime makes it frameRate-independent;
			currentAngle %= Mathf.PI*2;
		}
		
		else { // disturbed-by-player behaviour
			//this.rigidbody.mass=6;
			this.GetComponent<Rigidbody>().drag=.0f;
			targetPosition = new Vector3( player.transform.position.x, this.transform.localPosition.y, player.transform.position.z );
			
			// if close to camera, move your ass from here
			if( Vector3.Distance( targetPosition, this.transform.position ) > 5 ) this.GetComponent<Rigidbody>().AddForce( (targetPosition-this.transform.position)/5 , ForceMode.Force );
			// else get to camera
			else this.GetComponent<Rigidbody>().AddForce( this.GetComponent<Rigidbody>().velocity/10 , ForceMode.Force );
			//this.rigidbody.MovePosition(this.transform.position + (targetPosition-this.transform.localPosition)/5);
		}
		
		// make tail
		GameObject tmpParent = Instantiate( tailParticle , this.transform.position-new Vector3(0,0,0), tailParticle.transform.rotation) as GameObject;
		tmpParent.transform.parent = GameObject.Find("creatureTailParent").transform;
	}
}
