Shader "Custom/ttt" {

  Properties {
		_Module ("Module", Float ) = 8
		_Height ("Height", Float ) = 0
		_ZPos ("Zpos", Float ) = 0
		_E("e", Float ) = 1
		
	}
	

	
	SubShader{
		Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag


		uniform float _Module;
		uniform float _Height;
		uniform float _ZPos, _E;
		
			
		//base Input structs
		struct vertexInput{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		
		};
		
		struct vertexOutput{
		float4 pos : SV_POSITION;
		float4 col : COLOR;
		float4 posWorld : TEXCOORD0;
        float3 normalDir : TEXCOORD1;
		};
		
		vertexOutput vert(vertexInput v){
			vertexOutput o;
			
			float4 myPos = float4(cos(v.normal.x),v.vertex.x,v.normal.x,sin(v.normal.z));
		 	o.col = myPos;
		 	
		 	o.posWorld = mul(_Object2World, v.vertex);
            o.normalDir = normalize( mul( float4 ( v.normal, 0.0 ), _World2Object).xyz );
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			
			return o;
		}
		
		fixed4 frag(float4 sp:VPOS) : SV_Target {
                float2 wcoord = sp.xy/(_ScreenParams.xy * _E);
                float vig = clamp(3.0/_Module*length(wcoord-0.5),_Height,_ZPos);
                return lerp (fixed4(wcoord,0.0,1.0),fixed4(0.3,0.3,0.3,1.0),vig);
            }
		
		ENDCG
		}	
	
	}
	


}