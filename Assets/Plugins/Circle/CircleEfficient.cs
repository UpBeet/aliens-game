//--Martin Ysa (2013)--//
// -- Circle.cs -- //
//-- use polar coordinates for draw all vertices --//

using UnityEngine;
using System.Collections;

//-- Adding components --//
[RequireComponent (typeof (MeshCollider))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]

public class CircleEfficient : MonoBehaviour {
        
		
		public float radius = 1.0f;
		public int segments = 20;
		public bool noise = false;
		public float noiseIntensity = 0.4f;
	
		float step;
		float cAngle;
		float radialFactor;
		float TangencialFactor;


        
        public void Rebuild(){
               //--mesh filter--//
				MeshFilter meshFilter = GetComponent<MeshFilter>();
                if (meshFilter==null){
                        Debug.LogError("MeshFilter not found!");
                        return;
                }
                
				Mesh mesh = meshFilter.sharedMesh;
                if (mesh == null){
                        meshFilter.mesh = new Mesh();
                        mesh = meshFilter.sharedMesh;
                }
                mesh.Clear();
		
				
				//-- mesh Collider --//
		 		MeshCollider meshCol = GetComponent<MeshCollider>();
                if (meshCol==null){
                        Debug.LogError("MeshCollider not found!");
                        return;
                }
                
				Mesh newMeshCollider = meshCol.sharedMesh;
                if (newMeshCollider == null){
                        meshCol.sharedMesh = new Mesh();
                        newMeshCollider = meshCol.sharedMesh;
                }
                newMeshCollider.Clear();
		
		
		
			
			if(segments < 3){
				Debug.LogError("Num of segments lower to 3 DENIED!");
				return;
			}
		
			//--step is the angle of forward --//
			step = (2 *Mathf.PI) / segments;
			cAngle = 2*Mathf.PI; //-- start in 360 and going decrement to 0
			
			TangencialFactor = Mathf.Tan (step);
			radialFactor = Mathf.Cos (step);
			
			float x = radius;
			float y = 0;


		
			// -- each point along circle --//
			Vector3 []cVertices = new Vector3[segments+1];	
			
			//--First vertex --//
			cVertices[0] = new Vector3(0,0,0); // center of circle
			
		
			//--Generate vertices remains along the path--//
			
			//--stable cicle --//
			for(int i=1; i<(segments+1); i++){
					
				float tx = -y;
				float ty = x;

			x += tx * TangencialFactor;
			y += ty * TangencialFactor;

					
				x *= radialFactor;
				y *= radialFactor;

				cVertices[i] = new Vector3(x, y, 0);
			}
			
			
		
			
			
			int idx = 1;
			int indices = (segments)*3;
		
			// -- Already have vertices, now build triangles --//
			int []cTriangles = new int[indices]; // one triagle for each section (has 3 vertex per triang)
			
		
		for(int i=0; i<(indices); i+=3){
				cTriangles[i+1] = 0; //center of circle
				cTriangles[i] = idx; //next vertex
				
			
				if(i >= (indices - 3)){
				//-- if is the last vertex (one loop)
					cTriangles[i+2] = 1;	
				}else{
				//-- if is the next point --//
					cTriangles[i+2] = idx+1; //next next vertex	
				}
				idx++;
				
			
			}
				
			
			mesh.vertices = cVertices;
			mesh.triangles = cTriangles;
			
			// Mesh collider //
			newMeshCollider.name = "circleCollider";
			newMeshCollider.vertices = cVertices;
			newMeshCollider.triangles = cTriangles;
		
			//-- adding touch --//
			GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Diffuse")); // REF1
          //  renderer.sharedMaterial.color = new Color(240/255f,95/255f,95/255f);
            
			//-- recalculate limits --//
			mesh.RecalculateNormals();
            mesh.RecalculateBounds();
			mesh.Optimize();
        }
        
}