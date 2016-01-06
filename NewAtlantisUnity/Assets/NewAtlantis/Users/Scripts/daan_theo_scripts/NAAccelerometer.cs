using UnityEngine;
using System.Collections;

public class NAAccelerometer : MonoBehaviour {
	//[Header("Settings...")]
	[Tooltip("Size of the smoothing buffer in frames.")]
	public int smoothingBuffer = 60; // in frames
	
	// variables for use in other scripts
	[HideInInspector, System.NonSerialized]
	public Vector3 direction;
	//[HideInInspector, System.NonSerialized]
	public float averageFriction = 0;
	
	private int[] frictionTable;
	private Vector3 lastPosition;

	// Use this for initialization
	void Start () {
		frictionTable = new int[smoothingBuffer];
		for(int i=0; i<smoothingBuffer; i++) frictionTable[i]=0;
		
		direction = Vector3.zero;
		lastPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		direction = this.transform.position - lastPosition;
		averageFriction = direction.magnitude;
		
		// cal average friction
		for(int i=frictionTable.Length-1; i>0; i--){
			frictionTable[i] = frictionTable[i-1];
			averageFriction += frictionTable[i];
		}
		frictionTable[0] = Mathf.RoundToInt(direction.magnitude);
		averageFriction /= frictionTable.Length;
		
		// remember last position
		lastPosition = this.transform.position;
	}
}
