using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public static BackgroundScroller instance;

    public float scrollSpeed = 2f;

    private Vector2 savedOffset;

    Renderer render;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
        render = GetComponent<Renderer>();
    }

    void Start()
    {
        savedOffset = render.sharedMaterial.GetTextureOffset("_MainTex");
    }

    void Update()
    {
       
        savedOffset.x += scrollSpeed * Time.deltaTime;
        savedOffset.x = Mathf.Repeat(savedOffset.x, 1);

        Vector2 offset = savedOffset;
        render.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

}