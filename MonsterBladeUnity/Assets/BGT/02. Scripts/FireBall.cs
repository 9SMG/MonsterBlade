using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FireBall : MonoBehaviour
{
    public static FireBall NewFireBallLine(GameObject fireBallPrefab)
    {
        GameObject obj = Instantiate(fireBallPrefab);
        return obj.GetComponent<FireBall>();
    }

    LineRenderer mLineRenderer;

    public float mLineWidth = 0.05f;
    public Color mLineColor = Color.red;
    public float mDrawingTime = 1.0f;
    public float minY = 1.0f;
    public float maxY = 5.0f;
    public float fixedLineLength = 10.0f;
    public float particleMoveSpeed = 20f;
    GameObject particlePrefab;
    private Vector3 targetPos;

    void Awake()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        mLineRenderer.startWidth = mLineRenderer.endWidth = mLineWidth;
        mLineRenderer.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
        mLineRenderer.sharedMaterial.color = mLineColor;
    }

    public void CreateFireBall(Vector3 bossPosition, Transform playerTransform, float drawingTime, GameObject shot)
    {
        mDrawingTime = drawingTime;
        particlePrefab = shot;
        Vector3 startPos = bossPosition;
        startPos.y += Random.Range(minY, maxY);
        targetPos = playerTransform.position;
        mLineRenderer.SetPosition(0, startPos);
        StartCoroutine(COR_DrawLine(startPos, targetPos));
    }

    IEnumerator COR_DrawLine(Vector3 startPos, Vector3 targetPos)
    {
        float elapsedTime = 0f;
        Vector3 linePos = startPos;

        while (elapsedTime < mDrawingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / mDrawingTime);
            linePos = Vector3.Lerp(startPos, targetPos, t);

            mLineRenderer.SetPosition(1, linePos);

            yield return null;
        }

        mLineRenderer.SetPosition(1, targetPos);
        ShotParticle();
    }

    void ShotParticle()
    {
        if (particlePrefab != null)
        {
            Vector3 startPos = mLineRenderer.GetPosition(0);

            GameObject projectile = Instantiate(particlePrefab, startPos, Quaternion.identity);

            Vector3 direction = (targetPos - startPos).normalized;
            float distance = Vector3.Distance(startPos, targetPos);

            float moveTime = distance / particleMoveSpeed;

            StartCoroutine(MoveParticle(projectile.transform, direction, distance, moveTime));
        }
    }

    IEnumerator MoveParticle(Transform objectTransform, Vector3 direction, float distance, float moveTime)
    {
        bool check = true;
        float elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            float step = (distance / moveTime) * Time.deltaTime;
            objectTransform.Translate(direction * step, Space.World);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectTransform.position = targetPos;

        Destroy(objectTransform.gameObject);
        Destroy(this.gameObject);
    }
}