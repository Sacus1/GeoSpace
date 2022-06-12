using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;
public class Editor : MonoBehaviour
{
    // Attributs
    public static List<GameObject> playableObject = new List<GameObject>();
    // stack of history to undo
    public Camera mainCamera;

    // left , middle click and mouse position
    public InputAction _mousePos;
    public static InputAction mousePos;
    // material when object is selected and not selected
    public static Material mat, normalMat;
    public Material _mat, _normalMat;
    public static GameObject selectedObject;
    // All face of the level
    public static Face[] newFaces;
    // All face of the level that are selected
    public static List<Face> selectedFaces = new List<Face>();
    // the object to be created
    public static bool play;
    [HideInInspector]
    public static ProBuilderMesh mesh;
    private PlayerInput playerInput;
    void Awake()
    {
        // set material
        mat = _mat;
        normalMat = _normalMat;
        // set mouse position
        mousePos = _mousePos;
        // enable the input system
        mousePos.Enable();
    }
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        mesh = GetComponent<ProBuilderMesh>();
        newFaces = new Face[mesh.faceCount];
        mesh.faces.CopyTo(newFaces, 0);
    }
    #region Deplacement
    void OnMoveMouse(InputValue mvtValue)
    {
        // get action2 value (bool) from action2Input
        bool action2 = playerInput.currentActionMap["Action2"].ReadValue<float>() > 0.5f;
        // same for action3
        bool action3 = playerInput.currentActionMap["Action3"].ReadValue<float>() > 0.5f;
        if (action2)
        {
            // get the mouse delta
            Vector2 Vect = mvtValue.Get<Vector2>();
            // move the camera
            mainCamera.transform.position += mainCamera.transform.up * Vect.y * Time.fixedDeltaTime + mainCamera.transform.right * Time.fixedDeltaTime * Vect.x; // On deplace la camera
        }
        else if (action3)
        {
            // get mouse position
            Vector3 mousePosition = mousePos.ReadValue<Vector2>();
            // get the mouse delta
            Vector2 Vect = mvtValue.Get<Vector2>();
            // rotate the camera
            mainCamera.transform.RotateAround(mainCamera.ScreenToWorldPoint(mousePosition) + mainCamera.transform.forward * 5, new Vector3(Vect.y, Vect.x, 0), 5);
            mainCamera.transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, 0);//On normalise l'angle pour pas se retrouvé a l'envers
        }
        else if (selectedObject != gameObject && !Selector.arrows.isArrow(selectedObject) && selectedObject != null)
        {
            if (GetComponent<PlayerInput>().currentActionMap["Action1"].ReadValue<float>() < 0.5f)
            {
                Selector.arrows.selected = 0;
                return;
            }
            // take delta value
            Vector2 delta = mvtValue.Get<Vector2>();
            // mouse position i
            Vector3 mousePosition = mousePos.ReadValue<Vector2>();
            // set mouse position z to distance between camera and object
            mousePosition.z = Vector3.Distance(mainCamera.transform.position, selectedObject.transform.position);
            // mouse position in world
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            if (Selector.arrows.selected == 1)
            {
                // arrow offset
                float offset = Selector.arrows.up.transform.localScale.y / 2 + 1;
                // Move the selected object y at mouse position in world minus arrow offset
                selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, mouseWorldPosition.y - offset, selectedObject.transform.position.z);
            }
            else if (Selector.arrows.selected == 2)
            {
                // arrow offset
                float offset = Selector.arrows.right.transform.localScale.x / 2 + 1;
                // Move the selected object x at mouse position in world minus arrow offset
                selectedObject.transform.position = new Vector3(mouseWorldPosition.x - offset, selectedObject.transform.position.y, selectedObject.transform.position.z);
            }
            else if (Selector.arrows.selected == 3)
            {
                // arrow offset
                float offset = Selector.arrows.forward.transform.localScale.z / 2 + 1;
                // Move the selected object z at mouse position in world minus arrow offset
                selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, mouseWorldPosition.z - offset);
            }

        }
    }
    #endregion
    void OnScroll(InputValue scrollValue)
    {
        // get the camera go forward by the scroll value
        Vector2 scroll = scrollValue.Get<Vector2>();
        // move the camera
        mainCamera.transform.position += mainCamera.transform.forward * scroll.y * Time.fixedDeltaTime;

    }
    void OnPlay()
    {
        mainCamera.enabled = (false);
        foreach (GameObject item in playableObject)
        {
            Rigidbody rb;
            PlayerInput inp;
            Collider col;
            if (item.TryGetComponent<Rigidbody>(out rb))
                rb.useGravity = true;
            if (item.TryGetComponent<PlayerInput>(out inp))
                inp.ActivateInput();
            if (item.TryGetComponent<Collider>(out col))
            {
                col.isTrigger = false;
            }
            foreach (MonoBehaviour script in item.GetComponents<MonoBehaviour>())
            {
                script.enabled = true;
            }
        }
        play = true;
    }
    void OnDelete(){
        if (selectedObject != null && selectedObject != gameObject){
            playableObject.Remove(selectedObject);
            Destroy(selectedObject);
            selectedObject = null;
        }
    }
}
