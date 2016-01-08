Shader "Custom/SimplestShader" {

	Properties{
		_Module("Module", Float) = 8
		_MovePos("MovePos", Vector) = (1.0 ,1.0, 1.0, 1.0)
	}


	SubShader{
		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		uniform float _Module;
	    uniform float4 _MovePos;


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
		o.col = float4(v.normal * _Module, 1.0);
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex + v.vertex * sin(v.vertex * fmod(cos(v.vertex ) , v.vertex)));
	
		return o;
	}

	float4 frag(vertexOutput i) : COLOR{
		return i.col;
	}

		ENDCG
	}

	}



}
