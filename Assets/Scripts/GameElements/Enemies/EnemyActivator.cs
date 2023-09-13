using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class EnemyActivator : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            //Debug.Log(other.gameObject.name);
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().ActivateEnemy();
            }
        }
    }
}