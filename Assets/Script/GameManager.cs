using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static bool instanciate;
    public GameObject CloneCube, CloneSphere;
    private GameObject cube, sphere;
    private void ResetEnergy(Scene a,Scene b){
        EnergyScript.ResetEnergy();
    }
    private void Awake()
    {
        if (instanciate) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        instanciate = true;
        SceneManager.activeSceneChanged += ResetEnergy;
        }
    public void Start()
    {
        localization.translate();
        // when the scene is loaded make a anoymous function
        SceneManager.sceneLoaded += delegate (Scene a, LoadSceneMode b)
        {
        // translate the scene
            localization.translate();
        };
    }
    public void createCube(Transform pos)
    {
        if (cube != (GameObject)null)
        {
            Destroy(cube);
        }
        cube = Instantiate(CloneCube, pos.position, pos.rotation, transform) as GameObject;
        cube.name = "CCube";

    }

    public void createSphere(Transform pos)
    {
        if (sphere != (GameObject)null)
        {
            Destroy(sphere);
        }
        sphere = Instantiate(CloneSphere, pos.position, pos.rotation, transform) as GameObject;
        sphere.name = "CSphere";
    }
}
