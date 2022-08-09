using System;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private InputController _inputController;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        var worldPoint = _camera.ScreenToWorldPoint(_inputController.PointerDownPosition);
        transform.position = worldPoint;
    }
}