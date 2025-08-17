using UnityEngine;

public class HoldNote : MonoBehaviour
{
    public Transform tubeFrontTransform;
    public Transform tubeBackTransform;
    public Transform cabinet;
    public Transform catTransform;

    public float duration = 0.1f; // 必要なホールド時間
    private bool isHolding = false;
    private float holdTime = 0f;
    private bool scored = false;
    private bool missed = false;
    private bool isPressed = false;
    private bool touchedLine = false;

    private float moveSpeed = 2f;
    public float beatOffsetTime = 2f;

    private Vector3 startPosition;
    private Vector3 goalPosition;

    void Start()
    {
        startPosition = transform.position;
        goalPosition = GameObject.FindGameObjectWithTag("Line").transform.position;

        float distance = Vector3.Distance(startPosition, goalPosition);
        moveSpeed = distance / beatOffsetTime;
    }

    public void Hold()
    {
        if (!isHolding)
        {
            isHolding = true;
            Debug.Log("Hold started");
        }

        holdTime += Time.deltaTime;
    }

    public void Release()
    {
        if (!scored && isHolding)
        {
            Debug.Log("Released too early, miss");
            GameManager.Instance.Miss();
            Destroy(gameObject);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        Vector3 move = Vector3.left * moveSpeed * Time.deltaTime;

        // 筒とキャビネットは常に動く
        tubeFrontTransform.Translate(move);
        tubeBackTransform.Translate(move);
        cabinet.Translate(move);

        // 猫はホールド中は止める、ホールド終了 or 成功後は動かす
        if (!isHolding || scored)
        {
            catTransform.Translate(move);
        }

        // 判定ラインを過ぎた場合のミス判定
        // 成功していない場合のみ発動する
        // 判定ラインを過ぎた場合のミス判定
        // 成功していない場合のみ発動する
        if (tubeFrontTransform.position.x < -11f && !missed && !scored)
        {
            missed = true;

            // ノーツを即時無効化
            touchedLine = false;
            isHolding = false;
            gameObject.tag = "Untagged"; // 他の処理から参照されないようにする

            Debug.Log("Miss called because note passed line at " + Time.time);
            GameManager.Instance.Miss();

            // 少し待ってから消える（エフェクト用の猶予）
            Invoke(nameof(DestroySelf), 0.5f);
        }



        HandleInput();
    }

    void HandleInput()
    {
        // 押下開始
        if (touchedLine && !isPressed && (IsTouchBegan() || Input.GetMouseButtonDown(0)))
        {
            isPressed = true;
            isHolding = true;
            holdTime = 0f;
            scored = false;
            Debug.Log("Hold started");
        }

        // ホールド中の処理
        if (isHolding)
        {
            Hold();

            // 規定時間ホールドで成功
            if (!scored && holdTime >= duration)
            {
                scored = true;
                isHolding = false;

                // 成功時は Miss フラグを立てない
                touchedLine = false;
                gameObject.tag = "Untagged";

                Debug.Log("Hold success!");
                GameManager.Instance.AddScore();
                GameManager.Instance.AddCombo();

                Invoke(nameof(DestroySelf), 1.0f);
            }
        }

        // 離した場合
        if (isPressed && (IsTouchEnded() || Input.GetMouseButtonUp(0)))
        {
            isPressed = false;

            if (!scored)
            {
                Debug.Log("Released too early!");
                missed = true;
                isHolding = false;
                GameManager.Instance.Miss();
                Debug.Log("Miss called from HoldNote at " + Time.time);
                Invoke(nameof(DestroySelf), 1.0f);
            }
        }
    }

    bool IsTouchBegan()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    bool IsTouchEnded()
    {
        return Input.touchCount > 0 &&
               (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Line"))
        {
            touchedLine = true;
        }
    }
}
