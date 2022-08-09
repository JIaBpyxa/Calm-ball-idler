using UnityEngine;

namespace Bricker.Game
{
    public class HarvestableSpawner : MonoBehaviour
    {
        private IHarvestable.Factory _factory;

        private void Construct(IHarvestable.Factory factory)
        {
            _factory = factory;
        }

        private void Start()
        {
            
            //Observable.IntervalFrame(30).TakeUntilDisable(this).Subscribe(x => Debug.Log(x), () => Debug.Log("completed!"));
        }
    }
}