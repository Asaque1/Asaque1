using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 120; // ���� ü��

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("���� ���ظ� �Ծ����ϴ�: " + damage + " ���� ü��: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("���� �׾����ϴ�!");
        Destroy(gameObject); // �� ������Ʈ ����
    }
}
