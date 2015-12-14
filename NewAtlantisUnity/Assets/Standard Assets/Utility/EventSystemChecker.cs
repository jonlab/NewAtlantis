<<<<<<< HEAD
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemChecker : MonoBehaviour
{
    //public GameObject eventSystem;

	// Use this for initialization
	void Awake ()
	{
	    if(!FindObjectOfType<EventSystem>())
        {
           //Instantiate(eventSystem);
            GameObject obj = new GameObject("EventSystem");
            obj.AddComponent<EventSystem>();
            //obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;
            obj.AddComponent<TouchInputModule>();
        }
	}
}
=======
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemChecker : MonoBehaviour
{
    //public GameObject eventSystem;

	// Use this for initialization
	void Awake ()
	{
	    if(!FindObjectOfType<EventSystem>())
        {
           //Instantiate(eventSystem);
            GameObject obj = new GameObject("EventSystem");
            obj.AddComponent<EventSystem>();
            //obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;
            obj.AddComponent<TouchInputModule>();
        }
	}
}
>>>>>>> cc58b2cb32f6563ea23f0550281efd5fb4b5637f
