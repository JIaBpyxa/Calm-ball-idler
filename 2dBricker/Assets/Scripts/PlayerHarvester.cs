using System;
using Bricker.Game;
using UnityEngine;

public class PlayerHarvester : Harvester
{
    [SerializeField] private InputController _inputController;

    private void Start()
    {
        _inputController.PointerWorldPosition.Subscribe(MoveToPosition);
    }

    private void MoveToPosition(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }
}