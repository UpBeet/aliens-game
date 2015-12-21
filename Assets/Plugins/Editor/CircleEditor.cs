using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (Circle))] 
public class CircleEditor : Editor {
        [MenuItem ("GameObject/Create Other/Circle")]
        static void Create(){
                GameObject gameObject = new GameObject("Circle");
                Circle s = gameObject.AddComponent<Circle>();
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                meshFilter.mesh = new Mesh();
                s.Rebuild();
        }
        
        public override void OnInspectorGUI ()
        {
                Circle obj;

                obj = target as Circle;

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