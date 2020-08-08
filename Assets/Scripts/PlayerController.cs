
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
        [SerializeField] private float rotateSpeed = 180.0f;
        [SerializeField] private float shootDistance = 4.0f;
        [SerializeField] private LayerMask shootMask;

        private Vector2 direction;
        private Vector2 inputAxis;
        private new Rigidbody2D rigidbody;
        private Vector2 currentVelocity = Vector2.zero;
        private bool shoot = false;
        private float lastFire = 0;
        private bool fire = false;
        private bool crouch = false;
        private bool stealth = false;
        private bool dead = false;
        private bool sit = false;

        public bool Crouched { get { return crouch; }}

        public bool Stealthed { get { return stealth; } }

        public bool Sit { get { return sit;} set { sit = value;}}

        public bool Dead { get { return dead;} set { dead = value;}}

        // Start is called before the first frame update
        void Awake()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Reset()
        {
            this.inputAxis = Vector2.zero;
            direction = Vector2.zero;
            transform.right = Vector2.right;
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

            Vector2 targetVelocity = targetVelocity = new Vector2(inputAxis.x * (crouch ? crouchSpeed : runSpeed), rigidbody.velocity.y);

            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref currentVelocity, movementSmoothing);

            UpdateShoot();
            UpdateStealth();
            UpdateCrosshair();
        }

        private void UpdateShoot()
        {
            if (shoot && (Time.time - lastFire > fireRate) && GameManager.Instance.ConsumeBullet())
            {
                lastFire = Time.time;
                fire = true;
                AudioManager.Instance.PlayGunshot();

                CheckHit();
            }
            else
            {
                fire = false;
            }
        }

        private void CheckHit()
        {
            RaycastHit2D hit = Physics2D.Raycast(crosshairAnchor.transform.position, crosshairAnchor.right, shootDistance, shootMask);
            if (hit.collider != null)
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

        public void LateUpdateBehaviour()
        {
            Animate();
        }

        private void UpdateStealth()
        {
            // TODO: Maybe have a regen facility after a certain period off stealth?
            if (!stealth || !GameManager.Instance.ConsumeStealth(stealthRate * Time.deltaTime))
            {
                stealth = false;
            }
        }

        private void UpdateCrosshair()
        {
            lookAngle += (inputAxis.y * Time.deltaTime * rotateSpeed);
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
            //animator.SetBool("jump", !grounded);
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

        public void StartEvent(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameManager.Instance.CurrentState.Start();
            }
        }
    }
}