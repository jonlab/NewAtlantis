using UnityEngine;
using System.Collections;

public class NANetwork : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}




	[RPC]
	void SetColor(Color color) 
	{
		gameObject.GetComponent<MeshRenderer>().material.color = color;
	}

	[RPC]
	void Chat(string _name, string _message) 
	{
		ChatManager.Log(_name, _message, 0);
		LogManager.LogWarning(_name + " : " + _message);

	}



	[RPC]
	void DestroyObject(NetworkViewID viewID)
	{
		NetworkView nv = NetworkView.Find (viewID);
		GameObject go = nv.gameObject;
		lock(NA.player_objects)
		{
			Debug.Log ("removing " + viewID);
			NA.player_objects.Remove(go);
		}
		GameObject.Destroy(go);
	}



	[RPC]
	void SpawnAvatar(NetworkViewID viewID, Vector3 color, string name) 
	{
		//appelé chez tout le monde pour créer un avatar
		GameObject clone;
		//clone = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		clone = GameObject.Instantiate(NA.app.goPrefabAvatar, Vector3.zero, Quaternion.identity) as GameObject;

		clone.name = name;
		Collider.Destroy(clone.GetComponent<Collider>());
		NetworkView nView = clone.AddComponent<NetworkView>();
		nView.viewID = viewID;
		nView.stateSynchronization = NetworkStateSynchronization.Unreliable;


		Light l = clone.AddComponent<Light>();
		l.intensity = 2f;
		l.enabled = false;
		l.type = LightType.Spot;
		l.range = 100f;
		l.spotAngle = 90f;


		if (nView.owner == Network.player)
		{
			NA.goAvatar = clone;
		}
		else
		{
			//NA.player_objects.Add(clone); //this is considered as a player object
		}

		MeshRenderer renderer = clone.GetComponent<MeshRenderer>();
		if (renderer != null)
		{
			renderer.material.color = new Color(color.x, color.y, color.z, 0.3f);
		}
		else
		{
			Color col = new Color(color.x, color.y, color.z, 0.3f);
			Renderer[] renderers = clone.GetComponentsInChildren<Renderer>();
			Material m = null;
			foreach (Renderer r in renderers)
			{
				if (m == null)
				{
					m = r.material;

				}
				r.material = m;
				//r.sharedMaterial.color = col;
				m.color = col;
			}
		}
		clone.transform.parent = NA.app.goRootAvatars.transform;
		//LogManager.Log ("New Avatar:" +  name + " owner:" + nView.owner);
		LogManager.Log (name + " joined!");
		NA.AddAvatar(clone);


	}





	[RPC]
	void ConnectToSpace(string _space) 
	{
		//Connect(_space);
	}


	[RPC]
	void Refresh()
	{
		NAServer.Get();
	}


	[RPC]
	void LoadObject(string _name, NetworkViewID _viewID, Vector3 _pos, Vector3 _angles, Vector3 _scale, string _filename, string _id) 
	{
		//on regarde si l'object n'existe pas déjà
		foreach (NAObject o in NA.app.listObjects) 
		{
			if (o.id == _id)
				return;
		}
		Debug.Log ("RPC LoadObject " + _name + " " + _filename);
		// créer un objet vide pour la synchro, puis ajouter l'objet téléchargé en child
		NAObject n = new NAObject (NA.app.goRootSpace, _name, _pos, _angles, _scale, _filename, _viewID);
		n.id = _id;
		NA.app.listObjects.Add(n);
		n.Download();
	}

	[RPC]
	public void ServerCloneObject(string name, Vector3 position, Vector3 forward, Vector3 color) 
	{
		if (!Network.isServer)
		{
			return;
		}
		LogManager.Log ("ServerCloneObject");
		GetComponent<NetworkView>().RPC("CloneObject", RPCMode.AllBuffered, name, Network.AllocateViewID(), position, forward, color);
	}

	[RPC]
	void CloneObject(string name, NetworkViewID viewID, Vector3 location, Vector3 forward, Vector3 color) 
	{
		GameObject clone = null;
		GameObject model = null;
		foreach (NAObject o in NA.instanciables)
		{
			if (o.name == name)
			{
				model = o.go;
			}
		}
		//instead of cloning the root, we clone the first child (for synchronization purpose on physicallized objects)
		if (model.transform.childCount > 0)
		{
			model = model.transform.GetChild(0).gameObject;
		}

		clone = GameObject.Instantiate(model, Vector3.zero, Quaternion.identity) as GameObject;

		LogManager.LogWarning("clone " + name);

		//reset position of main child
		clone.transform.GetChild(0).gameObject.transform.localPosition = Vector3.zero;

		//remove previous network view
		NetworkView nViewOriginal = clone.GetComponent<NetworkView>();
		if (nViewOriginal)
		{
			NetworkView.Destroy(nViewOriginal);
		}

		NetworkView nView = clone.AddComponent<NetworkView>();
		nView.viewID = viewID;
		nView.stateSynchronization = NetworkStateSynchronization.Unreliable;

		clone.transform.position = location;

		MeshRenderer renderer = clone.GetComponent<MeshRenderer>();
		if (renderer != null)
		{
			renderer.material.color = new Color(color.x, color.y, color.z);
		}
		//clone.transform.forward = transform.rotation * Vector3.Normalize(forward) ;
		//Rigidbody rb = clone.AddComponent<Rigidbody>();
		if (NA.isServer() || NA.isStandalone())
		{
			/*Rigidbody rb = clone.AddComponent<Rigidbody>();
			rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
			rb.AddForce(forward*200f);
			*/
		}
		else
		{
			//client, we need the RB for local collisions but in kinematic mode only
			//rb.isKinematic = true;
		}
		NA.player_objects.Add(clone);
	}


	[RPC]
	public void SetLightState(string name, bool on, float intensity, float spotAngle, Vector3 eulerAngles) 
	{
		LogManager.Log("received SetLightState " + name + " " + on + " " + intensity);
		//find avatar
		//switch on light
		GameObject go = GameObject.Find(name);
		if (go != null)
		{
			Light l = go.GetComponent<Light>();
			if (l != null)
			{
				l.enabled = on;
				l.intensity = intensity;
				l.spotAngle = spotAngle;
				go.transform.eulerAngles = eulerAngles;
			}
		}
	}

    [RPC]
    public void DestroyAllSpaceObjects()
    {
        NA.app.DestroyAllSpaceObjects();
    }

}
