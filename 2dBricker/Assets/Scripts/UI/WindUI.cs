using System.Collections.Generic;
using DG.Tweening;
using I2.Loc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class WindUI : MonoBehaviour
    {
        [SerializeField] private List<Image> _windImages;
        [Space] [SerializeField] private float _mediumWindValue = 4f;
        [SerializeField] private float _hardWindValue = 7f;
        [Space] [SerializeField] private LocalizationParamsManager _windTextParamsManager;

        private float _windImageWidth;

        [Inject]
        private void Construct(WindService windService)
        {
            windService.WindSpeed.Subscribe(UpdateWindSpeedData);
        }

        private void Awake()
        {
            _windImageWidth = _windImages[0].rectTransform.rect.width;
        }

        private void UpdateWindSpeedData(float windSpeed)
        {
            int activeWindImages;
            if (Mathf.Abs(windSpeed) <= _mediumWindValue)
            {
                activeWindImages = 1;
            }
            else if (Mathf.Abs(windSpeed) <= _hardWindValue)
            {
                activeWindImages = 2;
            }
            else
            {
                activeWindImages = 3;
            }

            for (var imageId = 0; imageId < 3; imageId++)
            {
                var isActive = imageId < activeWindImages;
                var scale = windSpeed >= 0 ? 1f : -1f;
                var width = isActive ? _windImageWidth : 0f;
                _windImages[imageId].rectTransform.DOSizeDelta(new Vector2(width, 0), .3f);
                _windImages[imageId].transform.DOScaleX(scale, .3f);
            }

            _windTextParamsManager.SetParameterValue("WIND_SPEED", $"{windSpeed:f1}");
        }
    }
}