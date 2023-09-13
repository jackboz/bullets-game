using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Finish : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Actions.OnLevelWon?.Invoke();
            }
        }
    }
}
