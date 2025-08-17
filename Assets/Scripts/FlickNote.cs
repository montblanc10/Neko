using UnityEngine;

public class FlickNote : MonoBehaviour
{
    private float moveSpeedHorizontal = 2f;
    public float moveSpeedVertical = 5f;

    private bool touchedLine = false;
    private bool scored = false;
    private bool missed = false;
    private bool flickedUp = false;

    public float bpm = 120f;
    public float beatOffsetTime = 0.5f;

    private Vector3 startPosition;
    private Vector3 goalPosition;

    // 子オブジェクト参照
    private Transform imageMain; // 横移動だけする
    private Transform imageUp;   // フリックで上に飛ぶ

    void Start()
    {
        // 子オブジェクト取得（名前はHierarchyに合わせて変更）
        imageMain = transform.Find("Image_Main");
        imageUp = transform.Find("Image_Up");

        // 横スピード計算
        startPosition = transform.position;
        goalPosition = GameObject.FindGameObjectWithTag("Line").transform.position;

        float distance = Vector3.Distance(startPosition, goalPosition);
        moveSpeedHorizontal = distance / beatOffsetTime;
    }

    void Update()
    {
        // 親ごと横移動
        transform.Translate(Vector2.left * moveSpeedHorizontal * Time.deltaTime);

        // flick後は imageUp だけ上に移動
        if (flickedUp && imageUp != null)
        {
            imageUp.Translate(Vector2.up * moveSpeedVertical * Time.deltaTime);
        }

        // フリック判定
        if (touchedLine && !scored && !missed && IsFlickUp())
        {
            flickedUp = true;
            scored = true;
            GameManager.Instance.AddCombo();
            GameManager.Instance.AddScore();
        }

        // 判定ラインを過ぎたらミス
        if (transform.position.x < -11f && !scored && !missed)
        {
            missed = true;
            touchedLine = false;
            gameObject.tag = "Untagged";
            GameManager.Instance.Miss();
            Destroy(gameObject);
        }

        // 上に飛び切ったら削除（imageUp が一定高さを超えたら）
        if (flickedUp && imageUp != null && imageUp.position.y > 9f)
        {
            Invoke(nameof(DestroySelf), 0.5f);
        }
    }

    bool IsFlickUp()
    {
        bool swipeUp = false;

        // スマホ用
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 swipe = touch.position - touch.rawPosition;
                if (swipe.y > 50f) swipeUp = true;
            }
        }

        // PCテスト用（上矢印キー）
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            swipeUp = true;
        }

        return swipeUp;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Line"))
        {
            touchedLine = true;
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
