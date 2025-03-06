using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( LineRenderer))]
public class LineRenderOffestAnimation : MonoBehaviour
{
    Material _mat;
    float _offset;

    [SerializeField] float _Speed = 1;

    void OnEnable()
    {
        _offset = 0;
    }


    void Awake()
    {
        _mat = GetComponent<LineRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(_mat == null) return;

        _mat.mainTextureOffset = new Vector2(_offset, 0);
        _offset += Time.deltaTime * _Speed;
    }
}
