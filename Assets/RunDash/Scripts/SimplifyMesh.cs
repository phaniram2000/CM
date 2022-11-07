using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;


public class SimplifyMesh : MonoBehaviour
{
    //public float quality = 0.5f;

    // void OnGUI()
    //    {
    //    if (GUI.Button(Rect.MinMaxRect(0, 0, 200, 200), "Simplify Mesh"))
    //    {
    //        var originalMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    //        var meshSimplifer = new UnityMeshSimplifier.MeshSimplifier();
    //        meshSimplifer.Initialize(originalMesh);
    //        meshSimplifer.SimplifyMesh(quality);
    //        var destMesh = meshSimplifer.ToMesh();
    //        GetComponent<SkinnedMeshRenderer>().sharedMesh = destMesh;
    //    }
    //}
}
