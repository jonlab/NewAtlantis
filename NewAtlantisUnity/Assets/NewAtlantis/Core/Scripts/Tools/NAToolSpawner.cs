using UnityEngine;
using System.Collections;



public enum SpawnMode
{
	Raycast,
	InFront
	
}

public class NAToolSpawner : NAToolBase {


	public GameObject 	prefab; 
	public float 		distance = 1f;
	public Vector3 		localForce = Vector3.forward;
	public string 		objectName = "cube";
	public SpawnMode 	mode = SpawnMode.InFront;
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
		Vector3 position = transform.position+transform.forward*distance;
		//test
		if (mode == SpawnMode.Raycast)
		{
			RaycastHit hit;
			GameObject go = NA.PickObject(new Ray(transform.position, transform.forward), out hit);
			if (go != null)
			{
				position = hit.point;
				position += new Vector3(0,0.5f,0);
				//worldforce = Vector3.zero;
			}
		}

		if (Network.isServer)
		{
			ServerSpawnObject(objectName, position, worldforce, NA.colorAvatar);
		}
		else
		{
			//we send to the server
            GetComponent<NetworkView>().RPC("ServerSpawnObject", RPCMode.Server, objectName, position, worldforce, NA.colorAvatar);
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
		LogManager.Log("client : SpawnObject");
		GameObject clone = null;
		if (name == "sphere")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			clone.transform.localScale = Vector3.one*1f;
			AudioSource audio = clone.AddComponent<AudioSource>();
			clone.AddComponent<NAPhysicsAudioSource>();
			clone.AddComponent<NAReverbEffector>();
			NAAudioSynthFM fm = clone.AddComponent<NAAudioSynthFM>();
			PhysicMaterial m = new PhysicMaterial();
			m.bounciness = 1f;
			m.bounceCombine = PhysicMaterialCombine.Maximum;
			Collider c = clone.GetComponent<Collider>();
			c.material = m;

			fm.duration = 1f;
			fm.CarrierFrequency = 20f+Random.value*1000f;
			fm.ModulatorFrequency = Random.value*10f;
			fm.ModulationAmount = Random.value*0.3f;
			clone.AddComponent<NAPlayOnCollide>();
			audio.playOnAwake = false;
			fm.Compute();
		}
		else if (name == "cylinder")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		}
		else if (name.Contains("trunk"))
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
			clone.transform.position = location;
			clone.transform.localScale = new Vector3(1f,0.4f, 0.6f);
			AudioSource src = clone.AddComponent<AudioSource>();
			src.playOnAwake = false;
            src.spatialBlend = 1f;
            clone.AddComponent<NAPhysicsAudioSource>();
			clone.AddComponent<NAPlayOnCollide>();
			clone.AddComponent<NAAudioRecorder>();
			NA.DecorateAudioSource(src); //FIXME
			clone.AddComponent<NetworkSync>(); //FIXME
		}
		else if (name == "cube")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}

		else if (name == "CubeColor")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
			clone.AddComponent<NAColorRandomChanger>();
		}
		else if (name == "capsule")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		}
		else
		{
			//cas général
			clone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//clone = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
			//clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
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
			if (name.Contains("nog"))
			{
				
			}
			else
			{
				Rigidbody rb = clone.AddComponent<Rigidbody>();
				rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
				rb.AddForce(forward*200f);
			}

		}
		else
		{
			//client, we need the RB for local collisions but in kinematic mode only
			//rb.isKinematic = true;
		}
		NA.player_objects.Add(clone);
	}

}
