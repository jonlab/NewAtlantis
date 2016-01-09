Shader "Custom/A-Color" {
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
		//o.col = float4(v.normal, 1.0);
		//o.col = float4(cos(v.normal), 1.0);

		//o.col = float4(fmod(v.normal,_LerpValue), distance(v.normal,v.vertex));

		//o.col = float4(cos(v.normal), v.normal);
		o.col = reflect(float4(abs(v.normal), 1.0) , _MovePos);


		//o.pos = mul(UNITY_MATRIX_MVP, saturate(v.vertex));
		//o.pos = mul(UNITY_MATRIX_MVP, smoothstep(v.vertex,_MovePos,_LerpValue));
		//o.pos = mul(UNITY_MATRIX_MVP, normalize(v.vertex * distance(v.vertex, v.normal) * sin(v.vertex)));

		//COOL
		o.pos = mul(UNITY_MATRIX_MVP, fmod(v.vertex , _LerpValue) );
		

		//o.pos = mul(UNITY_MATRIX_MVP, v.vertex);


		//o.pos = mul(UNITY_MATRIX_MVP + cos(UNITY_MATRIX_MVP) /_LerpValue, v.vertex);


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