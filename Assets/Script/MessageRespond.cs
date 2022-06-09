using UnityEngine;
using TMPro;
public class MessageRespond : MonoBehaviour
{
    public int lvl = 1;
    private int maxLevel;
    private void Start()
    {
        maxLevel = PlayerPrefs.GetInt("Level", 2) - 1;
    }
    public void Activate(int mod)
    {
        if (gameObject.name == "Play")
        {
            lvl += mod == 0 ? -1 : 1;
            lvl = lvl < 1 ? 1 : lvl;
            lvl = lvl > maxLevel ? maxLevel : lvl;
            transform.Find("NiveauN").gameObject.GetComponent<TextMeshPro>().text = lvl.ToString();
        }
        if (gameObject.CompareTag("Door"))
        {
            transform.Translate(0, 5, 0);
        }
    }
    public void Desactivate(int mod)
    {
        if (gameObject.CompareTag("Door"))
        {
            transform.Translate(0, -5, 0);
        }
    }

}