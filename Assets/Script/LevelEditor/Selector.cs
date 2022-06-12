using UnityEngine;
using UnityEngine.ProBuilder;
using static Editor;
public class Selector : MonoBehaviour
{
    // material before object was selected
    private Material oldMat;
    private Camera mainCamera;
    public Arrows _arrows;
    public static Arrows arrows;
    void Start()
    {
        arrows = _arrows;
        mainCamera = Camera.main;
    }
    void OnAction1()
    {
        RaycastHit hit;
        // get the mouse position
        Vector3 mousePosition = mousePos.ReadValue<Vector2>();
        // raycast to get the object or face
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.ScreenToWorldPoint(mousePosition + new Vector3(0, 0, 1000)), out hit, Mathf.Infinity))
        {
            GameObject old = selectedObject;
            selectedObject = hit.transform.gameObject;
            MeshRenderer m;
            // if the old object has a mesh renderer and is not the level
            if (old != null && old.TryGetComponent<MeshRenderer>(out m) && old != gameObject)
                // we change the material to previous one
                m.material = oldMat;
            // if the object is the level then we select a face
            if (selectedObject == gameObject)
            {
                arrows.SetVisible(false);
                int i;
                // search for the face
                for (i = 0; i < newFaces.Length; i++)
                {
                    if ((newFaces[i][0] - 2) / 4 == hit.triangleIndex / 2)
                        break;
                }
                // if a face is found
                if (i < newFaces.Length)
                {
                    if (selectedFaces.Contains(newFaces[i]))
                        RemoveSelectedFace(newFaces[i]);
                    else
                        AddSelectedFace(newFaces[i]);
                }

            }
            else if (arrows.isArrow(selectedObject))
            {
                if (selectedObject == arrows.up)
                    arrows.Select('y');
                else if (selectedObject == arrows.right)
                    arrows.Select('x');
                else if (selectedObject == arrows.forward)
                    arrows.Select('z');
                selectedObject = old;

            }
            else
            {
                // deselect all faces
                foreach (Face f in selectedFaces)
                {
                    RemoveSelectedFace(f);
                }
                // set arrow visible
                arrows.SetVisible(true);
                // if the object has a mesh renderer
                if (selectedObject.TryGetComponent<MeshRenderer>(out m))
                {
                    // we change the material
                    oldMat = m.material;
                    m.material = mat;
                }
            }
        }
        // visualize the raycast
        Debug.DrawRay(mainCamera.transform.position, mainCamera.ScreenToWorldPoint(mousePosition + new Vector3(0, 0, 1000)), Color.red, 10);
    }
    void AddSelectedFace(Face face)
    {
        mesh.ToMesh();
        selectedFaces.Add(face);
        mesh.SetMaterial(selectedFaces, mat);
        mesh.Refresh();
        mesh.ToMesh(); // Jsp pourquoi mais ca marche pas sans
        mesh.Refresh();
    }
    void RemoveSelectedFace(Face face)
    {
        mesh.ToMesh();
        selectedFaces.Remove(face);
        mesh.SetMaterial(new Face[] { face }, normalMat);
        mesh.Refresh();
        mesh.ToMesh(); // Jsp pourquoi mais ca marche pas sans
        mesh.Refresh();
    }
    private void Update()
    {
        // arrow follow selected object if exists and is not the level and is not an arrow
        if (selectedObject != null && selectedObject != gameObject && !arrows.isArrow(selectedObject))
        {
            arrows.SetPos(selectedObject.transform.position);
        }
    }
}