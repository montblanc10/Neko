using UnityEngine;

public class HoldNote : MonoBehaviour
{
    public Transform tubeFrontTransform;
    public Transform tubeBackTransform;
    public Transform cabinet;
    public Transform catTransform;

    public float duration = 0.1f; // �K�v�ȃz�[���h����
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

        // ���ƃL���r�l�b�g�͏�ɓ���
        tubeFrontTransform.Translate(move);
        tubeBackTransform.Translate(move);
        cabinet.Translate(move);

        // �L�̓z�[���h���͎~�߂�A�z�[���h�I�� or ������͓�����
        if (!isHolding || scored)
        {
            catTransform.Translate(move);
        }

        // ���胉�C�����߂����ꍇ�̃~�X����
        // �������Ă��Ȃ��ꍇ�̂ݔ�������
        // ���胉�C�����߂����ꍇ�̃~�X����
        // �������Ă��Ȃ��ꍇ�̂ݔ�������
        if (tubeFrontTransform.position.x < -11f && !missed && !scored)
        {
            missed = true;

            // �m�[�c�𑦎�������
            touchedLine = false;
            isHolding = false;
            gameObject.tag = "Untagged"; // ���̏�������Q�Ƃ���Ȃ��悤�ɂ���

            Debug.Log("Miss called because note passed line at " + Time.time);
            GameManager.Instance.Miss();

            // �����҂��Ă��������i�G�t�F�N�g�p�̗P�\�j
            Invoke(nameof(DestroySelf), 0.5f);
        }



        HandleInput();
    }

    void HandleInput()
    {
        // �����J�n
        if (touchedLine && !isPressed && (IsTouchBegan() || Input.GetMouseButtonDown(0)))
        {
            isPressed = true;
            isHolding = true;
            holdTime = 0f;
            scored = false;
            Debug.Log("Hold started");
        }

        // �z�[���h���̏���
        if (isHolding)
        {
            Hold();

            // �K�莞�ԃz�[���h�Ő���
            if (!scored && holdTime >= duration)
            {
                scored = true;
                isHolding = false;

                // �������� Miss �t���O�𗧂ĂȂ�
                touchedLine = false;
                gameObject.tag = "Untagged";

                Debug.Log("Hold success!");
                GameManager.Instance.AddScore();
                GameManager.Instance.AddCombo();

                Invoke(nameof(DestroySelf), 1.0f);
            }
        }

        // �������ꍇ
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
