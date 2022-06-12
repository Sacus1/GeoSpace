using UnityEngine;
using UnityEngine.InputSystem;
using static Editor;
public class NewObject : MonoBehaviour
{
    private int indexObject = 0;
    // menu to select object
    public GameObject dropDown;
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
                GameObject s = Instantiate(Resources.Load("Sphere") as GameObject); //On fait apparaitre une sphere dans le level
                s.name = "Sphere";
                Rigidbody rb;
                PlayerInput inp;
                Collider col;
                //On desactive tout ses composant
                if (s.TryGetComponent<Rigidbody>(out rb))
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                }
                if (s.TryGetComponent<PlayerInput>(out inp))
                {
                    inp.DeactivateInput();
                }
                if (s.TryGetComponent<Collider>(out col))
                {
                    col.isTrigger = true;
                }
                foreach (MonoBehaviour script in s.GetComponents<MonoBehaviour>())
                {
                    script.enabled = false;
                }
                playableObject.Add(s); // On le met dans la liste des objet du jeu
                break;
            case 1://Exit
                GameObject e = Instantiate(Resources.Load("Exit") as GameObject, GameObject.Find("Level").transform);
                e.name = "Exit";
                playableObject.Add(e);
                break;
            case 2: //Camera
                GameObject o = Instantiate(Resources.Load("Camera") as GameObject, GameObject.Find("Cams").transform);
                o.name = "Camera";
                playableObject.Add(o);
                break;
        }
        dropDown.SetActive(false); //On enleve le menu
    }
}