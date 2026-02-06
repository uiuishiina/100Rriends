using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [SerializeField] private GravitySettings gravitySettings;
    private Rigidbody rb;
    private Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Unity�̃f�t�H���g�d�͂𖳌���
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (gravitySettings != null)
        {
            // �J�X�^���d�͂�K�p
            Vector3 gravity = gravitySettings.GetGravityVector();
            velocity += gravity * Time.fixedDeltaTime;

            // �ő嗎�����x�𐧌�
            if (velocity.magnitude > gravitySettings.maxFallSpeed)
            {
                velocity = velocity.normalized * gravitySettings.maxFallSpeed;
            }

            rb.linearVelocity = velocity;
        }
    }
}
