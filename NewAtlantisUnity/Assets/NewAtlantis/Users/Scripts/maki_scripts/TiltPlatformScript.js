#pragma strict

var clicked = false;

//mouse data variables
var mouseLocationX = 0;
var mouseLocationY = 0;

var setX = 0;
var setY = 0;

var distanceX = 0;
var distanceY = 0;

var distanceMax = 300;

//tilt variables
var tiltAxisOneAmount = 0.0f;
var tiltAxisTwoAmount = 0.0f;

var maxTiltAmount = 5.0f;

var maxAxisOneTilt = 2.0f;
var minAxisOneTilt = -2.0f;

var maxAxisTwoTilt = 2.0f;
var minAxisTwoTilt = -2.0f;

//setting our rate of reset change
var changeRate = 0.2f;

function Start () {

}

function Update () {
    //Debug.Log("mousePosition = " + Input.mousePosition);

    if(clicked){
    	distanceX = Input.mousePosition.x - setX;
    	distanceY = Input.mousePosition.y - setY;
    	
    	var absDistX = Mathf.Abs(distanceX);
    	var absDistY = Mathf.Abs(distanceY);
    	
//   	    Debug.Log("distance X = " + distanceX);
    	//Debug.Log("distance Y = " + distanceY); 
    	
    	if(absDistX > distanceMax){
    		if(distanceX > 0)
    	    {distanceX = distanceMax;}
    	    
    	    else
    	    {distanceX = -1 * distanceMax;}
    	}
    	
    	if(absDistY > distanceMax){
    		if(distanceY > 0)
    	    {distanceY = distanceMax;}
       	    else
    	    {distanceY = -1*distanceMax;}
    	}
    	
    	//mapping values 
    	var ratioX = maxAxisOneTilt/distanceMax;
    	var ratioY = maxAxisTwoTilt/distanceMax;
    	
    	//Debug.Log("ratioX = " + ratioX);
    	
    	tiltAxisOneAmount = distanceX * ratioX;
    	tiltAxisTwoAmount = distanceY * ratioY;
    
    	//tiltAxis Limitations
    	
    	//Debug.Log("tiltAxisOneAmount = " + tiltAxisOneAmount); 
    	
    	
    	//Debug.Log("distance X = " + distanceX);
    	//Debug.Log("distance Y = " + distanceY); 
    }
    
    else{
        if(tiltAxisOneAmount != 0){
           var differOne = Mathf.Abs(0-tiltAxisOneAmount);
         
           changeRate = differOne/ 10;
           
           if(differOne < 0.2){
               tiltAxisOneAmount = 0;
           }
           
           else{
               if(tiltAxisOneAmount > 0){
                   tiltAxisOneAmount -= changeRate;
               }
               
               else{
                   tiltAxisOneAmount += changeRate;
               }
           }
        }
        
        if(tiltAxisTwoAmount != 0){
           var differTwo = Mathf.Abs(0-tiltAxisTwoAmount);
           
            changeRate = differTwo/10;
            
            if(differTwo < 0.2){
               tiltAxisTwoAmount = 0;
            }
            
            else{
                if(tiltAxisTwoAmount > 0){
                    tiltAxisTwoAmount -= changeRate;
                }
                else{
                    tiltAxisTwoAmount += changeRate;
                }
           }
        } 
    }
    
    gameObject.transform.eulerAngles = new Vector3(tiltAxisTwoAmount,  90, tiltAxisOneAmount);

}

function OnMouseDown () {
   // Debug.Log("CLICKED");

    setX = Input.mousePosition.x;
    setY = Input.mousePosition.y;
    
    clicked = true;
}

function OnMouseUp () {
    clicked = false;
   // Debug.Log("UNCLICKED");
}