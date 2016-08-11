Shader "azure[Sky]/Custom/Cubemap Fog"
{
	Properties
	{
		_SkyRotate("Rotation Angle", Range(0,360)) = 0
		[NoScaleOffset] _SkyTexCube ("Cubemap Skybox", Cube) = "white" {}
		_GlobalColor("Global Color", Color) = (1,1,1,1)
		_NormalFogColor("Normal Fog Color", Color) = (1,1,1,1)
		_NormalFogDistance("Normal Fog Distance", Range(0,50)) = 8
		_BlendFogDistance("Blend Distance", Range(0,50)) = 50
		_ScatteringFogDistance("Scattering Fog Distance", Range(0,50)) = 3

		_DenseFogColor("Dense Fog Color", Color) = (1,1,1,1)
		_DenseFogIntensity("Dense Fog Intensity", Range(0,1)) = 0
		_DenseFogAltitude("Dense Fog Altitude", Range(-1,1)) = -0.8
	}
	SubShader
	{
		Cull Front
		ZWrite Off
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile HDR_ON HDR_OFF
			#include "UnityCG.cginc"

			#define _pi 3.141592653589

			uniform float     _Altitude;
			uniform float4    _NormalFogColor;
			uniform float     _NormalFogDistance;
			uniform float     _ScatteringFogDistance;
			uniform float     _BlendFogDistance;
			uniform float4    _GlobalColor;

			uniform sampler2D   _MainTex;
			uniform samplerCUBE _SkyTexCube;
			uniform sampler2D   _CameraDepthTexture;
			uniform float4x4    _FrustumCorners;
			uniform float4      _MainTex_TexelSize;

			uniform float4    _DenseFogColor;
			uniform float     _DenseFogIntensity;
			uniform float     _DenseFogAltitude;

			uniform float     _SkyRotate;

			struct appdata
			{
				float4 vertex   : POSITION;
			    float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 Position        : SV_POSITION;
    			float2 uv 	           : TEXCOORD0;
				float4 interpolatedRay : TEXCOORD1;
				float2 uv_depth        : TEXCOORD2;
			};

			v2f vert (appdata v)
			{
				v2f o;
    			UNITY_INITIALIZE_OUTPUT(v2f, o);
    			
    			half index = v.vertex.z;
				v.vertex.z = 0.1;
				o.uv       = v.texcoord.xy;
				o.uv_depth = v.texcoord.xy;
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1-o.uv.y;
				#endif
				o.interpolatedRay   = _FrustumCorners[(int)index];
				o.interpolatedRay.w = index;

				//Sky Rotation
				float angle = radians(_SkyRotate);
				float s = sin ( angle );
                float c = cos ( angle );
                float2x2 rotationMatrix = float2x2( c, -s, s, c);
				o.interpolatedRay.xz  = mul( o.interpolatedRay.xz, rotationMatrix );
    			
    			o.Position = mul(UNITY_MATRIX_MVP, v.vertex);

				return o;
			}

			fixed4 frag (v2f IN) : SV_Target
			{
			   //-------------------------------------------------------------------------------------------------------
			   //-------------------------------------------Directions--------------------------------------------------
			   float  depth       = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,IN.uv_depth)));
               float3 viewDir     = normalize(depth * IN.interpolatedRay);

			   ////////
			   ///UV///
			   float2 uv    = float2(atan2(viewDir.z, -viewDir.x), -acos(viewDir.y)) / float2(2.0*_pi, _pi);


			   ///////////////
			   // Apply Fog //
			   float  Mask       =    saturate( lerp(1.0, 0.0, depth) * _ProjectionParams.z );																			    
			   float3 screen     =    tex2D(_MainTex, IN.uv); // Original scene
			   float3 sky = texCUBE (_SkyTexCube, viewDir).rgb;

			   float3 normalFog    =  lerp(screen,_NormalFogColor, Mask);
			          normalFog    =  lerp(screen,normalFog, pow(saturate(depth * _NormalFogDistance),0.45));
			          normalFog    =  pow(normalFog,1.0);
			   
			   
			   float3 inScatteringFog =    lerp(screen,sky, Mask);                  						                                 // Creating the fog color.
			          inScatteringFog =    lerp(screen, inScatteringFog * _GlobalColor, pow(saturate(depth * _ScatteringFogDistance),0.45)); // Mixing the fog with the scene, according to the depth.
			   
			   float3 finalFog = lerp(normalFog, inScatteringFog, pow(saturate(depth * _BlendFogDistance),0.45));

			   //Dense Fog
			   float  denseFogAltitude =viewDir.y - _DenseFogAltitude;
			   float3 denseFog = lerp(_DenseFogColor, sky, saturate(pow(denseFogAltitude,5)));
			   denseFog = lerp(finalFog, denseFog, saturate(pow(depth * 25,0.25)) * _DenseFogIntensity);
			   
			   return float4(denseFog,1.0);
//			   return float4(Mask,Mask,Mask,1.0);             // To see the mask
//			   return float4(depth*10,depth*10,depth*10,1.0); // To see the depth

			}
			ENDCG
		}
	}
}
