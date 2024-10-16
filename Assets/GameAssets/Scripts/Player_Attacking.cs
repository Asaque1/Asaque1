using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Attacking : MonoBehaviour
{
    private Collider2D[] attackColliders; // 공격 콜라이더 배열
    private int currentAttackIndex = 0; // 현재 공격 인덱스

    void Start()
    {
        // 무기 오브젝트에 있는 모든 Collider2D를 가져옴
        attackColliders = GetComponents<Collider2D>();

        // 콜라이더 비활성화
        foreach (var collider in attackColliders)
        {
            collider.enabled = false;
        }
    }

    void Update()
    {
        // 마우스 왼쪽 클릭 시 공격
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        // 이전 공격 콜라이더 비활성화
        if (currentAttackIndex > 0)
        {
            attackColliders[currentAttackIndex - 1].enabled = false;
        }

        // 현재 공격 콜라이더 활성화
        attackColliders[currentAttackIndex].enabled = true;

        // 콜라이더가 적과 충돌하면 피해를 줄 수 있도록 함
        StartCoroutine(DisableColliderAfterTime(currentAttackIndex));

        // 공격 인덱스 업데이트 (1, 2, 3 순서로 진행)
        currentAttackIndex = (currentAttackIndex + 1) % attackColliders.Length;
    }

    private IEnumerator DisableColliderAfterTime(int index)
    {
        // 잠시 대기
        yield return new WaitForSeconds(1f); // 필요한 대기 시간 조정 가능

        // 콜라이더 비활성화
        attackColliders[index].enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // 각 공격에 따라 다르게 피해를 줄 수 있음
                int damage = 10 + currentAttackIndex * 10; // 예시: 1단계 10, 2단계 20, 3단계 30

                enemy.TakeDamage(damage);
            }
        }
    }

}
