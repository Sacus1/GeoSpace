using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Cameras : MonoBehaviour
{
    //Camera dans la scene
    public List<GameObject> cams = new List<GameObject>();
    //L'indice de la camera selectionne
    private int SelectedCam = 0;
    private InputAction Lclick, Rclick;
    //Resultat du raycast utilise pour savoir si la cam voit le joueur
    private RaycastHit hit;
    //Changement de camera , desactive l'ancienne camera et active la nouvelle
    private void ChangeCam(int index)
    {
        cams[SelectedCam].GetComponent<Camera>().enabled = false;
        cams[SelectedCam].GetComponent<AudioListener>().enabled = false;
        SelectedCam = index;
        cams[SelectedCam].GetComponent<Camera>().enabled = true;
        cams[SelectedCam].GetComponent<AudioListener>().enabled = true;
    }
    private int normalCam(int index)
    {
        index = index > cams.Count - 1 ? 0 : index;
        index = index < 0 ? cams.Count - 1 : index;
        return index;
    }
    private void OnDestroy()
    {
        Lclick.Disable();
        Rclick.Disable();
    }
    private void Awake()
    {
        Lclick = new InputAction(binding: "<Mouse>/leftButton");
        Lclick.performed += ctx =>
             {
                 if (!this.enabled) return;
                 // On cherche la camera precedente
                 int index = normalCam(SelectedCam - 1);
                 while (index != SelectedCam)
                 {
                     Vector3 camPos = cams[index].transform.position;
                     // Si la camera voit le joueur
                     if (Physics.Raycast(camPos, transform.position - camPos, out hit) && (hit.transform.gameObject.CompareTag("Player"))) break;
                     index = normalCam(index - 1);
                 }
                 ChangeCam(index);
             };
        Lclick.Enable();
        Rclick = new InputAction(binding: "<Mouse>/rightButton");
        Rclick.performed += ctx =>
        {
            if (!this.enabled) return;
            // On cherche la prochaine camera
            int index = normalCam(SelectedCam + 1);
            while (index != SelectedCam)
            {
                Vector3 camPos = cams[index].transform.position;
                // Si la camera voit le joueur
                if (Physics.Raycast(camPos, transform.position - camPos, out hit) && (hit.transform.gameObject.tag == "Player")) break;

                index = normalCam(index + 1);
            }
            ChangeCam(index);
        };
        Rclick.Enable();
    }
    void OnCameraLock()
    {
        PlayerPrefs.SetInt("CameraMode", PlayerPrefs.GetInt("CameraMode", 0) == 0 ? 1 : 0);
    }
    void Start()
    {
        //On defini recupere les objet dans la scene
        List<GameObject> objets = new List<GameObject>(gameObject.scene.GetRootGameObjects());
        Transform level = GameObject.Find("Level").transform;
        for (var i = 0; i < level.childCount ; i++)
        {
            objets.Add(level.GetChild(i).gameObject);
        }
        foreach (GameObject o in objets)
        {
            //Si il s'agit d'un camera on l'ajoute a la liste des camera
            if (o.TryGetComponent(typeof(Camera), out Component component))
            {
                cams.Add(o);
                o.GetComponent<Camera>().enabled = false;
                o.GetComponent<AudioListener>().enabled = false;
            }
        }
        // On active la premiere camera
        cams[SelectedCam].GetComponent<Camera>().enabled = true;
        cams[SelectedCam].GetComponent<AudioListener>().enabled = true;
        int index = -1;
        while (index < 0 || index != SelectedCam)
        {
            index = index > -1 ? index : SelectedCam;
            Vector3 camPos = cams[index].transform.position;
            // Si la camera voit le joueur
            if (Physics.Raycast(camPos, transform.position - camPos, out hit) && hit.transform.gameObject.CompareTag("Player")) break;
            index = normalCam(index + 1);
        }
        ChangeCam(index);
    }
    void UpdateCam()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        double posX = mousePos.x;
        // Si on est dans la partie droite de l'ecran
        if (posX > Screen.width * 2 / 3)
        {
            // on deplace la camera vers la droite
            cams[SelectedCam].transform.localEulerAngles += new Vector3(0, 20 * (float)(posX / Screen.width) * Time.fixedDeltaTime, 0);
        }
        // Si on est dans la partie gauche de l'ecran
        else if (posX < Screen.width / 3)
        {
            // on deplace la camera vers la gauche
            cams[SelectedCam].transform.localEulerAngles -= new Vector3(0, 20 * (float)(1 - posX / Screen.width) * Time.fixedDeltaTime, 0);
        }
        double posY = mousePos.y;
        // Si on est dans la partie haute de l'ecran
        if (posY > Screen.height * 2 / 3)
        {
            // on deplace la camera vers le haut
            cams[SelectedCam].transform.localEulerAngles -= new Vector3((float)(20 * (float)(posY / Screen.height)) * Time.fixedDeltaTime, 0, 0);
        }
        // Si on est dans la partie basse de l'ecran
        else if (posY < Screen.height / 3)
        {
            // on deplace la camera vers le bas
            cams[SelectedCam].transform.localEulerAngles += new Vector3((float)(20 * (float)(1 - posY / Screen.height)) * Time.fixedDeltaTime, 0, 0);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Si le joueur appuis sur click droit
        if (PlayerPrefs.GetInt("CameraMode", 1) == 1)
        {
            // Auto cam
            cams[SelectedCam].transform.LookAt(transform);
            cams[SelectedCam].GetComponent<Camera>().fieldOfView = 60;
        }
        else
            // Manual cam
            UpdateCam();
    }
}
