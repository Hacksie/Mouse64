
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Animator animator = null;
        [SerializeField] private Animator muzzleAnimator = null;
        [SerializeField] private Transform crosshairAnchor = null;

        [Header("Settings")]
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private float runSpeed = 3;
        [SerializeField] private float crouchSpeed = 3;
        [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
        [SerializeField] private float stealthRate = 1f;
        [SerializeField] private float lookAngle = 0;
        [SerializeField] private float maxAngle = 75.0f;
        [SerializeField] private float minAngle = -25.0f;
        [SerializeField] private float shootDistance = 4.0f;
        [SerializeField] private float interactDistance = 0.5f;
        [SerializeField] private LayerMask shootMask = 0;
        [SerializeField] private LayerMask interactMask = 0;

        private Vector2 direction;
        private Vector2 inputAxis;
        private new Rigidbody2D rigidbody;
        private Vector2 currentVelocity = Vector2.zero;
        private bool shoot = false;
        private float lastFire = 0;
        private bool fire = false;
        private bool crouch = false;
        private bool stealth = false;
        private bool interact = false;
        private bool dead = false;
        private bool sit = false;

        public bool Crouched { get { return crouch; } }
        public bool Stealthed { get { return stealth; } }
        public bool Sit { get { return sit; } set { sit = value; } }
        public bool Dead { get { return dead; } set { dead = value; rigidbody.velocity = Vector3.zero; shoot = false; } }

        // Start is called before the first frame update
        void Awake()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Reset()
        {
            this.animator.Play("mouse idle");
            this.transform.position = new Vector3(2, 0.275f, 0);
            this.inputAxis = Vector2.zero;
            direction = Vector2.zero;
            transform.right = Vector2.right;
            rigidbody.velocity = Vector2.zero;
            fire = false;
            shoot = false;
            lastFire = 0;
            crouch = false;
            stealth = false;
            interact = false;
            dead = false;
            sit = false;
            lookAngle = 0;
        }

        // Update is called once per frame
        public void UpdateBehavior()
        {
            if (inputAxis.sqrMagnitude > Vector2.kEpsilon)
            {
                direction = inputAxis;
            }

            if (direction.x != 0)
            {
                transform.right = new Vector2(direction.x, 0);
            }

            Vector2 targetVelocity = new Vector2(inputAxis.x * (crouch ? crouchSpeed : runSpeed), rigidbody.velocity.y);

            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref currentVelocity, movementSmoothing);

            UpdateInteract();
            UpdateShoot();
            UpdateStealth();
            UpdateCrosshair();
        }

        private void UpdateInteract()
        {
            if (interact)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(crosshairAnchor.transform.position, crosshairAnchor.right, interactDistance, interactMask);

                foreach (var hit in hits)
                {
                    if (hit.collider != null && (hit.collider.CompareTag("Door") || hit.collider.CompareTag("Entity") || hit.collider.CompareTag("Interactable")))
                    {
                        IEntity e = hit.collider.GetComponent<IEntity>();
                        if (e != null)
                        {
                            e.Hit();
                        }
                        else
                        {
                            Logger.LogError(this, "No IEntity interface to hit");
                        }
                    }
                }
            }
        }


        private void UpdateShoot()
        {
            if (shoot)
            {
                var hitEntity = GetMeleeHit();

                if (hitEntity == null && (Time.time - lastFire > fireRate) && GameManager.Instance.Data.bullets > 0)
                {
                    GameManager.Instance.ConsumeBullet(1);
                    lastFire = Time.time;
                    fire = true;
                    AudioManager.Instance.PlayGunshot();
                    hitEntity = GetShootHit();
                }

                if (hitEntity != null)
                {
                    hitEntity.Hit();
                }
            }
            else
            {
                fire = false;
            }

        }

        private IEntity GetMeleeHit()
        {
            RaycastHit2D hit = Physics2D.Raycast(crosshairAnchor.transform.position, crosshairAnchor.right, interactDistance, shootMask);
            if (hit.collider != null && hit.collider.CompareTag("Entity"))
            {
                return hit.collider.GetComponent<IEntity>(); ;

            }
            return null;
        }

        private IEntity GetShootHit()
        {
            RaycastHit2D hit = Physics2D.Raycast(crosshairAnchor.transform.position, crosshairAnchor.right, shootDistance, shootMask);
            if (hit.collider != null && hit.collider.CompareTag("Entity"))
            {
                return hit.collider.GetComponent<IEntity>();

            }

            return null;
        }

        public void LateUpdateBehaviour()
        {
            Animate();
        }

        private void UpdateStealth()
        {
            if (stealth && GameManager.Instance.Data.energy > 0)
            {
                GameManager.Instance.ConsumeStealth(stealthRate * Time.deltaTime);
            }
            else
            {
                stealth = false;
            }
        }

        private void UpdateCrosshair()
        {
            lookAngle += (inputAxis.y * Time.deltaTime * GameManager.Instance.PlayerPreferences.lookSpeed);
            lookAngle = Mathf.Clamp(lookAngle, minAngle, maxAngle);

            float newAngle = lookAngle;

            if (transform.right.x < 0)
            {
                newAngle = 180 - lookAngle;
            }

            crosshairAnchor.rotation = Quaternion.Euler(0, 0, newAngle);
        }

        private void Animate()
        {
            animator.SetFloat("velocity", Mathf.Abs(inputAxis.x));
            animator.SetBool("shoot", this.shoot);
            animator.SetBool("crouch", this.crouch);
            animator.SetBool("stealth", this.stealth);
            animator.SetBool("dead", this.dead);
            animator.SetBool("sit", this.sit);
            muzzleAnimator.SetBool("shoot", this.fire);

        }

        public void MoveEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed)
            {
                this.inputAxis = Vector2.zero;
                return;
            }
            inputAxis = context.ReadValue<Vector2>();
        }

        public void JumpEvent(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameManager.Instance.SetDead();
            }
        }

        public void CrouchEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed)
            {
                this.crouch = false;
                return;
            }

            if (context.performed)
            {
                this.crouch = true;
            }
            else if (context.canceled)
            {
                this.crouch = false;
            }
        }

        public void FireEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed)
            {
                this.shoot = false;
                return;
            }

            if (context.performed)
            {
                this.shoot = true;
            }
            else if (context.canceled)
            {
                this.shoot = false;
            }
        }

        public void StealthEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed)
            {
                this.stealth = false;
                return;
            }

            if (context.performed)
            {
                this.stealth = true;
            }
            else if (context.canceled)
            {
                this.stealth = false;
            }
        }

        public void InteractEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed)
            {
                this.interact = false;
                return;
            }

            if (context.performed)
            {
                Logger.Log(this, "interact");
                this.interact = true;
            }
            else if (context.canceled)
            {
                this.interact = false;
            }
        }

        public void StartEvent(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameManager.Instance.CurrentState.Start();
            }
        }
    }
}