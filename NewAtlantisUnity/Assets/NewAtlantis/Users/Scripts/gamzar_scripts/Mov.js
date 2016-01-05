#pragma strict

public var moduloCount = 100;
public var direction = new Vector3(0,1,0);
// Use this for initialization
	function Start () {


	}
	
	// Update is called once per frame
	function Update () {
	
	
	
	if (moduloCount < 1)moduloCount=1;
	
	
	
		//transform.Rotate(Vector3.up*Time.deltaTime*10);
		//transform.Rotate(new Vector3(0,1,0));
		
		if(Time.frameCount % moduloCount > moduloCount/2 ){
		transform.Translate(direction*Time.deltaTime*1);
		
		
		}else{
		
		transform.Translate(-direction*Time.deltaTime*0.8);
		
		
		}
		
		
	}

