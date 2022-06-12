using UnityEngine;
using TMPro;
public class LevelSelector : MessageRespondObj
{
    public TextMeshPro text;
    int maxLevel ;
    public int lvl = 0;
    private void Start() {
        maxLevel = PlayerPrefs.GetInt("Level", 1);
        text.text =  lvl.ToString();
    }
    public override void Activate(int mod)
    {
        // Si mod == 0 alors lvl + 1
        if (mod == 0)
        {
            lvl++;
            if (lvl > maxLevel)
                lvl = maxLevel;
            text.text = lvl.ToString();
        }
        // Si mod == 1 alors lvl - 1
        else if (mod == 1)
        {
            lvl--;
            if (lvl < 1)
                lvl = 1;
            text.text = lvl.ToString();
        }
    }
    public override void Desactivate(int mod)
    {
    }
}
