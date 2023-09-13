using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 20f;
        [SerializeField]
        private float _lifetime = 20f; // sec

        private Vector3 _direction;
        private float _lifetimeTimer = 0;

        private void Awake()
        {
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            _lifetimeTimer += Time.deltaTime;
            if (_lifetimeTimer > _lifetime)
            {
                Destroy(gameObject);
            }
        }

        public void Fire(Vector3 direction)
        {
            _direction = direction;
            transform.rotation = Quaternion.LookRotation(_direction, Vector3.up);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyHealth>()?.Hit();
                Destroy(gameObject);
            }
        }
    }
}