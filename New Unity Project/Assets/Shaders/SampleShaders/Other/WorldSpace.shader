// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Other/WorldSpace" {
   SubShader {
      Pass {
         CGPROGRAM

         #pragma vertex vert  
         #pragma fragment frag 
 
         // uniform float4x4 _Object2World; 
            // automatic definition of a Unity-specific uniform parameter

         struct vertexInput {
            float4 vertex : POSITION;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 position_in_world_space : TEXCOORD0;
         };

         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output; 
 
            output.pos =  UnityObjectToClipPos(input.vertex);
            output.position_in_world_space = 
               mul(unity_ObjectToWorld, input.vertex);
               // transformation of input.vertex from object 
               // coordinates to world coordinates;
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR 
         {
             float dist = distance(input.position_in_world_space, 
               float4(0.0, 0.0, 0.0, 1.0));
               // computes the distance between the fragment position 
               // and the origin (the 4th coordinate should always be 
               // 1 for points).

//               float4 c1 = float4(0.0, 1.0, 0.0, 1.0);
//               float4 c2 = float4(0.1, 0.1, 0.1, 1.0);
//               return lerp(c1, c2, dist / 10.0);
            if (dist < 5.0)
            {
               return float4(0.0, 1.0, 0.0, 1.0); 
                  // color near origin
            }
            else
            {
               return float4(0.1, 0.1, 0.1, 1.0); 
                  // color far from origin
            }
         }
 
         ENDCG  
      }
   }
}