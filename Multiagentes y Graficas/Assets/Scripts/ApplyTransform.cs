using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ApplyTransform : MonoBehaviour
{
    [SerializeField] Vector3 displacement;
    [SerializeField] float angle;
    [SerializeField] AXIS rotationAXIS;
    Mesh mesh;
    Vector3[] vertices;
    Vector3[] newVertices;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshFilter>().mesh;
        vertices = mesh.vertices;

        //Create a copy to testing the vertices
        newVertices = new Vector3 [vertices.Length];
        for (int i = 0; i<vertices.Length; i++){
            newVertices[i] = vertices[i];
        }

    }

    // Update is called once per frame
    void Update()
    {
      DoTransform();
      DoRotate();   
    }

    void DoTransform(){
        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x*Time.deltaTime,displacement.y*Time.deltaTime,displacement.z*Time.deltaTime);
        Matrix4x4 composite = move;
        for(int i = 0; i<newVertices.Length; i++){
            Vector4 temp = new Vector4(newVertices[i].x,newVertices[i].y,newVertices[i].z,1);
            newVertices[i] = composite * temp;
        }
        mesh.vertices = newVertices;
    }

    void DoRotate(){
        Matrix4x4 rotate = HW_Transforms.RotateMat(angle,rotationAXIS);
        Matrix4x4 composite = rotate;
        for(int i = 0; i<newVertices.Length; i++){
            Vector4 temp = new Vector4(newVertices[i].x,newVertices[i].y,newVertices[i].z,1);
            newVertices[i] = composite * temp;
        }
        mesh.vertices = newVertices;
    }
}
