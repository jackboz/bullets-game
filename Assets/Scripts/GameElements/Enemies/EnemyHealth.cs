using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField]
        private int _totalHealth = 1;
        public int Health { get; private set; }
        public bool IsDead { get; private set; }

        void Awake()
        {
            Health = _totalHealth;
            IsDead = false;
        }

        public void Hit()
        {
            if (Health > 0)
            {
                Health -= 1;
            }
            if (Health == 0)
            {
                IsDead = true;
                Enemy enemy = GetComponent<Enemy>();
                if (enemy != null)
                {
                    //Debug.Log("" + name + " died");
                    Actions.OnEnemyKilled?.Invoke(enemy);
                }
            }
        }
    }
}