using UnityEditor;
using UnityEngine;

public class FindDeprecatedRigidbodies : EditorWindow
{
    [MenuItem("Tools/Find Deprecated Rigidbodies")]
    public static void ShowWindow()
    {
        GetWindow<FindDeprecatedRigidbodies>("Find Deprecated Rigidbodies");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Deprecated Rigidbodies"))
        {
            FindDeprecatedRigidbodiesInScene();
        }
    }

    private static void FindDeprecatedRigidbodiesInScene()
    {
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<Rigidbody>() == null && obj.GetComponent<Collider>() == null)
            {
                Debug.LogWarning("GameObject " + obj.name + " might be using deprecated rigidbody property.", obj);
            }
        }
    }
}
