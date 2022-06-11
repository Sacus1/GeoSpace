
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionPlayer : MonoBehaviour
{
    public float distance = 3;
    GameObject energyObject, Cube, Sphere;
    private LevelLoader loader;
    private void Start()
    {
        Sphere = Resources.Load("Sphere") as GameObject;
        Cube = Resources.Load("Cube") as GameObject;
        loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        if (PlayerPrefs.GetInt("Tutorial", 0) == 1 && SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Finish":
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    PlayerPrefs.SetInt("Tutorial", 1);
                    loader.LoadLevel(collision.gameObject.GetComponent<LevelSelector>().lvl + 1);
                    break;
                }
                if (SceneManager.GetActiveScene().buildIndex + 2 >= SceneManager.sceneCountInBuildSettings)
                {
                    loader.LoadLevel(1);
                    PlayerPrefs.DeleteKey("Level");
                    break;
                }
                if (GameObject.Find("GUI"))
                {
                    string bests = "Level" + (SceneManager.GetActiveScene().buildIndex - 2) + " Time";
                    if (PlayerPrefs.GetFloat(bests,Mathf.Infinity) < GameObject.Find("GUI").GetComponent<Buttons>().T)
                        PlayerPrefs.SetFloat(bests, GameObject.Find("GUI").GetComponent<Buttons>().T);
                    if (PlayerPrefs.GetInt("Level") < SceneManager.GetActiveScene().buildIndex + 1)
                        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
                }
                loader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case "Energy":
                energyObject = collision.gameObject;
                EnergyScript.AddEnergy(20);
                StartCoroutine(waiter());
                break;
            case "Exit":
                print("quit");
                Application.Quit();
                break;
        }
        switch (collision.gameObject.name)
        {
            case "EnterSphere":
                if (gameObject.name == "Sphere")
                    GetComponent<Rigidbody>().velocity -= GetComponent<Rigidbody>().velocity * 4;
                else
                {
                    GameObject c = Instantiate(Sphere, collision.transform.up * distance + collision.transform.position - collision.transform.right, transform.rotation, GameObject.Find("Level").transform);
                    c.name = "Sphere";
                    c.GetComponent<mouvement>().cube = false;
                    Destroy(gameObject);
                }
                break;
            case "EnterCube":
                if (gameObject.name == "Cube")
                    GetComponent<Rigidbody>().velocity = -GetComponent<Rigidbody>().velocity * 4;
                else
                {
                    GameObject c = Instantiate(Cube, collision.transform.up * -distance + collision.transform.position - collision.transform.right, transform.rotation, GameObject.Find("Level").transform);
                    c.name = "Cube";
                    c.GetComponent<mouvement>().cube = true;
                    Destroy(gameObject);
                }
                break;
        }
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Drain"))
        {
            EnergyScript.AddEnergy(-8f * Time.fixedDeltaTime);
        }
    }
    IEnumerator waiter()
    {

        GameObject g = energyObject;
        g.SetActive(false);
        yield return new WaitForSeconds(5);
        g.SetActive(true);
    }
}
