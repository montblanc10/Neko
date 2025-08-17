using UnityEngine;

public class MoveLeftOnly : MonoBehaviour
{
    private float moveSpeedHorizontal = 13f;

    public float bpm = 120f;
    public float beatOffsetTime = 0.5f;

    private Vector3 startPosition;
    private Vector3 goalPosition;
    void Update()
    {
        // –ˆƒtƒŒ[ƒ€¶‚ÉˆÚ“®
        transform.Translate(Vector2.left * moveSpeedHorizontal * Time.deltaTime);
    }
}
