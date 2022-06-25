using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SpaceScrapper
{
    public class SimpleHealthbar : MonoBehaviour
    {
        [SerializeField]
        private LivingEntity observedEntity;
        [SerializeField]
        private Slider healthBar;
        [SerializeField]
        private TextMeshProUGUI nameText;

        private void Awake()
        {
            observedEntity.OnBecameActive += Initialize;
            gameObject.SetActive(false); //disabled by default. this fucks with OnEnable / OnDisable a little bit but its fine.
        }

        private void OnEnable()
        {
            observedEntity.OnMaxHealthChanged += OnMaxChanged;
            observedEntity.OnHealthChanged += healthBar.SetValueWithoutNotify;
            observedEntity.OnEntityDied += OnObservedEntityDied;
        }

        private void OnDisable()
        {
            observedEntity.OnMaxHealthChanged -= OnMaxChanged;
            observedEntity.OnHealthChanged -= healthBar.SetValueWithoutNotify;
            observedEntity.OnEntityDied -= OnObservedEntityDied;
        }

        private void OnDestroy()
        {
            observedEntity.OnBecameActive -= Initialize;
        }

        private void Initialize()
        {
            healthBar.maxValue = observedEntity.MaxHealth;
            healthBar.value = observedEntity.CurrentHealth;
            if(nameText)
                nameText.text = observedEntity.name;
            gameObject.SetActive(true);
        }

        private void OnObservedEntityDied()
        {
            gameObject.SetActive(false);
        }

        private void OnMaxChanged(float obj)
        {
            healthBar.maxValue = obj;
        }

    }
}
