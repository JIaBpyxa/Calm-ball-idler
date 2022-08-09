using System;
using UnityEngine;

namespace Bricker.Game
{
    public abstract class AbstractBall : MonoBehaviour, IHarvestable
    {
        public Action OnHarvested { get; set; }

        public abstract void Harvest();
    }
}