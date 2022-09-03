using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vorval.CalmBall.Service
{
    public class GameLoader : MonoBehaviour
    {
        private async void Awake()
        {
            await Task.Delay(TimeSpan.FromSeconds(2f));
            SceneManager.LoadSceneAsync("Scenes/IdleScene");
        }
    }
}