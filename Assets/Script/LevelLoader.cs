using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;
    public void LoadLevel(int index){

        StartCoroutine(LoadLevelEnum(index));
    }
    IEnumerator LoadLevelEnum(int index){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }
}
