using UnityEngine;
using System.Collections;

public class littledome : MonoBehaviour {

    public mainaudio audioman;
    public static bool should_change_key = false;

	// little dome controls transitions between keys

    void OnCollisionEnter(Collision other) {
        if (!NA.isClient()) {
            should_change_key = true;

            // and also play a cue
            int cuestem;
            if (mainaudio.current_intensity == 0 || mainaudio.current_intensity == 3 || mainaudio.current_intensity == 4 || mainaudio.current_intensity == 5){
                cuestem = 0;
            }
            else {
                cuestem = 1;
            }

            int cuegroup;
            if (mainaudio.current_key == 0 || mainaudio.current_key == 1 || mainaudio.current_key == 2){
                cuegroup = 0;
            }
            else if (mainaudio.current_key == 4 || mainaudio.current_key == 5){
                cuegroup = 4;
            }
            else {
                cuegroup = 3;
            }


            
            if (NA.isServer()){
				GetComponent<NetworkView>().RPC("choose_cue", RPCMode.All, cuegroup, cuestem);
			}
			else if (NA.isStandalone()){
				choose_cue(cuegroup, cuestem);
			}
        }
	}

    [RPC]
    void choose_cue (int cuegroup, int cuestem){
        switch (cuegroup){
            case 0:
                audioman.cues.clip = audioman.cues0[cuestem];
                break;
            case 3:
                audioman.cues.clip = audioman.cues3[cuestem];
                break;
            case 4:
                audioman.cues.clip = audioman.cues4[cuestem];
                break;
            default:
                break;
        }

        audioman.cues.Play();
    }
}
