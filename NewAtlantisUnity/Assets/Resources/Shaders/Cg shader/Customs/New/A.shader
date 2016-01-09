Shader "Custom/A" {
	Properties{
		//NO PROPERTIES
		_MovePos("MovePos",Vector) = (1.0,1.0,1.0,1.0)
		_LerpValue("LerpValue",Float) = 1
	}

		SubShader{
			Pass{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag

			uniform float4 _MovePos;
	uniform float _LerpValue;
		//base Input structs
	struct vertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;

	};

	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 col : COLOR;
	};

	vertexOutput vert(vertexInput v) {
		vertexOutput o;
		o.col = float4(v.normal, 1.0);
		o.pos = mul(UNITY_MATRIX_MVP,  reflect(v.vertex,_MovePos) );

	   //if(distance(v.vertex,v.normal) > _LerpValue)o.pos = mul(UNITY_MATRIX_MVP, lerp(v.vertex, _MovePos, _LerpValue));

		
		return o;
	}

	float4 frag(vertexOutput i) : COLOR{
		return i.col;
	}

		ENDCG
	}

	}



}