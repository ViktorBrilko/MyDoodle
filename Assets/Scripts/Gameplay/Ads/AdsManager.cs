using System;
using Gameplay.Signals;
using GoogleMobileAds.Api;
using UnityEngine;
using Zenject;

namespace Gameplay.Ads
{
    public class AdsManager : IInitializable, IDisposable
    {
        private BannerView _bannerView;
        private RewardedAd _rewardedAd;
        private SignalBus _signalBus;
        private AdsConfig _config;

        [Inject]
        public void Construct(SignalBus signalBus, AdsConfig config)
        {
            _config = config;
            _signalBus = signalBus;

            Init();
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDeath);
            LoadBannerView();
            LoadRewardedAd();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDeath);
            DestroyBannerView(_bannerView);
            DestroyRewardedAd(_rewardedAd);
        }

        private void OnPlayerDeath()
        {
            ShowRewardedAd();
        }

        private void Init()
        {
            MobileAds.Initialize((InitializationStatus initstatus) =>
            {
                if (initstatus == null)
                {
                    Debug.LogError("Google Mobile Ads initialization failed.");
                    return;
                }

                Debug.Log("Google Mobile Ads initialization complete.");
            });
        }

        private void DestroyBannerView(BannerView bannerView)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        private void DestroyRewardedAd(RewardedAd rewardedAd)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        private void RegisterReloadHandler(RewardedAd ad)
        {
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad full screen content closed.");

                LoadRewardedAd();
            };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);

                LoadRewardedAd();
            };
        }

        private void LoadBannerView()
        {
            var adRequest = new AdRequest();
            _bannerView = new BannerView(_config.ADUnitIDBanner, AdSize.Banner, AdPosition.Top);
            _bannerView.LoadAd(adRequest);
        }

        private void LoadRewardedAd()
        {
            var adRequest = new AdRequest();

            RewardedAd.Load(_config.ADUnitIDRewarded, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    return;
                }

                _rewardedAd = ad;
                RegisterReloadHandler(ad);
            });
        }

        private void ShowRewardedAd()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((Reward reward) =>
                {
                    // ничего тут не делал так как внедрение каких-то гемов за просмотр затянет еще время, но если надо - сделаю
                });
            }
        }
    }
}