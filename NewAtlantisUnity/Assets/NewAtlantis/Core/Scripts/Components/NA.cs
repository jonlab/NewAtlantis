using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public  enum SyncMode
{
	FullAuthoritative,
	RigibodiesAndAudioSources,
	AudioSourcesOnly,
	NoInDepthSync
}
//main New Atlantis engine class
public static class NA
{
	private static List<GameObject> listAvatars = new List<GameObject>();
	public static App app = null;
	public static NANetwork network = null;
	public static List<GameObject>	player_objects = new List<GameObject>();
	public static GameObject goAvatar = null;
	public static Space CurrentSpace = null;
	public static Space PreviousSpace = null;
	public static Font[] fonts = new Font[4];

	public static SyncMode syncMode = SyncMode.NoInDepthSync;

    public static float JoystickSmoothing = 0.92f;

	public static Vector3 colorAvatar;

	public static List<NAObject>	instanciables = new List<NAObject>();

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

	public static bool isViewer(GameObject go)
	{
		App app = go.GetComponent<App>();
		return (app != null);
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
	public static bool bAugmentAudioSources = false;

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

	public static void AddAvatar(GameObject _goAvatar)
	{
		listAvatars.Add(_goAvatar);
	}

	public static void RemoveAvatar(GameObject _goAvatar)
	{
		listAvatars.Remove(_goAvatar);
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
				if (pos.y < -1000) //destroy object under y == -100
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

	public static void PausePhysics()
	{
		Time.timeScale = 0;
	}
	
	public static void PlayPhysics()
	{
		Time.timeScale = 1;
	}

	public static Font GetFont(int level)
	{
		return fonts[level];
	}

	public static void PatchAllMaterials(GameObject root)
	{
		
		Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers)
		{
			//hack to reaffect the shader
			//r.sharedMaterial.shader = Shader.Find(r.sharedMaterial.shader.name);
			//r.material.shader = Shader.Find(r.material.shader.name);
			foreach (Material m in r.sharedMaterials)
			{
				try
				{
					if (m.shader != null)
					{
					
						Shader s = Shader.Find(m.shader.name);
						if (s != null)
						{
							m.shader = s;
							//   LogManager.LogError("Shader name : " + m.shader.name);
						}
						else
						{
							LogManager.LogWarning("can't find shader : " + m.shader.name);
						}
					}
				}
				catch (System.Exception e)
				{
					LogManager.LogWarning("shader exception");
				}
			}
			/*foreach (Material m in r.materials)
			{
				m.shader = Shader.Find(m.shader.name);
			}*/
		}
		Terrain[] terrains = root.GetComponentsInChildren<Terrain>();
		foreach (Terrain t in terrains)
		{
			t.terrainData.RefreshPrototypes();

			//t.terrainData.
			foreach (TreePrototype tp in t.terrainData.treePrototypes)
			{
				if (tp.prefab != null)
				{
					Renderer[] trenderers = tp.prefab.GetComponentsInChildren<Renderer>();
					foreach (Renderer r in trenderers)
					{
						Debug.Log("=>patching " + r.name);
						foreach (Material m in r.sharedMaterials)
						{
							
							try
							{
								if (m.shader != null)
								{

									string sname = m.shader.name;

									if (m.shader.name.Contains("Bark"))
									{
										sname = "Nature/Tree Creator Bark";
									}
									else if (m.shader.name.Contains("Leaves"))
									{
										sname = "Nature/Tree Creator Leaves";
									}
									Shader s = Shader.Find(sname);
									if (s != null)
									{
										m.shader = s;
										//   LogManager.LogError("Shader name : " + m.shader.name);
									}
									else
									{
										LogManager.LogWarning("can't find shader : " + m.shader.name);
									}
								}
							}
							catch (System.Exception e)
							{
								LogManager.LogWarning("shader exception");
							}
						}
					}
				}

			}

		}

	}
	public static void PatchMaterials(GameObject root)
	{
		try
		{
			Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
			foreach (Renderer r in renderers)
			{
				if (r.sharedMaterial != null)
				{
					if (
						(r.sharedMaterial.shader.name == "Standard") ||
						(r.sharedMaterial.shader.name == "Diffuse") ||
						 (r.sharedMaterial.shader.name == "Specular") ||
						(r.sharedMaterial.shader.name == "FX/Water") ||
						(r.sharedMaterial.shader.name.Contains("Custom")) ||
						(r.sharedMaterial.shader.name.Contains("Particles")) ||
							(r.sharedMaterial.shader.name.Contains("Legacy"))
					)
					{
						//hack to reaffect the shader
						foreach (Material m in r.sharedMaterials)
						{
							m.shader = Shader.Find(m.shader.name);
						}
					}

				}
			}

					/*//exceptions
					if (
						(r.sharedMaterial.name == "WaterProDaytime") ||
						(r.sharedMaterial.name.Contains("transparentcolonne")) ||
						(r.sharedMaterial.name == "Collider") ||
						(r.sharedMaterial.name == "New Material 1") || //luca
						(r.sharedMaterial.name == "CupolaMat") || //luca
						(r.sharedMaterial.name == "Default-Particle") || //luca
						(r.sharedMaterial.name == "tram_vert") || //oscar
						(r.sharedMaterial.name == "transparent") || //oscar
						(r.sharedMaterial.name == "ParticleBokeh") ||  //oscar
						(r.sharedMaterial.name == "material-nuages") || //juliette
						(r.sharedMaterial.name == "quadrillage_1") ||  //marion
						(r.sharedMaterial.name == "quadrillage_2") ||  //marion
						(r.sharedMaterial.name == "quadrillage_3") ||  //marion
						(r.sharedMaterial.name == "quadrillage_4") ||  //marion
						(r.sharedMaterial.name == "DEGRADE1") ||  //marion
						(r.sharedMaterial.name == "0") ||  //marion
						//234
						(r.sharedMaterial.name == "DEGRADE5")   //marion

					)
					{
						//r.sharedMaterial.shader = Shader.Find(r.sharedMaterial.shader.name);
						r.sharedMaterial.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
					}
				}

				//r.material.shader = Shader.Find(r.material.shader.name);
				//r.material.SetFloat("", 0);
				//r.material.CopyPropertiesFromMaterial(r.material);
				//r.material = Material.Instantiate(r.material);
				//r.sharedMaterial = Material.Instantiate(r.sharedMaterial);
				//Shader.WarmupAllShaders();
				//r.material.shader = r.material.shader;
				//r.material.shader = Shader.Instantiate(r.material.shader);
				//r.sharedMaterial.shader.
				//r.sharedMaterial.
				//r.sharedMaterial.shader = Shader.Find("Transparent/Diffuse");
				//r.sharedMaterial.shader.maximumLOD = r.sharedMaterial.shader.maximumLOD+1;
				//r.sharedMaterial.
				//r.sharedMaterial.SetPass(0);


			}



			/*ParticleSystem[] particles = root.GetComponentsInChildren<ParticleSystem>();
						foreach (ParticleSystem p in particles)
						{
							if (p.sharedMaterial != null)
							{
								//exceptions
								if (
									(r.sharedMaterial.name == "WaterProDaytime") ||
									(r.sharedMaterial.name.Contains("transparentcolonne")) ||
									(r.sharedMaterial.name == "Collider") ||
									(r.sharedMaterial.name == "Default-Particle") 
								)
								{
									r.sharedMaterial.shader = Shader.Find(r.sharedMaterial.shader.name);
								}
							}
						}
				*/		

			/*
			Material[] materials = Object.FindObjectsOfType(typeof(Material)) as Material[];
			foreach (Material m in materials)
			{
				Debug.Log("Material : " + m.name);

				m.shader = Shader.Find(m.shader.name); //hack to force reapply of Shader (Unity 5.3 bug)
			}
			*/
		}
		catch (System.Exception e)
		{
			LogManager.LogError("Patch materials error");
		}
	}



	public static Texture2D GeneratePreviewPNG(GameObject model, int width, int height)
	{
		//RenderSettings.ambientLight = Color.white;
		//we will use the layer 12 to render selectively
		Vector3 pos = Vector3.zero;
		List<GameObject> temp = new List<GameObject>();
		GameObject go = GameObject.Instantiate(model) as GameObject;

		Transform[] transforms = go.GetComponentsInChildren<Transform>();
		float maxsize = 0;
		foreach (Transform t in transforms)
		{
			Renderer r = t.GetComponent<Renderer>();
			if (r != null)
			{
				if (r.bounds.extents.x > maxsize)
					maxsize = r.bounds.extents.x;
				if (r.bounds.extents.y > maxsize)
					maxsize = r.bounds.extents.y;
				if (r.bounds.extents.z > maxsize)
					maxsize = r.bounds.extents.z;
			}
			t.gameObject.layer = 12;
		}

		//go.layer = 12;
		pos = go.transform.position;
		temp.Add (go);

		//camera
		GameObject goCamera = new GameObject();
		Camera cam = goCamera.AddComponent<Camera>();

		//cam position 
		//goCamera.transform.position = pos+Vector3.forward*10+Vector3.up*5; //FIXME
		goCamera.transform.position = pos+Vector3.forward*maxsize+Vector3.up*maxsize/2f+Vector3.right*maxsize/2f;
		goCamera.transform.LookAt(pos);
		goCamera.layer = 12;

		//light
		GameObject goLight = new GameObject();
		Light light = goCamera.AddComponent<Light>();
		light.type = LightType.Directional;
		light.intensity = 1;
		goLight.layer = 12;


		Texture2D screenShot = null;
		RenderTexture rt  = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
		//Render Camera
		RenderTexture.active = rt;
		cam.targetTexture = rt;
		Rect rectBak = cam.rect;
		cam.rect = new Rect(0,0,1,1);
		cam.clearFlags = CameraClearFlags.Color;
		cam.backgroundColor = Color.black;
		cam.cullingMask = 1<<12;
		cam.Render();
		cam.rect = rectBak;
		//currentCamera.GetComponent<GUILayer>();
		//Create the blank texture container
		if (screenShot == null)
		{
			screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);
		}
		//Assign rt as the main render texture, so everything is drawn at the higher resolution
		RenderTexture.active = rt;
		//Read the current render into the texture container, screenShot
		screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
		screenShot.Apply();


		//destroy 

		RenderTexture.active = null;
		cam.targetTexture = null;
		RenderTexture.Destroy(rt);
		GameObject.DestroyImmediate(goCamera);
		GameObject.DestroyImmediate(goLight);
		foreach (GameObject g in temp)
		{
			GameObject.DestroyImmediate(g);
		}

		return screenShot;
	}

	public static GameObject PickObject(Ray ray, out RaycastHit hit)
	{
		if (Physics.Raycast(ray, out hit))
		{
			return hit.collider.gameObject;
		}
		hit = new RaycastHit();
		return null;
	}

	public static string GetGameObjectPath(Transform transform)
	{
		string path = transform.name;
		while (transform.parent != null)
		{
			transform = transform.parent;
			path = transform.name + "/" + path;
		}
		return path;
	}



}
