using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class mainaudio : MonoBehaviour {

    float counter = 0f;
    float prevcounter = 0f;

    public static int current_key = 0;
    public static int current_intensity = 0;


    public List<AudioClip> stems0 = new List<AudioClip>();
    public List<AudioClip> stems1 = new List<AudioClip>();
    public List<AudioClip> stems2 = new List<AudioClip>();
    public List<AudioClip> stems3 = new List<AudioClip>();
    public List<AudioClip> stems4 = new List<AudioClip>();
    public List<AudioClip> stems5 = new List<AudioClip>();

    public List<AudioClip> cues0 = new List<AudioClip>();
    public List<AudioClip> cues3 = new List<AudioClip>();
    public List<AudioClip> cues4 = new List<AudioClip>();

    public List<AudioClip> chord_clips = new List<AudioClip>();

    public AudioSource source0;
    public AudioSource source1;
    public AudioSource source2;
    public AudioSource source3;
    public AudioSource source4;
    public AudioSource source5;

    public AudioSource cues;
    public AudioSource chords;

    void Start () {
        set_and_play(current_key, current_intensity);
    }

    void Update () {
        if (!NA.isClient()) //server and standalone
        {
            float measure = 1.153846f;

            counter += Time.deltaTime;
            counter = counter % (measure*2);



            if (prevcounter > counter){
            
                // we JUST looped around
                if (littledome.should_change_key){
                    current_key = (current_key + 1)%6;
                    littledome.should_change_key = false;


                    if (NA.isServer()){
				        GetComponent<NetworkView>().RPC("set_and_play", RPCMode.All, current_key, current_intensity);
                        GetComponent<NetworkView>().RPC("play_chord", RPCMode.All, current_key);
			        }
			        else if (NA.isStandalone()){
				        set_and_play(current_key, current_intensity);
                        play_chord(current_key);
			        }
                }
            
                if (gate.should_change_intensity){
                    current_intensity = (current_intensity + 1)%6;
                    gate.should_change_intensity = false;


                    if (NA.isServer()){
				        GetComponent<NetworkView>().RPC("set_volume", RPCMode.All, current_intensity);
                        GetComponent<NetworkView>().RPC("play_cue", RPCMode.All, current_key, current_intensity);
			        }
			        else if (NA.isStandalone()){
                        set_volume(current_intensity);
                        play_cue(current_key, current_intensity);
			        }
                }
            }

            prevcounter = counter;
        }
    }

    [RPC]
    public void set_and_play (int c_key, int c_intensity) {
        source0.clip = stems0[c_key];
        source1.clip = stems1[c_key];
        source2.clip = stems2[c_key];
        source3.clip = stems3[c_key];
        source4.clip = stems4[c_key];
        source5.clip = stems5[c_key];

        set_volume(c_intensity);

        source0.Play();
        source1.Play();
        source2.Play();
        source3.Play();
        source4.Play();
        source5.Play();
    }

    [RPC]
    public void set_volume (int c_intensity) {

        source0.volume = 0;
        source1.volume = 0;
        source2.volume = 0;
        source3.volume = 0;
        source4.volume = 0;
        source5.volume = 0;

        switch (c_intensity){
            case 0:
                source0.volume = 1;
                break;
            case 1:
                source1.volume = 1;
                break;
            case 2:
                source2.volume = 1;
                break;
            case 3:
                source3.volume = 1;
                break;
            case 4:
                source4.volume = 1;
                break;
            case 5:
                source5.volume = 1;
                break;
            default:
                break;
        }
    }

    [RPC]
    public void play_chord (int c_key) {
        chords.clip = chord_clips[c_key];
        chords.Play();
    }

    [RPC]
    public void play_cue (int c_key, int c_intensity) {

        if (c_intensity > 1){

            int cuestem;
            if (c_intensity == 2 || c_intensity == 3){
                cuestem = 0;
            }
            else {
                cuestem = 1;
            }

            int cuegroup;
            if (c_key == 0 || c_key == 1 || c_key == 2){
                cuegroup = 0;
            }
            else if (c_key == 4 || c_key == 5){
                cuegroup = 4;
            }
            else {
                cuegroup = 3;
            }

            switch (cuegroup){
            case 0:
                cues.clip = cues0[cuestem];
                break;
            case 3:
                cues.clip = cues3[cuestem];
                break;
            case 4:
                cues.clip = cues4[cuestem];
                break;
            default:
                break;
            }

            cues.Play();
        }

    }
}