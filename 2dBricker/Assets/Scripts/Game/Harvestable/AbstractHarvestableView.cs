using System;
using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public abstract class AbstractHarvestableView : MonoBehaviour
    {
        [Space] [SerializeField] protected float activationDuration = .1f;
        [SerializeField] protected float deactivationDuration = .1f;

        protected Vector3 initialScale;

        private void Awake()
        {
            initialScale = transform.localScale;
        }

        public abstract void Activate();
        public abstract void Deactivate(Action deactivationAction);
    }
}