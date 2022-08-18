using System;
using UniRx;
using Zenject;

namespace Vorval.CalmBall.Service
{
    public class ScoreModifierService : ILoadingOperation
    {
        public Action<ILoadingOperation> OnOperationFinished { get; set; }
        public readonly BoolReactiveProperty IsActive;
        public float RewardedScoreModifierDuration { get; private set; }
        public FloatReactiveProperty RewardedScoreModifierCoefficient { get; private set; }

        private AdsService _adsService;


        [Inject]
        private ScoreModifierService(ConfigRemoteService configRemoteService, AdsService adsService,
            LoadingService loadingService)
        {
            configRemoteService.OnRemoteDataLoaded += UpdateRemoteData;
            adsService.OnRewardedCompleted += HandleRewardedAdCompleted;
            IsActive = new BoolReactiveProperty(false);
            RewardedScoreModifierCoefficient = new FloatReactiveProperty(1f);

            loadingService.AddLoadingOperation(this);
        }

        private void UpdateRemoteData(ConfigRemoteService.RemoteData remoteData)
        {
            RewardedScoreModifierDuration = remoteData.RewardedScoreModifierDuration;
            RewardedScoreModifierCoefficient.Value = remoteData.RewardedScoreModifierCoefficient;

            OnOperationFinished?.Invoke(this);
        }

        private void HandleRewardedAdCompleted(AdsService.RewardedType rewardedType)
        {
            if (rewardedType == AdsService.RewardedType.ScoreModifier)
            {
                ActivateRewardedScoreModifier();
            }
        }

        private void ActivateRewardedScoreModifier()
        {
            IsActive.Value = true;
            var saveObservable = Observable.Timer(TimeSpan.FromSeconds(RewardedScoreModifierDuration));
            ObservableExtensions.Subscribe(saveObservable, _ => ResetScoreModifier());

            void ResetScoreModifier()
            {
                IsActive.Value = false;
                RewardedScoreModifierCoefficient.Value = 1f;
            }
        }
    }
}