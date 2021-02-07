using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージとライフを制御するコンポーネント
/// </summary>
public class Damageable : MonoBehaviour
{
    /// <summary>初期ライフ</summary>
    [SerializeField, Range(1, 1000)] int m_initialLife = 1000;
    /// <summary>現在のライフ</summary>
    int m_life;

    private void Start()
    {
        m_life = m_initialLife;
    }

    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(int damage)
    {
        m_life -= damage;
        Debug.Log(m_life);
        if (m_life < 1)
        {
            Destroy(gameObject);
        }
    }
}