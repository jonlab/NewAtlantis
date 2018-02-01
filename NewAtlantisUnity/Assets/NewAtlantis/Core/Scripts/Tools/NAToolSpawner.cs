using UnityEngine;
using System.Collections;


public class NAToolSpawner : NAToolBase {


	public GameObject 	prefab; 
	public float 		distance = 1f;
	public Vector3 		localForce = Vector3.forward;
	public string 		objectName = "dopplertrunk";// TEST 
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
		//on each client (including server)
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
            //Debug.LogError("ADD DOPPLER TRUNK in NATOOLSPAWNER" + name);

            clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
			clone.transform.position = location;
			clone.transform.localScale = new Vector3(1f,0.4f, 0.6f);
			AudioSource src = clone.AddComponent<AudioSource>();
			src.playOnAwake = false;
            src.spatialBlend = 1f;
            clone.AddComponent<NAPhysicsAudioSource>();
			clone.AddComponent<NAPlayOnCollide>();
            if (name != "dopplertrunk") 
			{ 
				NAAudioRecorder trunk = clone.AddComponent<NAAudioRecorder>(); 
				if (name.Contains("auto"))
				{
					//auto trunk loads automatically
					trunk.AutoLoad = true;
				}
			} 
			else 
			{ 
				clone.AddComponent<NAAudioDopplerRecorder>(); 
			}
			NA.DecorateAudioSource(src); //FIXME
			clone.AddComponent<NAPlayOnMidi>();
			clone.AddComponent<NetworkSync>(); //FIXME


		}
		else if (name == "cube")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}
		else if (name == "lmacube")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Cube);
			clone.AddComponent<NAPlayImpactOnCollide>();
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
		else if (name == "monster")
		{
			clone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			clone.transform.localScale = Vector3.one * 0.1f;
			//NAAnimalBehavior ab  =clone.AddComponent<NAAnimalBehavior> ();
			clone.AddComponent<Light> ();


			//GameObject goAudioSource1 = new GameObject ();
			//AudioSource as1 = goAudioSource1.AddComponent<AudioSource> ();

			//GameObject goAudioSource2 = new GameObject ();
			//AudioSource as2 = goAudioSource2.AddComponent<AudioSource> ();
			/*
			goAudioSource1.transform.parent = clone.transform;
			goAudioSource2.transform.parent = clone.transform;
			goAudioSource1.transform.localPosition = Vector3.zero;
			goAudioSource2.transform.localPosition = Vector3.zero;

			ab.audioSourceMove = as1;
			ab.audioSourceStand = as2;
			*/
			AudioSource src = clone.AddComponent<AudioSource>();
			src.playOnAwake = true;
			src.spatialBlend = 1f;

			NAAudioRecorder trunk = clone.AddComponent<NAAudioRecorder>(); 
			trunk.AutoLoad = true;
			trunk.AutoPlay = true;


		}	
		else if (name == "Voices")
		{
			clone = new GameObject ("Voice");
			AudioSource src = clone.AddComponent<AudioSource>();
			src.playOnAwake = true;
			src.loop = true;
			src.spatialBlend = 1f;
			NAAudioRecorder trunk = clone.AddComponent<NAAudioRecorder>(); 
			trunk.directory = "SoundFiles/Voices";
			trunk.AutoLoad = true;
			trunk.AutoPlay = true;
			clone.AddComponent<NAAiBoid>(); 
			//TO DO
			//NAAudioSynthGranularSynthesis granular = clone.AddComponent<NAAudioSynthGranularSynthesis> ();
			//granular.SourceClip = 

		}
		else if (name == "Hydrophone")
		{
			clone = new GameObject ("Hydrophone");
			AudioSource src = clone.AddComponent<AudioSource>();
			src.playOnAwake = true;
			src.loop = true;
			src.spatialBlend = 1f;
			NAAudioRecorder trunk = clone.AddComponent<NAAudioRecorder>(); 
			trunk.directory = "SoundFiles/Hydrophone";
			trunk.AutoLoad = true;
			trunk.AutoPlay = true;
			clone.AddComponent<NAAiBoid>(); 

			//add RISSET Plugin

			//TO DO
			//NAAudioSynthGranularSynthesis granular = clone.AddComponent<NAAudioSynthGranularSynthesis> ();
			//granular.SourceClip = 

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
			if (name.Contains("nog") || name.Contains("monster"))
			{
				//no g
				
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
