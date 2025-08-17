using UnityEngine;

public class TapNote : MonoBehaviour
{
    private float moveSpeedHorizontal = 2f;

    private bool touchedLine = false;
    private bool scored = false;
    private bool missed = false;

    public float bpm = 120f;
    public float beatOffsetTime = 0.5f;

    private Vector3 startPosition;
    private Vector3 goalPosition;

    private Animator animator; // 子オブジェクト用 Animator

    void Start()
    {
        startPosition = transform.position;
        goalPosition = GameObject.FindGameObjectWithTag("Line").transform.position;

        float distance = Vector3.Distance(startPosition, goalPosition);
        moveSpeedHorizontal = distance / beatOffsetTime;

        // 子オブジェクト "Image_Anim" の Animator を取得
        Transform animChild = transform.Find("Image_Anim");
        if (animChild != null)
        {
            animator = animChild.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("Image_Anim が見つかりません。階層名を確認してください。");
        }
    }

    void Update()
    {
        // 横移動のみ
        transform.Translate(Vector2.left * moveSpeedHorizontal * Time.deltaTime);

        // 入力判定（マウス + タッチ）
        bool tapped = false;
        if (Input.GetMouseButtonDown(0))
            tapped = true;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            tapped = true;

        // タップ成功時
        if (touchedLine && tapped && !scored && !missed)
        {
            scored = true;

            // 子オブジェクトのアニメーションを再生
            if (animator != null)
            {
                animator.SetTrigger("Tap");
            }

            GameManager.Instance.AddCombo();
            GameManager.Instance.AddScore();

            // アニメーション長を取得（未取得なら0.5秒）
            float animLength = 0.5f;
            if (animator != null && animator.GetCurrentAnimatorStateInfo(0).length > 0)
                animLength = animator.GetCurrentAnimatorStateInfo(0).length;

            Destroy(gameObject, animLength);
        }

        // ミス判定
        if (transform.position.x < -11f && !scored && !missed)
        {
            missed = true;
            touchedLine = false;
            gameObject.tag = "Untagged";
            GameManager.Instance.Miss();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Line"))
            touchedLine = true;
    }
}
