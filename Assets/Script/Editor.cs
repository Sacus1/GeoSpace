using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
public class Editor : MonoBehaviour
{
    public List<GameObject> playableObject;
    public Stack<Datas> history;
    public Camera mainCamera;
    public GameObject dropDown;
    public InputAction action2Input, action3Input, mousePos;
    public Material mat, normalMat;
    private Material oldMat;
    private GameObject selectedObject;
    private Face[] newFaces;
    private List<Face> selectedFaces = new List<Face>();
    private int indexObject = 0;
    private bool play, action2, action3;
    private ProBuilderMesh mesh;
    void Awake()
    {
        //Right click
        action2Input.started += ctx => //debut click
        {
            action2 = true;
        };
        action2Input.canceled += ctx => //fin click
        {
            action2 = false;
        };
        action2Input.Enable();
        //Middle click
        action3Input.started += ctx => //debut click
        {
            action3 = true;
        };
        action3Input.canceled += ctx =>//fin click
        {
            action3 = false;
        };
        action3Input.Enable();
        mousePos.Enable();
    }
    void Start()
    {
        history = new Stack<Datas>();
        mesh = GetComponent<ProBuilderMesh>();
        newFaces = new Face[mesh.faceCount];
        mesh.faces.CopyTo(newFaces, 0);
    }
    #region Sauvegarde
    void OnLoad()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        Datas d = Save.LoadFile(Application.persistentDataPath + "/level.json");
        if (d != null)
        {
            mesh.RebuildWithPositionsAndFaces(d.positions, d.faces);
            mesh.Refresh();
            foreach (var item in d.o)
            {
                playableObject.Add(item.ToGameObject());
            }
            newFaces = new Face[mesh.faceCount];
            mesh.faces.CopyTo(newFaces, 0);
            mesh.SetMaterial(newFaces, normalMat);
            mesh.Refresh();
        }
    }
    void OnSave()
    {
        mesh.SetMaterial(newFaces, normalMat);
        Save.SaveFile(Application.persistentDataPath + "/level.json", new Datas(mesh, playableObject.ToArray()));
    }
    #endregion
    #region Apparition d'objet
    //Quand on appuis sur la touche pour crée un objet
    void OnCreate()
    {
        dropDown.SetActive(true); // On ouvre le menu
    }
    //Quand on change l'objet a faire spawn
    public void ChangeIndex(int i)
    {
        indexObject = i;
    }
    public void SpawnObject()
    {
        switch (indexObject)
        {
            case 0:// Sphere
                GameObject s = Instantiate(Resources.Load("Sphere") as GameObject, GameObject.Find("Level").transform); //On fait apparaitre une sphere dans le level
                s.name = "Sphere";
                playableObject.Add(s); // On le met dans la liste des objet du jeu
                Rigidbody rb;
                PlayerInput inp;

                Collider col;
                //On desactive tout ses composant
                if (s.TryGetComponent<Rigidbody>(out rb))
                    rb.useGravity = false;
                if (s.TryGetComponent<PlayerInput>(out inp))
                    inp.DeactivateInput();
                if (s.TryGetComponent<Collider>(out col))
                {
                    col.isTrigger = true;
                }
                foreach (MonoBehaviour script in s.GetComponents<MonoBehaviour>())
                {
                    script.enabled = false;
                }
                break;
            case 1://Exit
                GameObject e = Instantiate(Resources.Load("Exit") as GameObject, GameObject.Find("Level").transform);
                e.name = "Exit";
                playableObject.Add(e);
                break;
            case 2: //Camera
                GameObject o = Instantiate(Resources.Load("Camera") as GameObject, GameObject.Find("Level").transform);
                o.name = "Camera";
                playableObject.Add(o);
                break;
        }
        dropDown.SetActive(false); //On enleve le menu
    }

    #endregion
    #region  Selection Face/Object
    void OnAction1()
    {
        RaycastHit hit;
        Vector3 mousePosition = mousePos.ReadValue<Vector2>();
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.ScreenToWorldPoint(mousePosition + new Vector3(0, 0, 50)), out hit, Mathf.Infinity))
        {
            GameObject old = selectedObject;
            selectedObject = hit.transform.gameObject;
            if (selectedObject == gameObject)
            {
                int i;
                for (i = 0; i < newFaces.Length; i++)
                {
                    if ((newFaces[i][0] - 2) / 4 == hit.triangleIndex / 2)
                        break;
                }
                if (i < newFaces.Length)
                {
                    if (selectedFaces.Contains(newFaces[i]))
                        RemoveSelectedFace(newFaces[i]);
                    else
                        AddSelectedFace(newFaces[i]);
                }
            }
            else
            {
                MeshRenderer m;
                if (selectedObject.TryGetComponent<MeshRenderer>(out m))
                {
                    oldMat = m.material;
                    m.material = mat;
                }
                if (old.TryGetComponent<MeshRenderer>(out m) && !old.TryGetComponent<ProBuilderMesh>(out ProBuilderMesh pm))
                    m.material = oldMat;
                else if (old.TryGetComponent<ProBuilderMesh>(out ProBuilderMesh pm2))
                {
                    mesh.SetMaterial(newFaces, normalMat);
                }
            }
        }
    }
    void AddSelectedFace(Face face)
    {
        mesh.ToMesh();
        selectedFaces.Add(face);
        mesh.SetMaterial(selectedFaces, mat);
        mesh.Refresh();
        mesh.ToMesh(); // Jsp pourquoi mais ca marche pas sans
        mesh.Refresh();
    }
    void RemoveSelectedFace(Face face)
    {
        mesh.ToMesh();
        selectedFaces.Remove(face);
        mesh.SetMaterial(new Face[]{face}, normalMat);
        mesh.Refresh();
        mesh.ToMesh(); // Jsp pourquoi mais ca marche pas sans
        mesh.Refresh();
    }
    #endregion
    #region Deplacement object/face
    //Quand on utilise les direction wasd + qe
    void OnMove(InputValue movementValue)
    {
        if (play) return; // Si on est en train de jouer
        if (selectedObject != null)
        {
            selectedObject.transform.Translate(movementValue.Get<Vector3>() * 2);
        }
    }
    void OnExtrude()
    {
        if (play) return;
        if (selectedObject != gameObject) return;
        history.Push(new Datas(mesh, playableObject.ToArray()));
        mesh.SetMaterial(ExtrudeElements.Extrude(mesh, selectedFaces, ExtrudeMethod.FaceNormal, -2), normalMat);
        newFaces = new Face[mesh.faceCount]; // On actualise les faces
        mesh.faces.CopyTo(newFaces, 0);
        mesh.ToMesh(); // Jsp pourquoi mais ca marche pas sans
        mesh.Refresh();
    }
    void OnUndo()
    {
        for (var i = playableObject.Count; i > 0; i--)
        {
            Destroy(playableObject[i]);
            playableObject.RemoveAt(i);
        }
        Datas d;
        if (history.TryPop(out d))
        {
            mesh.ToMesh();
            mesh.RebuildWithPositionsAndFaces(d.positions, d.faces);
            mesh.Refresh();
            foreach (var item in d.o)
            {
                playableObject.Add(item.ToGameObject());
            }
            newFaces = new Face[mesh.faceCount];
            mesh.faces.CopyTo(newFaces, 0);
            mesh.SetMaterial(newFaces, normalMat);
            mesh.ToMesh();
            mesh.Refresh();
        }

    }
    #endregion
    #region Deplacement
    void OnMoveMouse(InputValue mvtValue)
    {
        if (action2)
        {
            Vector2 Vect = mvtValue.Get<Vector2>();
            mainCamera.transform.position += mainCamera.transform.up * Vect.y * Time.fixedDeltaTime + mainCamera.transform.right * Time.fixedDeltaTime * Vect.x; // On deplace la camera
        }
        else if (action3)
        {
            Vector3 mousePosition = mousePos.ReadValue<Vector2>();
            Vector2 Vect = mvtValue.Get<Vector2>();
            mainCamera.transform.RotateAround(mainCamera.ScreenToWorldPoint(mousePosition) + mainCamera.transform.forward * 5, new Vector3(Vect.y, Vect.x, 0), 5);
            mainCamera.transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, 0);//On normalise l'angle pour pas se retrouvé a l'envers
        }
    }
    #endregion

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
}
