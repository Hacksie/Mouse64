using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class SecurityTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float distance = 1.5f;
        [SerializeField] private LayerMask shootMask = 0;
        [SerializeField] private bool seeStealthed = false;
        

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (seeStealthed || !GameManager.Instance.Player.Stealthed)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, GameManager.Instance.Player.transform.position - transform.position, distance, shootMask);
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        GameManager.Instance.IncreaseAlert();
                    }
                }
            }
        }
    }
}