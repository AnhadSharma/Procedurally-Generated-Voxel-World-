using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicOcclusion : MonoBehaviour
{
    private Renderer _renderer;
    private Camera _mainCamera;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the block is inside the camera's view frustum
        if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(_mainCamera),_renderer.bounds))
        {
            _renderer.enabled = true;
        }
        else
        {
            _renderer.enabled = false;
        }
    }
}