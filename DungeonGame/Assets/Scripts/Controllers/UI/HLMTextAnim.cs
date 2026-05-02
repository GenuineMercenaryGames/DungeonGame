using NUnit.Framework;
using UnityEngine;

public class HLMTextAnim : MonoBehaviour
{
    [SerializeField] private float maxOffsetX;
    [SerializeField] private float maxOffsetY;
    [SerializeField] private float speed;
    [SerializeField] private float distance;

    [SerializeField] private Transform[] layers;

    private float accumulatedTime;

    void Start()
    {
        accumulatedTime = 0.0f;
        for (int i = 1; i < layers.Length; ++i)
            layers[i].transform.position = layers[0].transform.position;
    }

    void Update()
    {
        for (int i = 1; i < layers.Length; ++i)
        {
            float w = ComputeWeight(i);
            float tx = Mathf.Sin(accumulatedTime * speed) * distance * w;
            float ty = Mathf.Cos(accumulatedTime * speed) * distance * w;

            float initialX = layers[0].position.x;
            float initialY = layers[0].position.y;

            float offsetX = maxOffsetX;
            float offsetY = maxOffsetY;

            float currentX = initialX + tx * offsetX;
            float currentY = initialY + ty * offsetY;

            Vector3 currentPosition = new Vector3(currentX, currentY, 0.0f);
            layers[i].transform.position = currentPosition;
        }
        accumulatedTime += Time.deltaTime;
    }

    float ComputeWeight(int index)
    {
        int minIndex = 0;
        int maxIndex = layers.Length - 1;
        float num = (float)(index);
        float denom = (float)(maxIndex - minIndex);
        float weight = num / denom;
        return weight;
    }
}
