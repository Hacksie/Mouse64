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
        [SerializeField] private Transform crosshairAnchor = null;


        [Header("Settings")]
        [SerializeField] private float unstealthAlertDistance = 6;
        [SerializeField] private float crouchAlertDistance = 4.5f;
        [SerializeField] private float reactionTime = 1f;
        [SerializeField] private float shootTime = 1f;
        [SerializeField] private int minDamage = 30;
        [SerializeField] private int maxDamage = 60;
        [SerializeField] private float patrolTime = 10f;
        [SerializeField] private float patrolSpeed = 2f;
        [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
        [SerializeField] private LayerMask shootMask;

        protected EntityState defaultState = EntityState.Idle;
        protected float reactionTimer = 0;
        protected bool reaction = false;
        protected float shootTimer = 0;
        protected float patrolTimer = 0;
        protected float patrolMin = 0;
        protected float patrolMax = 0;
        protected new Rigidbody2D rigidbody;
        protected Vector2 currentVelocity = Vector2.zero;



        protected new void Awake()
        {
            defaultState = state;
            this.direction.x = Random.value > 0.5 ? -1 : 1;
            base.Awake();
            this.collider = this.collider ?? GetComponent<Collider2D>();
            this.patrolMin = GameManager.Instance.LevelRenderer.CalcPosition(1).x;
            this.patrolMax = GameManager.Instance.LevelRenderer.CalcPosition(GameManager.Instance.Data.currentLevel.length - 2).x;
            rigidbody = GetComponent<Rigidbody2D>();
            //this.levelLength = GameManager.Instance.LevelRenderer.LevelCorridorWorldLength(GameManager.Instance.Data.currentLevel);
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
                case EntityState.Patrol:
                    UpdatePatrol();
                    break;
                case EntityState.Attack:
                    UpdateAttack();
                    break;
                case EntityState.Dead:
                    velocity = Vector2.zero;
                    rigidbody.velocity = Vector2.zero;
                    alertSprite?.SetActive(false);
                    break;
            }
        }

        private void UpdateIdle()
        {
            fire = false;
            reaction = false;
            reactionTimer = 0;
            alertSprite?.SetActive(false);
            velocity = Vector2.zero;

            if (direction.x != 0)
            {
                transform.right = new Vector2(direction.x, 0);
            }

            if (!GameManager.Instance.Player.Stealthed)
            {
                RaycastHit2D hit = Physics2D.Raycast(crosshairAnchor.transform.position, transform.right, GameManager.Instance.Player.Crouched ? crouchAlertDistance : unstealthAlertDistance, shootMask);
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    state = EntityState.Attack;
                    reactionTimer = Time.time;
                }
            }
        }

        private void UpdatePatrol()
        {
            fire = false;
            reaction = false;
            reactionTimer = 0;
            alertSprite?.SetActive(false);

            if (transform.position.x <= patrolMin)
            {
                direction.x = 1;
            }
            if (transform.position.x >= patrolMax)
            {
                direction.x = -1;
            }

            if (direction.x != 0)
            {
                transform.right = new Vector2(direction.x, 0);
            }

            velocity = new Vector2(direction.x * patrolSpeed, 0);

            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, velocity, ref currentVelocity, movementSmoothing);

            if (!GameManager.Instance.Player.Stealthed)
            {
                RaycastHit2D hit = Physics2D.Raycast(crosshairAnchor.transform.position, transform.right, GameManager.Instance.Player.Crouched ? crouchAlertDistance : unstealthAlertDistance, shootMask);
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    state = EntityState.Attack;
                    reactionTimer = Time.time;
                }
            }
        }

        private void UpdateAttack()
        {
            fire = false;
            alertSprite?.SetActive(true);
            direction = new Vector3(GameManager.Instance.Player.transform.position.x - this.transform.position.x, this.transform.position.y, this.transform.position.z);
            velocity = Vector2.zero;

            if (direction.x != 0)
            {
                transform.right = new Vector2(direction.x, 0);
            }

            if (!reaction && (Time.time - reactionTimer) >= reactionTime)
            {
                shootTimer = Time.time;
                reaction = true;
                Shoot();
            }

            if (reaction && (Time.time - shootTimer) >= shootTime)
            {
                shootTimer = Time.time;
                Shoot();
            }


            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, velocity, ref currentVelocity, movementSmoothing);

            RaycastHit2D hit = Physics2D.Raycast(crosshairAnchor.transform.position, transform.right, GameManager.Instance.Player.Crouched ? crouchAlertDistance : unstealthAlertDistance, shootMask);
            if (hit.collider == null || !hit.collider.CompareTag("Player"))
            {
                state = defaultState;
            }
        }

        protected virtual void Shoot()
        {
            RaycastHit2D hit = Physics2D.Raycast(crosshairAnchor.transform.position, transform.right, unstealthAlertDistance, shootMask);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                fire = true;
                GameManager.Instance.TakeDamage(Random.Range(minDamage, maxDamage));
                AudioManager.Instance.PlayGunshot();
                Logger.Log(this, "Shoot!");
            }
        }

        protected override void Animate()
        {
            base.Animate();
            muzzleAnimator.SetBool("shoot", this.fire);
        }
    }
}