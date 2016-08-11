// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "azure[Sky]/Custom/Panoramic Skybox"
{
	Properties
	{
		_SkyRotate("Rotation Angle", Range(0,360)) = 0
		[NoScaleOffset] _MainTex ("Panoramic Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" "IgnoreProjector"="True" }
	    Cull Back     // Render side
		Fog{Mode Off} // Don't use fog
    	ZWrite Off    // Don't draw to bepth buffer

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define pi 3.141592653589
			uniform float _SkyRotate;
			
//			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 Position     : SV_POSITION;
    			float3 WorldPos     : TEXCOORD0;
			};

			sampler2D _MainTex;
			
			v2f vert (appdata v)
			{
				v2f o;
//    			UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.Position = mul(UNITY_MATRIX_MVP, v.vertex);

				//Sky Rotation
				float angle = radians(_SkyRotate);
				float s = sin ( angle );
                float c = cos ( angle );
                float2x2 rotationMatrix = float2x2( c, -s, s, c);
				v.vertex.xz  = mul( v.vertex.xz, rotationMatrix );


    			o.WorldPos = normalize(mul(unity_ObjectToWorld, v.vertex).xyz);

				return o;
			}
			
			fixed4 frag (v2f IN) : SV_Target
			{
			    float3 viewDir    = normalize(IN.WorldPos);
				float2 uv = float2(-atan2(viewDir.z, -viewDir.x), -acos(viewDir.y)) / float2(2.0*pi, pi);
				return tex2D(_MainTex, uv);
			}
			ENDCG
		}
	}
}
