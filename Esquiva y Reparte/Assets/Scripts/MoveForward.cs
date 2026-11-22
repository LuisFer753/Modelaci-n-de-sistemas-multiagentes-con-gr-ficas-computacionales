using System.Collections;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float movement = 60.0f;
    public float moveTime = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MoveCars());
    }

    private IEnumerator MoveCars()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + (transform.forward * movement);

        while (true)
        {
            float elapsedTime = 0f;

            while (elapsedTime < moveTime)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}