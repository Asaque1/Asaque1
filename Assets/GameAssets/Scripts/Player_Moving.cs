using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Moving: MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public float jumpForce = 100f; // 점프 힘
    private Vector3 CurVt3;
    private Vector3 NextVt3;
    private Vector3 Vt3;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러 추가
    private bool isGrounded; // 땅에 있는지의 여부
    private Transform tf;
    private bool Attacking;
    private Collider2D[] attackColliders; // 공격 콜라이더 배열
    private int currentAttackIndex = 0; // 현재 공격 인덱스ㄴ


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 가져오기
        tf = GetComponent<Transform>();

        // 무기 오브젝트에 있는 모든 Collider2D를 가져옴
        attackColliders = GetComponents<Collider2D>();

        // 콜라이더 비활성화
        foreach (var collider in attackColliders)
        {
            collider.enabled = false;
        }
    }
    
    void Awake()
    {

    }

    void Update()
    {
        // 마우스 왼쪽 클릭 시 공격
        if (Input.GetMouseButtonDown(0)&&Attacking == false)
        {
            Attacking = true;
            PerformAttack();
        }

        animator.SetBool("Attack", Attacking);

        // 입력 받기 (상하좌우 방향)
        Vt3.x = Input.GetAxisRaw("Horizontal");
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);

        // 현재 위치
        CurVt3 = transform.position;
        NextVt3 = new Vector3(Vt3.x * moveSpeed, Vt3.y * moveSpeed, 0)*Time.fixedDeltaTime;
        // 애니메이션 상태 설정
        animator.SetBool("Move", Vt3.x != 0);

        // 스프라이트 방향 전환
        if (NextVt3.x < 0)
        {
            spriteRenderer.flipX = true; // 왼쪽을 볼 때 스프라이트 반전
        }
        else if (NextVt3.x > 0)
        {
            spriteRenderer.flipX = false; // 오른쪽을 볼 때 기본 상태
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false; // 공중에 떠있게 설정
        }
    }

    void FixedUpdate()
    {
        // 물리 엔진을 통한 이동
        if(Attacking == false)
        {
            transform.position = CurVt3 + NextVt3;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿았는지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // 바닥에 닿음
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

        Attacking = false;

        // 콜라이더 비활성화
        attackColliders[index].enabled = false;;
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