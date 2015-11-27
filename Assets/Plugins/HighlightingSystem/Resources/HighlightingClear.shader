Shader "Hidden/Highlighted/Clear"
{
	CGINCLUDE
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityShaderVariables.cginc"
	
	struct appdata_t
	{
		float4 vertex : POSITION;
		fixed4 color : COLOR;
	};

	struct v2f
	{
		float4 pos : POSITION;
		fixed4 color : COLOR;
	};

	v2f vert(appdata_t v)
	{
		v2f o;
		o.pos = v.vertex;
		o.color = v.color;
		return o;
	}
	
	fixed4 frag(v2f i) : COLOR
	{
		return i.color;
	}
	ENDCG

	SubShader
	{
		ZTest Always
		Cull Off
		Fog { Mode off }
		
		// Clear Color
		Pass
		{
			ZWrite Off
			CGPROGRAM
			ENDCG
		}

		// Clear Stencil
		Pass
		{
			Colormask 0
			ZWrite Off
			Stencil
			{
				Comp Always
				Pass Zero
			}
			CGPROGRAM
			ENDCG
		}

		// Clear Color, Depth and Stencil
		Pass
		{
			ZWrite On
			Stencil
			{
				Comp Always
				Pass Zero
			}
			CGPROGRAM
			ENDCG
		}
	}
	FallBack Off
}
