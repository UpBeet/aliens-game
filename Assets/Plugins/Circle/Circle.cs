//--Martin Ysa (2013)--//
// -- Circle.cs -- //
//-- use polar coordinates for draw all vertices --//

using UnityEngine;
using System.Collections;

//-- Adding components --//
[RequireComponent (typeof (MeshCollider))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]

public class Circle : MonoBehaviour {
        
		
		public float radius = 1.0f;
		public int segments = 20;
		public bool noise = false;
		public float noiseIntensity = 0.4f;
	
		float step;
		float cAngle;

        
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
				Debug.LogError("Num of segments must be 3 or more");
				return;
			}
		
			//--step is the angle of forward --//
			step = (2 *Mathf.PI) / segments;
			cAngle = 2*Mathf.PI; //-- start in 360 and going decrement to 0
			
		
			// -- each point along circle --//
			Vector3 []cVertices = new Vector3[segments+1];	
			
			//--First vertex --//
			cVertices[0] = new Vector3(0,0,0); // center of circle
			
		
			//--Generate vertices remains along the path--//
			if(noise){
				//-- Applying perlin noise --//
				float cNoise;
				float rnd;
				float cX;
				float cY;
			
				for(int i=1; i<(segments+1); i++){
					rnd = Random.Range(0.1f,noiseIntensity);
					cX = Mathf.Sin(cAngle);
					cY = Mathf.Cos(cAngle);
					cNoise = Mathf.PerlinNoise(cX*rnd,cY*rnd);
					
					cVertices[i] = new Vector3(cX * radius*2 *cNoise, cY * radius*2 * cNoise, 0);
					cAngle += step;
				}
				
			
			}else{
				//--stable cicle --//
				for(int i=1; i<(segments+1); i++){
					cVertices[i] = new Vector3(Mathf.Sin(cAngle)*radius, Mathf.Cos(cAngle)*radius, 0);
					cAngle += step;
				}
			}
			
			
		
			
			
			int idx = 1;
			int indices = (segments)*3;
		
			// -- Already have vertices, now build triangles --//
			int []cTriangles = new int[indices]; // one triagle for each section (has 3 vertex per triang)
			
		
			for(int i=0; i<(indices); i+=3){
				cTriangles[i] = 0; //center of circle
				cTriangles[i+1] = idx; //next vertex
				
			
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
			//GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Diffuse")); // REF1
            //GetComponent<Renderer>().sharedMaterial.color = new Color(67/255f,32/255f,0.5f);
            
			//-- recalculate limits --//
			mesh.RecalculateNormals();
            mesh.RecalculateBounds();
			mesh.Optimize();
        }
        
}