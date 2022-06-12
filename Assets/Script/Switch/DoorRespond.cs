using UnityEngine;
using System.Collections;
public class DoorRespond : MessageRespondObj
{
    // Attributs
    // Reverse behavior of the door
    public bool reverse = false;
    // An int value to identify the connection
    public int openID;
    // animator of the door
    public Animator animator;
    bool isPlaying = false;
    // Methods

    IEnumerator PlayAnime(bool state)
    {
        animator.SetBool("isOpen", state);
        isPlaying = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isPlaying = false;
    }
    IEnumerator WaitForAnimation(float time,bool state)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(PlayAnime(state));
    }
    // Use this for initialization
    void Start()
    {
        // Get the animator
        animator = GetComponent<Animator>();
        StartCoroutine(PlayAnime(true));
        if (!reverse)
        {
            StartCoroutine(WaitForAnimation(animator.GetCurrentAnimatorStateInfo(0).length, false));
        }
    }
    public override void Activate(int mod)
    {
        if (mod == openID)
        {
            if (!isPlaying)
            {
                StartCoroutine(PlayAnime(!reverse));
            }
            else
            {
                StartCoroutine(WaitForAnimation(animator.GetCurrentAnimatorStateInfo(0).length, !reverse));
            }
        }
    }
    public override void Desactivate(int mod)
    {
        if (mod == openID)
        {
            if (!isPlaying)
            {
                StartCoroutine(PlayAnime(reverse));
            }
            else
            {
                StartCoroutine(WaitForAnimation(animator.GetCurrentAnimatorStateInfo(0).length, reverse));
            }
        }
    }
}