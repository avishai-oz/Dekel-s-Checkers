using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Checkers.Domain; // בשביל Coord אם תרצה בהמשך (לא חובה)

[ExecuteAlways] // עובד גם בעורך וגם בזמן ריצה
public class BoardTilesBuilder : MonoBehaviour
{
    [Header("Hierarchy")]
    [SerializeField] private Transform boardRoot;     // גרור לכאן את Grid/Board
    [SerializeField] private GameObject tilePrefab;   // הפריפאב של משבצת אחת

    [Header("Layout")]
    [SerializeField, Min(1)] private int size = 8;    // גודל הלוח
    [SerializeField] private Vector2 tileSize = new Vector2(1, 1); // מרחק בין משבצות (ב-XZ)
    [SerializeField] private Vector3 origin = Vector3.zero;        // נקודת התחלה (0,0)
    [SerializeField] private float y = 0f;            // גובה ה־Tiles (ב־Y)

    [ContextMenu("Rebuild Tiles")]
    public void RebuildTiles()
    {
        if (boardRoot == null || tilePrefab == null)
        {
            Debug.LogError("BoardTilesBuilder: please set boardRoot and tilePrefab");
            return;
        }

        // נקה ילדים קיימים
        for (int i = boardRoot.childCount - 1; i >= 0; i--)
        {
            var child = boardRoot.GetChild(i);
            if (Application.isPlaying) Destroy(child.gameObject);
            else DestroyImmediate(child.gameObject);
        }

        // בנה מחדש
        for (int r = 0; r < size; r++)
        for (int c = 0; c < size; c++)
        {
            GameObject go;
#if UNITY_EDITOR
            // שמירה טובה יותר של קישור לפריפאב בעורך
            if (!Application.isPlaying)
                go = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab, boardRoot);
            else
                go = Instantiate(tilePrefab, boardRoot);
#else
            go = Instantiate(tilePrefab, boardRoot);
#endif
            go.name = $"({r},{c})";
            var t = go.transform;
            t.localPosition = new Vector3(origin.x + c * tileSize.x, y, origin.z + r * tileSize.y);
            t.localRotation = Quaternion.identity;
            t.localScale    = Vector3.one;
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
            EditorUtility.SetDirty(boardRoot.gameObject);
#endif

        Debug.Log($"BoardTilesBuilder: built {size*size} tiles under {boardRoot.name}");
    }
}
