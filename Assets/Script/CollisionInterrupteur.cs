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
        if (collision.gameObject.name == (cube ? "CCube" : "CSphere") || collision.gameObject.name == (cube ? "Cube" : "Sphere") )
        {
            hasObject = true;
            transform.Translate(0, -.2f, 0);
            GameObject.Find("Level").gameObject.SendMessage("Activate", modifier);
        }
    }
    private void Update()
    {
        if (hasObject)
        {
            Collider[] cs = Physics.OverlapBox(transform.position, transform.localScale / 2);
            hasObject = false;
            foreach (Collider c in cs)
            {
                if (c.gameObject.name == (cube ? "CCube" : "CSphere") || c.gameObject.name == (cube ? "Cube" : "Sphere") )
                    hasObject = true;
            }
            if (!hasObject)
            {
                GameObject.Find("Level").gameObject.SendMessage("Desactivate",modifier);
                transform.Translate(0, .2f, 0);

            }
        }
    }
}
