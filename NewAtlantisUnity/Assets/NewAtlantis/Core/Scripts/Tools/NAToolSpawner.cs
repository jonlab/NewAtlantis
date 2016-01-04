using UnityEngine;
using System.Collections;

public class NAToolSpawner : NAToolBase {


	public GameObject goPrefabCubeSimple; 
	public float distance = 1f;
	public Vector3 localForce = Vector3.forward;
	public string objectName = "cube";
	/*public NAToolSpawner ()
	{
		name = "spawner";
	}*/

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void Action() 
	{
		Vector3 worldforce = transform.rotation * localForce;
		if (Network.isServer)
		{
			ServerSpawnObject(objectName, transform.position+transform.forward*distance, worldforce, new Vector3(1,0,0)/*colorAvatar*/);
		}
		else
		{
			//we send to the server
			GetComponent<NetworkView>().RPC("ServerSpawnObject", RPCMode.Server, objectName, transform.position+transform.forward*distance, worldforce, new Vector3(1,0,0)/*colorAvatar*/);
		}
	}

	public override void Press() 
	{
		
	}

	public override void Maintain() 
	{
		
	}

	public override void Release() 
	{
		
	}



	[RPC]
	void SpawnBox(NetworkViewID viewID, Vector3 location) 
	{
		//Transform clone;
		GameObject clone;
		//clone = Instantiate(cubePrefab, location, Quaternion.identity) as Transform as Transform;
		clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
		NetworkView nView = clone.AddComponent<NetworkView>();
		//NetworkView nView;
		//nView = clone.GetComponent<NetworkView>();
		nView.viewID = viewID;
		
		if (Network.isServer)
		{
			clone.AddComponent<Rigidbody>();
		}
	}
	
	
	[RPC]
	void ServerSpawnObject(string name, Vector3 position, Vector3 forward, Vector3 color) 
	{
		if (!Network.isServer)
		{
			return;
		}
		LogManager.Log ("ServerSpawnObject");
		GetComponent<NetworkView>().RPC("SpawnObject", RPCMode.AllBuffered, name, Network.AllocateViewID(), position, forward, color);
	}
	
	[RPC]
	void SpawnObject(string name, NetworkViewID viewID, Vector3 location, Vector3 forward, Vector3 color) 
	{
		GameObject clone = null;
		if (name == "cube")
		{
			clone = GameObject.Instantiate(goPrefabCubeSimple, Vector3.zero, Quaternion.identity) as GameObject;
			//clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}
		else if (name == "sphere")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			clone.transform.localScale = Vector3.one*0.2f;
			AudioSource audio = clone.AddComponent<AudioSource>();
			audio.spatialBlend = 1.0f;
			NAAudioSynthFM fm = clone.AddComponent<NAAudioSynthFM>();
			fm.duration = 1f;
			fm.CarrierFrequency = 20f+Random.value*4000f;
			fm.ModulatorFrequency = Random.value*40f;
			fm.ModulationAmount = Random.value*0.2f;
			clone.AddComponent<NAPlayOnCollide>();
			audio.playOnAwake = false;
			fm.Compute();
		}
		else if (name == "cylinder")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		}
		else if (name == "trunk")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
			clone.transform.position = location;
			clone.transform.localScale = new Vector3(1f,0.4f, 0.6f);
			clone.GetComponent<Renderer>().material.color = Color.red;
			AudioSource src = clone.AddComponent<AudioSource>();
			src.playOnAwake = false;
			//clone.AddComponent<NAPlayOnCollide>();
			clone.AddComponent<NAAudioRecorder>();
			NA.DecorateAudioSource(src);
			clone.AddComponent<NetworkSync>();
		}
		else
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}
		
		NetworkView nView = clone.AddComponent<NetworkView>();
		nView.viewID = viewID;
		
		clone.transform.position = location;
		
		MeshRenderer renderer = clone.GetComponent<MeshRenderer>();
		if (renderer != null)
		{
			renderer.material.color = new Color(color.x, color.y, color.z);
		}
		clone.transform.forward = transform.rotation * Vector3.Normalize(forward) ;
		//Rigidbody rb = clone.AddComponent<Rigidbody>();
		if (NA.isServer() || NA.isStandalone())
		{
			Rigidbody rb = clone.AddComponent<Rigidbody>();
			rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
			rb.AddForce(forward*200f);
		}
		else
		{
			//client, we need the RB for local collisions but in kinematic mode only
			//rb.isKinematic = true;
		}
		NA.player_objects.Add(clone);
	}

}
