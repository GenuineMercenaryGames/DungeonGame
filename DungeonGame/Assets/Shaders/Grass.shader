Shader "Custom/Grass_URP_Windows"
{
    Properties
    {
        [Header(Shading)]
        _TopColor("Top Color", Color) = (1,1,1,1)
        _BottomColor("Bottom Color", Color) = (1,1,1,1)
        _TranslucentGain("Translucent Gain", Range(0,1)) = 0.5

        [Header(Tessellation)]
        _TessellationUniform("Tessellation Uniform", Range(1, 64)) = 1

        [Header(Blades)]
        _BladeWidth("Blade Width", Float) = 0.05
        _BladeWidthRandom("Blade Width Random", Float) = 0.02
        _BladeHeight("Blade Height", Float) = 0.5
        _BladeHeightRandom("Blade Height Random", Float) = 0.3
        _BladeForward("Blade Forward Amount", Float) = 0.38
        _BladeCurve("Blade Curvature Amount", Range(1, 4)) = 2
        _BendRotationRandom("Bend Rotation Random", Range(0, 1)) = 0.2

        [Header(Wind)]
        _WindDistortionMap("Wind Distortion Map", 2D) = "white" {}
        _WindStrength("Wind Strength", Float) = 1
        _WindFrequency("Wind Frequency", Vector) = (0.05, 0.05, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
            "RenderPipeline" = "UniversalPipeline"
        }

        Cull Off

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma target 4.6
            #pragma vertex vert
            #pragma hull hull
            #pragma domain domain
            #pragma geometry geo
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            #define BLADE_SEGMENTS 3
            #define GRASS_PI 3.14159265359
            #define GRASS_TWO_PI 6.28318530718

            CBUFFER_START(UnityPerMaterial)
                half4 _TopColor;
                half4 _BottomColor;
                float _TranslucentGain;

                float _TessellationUniform;

                float _BladeWidth;
                float _BladeWidthRandom;
                float _BladeHeight;
                float _BladeHeightRandom;
                float _BladeForward;
                float _BladeCurve;
                float _BendRotationRandom;

                float _WindStrength;
                float4 _WindFrequency;
                float4 _WindDistortionMap_ST;
            CBUFFER_END

            TEXTURE2D(_WindDistortionMap);
            SAMPLER(sampler_WindDistortionMap);

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct TessControlPoint
            {
                float3 positionOS : INTERNALTESSPOS;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct TessellationFactors
            {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;
            };

            struct DomainOutput
            {
                float3 positionOS : TEXCOORD0;
                float3 normalOS : TEXCOORD1;
                float4 tangentOS : TEXCOORD2;
                float2 uv : TEXCOORD3;
            };

            struct GeometryOutput
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            float rand(float3 co)
            {
                return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
            }

            float3x3 angle_axis_3x3(float angle, float3 axis)
            {
                axis = normalize(axis);

                float s;
                float c;
                sincos(angle, s, c);

                float t = 1.0 - c;
                float x = axis.x;
                float y = axis.y;
                float z = axis.z;

                return float3x3(
                    t * x * x + c,     t * x * y - s * z, t * x * z + s * y,
                    t * x * y + s * z, t * y * y + c,     t * y * z - s * x,
                    t * x * z - s * y, t * y * z + s * x, t * z * z + c
                );
            }

            TessControlPoint vert(Attributes v)
            {
                TessControlPoint o;
                o.positionOS = v.positionOS;
                o.normalOS = v.normalOS;
                o.tangentOS = v.tangentOS;
                o.uv = v.uv;
                return o;
            }

            TessellationFactors patch_constant_function(InputPatch<TessControlPoint, 3> patch)
            {
                TessellationFactors f;
                f.edge[0] = _TessellationUniform;
                f.edge[1] = _TessellationUniform;
                f.edge[2] = _TessellationUniform;
                f.inside = _TessellationUniform;
                return f;
            }

            [domain("tri")]
            [outputcontrolpoints(3)]
            [outputtopology("triangle_cw")]
            [partitioning("integer")]
            [patchconstantfunc("patch_constant_function")]
            TessControlPoint hull(InputPatch<TessControlPoint, 3> patch, uint id : SV_OutputControlPointID)
            {
                return patch[id];
            }

            [domain("tri")]
            DomainOutput domain(
                TessellationFactors factors,
                OutputPatch<TessControlPoint, 3> patch,
                float3 barycentric_coordinates : SV_DomainLocation)
            {
                DomainOutput o;

                o.positionOS =
                    patch[0].positionOS * barycentric_coordinates.x +
                    patch[1].positionOS * barycentric_coordinates.y +
                    patch[2].positionOS * barycentric_coordinates.z;

                o.normalOS = normalize(
                    patch[0].normalOS * barycentric_coordinates.x +
                    patch[1].normalOS * barycentric_coordinates.y +
                    patch[2].normalOS * barycentric_coordinates.z
                );

                o.tangentOS =
                    patch[0].tangentOS * barycentric_coordinates.x +
                    patch[1].tangentOS * barycentric_coordinates.y +
                    patch[2].tangentOS * barycentric_coordinates.z;

                o.uv =
                    patch[0].uv * barycentric_coordinates.x +
                    patch[1].uv * barycentric_coordinates.y +
                    patch[2].uv * barycentric_coordinates.z;

                return o;
            }

            GeometryOutput vertex_output(float3 positionOS, float3 normalOS, float2 uv)
            {
                GeometryOutput o;
                VertexPositionInputs pos_inputs = GetVertexPositionInputs(positionOS);
                o.positionCS = pos_inputs.positionCS;
                o.positionWS = pos_inputs.positionWS;
                o.normalWS = TransformObjectToWorldNormal(normalOS);
                o.uv = uv;
                return o;
            }

            GeometryOutput generate_grass_vertex(
                float3 vertex_position_os,
                float width,
                float height,
                float forward,
                float2 uv,
                float3x3 transform_matrix)
            {
                float3 tangent_point = float3(width, forward, height);
                float3 tangent_normal = normalize(float3(0.0, -1.0, forward));

                float3 local_position = vertex_position_os + mul(transform_matrix, tangent_point);
                float3 local_normal = mul(transform_matrix, tangent_normal);

                return vertex_output(local_position, local_normal, uv);
            }

            [maxvertexcount(BLADE_SEGMENTS * 2 + 1)]
            void geo(triangle DomainOutput input_patch[3], inout TriangleStream<GeometryOutput> tri_stream)
            {
                float3 pos = input_patch[0].positionOS;
                float3 v_normal = -normalize(input_patch[0].normalOS);
                float4 v_tangent = input_patch[0].tangentOS;

                float3 v_binormal = normalize(cross(v_normal, v_tangent.xyz) * v_tangent.w);

                float3x3 tangent_to_local = float3x3(
                    v_tangent.x,  v_binormal.x, v_normal.x,
                    v_tangent.y,  v_binormal.y, v_normal.y,
                    v_tangent.z,  v_binormal.z, v_normal.z
                );

                float3x3 facing_rotation_matrix =
                    angle_axis_3x3(rand(pos) * GRASS_TWO_PI, float3(0.0, 0.0, 1.0));

                float3x3 bend_rotation_matrix =
                    angle_axis_3x3(rand(pos.zzx) * _BendRotationRandom * GRASS_PI * 0.5, float3(-1.0, 0.0, 0.0));

                float2 wind_uv =
                    pos.xz * _WindDistortionMap_ST.xy +
                    _WindDistortionMap_ST.zw +
                    _WindFrequency.xy * _Time.y;

                float2 wind_sample =
                    (SAMPLE_TEXTURE2D_LOD(_WindDistortionMap, sampler_WindDistortionMap, wind_uv, 0).xy * 2.0 - 1.0) *
                    _WindStrength;

                float3 wind_axis = float3(wind_sample.x, wind_sample.y, 0.0);
                float wind_axis_length = length(wind_axis);

                if (wind_axis_length > 0.0001)
                {
                    wind_axis /= wind_axis_length;
                }
                else
                {
                    wind_axis = float3(1.0, 0.0, 0.0);
                }

                float3x3 wind_rotation = angle_axis_3x3(wind_axis_length * GRASS_PI, wind_axis);

                float3x3 transformation_matrix =
                    mul(mul(mul(tangent_to_local, wind_rotation), facing_rotation_matrix), bend_rotation_matrix);

                float3x3 transformation_matrix_facing =
                    mul(tangent_to_local, facing_rotation_matrix);

                float height =
                    (rand(pos.zyx) * 2.0 - 1.0) * _BladeHeightRandom + _BladeHeight;

                float width =
                    (rand(pos.xzy) * 2.0 - 1.0) * _BladeWidthRandom + _BladeWidth;

                float forward = rand(pos.yyz) * _BladeForward;

                for (int i = 0; i < BLADE_SEGMENTS; ++i)
                {
                    float t = (float)i / (float)BLADE_SEGMENTS;

                    float segment_height = height * t;
                    float segment_width = width * (1.0 - t);
                    float segment_forward = pow(t, _BladeCurve) * forward;

                    float3x3 transform_matrix =
                        (i == 0) ? transformation_matrix_facing : transformation_matrix;

                    tri_stream.Append(
                        generate_grass_vertex(
                            pos,
                            segment_width,
                            segment_height,
                            segment_forward,
                            float2(0.0, t),
                            transform_matrix
                        )
                    );

                    tri_stream.Append(
                        generate_grass_vertex(
                            pos,
                            -segment_width,
                            segment_height,
                            segment_forward,
                            float2(1.0, t),
                            transform_matrix
                        )
                    );
                }

                tri_stream.Append(
                    generate_grass_vertex(
                        pos,
                        0.0,
                        height,
                        forward,
                        float2(0.5, 1.0),
                        transformation_matrix
                    )
                );
            }

            half4 frag(GeometryOutput i, half facing : VFACE) : SV_Target
            {
                float3 normalWS = normalize(facing > 0.0 ? i.normalWS : -i.normalWS);

                float4 shadow_coord = TransformWorldToShadowCoord(i.positionWS);
                Light main_light = GetMainLight(shadow_coord);

                half ndotl = saturate(dot(normalWS, main_light.direction) + _TranslucentGain);
                half shadow = main_light.distanceAttenuation * main_light.shadowAttenuation;

                half3 ambient = SampleSH(normalWS);
                half3 top_light_intensity = ambient + shadow * main_light.color;

                half3 col = lerp(_BottomColor.rgb * top_light_intensity, _TopColor.rgb * top_light_intensity, i.uv.y);
                return half4(col, 1.0);
            }
            ENDHLSL
        }
    }
}