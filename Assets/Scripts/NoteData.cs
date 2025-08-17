namespace GameNamespace
{
    // �m�[�c�f�[�^��ێ�����N���X
    [System.Serializable]

    public class NoteData
    {
        public float beat; // ���ʂł̔�
        public float time; // ���ۂ̃m�[�c�o�����ԁi�v�Z�����j
        public NoteType noteType; // �m�[�c�̎��
        public FlickDirection flickDirection; // �t���b�N�̕����i�t���b�N�m�[�c�p�j
    }

    // �m�[�c�̎�ށiNormal, Hold, Flick�j���Ǘ�
    public enum NoteType
    {
        Normal,
        Hold,
        Flick // �� �����Ƀt���b�N��ǉ�
    }

    // �t���b�N�m�[�c�̕���
    public enum FlickDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}