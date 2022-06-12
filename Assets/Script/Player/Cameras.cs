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
        // find camera component in cam[selectedCam] descendant and disable it
        cams[SelectedCam].GetComponentInChildren<Camera>().enabled = false;
        // same with audio listener
        cams[SelectedCam].GetComponentInChildren<AudioListener>().enabled = false;
        SelectedCam = index;
        // find camera component in cam[selectedCam] descendant and enable it
        cams[SelectedCam].GetComponentInChildren<Camera>().enabled = true;
        // same with audio listener
        cams[SelectedCam].GetComponentInChildren<AudioListener>().enabled = true;
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
        //On defini recupere tout les gameObjects qui ont le tag "MainCamera" (pas camera car mainCamera est deja present dans le tag)
        cams = new List<GameObject>(GameObject.FindGameObjectsWithTag("MainCamera"));
        // on desactive tout les camera et audioListener pour eviter d'en avoir plusieurs
        foreach (GameObject cam in cams)
        {
            cam.GetComponentInChildren<Camera>().enabled = false;
            cam.GetComponentInChildren<AudioListener>().enabled = false;
        }
        // On active la premiere camera
        cams[0].GetComponentInChildren<Camera>().enabled = true;
        cams[0].GetComponentInChildren<AudioListener>().enabled = true;
        // on cherche la première camera qui voit le joueur
        int index = normalCam(SelectedCam + 1);
        while (index != SelectedCam)
        {
            Vector3 camPos = cams[index].transform.position;
            // Si la camera voit le joueur
            if (Physics.Raycast(camPos, transform.position - camPos, out hit) && (hit.transform.gameObject.tag == "Player")) break;

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
            cams[SelectedCam].transform.Find("Support/Mobile").localEulerAngles -= new Vector3(0, 0, (float)(20 * (float)(posX / Screen.height)) * Time.fixedDeltaTime);
        }
        // Si on est dans la partie gauche de l'ecran
        else if (posX < Screen.width / 3)
        {
            // on deplace la camera vers la gauche
            cams[SelectedCam].transform.Find("Support/Mobile").localEulerAngles += new Vector3(0, 0, (float)(20 * (float)(1 - posX / Screen.height)) * Time.fixedDeltaTime);
        }
        double posY = mousePos.y;
        // Si on est dans la partie haute de l'ecran
        if (posY > Screen.height * 2 / 3)
        {
            // on deplace la camera vers le haut
            cams[SelectedCam].transform.Find("Support/Mobile/Camera").localEulerAngles += new Vector3(0, 20 * (float)(posY / Screen.width) * Time.fixedDeltaTime, 0);
        }
        // Si on est dans la partie basse de l'ecran
        else if (posY < Screen.height / 3)
        {
            // on deplace la camera vers le bas
            cams[SelectedCam].transform.Find("Support/Mobile/Camera").localEulerAngles -= new Vector3(0, 20 * (float)(1 - posY / Screen.width) * Time.fixedDeltaTime, 0);
        }
    }
    // a method that normalize the angle between 0 and 360
    private float NormalizeAngle(float angle)
    {
        if (angle < 0)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return angle;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Si le joueur appuis sur click droit
        if (PlayerPrefs.GetInt("CameraMode", 1) == 1)
        {
            // Change the camera rotation to look at the player
            //player position
            Vector3 playerPos = transform.position;
            // camera rotation y
            float camRotY = NormalizeAngle(cams[SelectedCam].transform.localEulerAngles.y);
            // camera position
            Vector3 camPos = cams[SelectedCam].transform.position;
            // angle around y axis between player and camera
            float angle;
            if ((camRotY > 0 && camRotY <= 90))
            { // si la camera est en -z
                angle = Mathf.Atan2(playerPos.y - camPos.y, playerPos.z - camPos.z) * Mathf.Rad2Deg;
            }
            else if (camRotY > 90 && camRotY <= 180)
            { // si la camera est en -x
                angle = Mathf.Atan2(playerPos.y - camPos.y, playerPos.x - camPos.x) * Mathf.Rad2Deg;
            }
            else if (camRotY > 180 && camRotY <= 270)
            {// si la camera est en +z
                angle = 90 - Mathf.Atan2(playerPos.y - camPos.y, playerPos.z - camPos.z) * Mathf.Rad2Deg + 90;
            }
            else
            { // si la camera est en +x
                angle = 90 - Mathf.Atan2(playerPos.y - camPos.y, playerPos.x - camPos.x) * Mathf.Rad2Deg + 90;
            }

            cams[SelectedCam].transform.Find("Support/Mobile/Camera").localEulerAngles = new Vector3(180, angle, 0);
            //same around x axis
            float angle2 = Mathf.Atan2(playerPos.x - camPos.x, playerPos.z - camPos.z) * Mathf.Rad2Deg;
            cams[SelectedCam].transform.Find("Support/Mobile").localEulerAngles = new Vector3(0, 180, angle2 - cams[SelectedCam].transform.localEulerAngles.y + 90);
        }
        else
            // Manual cam
            UpdateCam();
    }
}
