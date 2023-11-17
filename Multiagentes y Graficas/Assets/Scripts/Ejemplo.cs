// public class Apply_transforms : MonoBehaviour
// {
//     [SerializeField] Vector3 displacement;
//     [SerializeField] float displacementAngle; 
//     [SerializeField] AXIS rotationAXIS;

//     //Wheel model
//     [SerializeField] GameObject wheelModel;

//     Mesh mesh;
//     Vector3[] baseVertices;//Array to store the base vertices
//     Vector3[] newVertices;//Array to store the new vertices

//     [SerializeField] Wheel[] wheel; //Array to store the wheels
//     void Start()
//     {
//         mesh = GetComponentInChildren<MeshFilter>().mesh;
//         baseVertices = mesh.vertices;

//         //Create a copy to testing the vertices
//         newVertices = new Vector3 [baseVertices.Length];
//         for (int i = 0; i<baseVertices.Length; i++){
//             newVertices[i] = baseVertices[i];
//         }

//         //Instantiate the wheels
//         for(int i = 0; i<wheel.Length; i++){
//             GameObject temp = Instantiate(wheelModel, new Vector3(0,0,0), Quaternion.identity);
//             wheel[i].mesh = temp.GetComponentInChildren<MeshFilter>().mesh;
//             wheel[i].baseVertices = wheel[i].mesh.vertices;
//             wheel[i].newVertices = new Vector3[wheel[i].baseVertices.Length];
//             for(int j = 0; j<wheel[i].baseVertices.Length; j++){ //Create a copy of the baseVertices
//                 wheel[i].newVertices[j] = wheel[i].baseVertices[j];
//             }
//         }
//     }
//     //This function obtains a new vector using displacement vectors in x and z
//     float GetAngle(Vector3 displacement) {
//     float angle = Mathf.Atan2(displacement.x, displacement.z) * Mathf.Rad2Deg;//Get the angle in degrees
//     return angle;
// }
//     void Update()
//     {
//         DoTransform();
//     }
//     //This function applies the transform to the car and the wheels
//     void DoTransform(){

//         displacementAngle = GetAngle(displacement);
//         Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x *Time.time,
//                                                       displacement.y *Time.time,
//                                                       displacement.z *Time.time);
//         Matrix4x4 rotate = HW_Transforms.RotateMat(displacementAngle, rotationAXIS);
//         Matrix4x4 composite = move * rotate;
//         Matrix4x4 initial_pos_wheel1 = HW_Transforms.TranslationMat(wheel[0].position.x,
//                                                                     wheel[0].position.y,
//                                                                     wheel[0].position.z);
//         Matrix4x4 initial_pos_wheel2 = HW_Transforms.TranslationMat(wheel[1].position.x,
//                                                                     wheel[1].position.y,
//                                                                     wheel[1].position.z);
//         Matrix4x4 initial_pos_wheel3 = HW_Transforms.TranslationMat(wheel[2].position.x,
//                                                                     wheel[2].position.y,
//                                                                     wheel[2].position.z);
//         Matrix4x4 inital_pos_wheel4 = HW_Transforms.TranslationMat(wheel[3].position.x,
//                                                                     wheel[3].position.y,
//                                                                     wheel[3].position.z);
//         Matrix4x4 rotate_wheel1 = HW_Transforms.RotateMat(wheel[0].rotation * Time.time, AXIS.X);
//         Matrix4x4 wheel_composite1 =  composite * initial_pos_wheel1 * rotate_wheel1;  
//         Matrix4x4 wheel_composite2 = composite * initial_pos_wheel2 * rotate_wheel1;
//         Matrix4x4 wheel_composite3 = composite * initial_pos_wheel3 * rotate_wheel1;
//         Matrix4x4 wheel_composite4 =  composite * inital_pos_wheel4 * rotate_wheel1;

//         //Apply the composite for the car
//         for(int i = 0; i<baseVertices.Length; i++){
//             Vector4 temp = new Vector4(baseVertices[i].x,
//                                        baseVertices[i].y,
//                                        baseVertices[i].z,1);
//             newVertices[i] = composite * temp;
//         }
        
//         //Apply the composite for each wheel
//         for(int i = 0; i<wheel[0].baseVertices.Length; i++){
//             Vector4 temp = new Vector4(wheel[0].baseVertices[i].x,
//                                        wheel[0].baseVertices[i].y,
//                                        wheel[0].baseVertices[i].z,1);

//             wheel[0].newVertices[i] = wheel_composite1 * temp;

//         }

//         for(int i = 0; i <wheel[1].baseVertices.Length; i++){
//             Vector4 temp = new Vector4(wheel[1].baseVertices[i].x,
//                                        wheel[1].baseVertices[i].y,
//                                        wheel[1].baseVertices[i].z,1);

//             wheel[1].newVertices[i] = wheel_composite2 * temp;
//         }

//         for(int i = 0; i <wheel[2].baseVertices.Length; i++){
//             Vector4 temp = new Vector4(wheel[2].baseVertices[i].x,
//                                        wheel[2].baseVertices[i].y,
//                                        wheel[2].baseVertices[i].z,1);

//             wheel[2].newVertices[i] = wheel_composite3 * temp;
//         }

//         for(int i = 0; i <wheel[3].baseVertices.Length; i++){
//             Vector4 temp = new Vector4(wheel[3].baseVertices[i].x,
//                                        wheel[3].baseVertices[i].y,
//                                        wheel[3].baseVertices[i].z,1);

//             wheel[3].newVertices[i] = wheel_composite4 * temp;
//         }
//         //Apply the new vertices to the mesh and recalculate the normals
//         mesh.vertices = newVertices;
//         mesh.RecalculateNormals();
//         wheel[0].mesh.vertices = wheel[0].newVertices; 
//         wheel[0].mesh.RecalculateNormals();
//         wheel[1].mesh.vertices = wheel[1].newVertices;
//         wheel[1].mesh.RecalculateNormals();
//         wheel[2].mesh.vertices = wheel[2].newVertices;
//         wheel[2].mesh.RecalculateNormals();
//         wheel[3].mesh.vertices = wheel[3].newVertices;
//         wheel[3].mesh.RecalculateNormals();
//     }

// }
// //Class to store the wheels
// [System.Serializable]
// public class Wheel{

//     public Mesh mesh;
//     public Vector3[] baseVertices;
//     public Vector3[] newVertices;
//     [SerializeField] public Vector3 position;
//     [SerializeField] public float rotation;

//     public Wheel(Vector3 pos){
//         position = pos;
//     }
// }