using UnityEngine;
using UnityEngine.ProBuilder;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public struct Objet
{
    public string name;
    public Vector3 position;
    public Objet(GameObject o){
        name = o.name;
        position = o.transform.position;
    }
    public GameObject ToGameObject(){
        GameObject o = GameObject.Instantiate(Resources.Load(name) as GameObject, GameObject.Find("Level").transform);
        o.name = name;
        o.transform.position = position;
        return o;
    }
    public override string ToString()
    {
        return name + position;
    }
}
[System.Serializable]
public class Datas
{
    public Vector3[] positions;
    public Face[] faces;
    public List<Objet> o;
    public Datas(ProBuilderMesh m, GameObject[] go)
    {
        positions = new Vector3[m.positions.Count];
        m.positions.CopyTo(positions,0);
        faces = new Face[m.faces.Count];
        m.faces.CopyTo(faces,0);
        o = new List<Objet>();
        for (var i = 0; i < go.Length; i++)
        {
            Objet obj = new Objet(go[i]);
            o.Add(obj);
        }
    }
}
