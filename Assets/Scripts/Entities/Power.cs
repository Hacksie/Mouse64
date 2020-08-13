using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Power : MonoBehaviour, IEntity
    {
        
        [Header("GameObjects")]
        [SerializeField] private GameObject trigger = null;

        [Header("Settings")]
        [SerializeField] private int powerRecovery = 33;

        protected void Awake()
        {
            
            
        }

        public void Alert()
        {

        }

        public void Hit()
        {
            Logger.Log(this, "use power");
            this.trigger.SetActive(false);
            GameManager.Instance.ConsumeStealth(-1 * powerRecovery);
        }

        public void UpdateBehaviour()
        {

        }

        public void UpdateLateBehaviour()
        {
            
        }
    }
}