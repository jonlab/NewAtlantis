using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//main New Atlantis engine class
public static class NA
{
	private static List<GameObject> listAvatars = new List<GameObject>();
	public static App app = null;
	public static List<GameObject>	player_objects = new List<GameObject>();
	public static GameObject goAvatar = null;

	public static Space CurrentSpace = null;
	public static Space PreviousSpace = null;


	public static GameObject Instantiate(Object prefab, Vector3 position, Quaternion rotation)
	{
		if (isServer() || isClient())
		{
			//The Network.Instantiate needs the prefab to be in the Assets of the built project...
			return Network.Instantiate(prefab, position, rotation, 0) as GameObject;

			GameObject go = GameObject.Instantiate(prefab, position, rotation) as GameObject;
			return go;
		}
		else
		{
			return GameObject.Instantiate(prefab, position, rotation) as GameObject;

		}

	}

	public static bool isClient()
	{
		return Network.isClient;
	}

	public static bool isServer()
	{
		return Network.isServer;
	}

	public static bool isStandalone()
	{
		return (!Network.isClient && !Network.isServer);
	}



	public static AudioListener listener = null;
	//public static Camera currentcam = null;
	public static bool bAugmentAudioSources = true;

	//public static bool bDisplayAudioSourceName = true;





	public static void DecorateAudioSource(AudioSource s)
	{
		NAReverbEffector eff = s.gameObject.GetComponent<NAReverbEffector>();
		if (eff == null)
			s.gameObject.AddComponent<NAReverbEffector>();
			
		AudioReverbFilter arf = s.gameObject.GetComponent<AudioReverbFilter>();
		if (arf == null)
			s.gameObject.AddComponent<AudioReverbFilter>();
		
		
		NAAudioSource src = s.gameObject.GetComponent<NAAudioSource>();
		if (src == null)
			s.gameObject.AddComponent<NAAudioSource>();
		
		if (NA.bAugmentAudioSources)
		{

			
			NAOcclusionFX occ = s.gameObject.GetComponent<NAOcclusionFX>();
			if (occ == null)
			{
				s.gameObject.AddComponent<NAOcclusionFX>();
				//workaround
				NAOcclusionFX occ2 = s.gameObject.AddComponent<NAOcclusionFX>();
				NAOcclusionFX.Destroy(occ2);
			}

		}

		/*if (Network.isServer)
		{
			NATogglePlayOnCollide poc = s.gameObject.AddComponent<NATogglePlayOnCollide>();
		}
		*/
		//

	}

	public static void AddAvatar(GameObject goAvatar)
	{
		listAvatars.Add(goAvatar);
	}

	public static void RemoveAvatar(GameObject goAvatar)
	{
		listAvatars.Remove(goAvatar);
	}

	public static void ClearAvatars()
	{
		listAvatars.Clear();
	}

	public static GameObject GetClosestAvatar(Vector3 position, float MaxDistance)
	{
		float min = MaxDistance;
		GameObject avatar = null;
		foreach (GameObject a in listAvatars)
		{
			float distance = (a.transform.position-position).magnitude;
			if (distance<min)
			{
				min = distance;
				avatar = a;
			}
		}
		return avatar;
	}


	public static List<GameObject> GetAvatars()
	{
		return listAvatars;
	}

	public static void DestroyPlayerObjects()
	{
		foreach (GameObject go in player_objects)
		{
			if (Network.isServer || Network.isClient)
			{
				Network.Destroy(go);
			}
			else
			{
				GameObject.Destroy(go);
			}
		}
		player_objects.Clear();
		
		GameObject.Destroy(goAvatar);
		goAvatar = null;
		
		NA.ClearAvatars();

	}


	public static void DestroyPlayerObjects2()
	{
		foreach (GameObject go in player_objects)
		{
			GameObject.Destroy(go);
		}
		player_objects.Clear();
		
		GameObject.Destroy(goAvatar);
		goAvatar = null;
	}

	public static void GC()
	{
		//lock(player_objects)
		{
			List<GameObject> listRemove = new List<GameObject>();
			foreach (GameObject go in player_objects)
			{
				Vector3 pos = go.transform.position;
				if (pos.y < -100) //destroy object under y == -100
				{
					if (!Network.isClient)
					{
						listRemove.Add (go);
						//NetworkView nv = go.GetComponent<NetworkView>();
						//GetComponent<NetworkView>().RPC("DestroyObject", RPCMode.AllBuffered, nv.viewID);
						//go.SetActive(false);
						/*if (Network.isServer)
						{
							Network.Destroy(go);
							player_objects.Remove(go);
						}
		                else if (!Network.isClient)
		                {
		                    GameObject.Destroy(go);
							player_objects.Remove(go);
						}
						break;
						*/
					}
					
				}
				
				
			}
			foreach (GameObject go in listRemove)
			{
				player_objects.Remove(go);
				if (Network.isServer)
				{
					NetworkView nv = go.GetComponent<NetworkView>();
					app.GetComponent<NetworkView>().RPC("DestroyObject", RPCMode.AllBuffered, nv.viewID);
				}
				else if (!Network.isClient)
				{
					GameObject.Destroy(go);
				}
				
			}
			listRemove.Clear();
		}
	}

	public static void SetAvatarPositionAndAngles(Vector3 pos, Vector3 angles)
	{
		if (goAvatar)
		{
			goAvatar.transform.position 	= pos;
			goAvatar.transform.eulerAngles 	= angles;
		}
	}

}
