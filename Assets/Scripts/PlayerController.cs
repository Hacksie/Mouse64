
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
        //[SerializeField] private Transform crosshair = null;
        [SerializeField] private Camera cam = null;
        //[SerializeField] private RectTransform rect;

        [Header("Settings")]
        [SerializeField] private PlayerSettings settings = null;


        //private Vector2 direction;
        private Vector2 inputAxis;
        private Vector2 mousePos;
        private new Rigidbody2D rigidbody;
        private Vector2 currentVelocity = Vector2.zero;
        private bool shoot = false;
        private float lastFire = 0;
        private bool firing = false;
        private bool crouch = false;
        private bool stealth = false;
        private bool interact = false;
        private bool dead = false;
        private bool sit = false;

        public bool Crouched { get { return crouch; } }
        public bool Stealthed { get { return stealth; } }
        public bool Sit { get { return sit; } set { sit = value; } }
        public bool Dead { get { return dead; } set { dead = value; Stop(); } }

        // Start is called before the first frame update
        void Awake()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void MoveEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed)
            {
                this.inputAxis.x = 0;
                return;
            }
            inputAxis.x = context.ReadValue<float>();
            //inputAxis = context.ReadValue<Vector2>();
        }

        public void LookEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed)
            {
                this.inputAxis.y = 0;
                return;
            }
            inputAxis.y = context.ReadValue<float>();
        }

        public void MousePosEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed)
            {
                //this.inputAxis.y = 0;
                return;
            }
            mousePos = context.ReadValue<Vector2>();
            //inputAxis.y += context.ReadValue<float>() * Time.deltaTime;
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
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed || !GameManager.Instance.CurrentState.Battle)
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
            if (!(GameManager.Instance.CurrentState.PlayerActionAllowed || GameManager.Instance.CurrentState.Battle))
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

            if (context.started)
            {
                this.interact = true;
            }
        }

        public void StartEvent(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameManager.Instance.CurrentState.Start();
            }
        }

        public void Stop()
        {
            rigidbody.velocity = Vector3.zero; shoot = false;
        }

        public void Reset()
        {
            this.animator.Play("mouse idle");
            this.transform.position = new Vector3(2, 0.275f, 0);
            this.inputAxis = Vector2.zero;
            //direction = Vector2.zero;
            transform.right = Vector2.right;
            rigidbody.velocity = Vector2.zero;
            firing = false;
            shoot = false;
            lastFire = 0;
            crouch = false;
            stealth = false;
            interact = false;
            dead = false;
            sit = false;
        }

        // Update is called once per frame
        public void UpdateBehavior()
        {
            //UpdateCrosshair();
            UpdateInteract();
            UpdateTarget();
            UpdateStealth();

            //Vector2 direction = Vector2.zero;
            Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x * (128f / Screen.width), mousePos.y * (72f / Screen.height), 0));
            Vector2 direction = (worldPos - transform.position).normalized;

            if (!firing && inputAxis.sqrMagnitude > Vector2.kEpsilon)
            {
                direction = inputAxis;
            }

            if (direction.x != 0)
            {
                transform.right = new Vector2(direction.x, 0);
            }

            Vector2 targetVelocity = new Vector2(inputAxis.x * (crouch ? settings.crouchSpeed : settings.runSpeed), rigidbody.velocity.y);

            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref currentVelocity, settings.movementSmoothing);
        }

        private void UpdateInteract()
        {
            //FIXME: Should we trigger everything though?
            if (this.interact)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(crosshairAnchor.transform.position, crosshairAnchor.right, settings.interactDistance, settings.interactMask);

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

            this.interact = false;
        }


        private void UpdateTarget()
        {
            this.firing = false;

            if (!GameManager.Instance.CurrentState.Battle)
            {
                return;
            }

            if (shoot)
            {

                var hitEntity = GetMeleeHit();

                if (hitEntity == null && (Time.time - lastFire > settings.fireRate) && GameManager.Instance.Data.bullets > 0)
                {
                    GameManager.Instance.ConsumeBullet(1);
                    lastFire = Time.time;
                    this.firing = true;
                    AudioManager.Instance.PlayGunshot();
                    hitEntity = GetShootHit();
                }

                if (hitEntity != null)
                {
                    hitEntity.Hit();
                }
            }
        }

        private IEntity GetMeleeHit()
        {
            RaycastHit2D hit = Physics2D.Raycast(crosshairAnchor.transform.position, crosshairAnchor.right, settings.interactDistance, settings.shootMask);
            if (hit.collider != null && hit.collider.CompareTag("Entity"))
            {
                return hit.collider.GetComponent<IEntity>(); ;

            }
            return null;
        }

        private IEntity GetShootHit()
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x * (128f / Screen.width), mousePos.y * (72f / Screen.height), 0));
            RaycastHit2D hit = Physics2D.Raycast(crosshairAnchor.transform.position, worldPos - crosshairAnchor.position, settings.shootDistance, settings.shootMask);
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
            if (!GameManager.Instance.CurrentState.Battle)
            {
                stealth = false;
                return;
            }

            if (stealth && GameManager.Instance.Data.energy > 0)
            {
                GameManager.Instance.ConsumeStealth(settings.stealthRate * Time.deltaTime);
            }
            else
            {
                stealth = false;
            }
        }

        
        private void Animate()
        {
            animator.SetFloat("velocity", Mathf.Abs(inputAxis.x));
            animator.SetBool("shoot", GameManager.Instance.CurrentState.Battle && this.shoot);
            animator.SetBool("crouch", this.crouch);
            animator.SetBool("stealth", this.stealth);
            animator.SetBool("dead", this.dead);
            animator.SetBool("sit", this.sit);
            animator.SetBool("outofbattle", !GameManager.Instance.CurrentState.Battle);
            muzzleAnimator.SetBool("shoot", GameManager.Instance.CurrentState.Battle && this.firing);
        }
    }
}