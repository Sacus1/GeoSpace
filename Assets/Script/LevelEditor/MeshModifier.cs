using UnityEngine;
using static Editor;
using UnityEngine.ProBuilder;
using System.Collections.Generic;
using UnityEngine.ProBuilder.MeshOperations;

public class MeshModifier : MonoBehaviour
{
    private Stack<Datas> history = new Stack<Datas>();
    void OnExtrude()
    {
        if (play) return;
        if (selectedObject != gameObject) return;
        // add level data state to undo
        history.Push(new Datas(mesh, playableObject.ToArray()));
        // extrude the selected faces
        mesh.SetMaterial(ExtrudeElements.Extrude(mesh, selectedFaces, ExtrudeMethod.FaceNormal, -2), normalMat);
        newFaces = new Face[mesh.faceCount]; // On actualise les faces
        mesh.faces.CopyTo(newFaces, 0);
        mesh.ToMesh(); // Jsp pourquoi mais ca marche pas sans
        mesh.Refresh();
    }
    void OnUndo()
    {
        if (play) return;
        // remove all objects from the level
        for (var i = playableObject.Count; i > 0; i--)
        {
            Destroy(playableObject[i]);
            playableObject.RemoveAt(i);
        }
        Datas d;
        // if there is something to undo
        if (history.TryPop(out d))
        {
            // we restore the level data
            mesh.ToMesh();
            mesh.RebuildWithPositionsAndFaces(d.positions, d.faces);
            mesh.Refresh();
            foreach (var item in d.o)
            {
                playableObject.Add(item.ToGameObject());
            }
            newFaces = new Face[mesh.faceCount];
            mesh.faces.CopyTo(newFaces, 0);
            mesh.SetMaterial(newFaces, normalMat);
            mesh.ToMesh();
            mesh.Refresh();
        }

    }

}