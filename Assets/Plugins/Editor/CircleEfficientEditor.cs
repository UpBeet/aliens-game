using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (CircleEfficient))] 
public class CircleEfficientEditor : Editor {
	[MenuItem ("GameObject/Create Other/CircleEfficient")]
        static void Create(){
                GameObject gameObject = new GameObject("Circle");
				CircleEfficient s = gameObject.AddComponent<CircleEfficient>();
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                meshFilter.mesh = new Mesh();
                s.Rebuild();
        }
        
        public override void OnInspectorGUI ()
        {
			CircleEfficient obj;

				obj = target as CircleEfficient;

                if (obj == null)
                {
                        return;
                }
        
                base.DrawDefaultInspector();
                EditorGUILayout.BeginHorizontal ();
                
                // Rebuild mesh when user click the Rebuild button
                if (GUILayout.Button("Rebuild")){
                        obj.Rebuild();
                }
				EditorGUILayout.EndHorizontal ();
        }
}