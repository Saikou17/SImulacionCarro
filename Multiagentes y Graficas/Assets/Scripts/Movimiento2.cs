using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento2 : MonoBehaviour
{
    //Creamos los parametros de nuestro movimiento
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

    //Creamos y guardamos los elementos del carro
    Mesh carMesh;// Malla del modelo del carro
    Vector3[] carBaseVertices; //Vertices originales del carro
    Vector3[] carNewVertices; //Vertices que iremos guardando del carro

    //Creamos y guardamos los elementos de las ruedas
    GameObject[] wheels = new GameObject[4];
    Mesh[] wheelMeshes = new Mesh[4];
    Vector3[][] wheelBaseVertices = new Vector3[4][];
    Vector3[][] wheelNewVertices = new Vector3[4][];

    void Start()
    {   
        //Guardamos la malla del carro para poder manipular sus vertices
        carMesh = GetComponentInChildren<MeshFilter>().mesh;
        //Guardamos los vertices originales del carro
        carBaseVertices = carMesh.vertices;
        //Para cada una de las ruedas, obtenemos su malla y sus vertices originales
        carNewVertices = new Vector3[carBaseVertices.Length];
        //Guardamos los vertices originales en carNewVertices
        for(int i=0; i<carBaseVertices.Length; i++){
            carNewVertices[i] = carBaseVertices[i];
        }

        //Inicializamos las cuatro ruedas
        for (int i = 0; i < 4; i++)
        {
            //Ingresamos de manera manual la posicion de las llantas
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
            //Instanciamos las llantas con su modelo, posicion y rotacion
            wheels[i] = Instantiate(llanta, wheelPosition, llanta.transform.rotation);
            //Guardamos las mallas de cada una de las ruedas
            wheelMeshes[i] = wheels[i].GetComponentInChildren<MeshFilter>().mesh;
            //Guardamos los vertices originales de cada una de las ruedas
            wheelBaseVertices[i] = wheelMeshes[i].vertices;
            //Guardamos los vertices nuevos de cada una de las ruedas
            wheelNewVertices[i] = new Vector3[wheelBaseVertices[i].Length];
            //Guardamos los vertices nuevos en wheelNewVertices
            for(int j=0; j<wheelBaseVertices[i].Length; j++){
                wheelNewVertices[i][j] = wheelBaseVertices[i][j];
            }
        }
    }

    void Update()
    {
        DoTransform();
        // DoPath();
    }

    //Funcion de movimiento
    void DoTransform()
    {
        //Creamos una matriz de traslacion y la multiplicamos por el vector de desplazamiento
        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x * Time.deltaTime,
                                                      displacement.y * Time.deltaTime,
                                                      displacement.z * Time.deltaTime);
        //Creamos una matriz de rotacion para el carro sobre el eje y
        // Matrix4x4 rotate = HW_Transforms.RotateMat(angle , rotationAxis);

        //Guardamo la nueva matriz de movimiento
        Matrix4x4 composite = move ;

        //Recorremos el arreglo de los vertices
        for (int i = 0; i < carNewVertices.Length; i++)
        {
            //Generamos un vector 4 con los vertices actuales de nuestro carro
            Vector4 temp = new Vector4(carNewVertices[i].x,
                                       carNewVertices[i].y,
                                       carNewVertices[i].z,
                                       1);
            //Aplicamos la matriz de transformacion a este vector para obtener su nuevo valor
            carNewVertices[i] = composite * temp;
        }

        //Cambiamos los vertices a su nueva posicion
        carMesh.vertices = carNewVertices;
        //Recalculamos los vectores normales de cada uno de los vertices
        carMesh.RecalculateNormals();

        for (int i = 0; i < wheels.Length; i++)
        {

            // Matrix4x4 moveWheel = HW_Transforms.TranslationMat(wheels[i].transform.position.x * Time.deltaTime,
            //                                                    wheels[i].transform.position.y * Time.deltaTime ,
            //                                                    wheels[i].transform.position.z * Time.deltaTime);

            Matrix4x4 spin = HW_Transforms.RotateMat(spinAngle * Time.deltaTime ,
                                                     AXIS.X);

            Matrix4x4 compositeWheel = composite ;

            for (int j = 0; j < wheelNewVertices[i].Length; j++)
            {
                Vector4 temp = new Vector4(wheelNewVertices[i][j].x,
                                           wheelNewVertices[i][j].y,
                                           wheelNewVertices[i][j].z,
                                           1);
                wheelNewVertices[i][j] = compositeWheel * temp;
            }

            wheelMeshes[i].vertices = wheelNewVertices[i];
            wheelMeshes[i].RecalculateNormals();
        }
    }

}
