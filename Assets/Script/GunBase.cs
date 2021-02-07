using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
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
    /// <summary>射撃音</summary>
    [SerializeField] AudioClip m_shootingSfx;
    [SerializeField, Range(50, 200)] int m_shootDamage = 50;
    [SerializeField, Range(50, 200)] int m_bullet = 50;
    float m_time = 0;
    [SerializeField] float m_interval = 0.1f; 

    public void Start()
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
    }

    public void Update()
    {
        Aim();
        Shoot();
    }
    public void OnDestroy()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Aim()
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

    public void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            while (m_bullet > 0)
            {
                m_time = Time.deltaTime;

                if (m_interval > m_time)
                {
                    m_bullet--;
                    if (m_shootingSfx)
                    {
                        AudioSource.PlayClipAtPoint(m_shootingSfx, this.transform.position);
                    }

                    m_target.Damage(m_shootDamage);
                }
            }
        }
    }
}
