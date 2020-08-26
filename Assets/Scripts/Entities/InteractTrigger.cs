using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public class InteractTrigger : MonoBehaviour, IEntity
    {
        [Header("Settings")]
        [SerializeField] private GameObject sprite = null;
        [SerializeField] private UnityEvent action = null;

        void Awake()
        {
            sprite.SetActive(false);
        }

        public void Alert()
        {

        }

        public void Hit()
        {
            action.Invoke();
        }

        public void UpdateBehaviour()
        {

        }

        public void UpdateLateBehaviour()
        {
            
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