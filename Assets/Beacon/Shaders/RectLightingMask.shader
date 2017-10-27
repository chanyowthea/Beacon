Shader "Custom/RectLightingMask" 
{
    Properties 
    {
        [PerRenderData] _MainTex ("Base (RGB)", 2D) = "white" {}
        _Center ("Center", vector) = (0.5, 0.5, 0, 0)
        _Length ("Length * 10000", float) = 0.5
        _MaxAlpha("_MaxAlpha", float) = 1.0
        _ZeroAlphaLengthRatio ("_ZeroAlphaLengthRatio", float) = 0.99
    }

    SubShader 
    {
        Tags
        {
            "Queue" = "Transparent"
        }
        pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
                        
            #include "UnityCG.cginc"            

            sampler2D _MainTex;
            float2 _Center;
            float _Length;
            float _MaxAlpha;
            float _ZeroAlphaLengthRatio;
            
            struct v2f 
            {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
            };
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = v.texcoord;
                return o;
            }
            
            half4 frag (v2f i) : COLOR
            {                
                half4 texcol = tex2D (_MainTex, i.uv);
                float2 d = abs(i.uv - _Center.xy);
                float2 offsetPos = i.uv - _Center.xy; 
                _Length /= 10000;
                float zeroAlphaLength = _Length * _ZeroAlphaLengthRatio; 
                float2 center;
                // 左右的渐变
                if(d.x > zeroAlphaLength && d.y < zeroAlphaLength)
                {
                	float2 centers[2];
                	centers[0] = float2(_Center.x -zeroAlphaLength, i.uv.y);
                	centers[1] = float2(_Center.x + zeroAlphaLength, i.uv.y); 
                	float dis = length(i.uv - centers[offsetPos.x < 0 ? 0 : 1]); 
            		texcol.a = _MaxAlpha * (1 - (_Length - dis) / (_Length - zeroAlphaLength));	
                }
                // 上下的渐变
                else if(d.y > _Length * _ZeroAlphaLengthRatio && d.x < zeroAlphaLength)
                {
                	float2 centers[2];
                	centers[0] = float2(i.uv.x, _Center.y -zeroAlphaLength);
                	centers[1] = float2(i.uv.x, _Center.y + zeroAlphaLength); 
                	float dis = length(i.uv - centers[offsetPos.y < 0 ? 0 : 1]); 
            		texcol.a = _MaxAlpha * (1 - (_Length - dis) / (_Length - zeroAlphaLength));	
                }
                else
                {
                	float2 centers[4];
                	centers[0] = _Center.xy + float2(-zeroAlphaLength, zeroAlphaLength);
                	centers[1] = _Center.xy + float2(zeroAlphaLength, zeroAlphaLength);
                	centers[2] = _Center.xy + float2(-zeroAlphaLength, -zeroAlphaLength);
                	centers[3] = _Center.xy + float2(zeroAlphaLength, -zeroAlphaLength);
                	float dis;
                	// 左上角
                	if(offsetPos.x < 0 && offsetPos.y > 0 && d.x > zeroAlphaLength && d.y > zeroAlphaLength)
                	{
                		dis = length(i.uv - centers[0]); 
            			texcol.a = _MaxAlpha * (1 - (_Length - dis) / (_Length - zeroAlphaLength));	
                	}
                	// 右上角
                	else if(offsetPos.x > 0 && offsetPos.y > 0 && d.x > zeroAlphaLength && d.y > zeroAlphaLength)
                	{
                		dis = length(i.uv - centers[1]); 
            			texcol.a = _MaxAlpha * (1 - (_Length - dis) / (_Length - zeroAlphaLength));	
                	}
                	// 左下角
                	else if(offsetPos.x < 0 && offsetPos.y < 0 && d.x > zeroAlphaLength && d.y > zeroAlphaLength)
                	{
                		dis = length(i.uv - centers[2]); 
            			texcol.a = _MaxAlpha * (1 - (_Length - dis) / (_Length - zeroAlphaLength));	
                	}
                	// 右下角
                	else if(offsetPos.x > 0 && offsetPos.y < 0 && d.x > zeroAlphaLength && d.y > zeroAlphaLength)
                	{
                		dis = length(i.uv - centers[3]); 
            			texcol.a = _MaxAlpha * (1 - (_Length - dis) / (_Length - zeroAlphaLength));	
                	}
                	// 中间Alpha始终为0的区域
                	else
                	{
                		texcol.a = 0; 
                	}
                }
                return texcol;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
