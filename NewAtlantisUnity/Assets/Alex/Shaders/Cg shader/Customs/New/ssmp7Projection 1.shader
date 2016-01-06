Shader "Custom/SimplestShader" {

	Properties{
		//_Module("Module", Float) = 8
		_Opacity("Opacity",  Range(0.0, 1.0)) = 0.75
		//_MovePos("MovePos", Vector) = (1.0 ,1.0, 1.0, 1.0)
	}


	SubShader{

		Tags{ "Queue" = "Transparent" }
		// draw after all opaque geometry has been drawn


		Pass{


	
		Cull Back // first pass renders only back faces 
				   // (the "inside")
	    ZWrite Off // don't write to depth buffer 
				   // in order not to occlude other objects
		Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag


		uniform float _Module; 
	    uniform float _Opacity;
	    uniform float4 _MovePos;
	

		//base Input structs
	struct vertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		//int index : INDEX;

	};

	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 col : COLOR;
	};




	vertexOutput vert(vertexInput v) {
		
		vertexOutput o;
		o.col = float4(v.normal , 1.0);
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex.xxyz );
	
		return o;
	}


	float4 frag(vertexOutput i) : COLOR{
		return float4(i.col.r,i.col.g,i.col.b,_Opacity);
	//return i.col;
	}

		ENDCG
	}

	}



}
