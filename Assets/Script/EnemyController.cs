using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の挙動をコントロールするコンポーネント
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    Animator animator;
    /// <summary>動く時にかける力</summary>
    [SerializeField] float m_movePower = 30f;
    /// <summary>歩行速度</summary>
    [SerializeField] float m_maxSpeed = 4f;
    Rigidbody m_rb = null;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // プレイヤーの方に移動させる
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            Vector3 dir = player.transform.position - this.transform.position;
            dir.y = 0;  // 上下方向は無視する
            this.transform.forward = dir;

            if (m_rb.velocity.magnitude < m_maxSpeed)
            {
                m_rb.AddForce(this.transform.forward * m_movePower);
            }
        }
        else
        {
            // プレイヤーが居なくなったら消える
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("Attack", true);
        }
    }
}