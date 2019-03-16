using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public static class Utils
{

    // Checking if any input is active
    public static bool AnyInputActive()
    {
        return Selectable.allSelectables.Any(sel => sel is InputField && ((InputField)sel).isFocused);
    }
    public static bool IsCursorOverUserInterface()
    {
        // IsPointerOverGameObject check for left mouse (default)
        if (EventSystem.current.IsPointerOverGameObject())
            return true;
        // IsPointerOverGameObject check for touches
        for (int i = 0; i < Input.touchCount; ++i)
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                return true;

        // OnGUI check
        return GUIUtility.hotControl != 0;
    }

    public static float GetZoomUniversal()
    {
        if (Input.mousePresent)
            return Utils.GetAxisRawScrollUniversal();
        return 0;
    }


    public static float GetAxisRawScrollUniversal()
    {
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll < 0) return -1;
        if (scroll > 0) return 1;
        return 0;
    }

    public static int GetStableHashCode(this string text)
    {
        unchecked
        {
            int hash = 23;
            foreach (char c in text)
                hash = hash * 31 + c;
            return hash;
        }
    }

    public static float ClosestDistance(Collider a, Collider b)
    {
        if (a.bounds.Intersects(b.bounds))
            return 0;

        return Vector3.Distance(a.ClosestPoint(b.transform.position), b.ClosestPoint(a.transform.position));
    }

    public static bool RaycastWithout(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance, GameObject ignore, int layerMask = Physics.DefaultRaycastLayers)
    {
        // remember layers
        Dictionary<Transform, int> backups = new Dictionary<Transform, int>();

        // set all to ignore raycast
        foreach (Transform tf in ignore.GetComponentsInChildren<Transform>(true))
        {
            backups[tf] = tf.gameObject.layer;
            tf.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        bool result = Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);

        // restore layers
        foreach (KeyValuePair<Transform, int> kvp in backups)
            kvp.Key.gameObject.layer = kvp.Value;

        return result;
    }

    // Same as RaycastWithout but with a Sphere
    public static bool SphereCastWithout(Vector3 origin, float sphereRadius, Vector3 direction, out RaycastHit hit, float maxDistance, GameObject ignore, int layerMask = Physics.DefaultRaycastLayers)
    {
        // remember layers
        Dictionary<Transform, int> backups = new Dictionary<Transform, int>();

        // set all to ignore raycast
        foreach (Transform tf in ignore.GetComponentsInChildren<Transform>(true))
        {
            backups[tf] = tf.gameObject.layer;
            tf.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        bool result = Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask);

        // restore layers
        foreach (KeyValuePair<Transform, int> kvp in backups)
            kvp.Key.gameObject.layer = kvp.Value;

        return result;
    }


}