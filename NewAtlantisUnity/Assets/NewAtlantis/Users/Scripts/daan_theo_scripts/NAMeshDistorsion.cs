using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(NAAccelerometer))]
public class NAMeshDistorsion : MonoBehaviour {
	
	[Space(15f)]
	[Tooltip("Between 0.0 and 0.99 for best results.")]
	public float amplitude = 5f;
	public float frictionSensitivity = 1.0f;
	[Tooltip("Not null nor negative")]
	public int distorsionLevel = 6;
	
	private Vector3[] originalVertices;
	//private float trickedFriction;
	private float offsetPosition;
	private float velocity;
	//private float meshHeight=1;
	
	private int renderVariant=0;
	private int curDirection=0;
	
	// Use this for initialization
	void Start () {
	
		// copy original vertices
		originalVertices = GetComponent<MeshFilter>().mesh.vertices;
		
		offsetPosition = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// failed to initialize ?
		if(originalVertices.Length<1) return;
		
		// handle audio
		float friction = this.gameObject.GetComponent<NAAccelerometer>().averageFriction * frictionSensitivity;
		//direction = (this.gameObject.GetComponent<NAFriction>().direction);
		
		velocity *= 0.99f; // reduce velocity
		velocity -= offsetPosition*.1f; // make velocity return to the center (pivot)
		velocity += friction * ( velocity>0 ? 1f:-1f ); // apply friction to element
		
		offsetPosition += velocity;
		
		// turn render variant ?
		int dir = (offsetPosition >= 0)?1:-1;
		if( curDirection != dir && offsetPosition < 0.2f){
			renderVariant++;
			renderVariant %= distorsionLevel;
			curDirection = dir;
		}
		
		// mesh modification
		Mesh myMesh = GetComponent<MeshFilter>().mesh;
		
		Vector3[] vertices = new Vector3[originalVertices.Length];
		Vector3[] normals = myMesh.normals;
		int i = 0;
		while (i < vertices.Length) {
			Vector3 pivot = myMesh.bounds.extents;
			float heightPercentage = ( (originalVertices[i].y+pivot.y)/myMesh.bounds.size.y );
			
			//vertices[i] = originalVertices[i] + normals[i] * ( ((float)((i+renderVariant)%distorsionLevel))/distorsionLevel)*offsetPosition*amplitude;
			vertices[i] = originalVertices[i] + normals[i] * Mathf.Round(-.3f + ((float)((i+renderVariant)%distorsionLevel))/distorsionLevel)*offsetPosition*amplitude;
			
			i++;
		}
		myMesh.vertices = vertices;
		myMesh.RecalculateBounds();
		
		//meshHeight = myMesh.bounds.extents.y*2;
		
	}
	
	// returns a value between -1f and 1f representing the flubber movement
	public float getDistorsion(){
		float ret = offsetPosition;
		//ret *= meshHeight;
		
		// stabilize mouvement ?
		if( Mathf.Abs(ret) < 0.001f ){
			ret=0;
		}
		
		return ret;
	}
	
	// automatically called on collision enter
	void OnTriggerEnter(Collider other){
		velocity += Random.value;
	}
}
