using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Door : MonoBehaviour, IEntity
    {
        protected Animator animator = null;
        [Header("GameObjects")]
        [SerializeField] private new Collider2D collider = null;
        [Header("State")]
        [SerializeField] private bool open = false;

        protected void Awake()
        {
            this.animator = GetComponent<Animator>();
            collider = collider ?? GetComponent<Collider2D>();
        }

        public void Alert()
        {

        }

        public void Hit()
        {
            Logger.Log(this, "open door");
            this.open = true;
            this.collider.enabled = false;
        }

        public void UpdateBehaviour()
        {

        }

        public void UpdateLateBehaviour()
        {
            this.animator.SetBool("open", this.open);
        }
    }
}