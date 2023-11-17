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
            if (i % 2 == 0)
            {
                wheelPosition = new Vector3(displacement.x * (i - 1), 0, -displacement.z / 2f);
            }
            else
            {
                wheelPosition = new Vector3(displacement.x * Mathf.Sign(i - 2), 0, displacement.z / 2f);
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
    }

    void DoTransform()
    {
        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x * Time.time,
                                                      displacement.y * Time.time,
                                                      displacement.z * Time.time);

        Matrix4x4 rotate = HW_Transforms.RotateMat(angle * Time.time, rotationAxis);

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
}
