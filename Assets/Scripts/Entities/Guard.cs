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
        [SerializeField] private float patrolTime = 10f;
        [SerializeField] private float patrolSpeed = 2f;
        [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
        protected EntityState defaultState = EntityState.Idle;
        protected float alertTime = 0;
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
            this.patrolMin = GameManager.Instance.LevelRenderer.CalcPosition(1, GameManager.Instance.LevelRenderer.FindTemplate(GameManager.Instance.Data.currentLevel.name)).x;
            this.patrolMax = GameManager.Instance.LevelRenderer.CalcPosition(GameManager.Instance.Data.currentLevel.length - 2, GameManager.Instance.LevelRenderer.FindTemplate(GameManager.Instance.Data.currentLevel.name)).x;
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
            alertSprite?.SetActive(false);
            velocity = Vector2.zero;

            if (direction.x != 0)
            {
                transform.right = new Vector2(direction.x, 0);
            }

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

        private void UpdatePatrol()
        {
            fire = false;
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
            velocity = Vector2.zero;

            if (direction.x != 0)
            {
                transform.right = new Vector2(direction.x, 0);
            }

            if ((Time.time - alertTime) >= shootTime)
            {
                alertTime = Time.time;
                Shoot();
            }


            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, velocity, ref currentVelocity, movementSmoothing);

            var sqrDistance = (transform.position - GameManager.Instance.Player.transform.position).sqrMagnitude;

            if (sqrDistance > (GameManager.Instance.Player.Crouched ? (crouchAlertDistance * crouchAlertDistance) : (unstealthAlertDistance * unstealthAlertDistance)))
            {
                state = defaultState;
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