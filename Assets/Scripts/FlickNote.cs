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

    // �q�I�u�W�F�N�g�Q��
    private Transform imageMain; // ���ړ���������
    private Transform imageUp;   // �t���b�N�ŏ�ɔ��

    void Start()
    {
        // �q�I�u�W�F�N�g�擾�i���O��Hierarchy�ɍ��킹�ĕύX�j
        imageMain = transform.Find("Image_Main");
        imageUp = transform.Find("Image_Up");

        // ���X�s�[�h�v�Z
        startPosition = transform.position;
        goalPosition = GameObject.FindGameObjectWithTag("Line").transform.position;

        float distance = Vector3.Distance(startPosition, goalPosition);
        moveSpeedHorizontal = distance / beatOffsetTime;
    }

    void Update()
    {
        // �e���Ɖ��ړ�
        transform.Translate(Vector2.left * moveSpeedHorizontal * Time.deltaTime);

        // flick��� imageUp ������Ɉړ�
        if (flickedUp && imageUp != null)
        {
            imageUp.Translate(Vector2.up * moveSpeedVertical * Time.deltaTime);
        }

        // �t���b�N����
        if (touchedLine && !scored && !missed && IsFlickUp())
        {
            flickedUp = true;
            scored = true;
            GameManager.Instance.AddCombo();
            GameManager.Instance.AddScore();
        }

        // ���胉�C�����߂�����~�X
        if (transform.position.x < -11f && !scored && !missed)
        {
            missed = true;
            touchedLine = false;
            gameObject.tag = "Untagged";
            GameManager.Instance.Miss();
            Destroy(gameObject);
        }

        // ��ɔ�ѐ؂�����폜�iimageUp ����荂���𒴂�����j
        if (flickedUp && imageUp != null && imageUp.position.y > 9f)
        {
            Invoke(nameof(DestroySelf), 0.5f);
        }
    }

    bool IsFlickUp()
    {
        bool swipeUp = false;

        // �X�}�z�p
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 swipe = touch.position - touch.rawPosition;
                if (swipe.y > 50f) swipeUp = true;
            }
        }

        // PC�e�X�g�p�i����L�[�j
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
