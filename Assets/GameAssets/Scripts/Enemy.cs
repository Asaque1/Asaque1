using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 120; // 적의 체력

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("적이 피해를 입었습니다: " + damage + " 남은 체력: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("적이 죽었습니다!");
        Destroy(gameObject); // 적 오브젝트 삭제
    }
}
