using RPGCharacterAnims.Actions;
using UnityEngine;
using UnityEngine.Events;

namespace RPGCharacterAnims
{
    [HelpURL("https://docs.unity3d.com/Manual/script-AnimationWindowEvent.html")]
    // This class is used to handle animation events in Unity.
    // It provides UnityEvents for various character actions and movement.
    // The class also handles the Animator component and character controller.
    public class Vid_AnimationEvent : MonoBehaviour
    {

        [Header("Projectile Settings")]
        public GameObject projectilePrefab;    // 생성할 투사체 프리팹
        public Transform skillPos;             // 발사 위치
        public float projectileSpeed = 10f;    // 속도

        // Event call functions for Animation events.

        public UnityEvent OnShoot = new UnityEvent();


        public AnimatorMoveEvent OnMove = new AnimatorMoveEvent();

        private RPGCharacterController rpgCharacterController;
        private Animator animator;

        void Awake()
        {
            rpgCharacterController = GetComponentInParent<RPGCharacterController>();
            animator = GetComponent<Animator>();
        }

        //public void Shoot() => OnShoot.Invoke();

        public void Shoot()
        {
            OnShoot.Invoke();

            if (projectilePrefab == null || skillPos == null)
            {
                Debug.LogWarning("Missing projectile or skill position");
                return;
            }

            GameObject projectile = Instantiate(projectilePrefab, skillPos.position, skillPos.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = skillPos.forward * projectileSpeed;
            }

            Destroy(projectile, 5f);

            //ProjectileMover mover = projectile.AddComponent<ProjectileMover>();
            //mover.Initialize(skillPos.forward, projectileSpeed);

            //Destroy(projectile, 5f);

        }

        // Used for animations that contain root motion to drive the character뭩
        // position and rotation using the 밠otion?node of the animation file.
        void OnAnimatorMove()
        {
            if (!animator) { return; }

            // Not used when using Navmesh Navigation.
            //if (rpgCharacterController.isNavigating) { return; }

            OnMove.Invoke(animator.deltaPosition, animator.rootRotation);
        }
    }
}

