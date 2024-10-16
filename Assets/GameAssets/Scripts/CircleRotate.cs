using UnityEngine;

public class CircleRotate : MonoBehaviour
{
    public Transform center; // 회전 중심
    public float radius = 2f; // 회전 반경
    public float speed = 1f; // 회전 속도
    private float angle; // 현재 각도
    public Transform target; // 바라볼 오브젝트의 Transform

    void Update()
    {
        // 각도 증가
        angle += speed * Time.deltaTime;

        // 원형 경로의 새로운 위치 계산 (Z축은 10으로 고정)
        float x = center.position.x + Mathf.Cos(angle) * radius;
        float y = center.position.y + Mathf.Sin(angle) * radius;
        float z = 10f; // Z축 고정

        // 오브젝트 위치 업데이트
        transform.position = new Vector3(x, y, z);

        if (target != null)
        {
            // 타겟의 위치로의 방향 벡터 계산
            Vector3 direction = target.position - transform.position;

            // 방향 벡터에서 회전값 계산
            if (direction != Vector3.zero)
            {
                // 회전 각도를 계산하고 오브젝트의 회전 적용
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 라디안을 각도로 변환
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Z축 기준으로 회전
            }
        }
    }
}
