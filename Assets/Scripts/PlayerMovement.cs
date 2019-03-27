using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum State : byte { IDLE, WALKING, RUNNING, CROUCHING, JUMPING }

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    new Camera camera;
    public CapsuleCollider capsule;
    //public Animator animator;
    public Health health;
    public CharacterController controller;


    [Header("State")]
    public State state = State.IDLE;
    [HideInInspector] public Vector3 moveDir;

    [Header("Walking")]
    public float walkSpeed = 5;

    [Header("Running")]
    public float runSpeed = 8;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Crouching")]
    public float crouchSpeed = 1.5f;
    public KeyCode crouchKey = KeyCode.C;

    [Header("Jumping")]
    public float jumpSpeed = 7;

    private Vector3 lastFall;

    [Header("Physics")]
    public float gravityMultiplier = 2;
    bool previouslyGrounded;

    [HideInInspector]
    public int soundCreated;
    public float verticalDir;
    public float horizontalDir;

    // Start is called before the first frame update
    void Start()
    {
        health.current = 100;
    }

    public bool isMoving()
    {
        if (state == State.RUNNING || state == State.WALKING)
        {
            return true;
        }

        return false;
    }



    // movement state machine //////////////////////////////////////////////////
    bool EventJumpRequested()
    {
        // the jump state needs to read here to make sure it is not missed
        // => only while grounded so jump key while jumping doesn't start
        //    a new jump immediately after landing
        return controller.isGrounded && !Utils.AnyInputActive() && Input.GetButtonDown("Jump");
    }

    bool EventCrouchToggle()
    {
        return !Utils.AnyInputActive() && Input.GetKeyDown(crouchKey);
    }

    bool EventLanded()
    {
        return !previouslyGrounded && controller.isGrounded;
    }

    float ApplyGravity(float moveDirY)
    {
        if (controller.isGrounded)
        {
            return Physics.gravity.y;
        }
        else
        {
            return moveDirY + Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }

    float GetWalkOrRunSpeed()
    {
        bool runRequested = !Utils.AnyInputActive() && Input.GetKey(runKey);
        return runRequested ? runSpeed : walkSpeed;
    }

    private State UpdateIDLE(Vector2 inputDir, Vector3 desiredDir)
    {
        // always set move direction
        moveDir.x = 0;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = 0;

        if (EventJumpRequested())
        {
            // start the jump movement into Y dir, go to jumping
            // note: no endurance>0 check because it feels odd if we can't jump
            moveDir.y = jumpSpeed;
            //PlayJumpSound();
            return State.JUMPING;
        }
        else if (EventLanded())
        {
            //PlayLandingSound();
            return State.IDLE;
        }
        else if (EventCrouchToggle())
        {
            return State.CROUCHING;
        }
        else if (inputDir != Vector2.zero)
        {
            return State.WALKING;
        }

        return State.IDLE;
    }

    private State UpdateWALKINGandRUNNING(Vector2 inputDir, Vector3 desiredDir)
    {

        verticalDir = inputDir.y;
        horizontalDir = inputDir.x;

        // walk or run?
        float speed = GetWalkOrRunSpeed();

        // always set move direction
        moveDir.x = desiredDir.x * speed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * speed;

        if (EventJumpRequested())
        {
            // start the jump movement into Y dir, go to jumping
            // note: no endurance>0 check because it feels odd if we can't jump
            moveDir.y = jumpSpeed;
            //PlayJumpSound();
            return State.JUMPING;
        }
        else if (EventLanded())
        {
            //PlayLandingSound();
            return State.IDLE;
        }
        else if (EventCrouchToggle())
        {
            return State.CROUCHING;
        }
        else if (inputDir == Vector2.zero)
        {
            return State.IDLE;
        }

        //ProgressStepCycle(inputDir, speed);
        return speed == walkSpeed ? State.WALKING : State.RUNNING;
    }

    private State UpdateCROUCHING(Vector2 inputDir, Vector3 desiredDir)
    {
        // always set move direction
        moveDir.x = desiredDir.x * crouchSpeed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * crouchSpeed;

        if (EventJumpRequested())
        {
            // stop crouching when pressing jump key. this feels better than
            // jumping from the crouching state.
            return State.IDLE;
        }
        else if (EventLanded())
        {
            //PlayLandingSound();
            return State.IDLE;
        }
        else if (EventCrouchToggle())
        {
            return State.IDLE;
        }

        //ProgressStepCycle(inputDir, crouchSpeed);
        return State.CROUCHING;
    }

    private State UpdateJUMPING(Vector2 inputDir, Vector3 desiredDir)
    {
        // walk or run?
        float speed = GetWalkOrRunSpeed();

        // always set move direction
        moveDir.x = desiredDir.x * speed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * speed;

        if (EventLanded())
        {
            //PlayLandingSound();
            return State.IDLE;
        }

        return State.JUMPING;
    }

    private Vector2 GetInputDirection()
    {
        float horizontal = 0;
        float vertical = 0;
        if (health.current > 0 && !Utils.AnyInputActive())
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }


        return new Vector2(horizontal, vertical).normalized;
    }

    private Vector3 GetDesiredDirection(Vector2 inputDir)
    {
        return transform.forward * inputDir.y + transform.right * inputDir.x;
    }

    private Vector3 GetDesiredDirectionOnGround(Vector3 desiredDir)
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hitInfo,
                                controller.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            return Vector3.ProjectOnPlane(desiredDir, hitInfo.normal).normalized;
        }
        return desiredDir;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputDir = GetInputDirection();
        Vector3 desiredDir = GetDesiredDirection(inputDir);
        Vector3 desiredGroundDir = GetDesiredDirectionOnGround(desiredDir);

        if (state == State.IDLE) state = UpdateIDLE(inputDir, desiredGroundDir);
        else if (state == State.WALKING) state = UpdateWALKINGandRUNNING(inputDir, desiredGroundDir);
        else if (state == State.RUNNING) state = UpdateWALKINGandRUNNING(inputDir, desiredGroundDir);
        else if (state == State.CROUCHING) state = UpdateCROUCHING(inputDir, desiredGroundDir);
        else if (state == State.JUMPING) state = UpdateJUMPING(inputDir, desiredGroundDir);

        previouslyGrounded = controller.isGrounded;
        if (!controller.isGrounded) lastFall = controller.velocity;

        controller.Move(moveDir * Time.deltaTime);


    }
}
