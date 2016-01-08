Shader "Custom/SimplestShader" {

	Properties{
		//NO PROPERTIES
	}
	
	SubShader{
		Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		//base Input structs
		struct vertexInput{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		
		};
		
		struct vertexOutput{
		float4 pos : SV_POSITION;
		float4 col : COLOR;
		};
		
		vertexOutput vert(vertexInput v){
			vertexOutput o;
		 	o.col = float4(v.normal, 1.0);
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			return o;
		}
		
		float4 frag(vertexOutput i) : COLOR{
		return i.col;
		}
		
		ENDCG
		}	
	
	}
	


}
