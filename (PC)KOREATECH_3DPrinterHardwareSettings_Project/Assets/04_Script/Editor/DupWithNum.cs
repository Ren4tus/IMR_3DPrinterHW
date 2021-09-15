using UnityEngine;
using UnityEditor;

public class DupWithNum : MonoBehaviour
{

    [MenuItem("GameObject/Duplicate With Numbering by ten %#v")]
    public static void DuplicateWithNumbering()
    {
        foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab | SelectionMode.Editable))
        {
            GameObject newObject = null;

            PrefabType pt = PrefabUtility.GetPrefabType(t.gameObject);
            if (pt == PrefabType.PrefabInstance || pt == PrefabType.ModelPrefabInstance)
            {
                // it's an instance of a prefab! Create a new instance of the same prefab!
                Object prefab = PrefabUtility.GetPrefabParent(t.gameObject);
                newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                // we've got a brand new prefab instance, but it doesn't have the same overrides as our original. Fix that...
                PropertyModification[] overrides = PrefabUtility.GetPropertyModifications(t.gameObject);
                PrefabUtility.SetPropertyModifications(newObject, overrides);

                //// okay, prefab is instantiated (or if it's not a prefab, we've just cloned it in the scene)
                //// Make sure it's got the same parent and position as the original!
                newObject.transform.parent = t.parent;
                newObject.transform.position = t.position;
                newObject.transform.rotation = t.rotation;
            }
            else
            {
                for (int i = 1; i <= 10; i++)
                {
                    // not a prefab... so just instantiate it!
                    newObject = Object.Instantiate(t.gameObject);

                    //always first object name of end with _01
                    string originalName = t.name;
                    string nameWithoutNumber = originalName.Substring(0, originalName.Length - 2);
                    int startNumber = int.Parse(originalName.Substring(originalName.Length - 2, 2));

                    //numbering example : _01, _02, _03 ... _10
                    originalName = string.Format("{0}{1:00}", nameWithoutNumber, startNumber + i);
                    newObject.name = originalName;

                    newObject.transform.parent = t.parent;
                    newObject.transform.position = t.position;
                    newObject.transform.rotation = t.rotation;
                    newObject.transform.localScale = t.localScale;
                }
            }
            // tell the Undo system we made this, so you can undo it if you did it by mistake
            Undo.RegisterCreatedObjectUndo(newObject, "Duplicate Without Renaming");
        }
    }

    [MenuItem("GameObject/Duplicate Without Renaming by ten %#v", true)]
    public static bool ValidateDuplicateWithNumbering()
    {
        return Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel | SelectionMode.ExcludePrefab | SelectionMode.Editable).Length > 0;
    }
}