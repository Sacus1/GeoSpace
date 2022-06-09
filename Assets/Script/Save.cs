using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class Save {
    public static void SaveFile(string destination,Datas datas)
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

     public static Datas LoadFile(string destination)
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
}