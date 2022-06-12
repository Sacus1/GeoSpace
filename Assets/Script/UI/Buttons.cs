using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Buttons : MonoBehaviour
{
    public Button MenuButton, RetryButton;
    public TMPro.TextMeshProUGUI Chrono, Best;
    public float T;
    private void Update()
    {
        T = Time.timeSinceLevelLoad;
        string milliseconde = ((int)(T * 100 % 100)).ToString();
        string seconde = ((int)T % 60).ToString();
        string minute = ((int)T / 60).ToString();
        Chrono.text = (minute.Length == 1 ? "0" + minute : minute) + ":" + (seconde.Length == 1 ? "0" + seconde : seconde) + ":" + (milliseconde.Length == 1 ? "0" + milliseconde : milliseconde);

    }
    public void Retry()
    {
        Globals.dead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }
    public void mainMenu()
    {
        Globals.dead = false;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    void Start()
    {
        float T = PlayerPrefs.GetFloat("Level " + (SceneManager.GetActiveScene().buildIndex - 2) + " Time", 0f);
        string milliseconde = ((int)(T * 100 % 100)).ToString();
        string seconde = ((int)T % 60).ToString();
        string minute = ((int)T / 60).ToString();
        Best.text = "Best:\n" + (minute.Length == 1 ? "0" + minute : minute) + ":" + (seconde.Length == 1 ? "0" + seconde : seconde) + ":" + (milliseconde.Length == 1 ? "0" + milliseconde : milliseconde);
        RetryButton.onClick.AddListener(Retry);
        MenuButton.onClick.AddListener(mainMenu);
    }
}
