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

    private Animator animator; // �q�I�u�W�F�N�g�p Animator

    void Start()
    {
        startPosition = transform.position;
        goalPosition = GameObject.FindGameObjectWithTag("Line").transform.position;

        float distance = Vector3.Distance(startPosition, goalPosition);
        moveSpeedHorizontal = distance / beatOffsetTime;

        // �q�I�u�W�F�N�g "Image_Anim" �� Animator ���擾
        Transform animChild = transform.Find("Image_Anim");
        if (animChild != null)
        {
            animator = animChild.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("Image_Anim ��������܂���B�K�w�����m�F���Ă��������B");
        }
    }

    void Update()
    {
        // ���ړ��̂�
        transform.Translate(Vector2.left * moveSpeedHorizontal * Time.deltaTime);

        // ���͔���i�}�E�X + �^�b�`�j
        bool tapped = false;
        if (Input.GetMouseButtonDown(0))
            tapped = true;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            tapped = true;

        // �^�b�v������
        if (touchedLine && tapped && !scored && !missed)
        {
            scored = true;

            // �q�I�u�W�F�N�g�̃A�j���[�V�������Đ�
            if (animator != null)
            {
                animator.SetTrigger("Tap");
            }

            GameManager.Instance.AddCombo();
            GameManager.Instance.AddScore();

            // �A�j���[�V���������擾�i���擾�Ȃ�0.5�b�j
            float animLength = 0.5f;
            if (animator != null && animator.GetCurrentAnimatorStateInfo(0).length > 0)
                animLength = animator.GetCurrentAnimatorStateInfo(0).length;

            Destroy(gameObject, animLength);
        }

        // �~�X����
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
