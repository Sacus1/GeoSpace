using UnityEngine;
public class DoorRespond : MessageRespondObj
{
    // Attributs
    // Reverse behavior of the door
    public bool reverse = false;
    // An int value to identify the connection
    public int openID;
    // animator of the door
    public Animator animator;
    // Methods

    // Use this for initialization
    void Start()
    {
        // Get the animator
        animator = GetComponent<Animator>();
        // Set animation to reverse
        animator.SetBool("isOpen", reverse);
        // Set the door position
        transform.Translate(0,reverse?-5:5,0);
    }
    public override void Activate(int mod)
    {
        if (mod == openID)
        {
            // If the door is not reversed, open it else close it
            animator.SetBool("isOpen", !reverse);
        }
    }
    public override void Desactivate(int mod)
    {
        if (mod == openID)
        {
            // If the door is not reversed, close it else open it
            animator.SetBool("isOpen", reverse);
        }
    }
}