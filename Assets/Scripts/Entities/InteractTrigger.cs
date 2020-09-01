using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public class InteractTrigger : MonoBehaviour, IEntity
    {
        [Header("Settings")]
        [SerializeField] private SpriteRenderer sprite = null;
        [SerializeField] private UnityEvent action = null;
        [SerializeField] private bool allowRepeats = false;

        void Awake()
        {
            if(sprite == null)
            {
                sprite = GetComponentInChildren<SpriteRenderer>();
            }
            sprite.gameObject.SetActive(false);
        }

        public void Alert()
        {

        }

        public void AddEvent(UnityAction action)
        {  
            this.action.AddListener(action);
        }

        public void Hit()
        {
            Logger.Log(this, "Hit");
            action.Invoke();
            if(!allowRepeats)
            {
                sprite.gameObject.SetActive(false);
                this.gameObject.SetActive(false);

            }
        }

        public void UpdateBehaviour()
        {

        }

        public void UpdateLateBehaviour()
        {

        }



        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                sprite.gameObject.SetActive(true);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                sprite.gameObject.SetActive(false);
            }
        }
    }
}