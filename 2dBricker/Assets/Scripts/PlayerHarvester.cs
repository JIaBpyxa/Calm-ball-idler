using System;
using UnityEngine;

namespace Bricker.Game
{
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
}