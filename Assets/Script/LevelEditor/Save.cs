using UnityEngine;
using UnityEngine.ProBuilder;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using static Editor;
public class Save : MonoBehaviour {
    public void SaveFile(string destination,Datas datas)
     {
         //string destination = Application.persistentDataPath + "/save.dat";
        string data = JsonUtility.ToJson(datas);
         FileStream file;
         if(File.Exists(destination)) file = File.OpenWrite(destination);
         else file = File.Create(destination);
         BinaryFormatter bf = new BinaryFormatter();
         bf.Serialize(file, data);
         file.Close();
     }

     public Datas LoadFile(string destination)
     {
         //string destination = Application.persistentDataPath + "/save.dat";
         FileStream file;

         if(File.Exists(destination)) file = File.OpenRead(destination);
         else
         {
             return (Datas)null;
         }

         BinaryFormatter bf = new BinaryFormatter();
         Datas data =  JsonUtility.FromJson<Datas>((string)bf.Deserialize(file));
         file.Close();

        return data;
     }
     void OnLoad()
    {
        // delete all object in playableObject
        for (int i = 0; i < playableObject.Count; i++)
        {
            Destroy(playableObject[i]);
            playableObject.RemoveAt(i);
        }
        // Get datas from the level file
        Datas datas = LoadFile(Application.persistentDataPath + "/level.json");
        if (datas != null)
        {
            // Load level face
            mesh.RebuildWithPositionsAndFaces(datas.positions, datas.faces);
            mesh.Refresh();
            newFaces = new Face[mesh.faceCount];
            mesh.faces.CopyTo(newFaces, 0);
            mesh.SetMaterial(newFaces, normalMat);
            mesh.Refresh();
            // Load playable object
            for (int i = 0; i < datas.o.Count; i++)
            {
                GameObject o = datas.o[i].ToGameObject();
                playableObject.Add(o);
            }
        }
    }
    void OnSave()
    {
        // TODO deselect all faces
        // Save level
        SaveFile(Application.persistentDataPath + "/level.json", new Datas(mesh, playableObject.ToArray()));
    }
}