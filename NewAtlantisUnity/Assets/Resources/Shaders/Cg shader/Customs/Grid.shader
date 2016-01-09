Shader "Custom/TextureCoordinates/ChessOpt" {
    Properties {
		_Module ("Module", Float ) = 8
	}
	
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            uniform float _Module;
		
		            
			fixed3 frag(v2f_img i) : SV_Target {
		
                bool p = fmod(i.uv.x * _Module ,2.0) < 1.0;
                bool q = fmod(i.uv.y * _Module ,2.0) > 1.0;
                
                return fixed3((p && q) , !(p || q),1.0);
            }
            ENDCG
        }
    }
}