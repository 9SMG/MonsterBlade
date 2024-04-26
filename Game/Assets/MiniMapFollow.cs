using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public Vector3 offset; // 미니맵 카메라의 위치 오프셋

    void LateUpdate()
    {
        // 플레이어의 위치를 기준으로 미니맵의 위치를 업데이트
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}