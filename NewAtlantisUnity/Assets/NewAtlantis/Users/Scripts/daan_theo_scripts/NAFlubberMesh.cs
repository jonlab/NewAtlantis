using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(NAAccelerometer))]
public class NAFlubberMesh : MonoBehaviour {
	
	[Space(15f)]
	[Tooltip("Between 0.0 and 0.99 for best results.")]
	public float flubberness = .91f; // between 0.0 and 1.0
	public float rigidity = .2f;
	public float frictionSensitivity = 1.0f;
	
	private Vector3[] originalVertices;
	//private float trickedFriction;
	private Vector3 offsetPosition;
	private Vector3 velocity;
	private Vector3 direction;
	private float meshHeight=1;
	
	// Use this for initialization
	void Start () {
		// copy original vertices
		originalVertices = GetComponent<MeshFilter>().mesh.vertices;
		
		offsetPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		// failed to initialize ?
		if(originalVertices.Length<1) return;
		
		// handle audio
		float friction = this.gameObject.GetComponent<NAAccelerometer>().averageFriction * frictionSensitivity;
		direction = (this.gameObject.GetComponent<NAAccelerometer>().direction);
		
		velocity *= flubberness; // reduce velocity
		velocity -= offsetPosition*rigidity; // make velocity go to the center (pivot)
		velocity -= direction*friction; // apply opject direction to velocity
		
		offsetPosition += velocity;
		
		// mesh modification
		Mesh myMesh = GetComponent<MeshFilter>().mesh;
		
		Vector3[] vertices = new Vector3[originalVertices.Length];
		//Vector3[] normals = myMesh.normals;
		int i = 0;
		while (i < vertices.Length) {
			Vector3 pivot = myMesh.bounds.extents;
			float heightPercentage = ((originalVertices[i].y+pivot.y)/myMesh.bounds.size.y );
			vertices[i] = originalVertices[i] + offsetPosition * heightPercentage;
			i++;
		}
		myMesh.vertices = vertices;
		myMesh.RecalculateBounds();
		
		meshHeight = myMesh.bounds.extents.y*2;
		
	}
	
	// returns a value between -1f and 1f representing the flubber movement
	public float getFlubberMovement(){
		float ret = offsetPosition.x+offsetPosition.z;
		ret *= meshHeight;
		
		// stabilize mouvement ?
		if( Mathf.Abs(ret) < 0.001f ){
			ret=0;
		}
		
		return ret;
	}
	
	// automatically called on collision enter
	void OnTriggerEnter(Collider other){
		velocity += new Vector3(Random.value, 0f, Random.value);
	}
}
