using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private HoldNote currentHoldNote = null;

    void Update()
    {
        if (IsPressing() && currentHoldNote != null)
        {
            currentHoldNote.Hold();  // �z�[���h��
        }
        else if (!IsPressing() && currentHoldNote != null)
        {
            currentHoldNote.Release();  // �z�[���h�𗣂���
            currentHoldNote = null;
        }
    }

    bool IsPressing()
    {
#if UNITY_EDITOR
        return Input.GetMouseButton(0);  // PC�p
#else
        return Input.touchCount > 0;     // �X�}�z�p
#endif
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("HoldNote"))
        {
            currentHoldNote = other.gameObject.GetComponent<HoldNote>();
            Debug.Log("HoldNote entered");
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("HoldNote"))
        {
            currentHoldNote = null;
            Debug.Log("HoldNote exited");
        }
    }
}
