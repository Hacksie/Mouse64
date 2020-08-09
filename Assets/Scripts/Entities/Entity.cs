using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Entity : MonoBehaviour, IEntity
    {
        public EntityState state = EntityState.Idle;
        protected Animator animator = null;
        protected bool fire = false;

        protected Vector2 velocity;
        protected Vector2 direction;

        protected void Awake()
        {
            this.animator = GetComponent<Animator>();
        }

        public virtual void Hit()
        {
            Logger.Log(this, "Hit!");
            state = EntityState.Dead;
        }

        public virtual void Alert()
        {
            if (state == EntityState.Idle)
            {
                Logger.Log(this, "Alert!");
                state = EntityState.Attack;
            }
        }

        public virtual void UpdateBehaviour()
        {

        }

        public virtual void UpdateLateBehaviour()
        {
            Animate();
        }

        protected virtual void Animate()
        {
            //Logger.Log(this, state.ToString());
            switch (state)
            {
                case EntityState.Idle:
                    animator.SetFloat("velocity", 0);

                    animator.SetBool("shoot", fire);
                    animator.SetBool("crouch", false);
                    animator.SetBool("stealth", false);
                    animator.SetBool("alert", false);
                    animator.SetBool("dead", false);
                    break;
                case EntityState.Patrol:
                    animator.SetFloat("velocity", Mathf.Abs(velocity.x));
                    animator.SetBool("shoot", fire);
                    animator.SetBool("crouch", false);
                    animator.SetBool("stealth", false);
                    animator.SetBool("alert", false);
                    animator.SetBool("dead", false);
                    break;                
                case EntityState.Attack:
                    animator.SetFloat("velocity", 0);
                    animator.SetBool("alert", true);
                    animator.SetBool("shoot", fire);
                    animator.SetBool("crouch", false);
                    animator.SetBool("stealth", false);
                    animator.SetBool("dead", false);
                    break;
                case EntityState.Dead:
                    animator.SetFloat("velocity", 0);
                    animator.SetBool("alert", false);
                    animator.SetBool("shoot", false);
                    animator.SetBool("crouch", false);
                    animator.SetBool("stealth", false);
                    animator.SetBool("dead", true);
                    //muzzleAnimator.SetBool("shoot", this.fire);                
                    break;
            }
        }
    }

    public enum EntityState
    {
        Idle,
        Patrol,
        Attack,
        Dead
    }
}