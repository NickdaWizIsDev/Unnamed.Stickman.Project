using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData data;

    public float volume;
    public float bloom;

    public bool finished;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (data == null)
        {
            data = this;
        }
    }
}