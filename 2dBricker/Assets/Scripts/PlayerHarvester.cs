using UniRx;
using UnityEngine;

namespace Vorval.CalmBall.Game
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