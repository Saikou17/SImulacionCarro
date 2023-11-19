using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public GameObject llanta;
    [Header("Velocidad y dirección del carro")]
    [SerializeField] Vector3 displacement;
    [Header("Angulo de rotación:")]
    [SerializeField] float angle;
    [Header("Eje de rotación:")]
    [SerializeField] AXIS rotationAxis;
    [Header("Velocidad de rotación:")]
    [SerializeField] float spinAngle;
    float timeAngle = 0f;
    Mesh carMesh;
    Mesh[] wheelMeshes = new Mesh[4];
    Vector3[] carBaseVertices;
    Vector3[][] wheelBaseVertices = new Vector3[4][];
    Vector3[] carNewVertices;
    Vector3[][] wheelNewVertices = new Vector3[4][];
    Vector3[] wheelScale = new Vector3[4];
    Vector3 carScale;
    GameObject[] wheels = new GameObject[4];

    void Start()
    {
        carScale = transform.localScale;
        carMesh = GetComponentInChildren<MeshFilter>().mesh;
        carBaseVertices = carMesh.vertices;
        carNewVertices = new Vector3[carBaseVertices.Length];

        for (int i = 0; i < 4; i++)
        {
            Vector3 wheelPosition;
            if (i == 0)
            {
                wheelPosition = new Vector3(1.0f , 0.3f, -1.0f);
            }
            else if (i == 1)
            {
                wheelPosition = new Vector3(-1.0f, 0.3f, -1.0f);
            }
            else if (i == 2)
            {
                wheelPosition = new Vector3(1.0f, 0.3f, 1.5f);
            }
            else
            {
                wheelPosition = new Vector3(-1.0f, 0.3f, 1.5f);
            }

            wheels[i] = Instantiate(llanta, wheelPosition, llanta.transform.rotation);
            wheelMeshes[i] = wheels[i].GetComponentInChildren<MeshFilter>().mesh;
            wheelBaseVertices[i] = wheelMeshes[i].vertices;
            wheelNewVertices[i] = new Vector3[wheelBaseVertices[i].Length];
            wheelScale[i] = wheels[i].transform.localScale;
        }
    }

    void Update()
    {
        DoTransform();
        DoPath();
    }

    void DoTransform()
    {
        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x * Time.time,
                                                      displacement.y * Time.time,
                                                      displacement.z * Time.time);

        Matrix4x4 rotate = HW_Transforms.RotateMat(angle , rotationAxis);

        Matrix4x4 composite = move * rotate;

        for (int i = 0; i < carNewVertices.Length; i++)
        {
            Vector4 temp = new Vector4(carBaseVertices[i].x,
                                       carBaseVertices[i].y,
                                       carBaseVertices[i].z,
                                       1);
            carNewVertices[i] = composite * temp;
        }

        carMesh.vertices = carNewVertices;
        carMesh.RecalculateNormals();

        for (int i = 0; i < wheels.Length; i++)
        {
            Matrix4x4 moveWheel = HW_Transforms.TranslationMat(wheels[i].transform.position.x,
                                                               wheels[i].transform.position.y,
                                                               wheels[i].transform.position.z);

            Matrix4x4 spin = HW_Transforms.RotateMat(spinAngle * Time.time,
                                                     AXIS.X);

            Matrix4x4 compositeWheel = moveWheel * composite * spin;

            for (int j = 0; j < wheelNewVertices[i].Length; j++)
            {
                Vector4 temp = new Vector4(wheelBaseVertices[i][j].x,
                                           wheelBaseVertices[i][j].y,
                                           wheelBaseVertices[i][j].z,
                                           1);
                wheelNewVertices[i][j] = compositeWheel * temp;
            }

            wheelMeshes[i].vertices = wheelNewVertices[i];
            wheelMeshes[i].RecalculateNormals();
        }
    }

    void DoPath(){
        float currentTime = Time.time;
        if (currentTime - timeAngle >= 10f){
            timeAngle = currentTime;
            switch(angle){
                case 0:
                    angle = 90;
                    displacement.x = displacement.z;
                    displacement.z = 0;
                    break;
                case 90:
                    angle = 180;
                    displacement.z = -displacement.x;
                    displacement.x = 0;
                    break;
                case 180:
                    angle = 270;
                    displacement.x = displacement.z;
                    displacement.z = 0;
                    break;
                case 270:
                    angle = 360;
                    displacement.z = -displacement.x;
                    displacement.x = 0;
                    break;
            }
        }
    }
}
