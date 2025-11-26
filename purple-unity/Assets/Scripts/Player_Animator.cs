using UnityEngine;

public class Player_animation : MonoBehaviour
{
    public string speedParam = "Speed";

    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (animator == null) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float speed = new Vector2(moveX, moveZ).magnitude;

        animator.SetFloat(speedParam, speed);
    }
}
