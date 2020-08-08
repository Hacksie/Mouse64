using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Guard : Entity
    {
        [Header("Game Objects")]
        [SerializeField] private new Collider2D collider = null;
        [SerializeField] private GameObject alertSprite = null;
        [SerializeField] private Animator muzzleAnimator = null;

        [Header("Settings")]
        [SerializeField] private float unstealthAlertDistance = 6;
        [SerializeField] private float crouchAlertDistance = 4.5f;
        [SerializeField] private float shootTime = 1f;
        [SerializeField] private int minDamage = 30;
        [SerializeField] private int maxDamage = 60;

        private Vector2 direction;
        private float alertTime = 0;


        protected new void Awake()
        {
            base.Awake();
            this.collider = this.collider ?? GetComponent<Collider2D>();

        }

        public override void Hit()
        {
            Logger.Log(this, "Guard Hit!");
            Kill();
        }

        protected virtual void Kill()
        {
            this.state = EntityState.Dead;
            collider.enabled = false;
        }

        public override void UpdateBehaviour()
        {
            base.UpdateBehaviour();

            switch (this.state)
            {
                case EntityState.Idle:
                    UpdateIdle();
                    break;
                case EntityState.Attack:
                    UpdateAttack();
                    break;
                case EntityState.Dead:
                    alertSprite?.SetActive(false);
                    break;
            }


        }

        private void UpdateIdle()
        {
            fire = false;
            alertSprite?.SetActive(false);
            if (!GameManager.Instance.Player.Stealthed)
            {
                var sqrDistance = (transform.position - GameManager.Instance.Player.transform.position).sqrMagnitude;

                if (sqrDistance <= (GameManager.Instance.Player.Crouched ? (crouchAlertDistance * crouchAlertDistance) : (unstealthAlertDistance * unstealthAlertDistance)))
                {
                    state = EntityState.Attack;
                    alertTime = Time.time;
                }
            }
        }

        private void UpdateAttack()
        {
            fire = false;
            alertSprite?.SetActive(true);
            direction = new Vector3(GameManager.Instance.Player.transform.position.x - this.transform.position.x, this.transform.position.y, this.transform.position.z);

            if (direction.x != 0)
            {
                transform.right = new Vector2(direction.x, 0);
            }

            if ((Time.time - alertTime) >= shootTime)
            {
                alertTime = Time.time;
                Shoot();
            }

            var sqrDistance = (transform.position - GameManager.Instance.Player.transform.position).sqrMagnitude;

            if (sqrDistance > (GameManager.Instance.Player.Crouched ? (crouchAlertDistance * crouchAlertDistance) : (unstealthAlertDistance * unstealthAlertDistance)))
            {
                state = EntityState.Idle;
            }
        }

        protected virtual void Shoot()
        {
            fire = true;
            GameManager.Instance.TakeDamage(Random.Range(minDamage, maxDamage));
            AudioManager.Instance.PlayGunshot();
            Logger.Log(this, "Shoot!");
        }

        protected override void Animate()
        {
            base.Animate();
            muzzleAnimator.SetBool("shoot", this.fire);
        }
    }
}