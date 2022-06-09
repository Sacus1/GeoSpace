using UnityEngine;
public class ObjectManager : MonoBehaviour
{
[System.Serializable]
public struct Interactable
{
    public GameObject g;
    public int m;
}
    public Interactable[] interactable ;
    public void Activate(int mod)
    {
        foreach (Interactable inte in interactable)
        {
            if (inte.m != mod) continue;
            inte.g.SendMessage("Activate",mod);
        }

    }
    public void Desactivate(int mod)
    {
        foreach (Interactable inte in interactable)
        {
            if (inte.m != mod) continue;
            inte.g.SendMessage("Desactivate",mod);
        }
    }

}
