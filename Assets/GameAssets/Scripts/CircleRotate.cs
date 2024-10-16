using UnityEngine;

public class CircleRotate : MonoBehaviour
{
    public Transform center; // ȸ�� �߽�
    public float radius = 2f; // ȸ�� �ݰ�
    public float speed = 1f; // ȸ�� �ӵ�
    private float angle; // ���� ����
    public Transform target; // �ٶ� ������Ʈ�� Transform

    void Update()
    {
        // ���� ����
        angle += speed * Time.deltaTime;

        // ���� ����� ���ο� ��ġ ��� (Z���� 10���� ����)
        float x = center.position.x + Mathf.Cos(angle) * radius;
        float y = center.position.y + Mathf.Sin(angle) * radius;
        float z = 10f; // Z�� ����

        // ������Ʈ ��ġ ������Ʈ
        transform.position = new Vector3(x, y, z);

        if (target != null)
        {
            // Ÿ���� ��ġ���� ���� ���� ���
            Vector3 direction = target.position - transform.position;

            // ���� ���Ϳ��� ȸ���� ���
            if (direction != Vector3.zero)
            {
                // ȸ�� ������ ����ϰ� ������Ʈ�� ȸ�� ����
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ������ ������ ��ȯ
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Z�� �������� ȸ��
            }
        }
    }
}
