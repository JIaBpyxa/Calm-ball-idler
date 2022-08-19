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
        public FloatReactiveProperty CurrentScoreModifierCoefficient { get; private set; }
        private AdsService _adsService;


        [Inject]
        private ScoreModifierService(ConfigRemoteService configRemoteService, AdsService adsService,
            LoadingService loadingService)
        {
            configRemoteService.OnRemoteDataLoaded += UpdateRemoteData;
            adsService.OnRewardedCompleted += HandleRewardedAdCompleted;
            IsActive = new BoolReactiveProperty(false);
            CurrentScoreModifierCoefficient = new FloatReactiveProperty(1f);
            RewardedScoreModifierCoefficient = new FloatReactiveProperty(2f);

            loadingService.AddLoadingOperation(this);
        }

        private void UpdateRemoteData(ConfigRemoteService.RemoteData remoteData)
        {
            RewardedScoreModifierDuration = remoteData.RewardedScoreModifierDuration;
            RewardedScoreModifierCoefficient.Value = remoteData.RewardedScoreModifierCoefficient;
            //CurrentScoreModifierCoefficient.Value = remoteData.RewardedScoreModifierCoefficient;

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
            CurrentScoreModifierCoefficient.Value = RewardedScoreModifierCoefficient.Value;
            var saveObservable = Observable.Timer(TimeSpan.FromSeconds(RewardedScoreModifierDuration));
            ObservableExtensions.Subscribe(saveObservable, _ => ResetScoreModifier());

            void ResetScoreModifier()
            {
                IsActive.Value = false;
                CurrentScoreModifierCoefficient.Value = 1f;
            }
        }
    }
}