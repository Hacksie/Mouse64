using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class BulletDrop : MonoBehaviour, IEntity
    {
        
        [Header("GameObjects")]
        [SerializeField] private GameObject trigger = null;

        [Header("Settings")]
        [SerializeField] private int bulletRecovery = 1;

        protected void Awake()
        {
            
            
        }

        public void Alert()
        {

        }

        public void Hit()
        {
            Logger.Log(this, "bullet pickup");
            this.trigger.SetActive(false);
            GameManager.Instance.ConsumeBullet(-1 * bulletRecovery);
            AudioManager.Instance.PlayBulletPickup();
        }

        public void UpdateBehaviour()
        {

        }

        public void UpdateLateBehaviour()
        {
            
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                Hit();
            }
        }
    }
}