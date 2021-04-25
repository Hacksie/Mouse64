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
        [SerializeField] private bool autoInvoke = false;

        void Awake()
        {
            if (sprite == null)
            {
                sprite = GetComponentInChildren<SpriteRenderer>();
            }
            //HideHover();
            //sprite.color = GameManager.Instance.GameSettings.defaultInteractColor;
        }

        public void Alert()
        {

        }

        public void ShowHover()
        {
            if (sprite != null)
            {
                sprite.color = GameManager.Instance.GameSettings.insideRangeInteractColor;
            }
        }



        public void HideHover()
        {
            if (sprite != null)
            {
                sprite.color = GameManager.Instance.GameSettings.outsideRangeInteractColor;
                //sprite.color = GameManager.Instance.GameSettings.defaultInteractColor;
            }
        }

        public void AddEvent(UnityAction action)
        {
            this.action.AddListener(action);
        }

        public void Hit()
        {
            Logger.Log(this, "Interact");
            action.Invoke();
            if (!allowRepeats)
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
                ShowHover();
                if(autoInvoke)
                {
                    Hit();
                }
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                HideHover();
                
            }
        }

    }
}