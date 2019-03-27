using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator ani;
    public Health health;
    public PlayerMovement movement;
    public PlayerHotbar hotbar;

    public CharacterController controller;

    private float velocity_z;
    private float velocity_x;


    void Update()
    {
        ani.SetBool("DEAD", health.current == 0);
        velocity_z = GetComponent<CharacterController>().velocity.z;
        velocity_x = GetComponent<CharacterController>().velocity.x;
        float speed = Mathf.Sqrt((velocity_x * velocity_x) + (velocity_z * velocity_z));
        float vert = 0;
        float hori = 0;

        if (movement.verticalDir < -0.001f || movement.verticalDir > 0.001f)
            vert = (speed * (movement.verticalDir / Mathf.Abs(movement.verticalDir))) / movement.runSpeed;

        if (movement.horizontalDir < -0.001f || movement.horizontalDir > 0.001f)
            hori = (speed * (movement.horizontalDir / Mathf.Abs(movement.horizontalDir))) / movement.runSpeed;

        ani.SetBool("MOVING", movement.isMoving());
        ani.SetFloat("Vertical_f", vert);
        ani.SetFloat("Horizontal_f", hori);
        ani.SetBool("CROUCHING", movement.state == State.CROUCHING);
        ani.SetBool("JUMPING", movement.state == State.JUMPING && !controller.isGrounded);
        ani.SetBool("IDLE", movement.state == State.IDLE);

    }
}