using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    /// <summary>FPS のカメラ</summary>
    [SerializeField] Camera m_mainCamera;
    /// <summary>照準となる UI オブジェクト</summary>
    [SerializeField] UnityEngine.UI.Image m_crosshair;
    /// <summary>照準に敵を捕らえていない時の色</summary>    
    [SerializeField] Color m_noTarget = Color.white;
    /// <summary>照準に敵を捕らえている時の色</summary>
    [SerializeField] Color m_onTarget = Color.red;
    /// <summary>射撃可能距離</summary>
    [SerializeField, Range(1, 200)] float m_shootRange = 10f;
    /// <summary>照準の Ray が当たる Layer</summary>
    [SerializeField] LayerMask m_shootingLayer;
    /// <summary>攻撃したらダメージを与えられる対象</summary>
    Damageable m_target;
    /// <summary>攻撃した時に加える力のスカラー量</summary>
    [SerializeField] float m_shootPower = 50f;
    /// <summary>射撃音</summary>
    [SerializeField] AudioClip m_shootingSfx;


    void Start()
    {
        // マウスカーソルを消す（実行中は ESC キーを押すとマウスカーソルが表示される）
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (!m_mainCamera)
        {
            m_mainCamera = Camera.main;
            if (!m_mainCamera)
            {
                Debug.LogError("Main Camera is not found.");
            }
        }
        //m_target = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        //Aim();
        Shoot();
    }

    void OnDestroy()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// 照準を操作する
    /// 照準にダメージを与えられる対象がいる場合に照準の色を変え、その対象を m_target に保存する
    /// </summary>
    void Aim()
    {
        Ray ray = m_mainCamera.ScreenPointToRay(m_crosshair.rectTransform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_shootRange, m_shootingLayer))
        {
            m_target = hit.collider.GetComponent<Damageable>();

            if (m_target)
            {
                m_crosshair.color = m_onTarget;
            }
            else
            {
                m_crosshair.color = m_noTarget;
            }
        }
        else
        {
            m_target = null;
            m_crosshair.color = m_noTarget;
        }
    }

    /// <summary>
    /// 敵を撃つ
    /// </summary>
    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            if (m_shootingSfx)
            {
                AudioSource.PlayClipAtPoint(m_shootingSfx, this.transform.position);
            }

            m_target.Damage(1);
            Debug.Log("撃った");

            //Rigidbody rb = m_target.GetComponent<Rigidbody>();
            //if (rb)
            //{
            //    // 斜め上方向に力を加える
            //    Vector3 dir = m_target.transform.position - this.transform.position;
            //    dir.y = 0;
            //    dir = (dir.normalized + Vector3.up).normalized;
            //    rb.AddForce(dir * m_shootPower, ForceMode.Impulse);
            //}

        }
    }
}
