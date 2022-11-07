using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureRotate : MonoBehaviour
{

    float scrollSpeed = 1f;
    Renderer rend;
    float offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offset += Time.deltaTime * scrollSpeed;
        rend.materials[0].SetTextureOffset("_BaseMap", new Vector2(0, -offset));
    }

}
