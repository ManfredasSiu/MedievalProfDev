using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private SpriteRenderer m_Renderer;

    private bool m_LookingDown = true;
    private bool m_LookingUp = false;
    private bool m_TurningLeft = false;
    private bool m_TurningRight = false;

    private bool m_Idle = true;

    // Start is callebefored  the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var velocity = m_Rigidbody.velocity;
        if (velocity != Vector2.zero)
        {
            m_LookingDown = velocity.y < 0.1;
            m_LookingUp = velocity.y > 0.1;
            m_TurningLeft = velocity.x > 0.2;
            m_TurningRight = velocity.x < 0.2;
            m_Idle = false;
        }
        else
        {
            m_Idle = true;
        }

        ManageAnimationParameters();

        if (m_TurningLeft)
        {
            m_Renderer.flipX = false;
        }
        if (m_TurningRight)
        {
            m_Renderer.flipX = true;
        }
    }

    private void ManageAnimationParameters()
    {
        m_Animator.SetBool("LookDown", m_LookingDown);
        m_Animator.SetBool("LookUp", m_LookingUp);
        m_Animator.SetBool("LookSideways", m_TurningLeft || m_TurningRight);
    }
}
