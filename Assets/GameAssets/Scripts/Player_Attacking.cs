using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Attacking : MonoBehaviour
{
    private Collider2D[] attackColliders; // ���� �ݶ��̴� �迭
    private int currentAttackIndex = 0; // ���� ���� �ε���

    void Start()
    {
        // ���� ������Ʈ�� �ִ� ��� Collider2D�� ������
        attackColliders = GetComponents<Collider2D>();

        // �ݶ��̴� ��Ȱ��ȭ
        foreach (var collider in attackColliders)
        {
            collider.enabled = false;
        }
    }

    void Update()
    {
        // ���콺 ���� Ŭ�� �� ����
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        // ���� ���� �ݶ��̴� ��Ȱ��ȭ
        if (currentAttackIndex > 0)
        {
            attackColliders[currentAttackIndex - 1].enabled = false;
        }

        // ���� ���� �ݶ��̴� Ȱ��ȭ
        attackColliders[currentAttackIndex].enabled = true;

        // �ݶ��̴��� ���� �浹�ϸ� ���ظ� �� �� �ֵ��� ��
        StartCoroutine(DisableColliderAfterTime(currentAttackIndex));

        // ���� �ε��� ������Ʈ (1, 2, 3 ������ ����)
        currentAttackIndex = (currentAttackIndex + 1) % attackColliders.Length;
    }

    private IEnumerator DisableColliderAfterTime(int index)
    {
        // ��� ���
        yield return new WaitForSeconds(1f); // �ʿ��� ��� �ð� ���� ����

        // �ݶ��̴� ��Ȱ��ȭ
        attackColliders[index].enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // �� ���ݿ� ���� �ٸ��� ���ظ� �� �� ����
                int damage = 10 + currentAttackIndex * 10; // ����: 1�ܰ� 10, 2�ܰ� 20, 3�ܰ� 30

                enemy.TakeDamage(damage);
            }
        }
    }

}
