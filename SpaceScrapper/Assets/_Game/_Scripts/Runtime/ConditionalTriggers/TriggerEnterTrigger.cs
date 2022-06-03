using UnityEngine;

namespace SpaceScrapper
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TriggerEnterTrigger : ConditionalTrigger
    {
        [SerializeField]
        private string playerTag;
        [SerializeField]
        private bool deactivateAfterTrigger = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(playerTag))
            {
                if(deactivateAfterTrigger)
                    gameObject.SetActive(false);
                TriggerAll();
            }
        }

    }
}
