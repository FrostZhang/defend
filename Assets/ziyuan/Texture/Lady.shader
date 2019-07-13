Shader "Cus/Lady"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OutLightColor ("_OutLightColor", COLOR) =(1,1,1,1)
		_OutLightPow ("_OutLightPow", float) = 128
		_OutLightStrength ("_OutLightStrength", float) = 0.5
		_Size ("_Size", float) = 0.5
		_SpecularGlass ("_SpecularGlass", float) = 128
		_RimColor ("_RimColor", COLOR) =(1,1,1,1)
		_RimStrength ("_RimStrength", float) = 0.5

	}
	SubShader
	{
		Tags{ "Queue" = "AlphaTest"  }
		Pass
		{
			Tags { "RenderType"="Opaque"}
			Cull Off ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal:NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 worldnor:NORMAL;
				float3 worldlightdir:TEXCOORD3;
				float3 worldviewdir:TEXCOORD2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _SpecularGlass;
			float _RimStrength;
			fixed4 _RimColor;
			v2f vert (appdata v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);  
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float3 worldpos = UnityObjectToWorldDir(v.vertex);
				o. worldnor =normalize( UnityObjectToWorldNormal(v.normal));
				o. worldlightdir =normalize( UnityWorldSpaceLightDir(worldpos));
				o. worldviewdir =normalize( UnityWorldSpaceViewDir(worldpos));
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float lambert = saturate(dot (i.worldnor,i.worldlightdir));

				float3 reflectdir = reflect(-i.worldlightdir , i.worldnor);
				float reflectstrenth = saturate(dot(reflectdir,i.worldviewdir));
				float spec = pow(reflectstrenth,_SpecularGlass);
				
				float3 halfview = i.worldlightdir + i.worldviewdir;
				float halflight = saturate(dot (normalize(halfview),i.worldlightdir));
				float halfspec = pow(halflight , _SpecularGlass);

				float rim = 1 -  max(0, dot(i.worldviewdir,i.worldnor));

				fixed4 col = tex2D(_MainTex, i.uv);
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz ;

				col.rbg *= ambient + (lambert+ halfspec) *_LightColor0.xyz ;
				col.rbg += _RimColor.rbg*pow(rim,1/ _RimStrength);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}

		Pass
		{
			Tags{ "LightMode" = "Always"  }
			Cull Front
			Blend SrcAlpha One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform float _Size;
			uniform float4 _OutLightColor;
			uniform float _OutLightPow;
			uniform float _OutLightStrength;


			struct v2f
			{
				float3 normal : TEXCOORD0;
				float3 viewdir : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				float3 nor = mul((float3x3)UNITY_MATRIX_T_MV,v.normal);
				float2 offset = TransformViewToProjection(nor.xy);
				//v.vertex.xy += offset /** v.vertex.z */* _Size;
				v.vertex.xyz += v.normal * _Size;
				o.vertex = UnityObjectToClipPos(v.vertex);

				float3 worldpos = UnityObjectToWorldDir(v.vertex);
				o. normal =normalize( UnityObjectToWorldNormal(v.normal));
				o. viewdir =normalize( UnityWorldSpaceViewDir(worldpos));
				o.viewdir = normalize( ObjSpaceViewDir(v.vertex));
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR
			{
				float4 col = _OutLightColor;
				col.a = pow(saturate(dot(i.viewdir, i.normal)), _OutLightPow);
				col.a *= _OutLightStrength*dot(i.viewdir, i.normal);
				return col;
			}

			ENDCG
		}
	}

	FallBack "Diffuse"
}
