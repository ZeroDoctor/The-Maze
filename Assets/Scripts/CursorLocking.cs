using System.Linq;
using UnityEngine;

public class CursorLocking : MonoBehaviour
{

    public GameObject[] windowsThatUnlockCursor;

    void Update()
    {
        // "go" is the all the ui that needs the cursor to be unlocked
        // so if none is specified than it default the cursor as being invisible
        Cursor.lockState = windowsThatUnlockCursor.Any(go => go.activeSelf) ? CursorLockMode.None : CursorLockMode.Locked;

        Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
    }
}
