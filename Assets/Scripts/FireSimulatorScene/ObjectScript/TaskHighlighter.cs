using UnityEngine;

public class TaskHighlighter : MonoBehaviour
{
    //void Start()
    //{
    //    if (gameObject.activeSelf) // ✅ Only disable if not Task 1
    //    {
    //        DisableHighlight();
    //    }
    //}

    public void EnableHighlight()
    {
        gameObject.SetActive(true); // ✅ Show the highlight
    }

    public void DisableHighlight()
    {
        gameObject.SetActive(false); // ✅ Hide the highlight
    }
}
