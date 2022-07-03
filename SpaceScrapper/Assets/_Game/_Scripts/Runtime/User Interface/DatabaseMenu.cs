using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;

namespace SpaceScrapper
{
    public class DatabaseMenu : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup group;
        [SerializeField]
        private InputActionReference openCloseAction;
        [SerializeField]
        private Transform entryParent;
        [SerializeField]
        private GameObject entrySelectablePrefab;
        [SerializeField]
        private DatabaseEntry[] entries;
        [SerializeField]
        private TextMeshProUGUI descriptionText, titleText;

        /// <summary>
        /// Use Awake to initialize it all.
        /// </summary>
        private void Awake()
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;

            descriptionText.text = string.Empty;
            titleText.text = string.Empty;

            //ew linq haha, but who cares its a one-time thing.
            foreach(var entry in entries.OrderBy(x => x.Title))
            {
                //instantiate a new button
                GameObject go = Instantiate(entrySelectablePrefab, entryParent);
                Button b = go.GetComponent<Button>();
                var text = go.GetComponentInChildren<TextMeshProUGUI>();

                text.text = entry.Title;

                b.onClick.AddListener(() => {
                    descriptionText.text = entry.Description;
                    titleText.text = entry.Title;
                    });
            }
        }

        private void OnEnable()
        {
            openCloseAction.action.performed += OnOpenClose;
        }

        private void OnDisable()
        {
            openCloseAction.action.performed -= OnOpenClose;
        }

        private void OnOpenClose(InputAction.CallbackContext ctxt)
        {
            group.alpha = 1 - group.alpha;
            group.interactable ^= true;
            group.blocksRaycasts ^= true;
        }
    }
}
