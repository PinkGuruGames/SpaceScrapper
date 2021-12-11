using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    public class LaserWeaponController : WeaponController
    {
        [SerializeField] private Transform laser;
        [Space]
        [SerializeField] private float laserStartDuration = 0.1f;
        [SerializeField] private AnimationCurve laserStartCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private Tween scaleTween;
        private Tween shakeTween;
        private Vector3? shakePositionOnStart;

        public override bool WantsToShoot
        {
            set
            {
                if (base.WantsToShoot == value)
                    return;

                base.WantsToShoot = value;

                if (value)
                {
                    LaserStart();
                }
                else
                {
                    LaserStop();
                }
            }
        }

        public void Start()
        {
            laser.gameObject.SetActive(false);
        }

        private void LaserStart()
        {
            KillTweens();
            laser.gameObject.SetActive(true);

            laser.localScale = new Vector3(0, 0, 1);

            scaleTween = laser.DOScale(1, laserStartDuration).SetEase(laserStartCurve);

            shakePositionOnStart = laser.localPosition;
            shakeTween = laser.DOShakePosition(1, strength: 0.1f, fadeOut: false).SetLoops(-1);
        }

        private void LaserStop()
        {
            KillTweens();
            laser.gameObject.SetActive(false);
        }

        private void KillTweens()
        {
            scaleTween.Complete();
            shakeTween.Kill();

            if (shakePositionOnStart.HasValue)
            {
                laser.localPosition = shakePositionOnStart.Value;
                shakePositionOnStart = null;
            }
        }
    }
}
