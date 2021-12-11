using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    public class BasicBulletController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifetime;

        public void Start()
        {
            Invoke(nameof(Kill), lifetime);
        }

        public void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }

        private void Kill()
        {
            Destroy(gameObject);
        }
    }
}
