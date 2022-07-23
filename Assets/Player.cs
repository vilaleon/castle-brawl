using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {
        TagAllChildren(transform, "Player");
    }

    void TagAllChildren(Transform rootTrans, string tag)
    {
        if (rootTrans.CompareTag("Untagged"))
        {
            rootTrans.tag = "Player";
        }

        foreach (Transform child in rootTrans)
        {
            TagAllChildren(child, tag);
        }
    }
}
