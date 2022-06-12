using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionInterrupteur : MonoBehaviour
{
    public bool cube;
    public bool hasObject;
    public int modifier = 0;
    private void Start()
    {
        cube = transform.parent.gameObject.name == "CubeInter";
    }
    void OnCollisionEnter(Collision collision)
    {
        if (hasObject) return;
        if (collision.gameObject.name == (cube ? "CCube" : "CSphere") || collision.gameObject.name == (cube ? "Cube" : "Sphere"))
        {
            // variable pour ne pas faire des calculs inutiles chaque update
            hasObject = true;
            // on appuis sur le bouton
            transform.Translate(0, -.2f, 0);
            // On active les objets lié a l'interrupteur dans la scene avec un broadcast
            GameObject.Find("Activable").BroadcastMessage("Activate", modifier);
        }
    }
    private void Update()
    {
        if (hasObject)
        {
            // On regarde si l'objet est toujours dans l'interrupteur
            Collider[] cs = Physics.OverlapBox(transform.position, transform.localScale / 2);
            hasObject = false;
            foreach (Collider c in cs)
            {
                // Si l'objet est toujours dans l'interrupteur on le code
                if (c.gameObject.name == (cube ? "CCube" : "CSphere") || c.gameObject.name == (cube ? "Cube" : "Sphere"))
                {
                    hasObject = true;
                    return;
                }
            }
            // Si l'objet n'est plus dans l'interrupteur
            if (!hasObject)
            {
                // On desactive les objets lié a l'interrupteur
                GameObject.Find("Activable").BroadcastMessage("Desactivate", modifier);
                // On remet le bouton dans sa position initiale
                transform.Translate(0, .2f, 0);
            }
        }
    }
}
