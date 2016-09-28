using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]

public class StreamGUI : MonoBehaviour {


	private AudioSource _source;
	private AudioClip myAudio;
	public WWW bookLoader;
	public string url = "http://ia902707.us.archive.org/11/items/NearlyCompleteHPLovecraftCollection/01_The_Whisperer_in_Darkness_01.mp3";

	void Start()
	{

		DontDestroyOnLoad(this);
		_source = this.GetComponent<AudioSource>();

	}

	void Update()
	{

		if (!this._source.isPlaying)
			this._source.Play ();

	}

	void OnGUI(){
		if (GUI.Button (new Rect (Screen.width/2-50, Screen.height-150, 100, 100), "Stream Book"))
			startBookNow ();
	}

	public void startBookNow()
	{

		StartCoroutine (LoadBook (url));

	}


	IEnumerator LoadBook(string url) {

		Debug.Log("load book");
		this.bookLoader = new WWW (url);
		yield return bookLoader;
		Debug.Log("after yield");
		if (bookLoader != null && bookLoader.isDone) {

			if(_source.isPlaying) {
				this._source.Stop();
			}
			myAudio = bookLoader.GetAudioClip(false,true) as AudioClip;
			this._source.clip = myAudio;
			this._source.loop = false;
			this._source.Play();
			Debug.Log ("Playing!");

		}

	}
}


