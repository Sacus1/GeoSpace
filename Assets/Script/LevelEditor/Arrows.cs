using UnityEngine;
[System.Serializable]
public class Arrows
{
    public GameObject up;
    public GameObject right;
    public GameObject forward;
    public int selected = 0; // 0 = none 1 = up 2 = right 3 = foward

    // constructor
    public Arrows(GameObject up, GameObject right, GameObject forward)
    {
        this.up = up;
        this.right = right;
        this.forward = forward;
    }
    public void Select(char axis)
    {
        switch (axis)
        {
            case 'y':
                selected = 1;
                break;
            case 'x':
                selected = 2;
                break;
            case 'z':
                selected = 3;
                break;
            default:
                selected = 0;
                break;
        }
    }
    public bool isArrow(GameObject o)
    {
        if (o == up || o == right || o == forward)
            return true;
        return false;
    }
    public void SetPos(Vector3 pos)
    {
        up.transform.position =  pos + new Vector3(0, up.transform.localScale.y / 2 + 1, 0);
        right.transform.position = pos + new Vector3(right.transform.localScale.x / 2 + 1, 0, 0);
        forward.transform.position = pos + new Vector3(0, 0, forward.transform.localScale.z / 2 + 1);
    }
    public void SetVisible(bool visible)
    {
        up.SetActive(visible);
        right.SetActive(visible);
        forward.SetActive(visible);
    }
}