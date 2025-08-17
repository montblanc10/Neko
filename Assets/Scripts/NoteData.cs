namespace GameNamespace
{
    // ノーツデータを保持するクラス
    [System.Serializable]

    public class NoteData
    {
        public float beat; // 譜面での拍
        public float time; // 実際のノーツ出現時間（計算される）
        public NoteType noteType; // ノーツの種類
        public FlickDirection flickDirection; // フリックの方向（フリックノーツ用）
    }

    // ノーツの種類（Normal, Hold, Flick）を管理
    public enum NoteType
    {
        Normal,
        Hold,
        Flick // ← ここにフリックを追加
    }

    // フリックノーツの方向
    public enum FlickDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}