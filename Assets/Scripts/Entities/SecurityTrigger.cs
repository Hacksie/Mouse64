using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class SecurityTrigger : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (!GameManager.Instance.Player.Stealthed)
                {
                    GameManager.Instance.IncreaseAlert();
                }
            }
        }
    }
}