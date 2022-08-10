using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class HarvestablePool : MonoBehaviour
    {
        private int _poolSize;
        private Object _prefab;
        private Queue<AbstractHarvestable> _queue;
        private AbstractHarvestable.Factory _factory;


        [Inject]
        private void Construct(AbstractHarvestable.Factory factory)
        {
            _factory = factory;
        }

        public void Init(Object prefab, int count)
        {
            _poolSize = count;
            _prefab = prefab;
            _queue = new Queue<AbstractHarvestable>();

            for (var id = 0; id < _poolSize; id++)
            {
                var newHarvestable = _factory.Create(_prefab);
                newHarvestable.Init();
                newHarvestable.IsActive.Subscribe(isActive =>
                {
                    if (!isActive) EnqueueHarvestable(newHarvestable);
                });
            }
        }

        public AbstractHarvestable GetHarvestable()
        {
            var poolObject = _queue.Dequeue();
            return poolObject;
        }

        private void EnqueueHarvestable(AbstractHarvestable harvestable)
        {
            _queue.Enqueue(harvestable);
        }
    }
}