using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class mouvement : MonoBehaviour
{
    public float Speed, JumpPower, turnSpeed,energySaut;
    private Rigidbody r;
    private Vector2 move;
    public bool cube;
    private void OnClone()
    {
        GameObject.Find("GameManager").SendMessage(cube?"createCube":"createSphere", transform);
        EnergyScript.AddEnergy(-5);
    }
    void OnFly(){
        r.AddForce(new Vector3(0, JumpPower , 0));
        EnergyScript.AddEnergy(-energySaut);
    }
    private void OnMove(InputValue movementValue)
    {
        move = movementValue.Get<Vector2>();
    }
    void Start()
    {
        r = GetComponent<Rigidbody>();
        if (!cube){
            StartCoroutine(resetCube());
        }
    }
    IEnumerator resetCube(){
        cube = true;
        yield return new WaitForSeconds(.1f);
        cube = false;
    }
    void FixedUpdate()
    {
        if (Globals.dead) return;
        if (cube)
        {
            r.velocity += new Vector3(0, 0, move.y * Speed * Time.fixedDeltaTime);
            r.velocity += new Vector3(move.x * Speed * Time.fixedDeltaTime, 0, 0);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles += new Vector3(0, move.x * turnSpeed * Time.fixedDeltaTime, 0);
            r.velocity += (transform.right * Speed * Time.fixedDeltaTime * move.y);
            transform.localEulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }

    }
}
