using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Moving: MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float jumpForce = 100f; // ���� ��
    private Vector3 CurVt3;
    private Vector3 NextVt3;
    private Vector3 Vt3;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������ �߰�
    private bool isGrounded; // ���� �ִ����� ����
    private Transform tf;
    private bool Attacking;
    private Collider2D[] attackColliders; // ���� �ݶ��̴� �迭
    private int currentAttackIndex = 0; // ���� ���� �ε�����


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ��������
        tf = GetComponent<Transform>();

        // ���� ������Ʈ�� �ִ� ��� Collider2D�� ������
        attackColliders = GetComponents<Collider2D>();

        // �ݶ��̴� ��Ȱ��ȭ
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
        // ���콺 ���� Ŭ�� �� ����
        if (Input.GetMouseButtonDown(0)&&Attacking == false)
        {
            Attacking = true;
            PerformAttack();
        }

        animator.SetBool("Attack", Attacking);

        // �Է� �ޱ� (�����¿� ����)
        Vt3.x = Input.GetAxisRaw("Horizontal");
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);

        // ���� ��ġ
        CurVt3 = transform.position;
        NextVt3 = new Vector3(Vt3.x * moveSpeed, Vt3.y * moveSpeed, 0)*Time.fixedDeltaTime;
        // �ִϸ��̼� ���� ����
        animator.SetBool("Move", Vt3.x != 0);

        // ��������Ʈ ���� ��ȯ
        if (NextVt3.x < 0)
        {
            spriteRenderer.flipX = true; // ������ �� �� ��������Ʈ ����
        }
        else if (NextVt3.x > 0)
        {
            spriteRenderer.flipX = false; // �������� �� �� �⺻ ����
        }

        // ����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false; // ���߿� ���ְ� ����
        }
    }

    void FixedUpdate()
    {
        // ���� ������ ���� �̵�
        if(Attacking == false)
        {
            transform.position = CurVt3 + NextVt3;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �ٴڿ� ��Ҵ��� Ȯ��
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // �ٴڿ� ����
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

        Attacking = false;

        // �ݶ��̴� ��Ȱ��ȭ
        attackColliders[index].enabled = false;;
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