using UnityEngine;

public static class EnergyScript
{
    // Start is called before the first frame update
    public const float ENERGY_MAX = 100f;
    private static float energy = 100f;
    public static void ResetEnergy()
    {
        energy = ENERGY_MAX;
    }
    public static void AddEnergy(float e)
    {
        if (!GameObject.Find("GUI")) return;
        RectTransform EnergyBar = GameObject.Find("GUI").transform.Find("Energy").transform.Find("EnergyInterior").GetComponent<RectTransform>();
        if (energy + e <= ENERGY_MAX)
        {
            energy += e;
        }
        else energy = ENERGY_MAX;
        if (e < 0)
        {
            energy += e;
        }
        EnergyBar.sizeDelta = new Vector2(energy * 2 - 4, EnergyBar.sizeDelta.y);
        EnergyBar.anchoredPosition = new Vector3(-energy * 2 + 2, EnergyBar.anchoredPosition.y, 0);
        if (energy <= 0)
        {
            GameObject.Find("GUI").transform.Find("GameOver").gameObject.SetActive(true);
            GameObject.FindWithTag("Player").transform.Find("Explosion").gameObject.SetActive(true);
            Globals.dead = true;
        }
    }

}
