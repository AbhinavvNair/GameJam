using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SeriousMovement : MonoBehaviour
{
    [Header("Serious Sam Settings")]
    private float WalkSpeed = 10f;       // Fast, but controllable
    private float RunSpeed = 16f;        // Distinct sprint
    private float Gravity = 30f;         // Heavy gravity (no floating)
    private float JumpHeight = 3f;

    [Header("Snappiness")]
    // High values = Instant start/stop. Lower values = Slight smoothing.
    private float GroundSnappiness = 50f;
    private float AirControl = 5f;

    private CharacterController controller;
    private Vector3 moveVelocity;       // X and Z movement
    private float verticalVelocity;     // Y movement (Gravity/Jump)

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. INPUT
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        // 2. DIRECTION (The "Anti-Fly" Math)
        // We get the camera's facing direction, but we force Y to 0.
        // This guarantees that looking up/down NEVER affects your movement speed.
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Calculate the direction we WANT to go
        Vector3 desiredDir = (forward * inputZ) + (right * inputX);
        if (desiredDir.magnitude > 1f) desiredDir.Normalize();

        // 3. CALCULATE SPEED
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;

        if (controller.isGrounded)
        {
            // GROUND MOVEMENT: Snappy
            // We use MoveTowards to reach target speed almost instantly
            verticalVelocity = -2f; // Sticky force

            Vector3 targetVelocity = desiredDir * targetSpeed;

            moveVelocity = Vector3.MoveTowards(
                moveVelocity,
                targetVelocity,
                GroundSnappiness * Time.deltaTime
            );

            // JUMP
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = Mathf.Sqrt(2 * Gravity * JumpHeight);
            }
        }
        else
        {
            // AIR MOVEMENT: You have control, but momentum is preserved
            // We apply the input on top of existing movement, but smoother
            Vector3 targetVelocity = desiredDir * targetSpeed;

            // Serious Sam allows you to change direction mid-air, but it's not instant
            moveVelocity = Vector3.MoveTowards(
                moveVelocity,
                targetVelocity,
                AirControl * Time.deltaTime
            );

            // GRAVITY
            verticalVelocity -= Gravity * Time.deltaTime;
        }

        // 4. APPLY
        Vector3 finalMove = moveVelocity;
        finalMove.y = verticalVelocity;

        controller.Move(finalMove * Time.deltaTime);
    }
}


//using UnityEngine;



//public class PlayerMovt : MonoBehaviour

//{

//    public float MoveSmoothTime;

//    public float Gravity = -10f;

//    public float WalkSpeed;

//    //public float RunSpeed; sprint speed

//    public float JumpSpeed;



//    private CharacterController controller;

//    private Vector3 CurrentMoveVelocity;

//    private Vector3 MoveDampVelocity;

//    private Vector3 CurrentForceVelocity;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created

//    void Start()

//    {

//        controller = GetComponent<CharacterController>();

//    }



//    // Update is called once per frame

//    void Update()

//    {

//        Vector3 player_input = new Vector3

//        {

//            x = Input.GetAxisRaw("Horizontal"),

//            y = 0f,

//            z = Input.GetAxisRaw("Vertical")

//        };

//        if (player_input.magnitude > 1f)

//        {

//            player_input.Normalize();

//        }



//        Vector3 MoveVector = transform.TransformDirection(player_input);



//        //float current_speed= Input.GetKey(KeyCode.LeftShift)?RunSpeed : WalkSpeed; use if implementing sprinting



//        CurrentMoveVelocity = Vector3.SmoothDamp(CurrentMoveVelocity,

//            MoveVector * WalkSpeed,

//            ref MoveDampVelocity,

//            MoveSmoothTime);



//        if (controller.isGrounded)

//        {

//            CurrentForceVelocity.y = -2.0f; //for pulling down when on slopes

//            if (Input.GetKeyDown(KeyCode.Space))

//            {

//                CurrentForceVelocity.y = JumpSpeed;



//            }

//        }

//        else

//        {

//            CurrentForceVelocity.y += Gravity * Time.deltaTime;

//        }



//        Vector3 FinalVelocity = CurrentMoveVelocity + CurrentForceVelocity;

//        controller.Move(FinalVelocity * Time.deltaTime);

//    }

//}