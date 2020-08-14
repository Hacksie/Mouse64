using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class InteractTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject sprite = null;

        void Awake()
        {
            sprite.SetActive(false);
        }

        public void Hide()
        {
            sprite.SetActive(false);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                sprite.SetActive(true);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                sprite.SetActive(false);
            }
        }        
    }
}