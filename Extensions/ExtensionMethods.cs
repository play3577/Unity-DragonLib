﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Contains all of Dragonlib extension methods
/// </summary>
public static class ExtensionMethods
{
    #region Transform

    /// <summary>
    /// Copies the position and rotation of another transform.
    /// </summary>
    /// <param name="transformToCopy">transform to copy values from</param>
    public static void CopyPositionAndRotation(this Transform tr, Transform transformToCopy)
    {
        tr.transform.position = transformToCopy.position;
        tr.transform.rotation = transformToCopy.rotation;
    }

    public static void ResetTransform(this Transform tr)
    {
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        tr.localScale = Vector3.one;
    }

    public static Transform FindChildByTag(this Transform root, string tag)
    {
        foreach (Transform t in root.GetComponentsInChildren<Transform>())
        {
            if (t != root && t.tag == tag) return t;
        }
        Debug.LogWarning("Tag " + tag + " not found in any children of " + root.name);
        return null;
    }

    public static Transform[] FindChildrenByTag(this Transform parent, string tag)
    {
        return parent.GetComponentsInChildren<Transform>().Where(t => t != parent && t.tag == tag).ToArray();
    }

    /// <summary>
    /// Returns all child GameObjects of this Transform.
    /// </summary>
    public static List<GameObject> GetAllChildren(this Transform transform)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform.transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }

    /// <summary>
    /// Destroy all children of this Transform.
    /// </summary>
    public static void DestroyAllChildren(this Transform transform)
    {
        if (transform == null)
        {
            return;
        }

        var children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        children.ForEach(child => Object.Destroy(child));
    }

    public static Transform SetX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        return transform;
    }

    public static Transform SetY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        return transform;
    }

    public static Transform SetZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
        return transform;
    }

    #endregion Transform

    #region GameObject

    /// <summary>
    /// Returns all components in this GameObject's children with a certain tag.
    /// </summary>
    public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag)
    where T : Component
    {
        List<T> results = new List<T>();

        if (gameObject.CompareTag(tag))
            results.Add(gameObject.GetComponent<T>());

        foreach (Transform t in gameObject.transform)
            results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));

        return results.ToArray();
    }

    /// <summary>
    /// Returns the components in the parents of the gameobject.
    /// </summary>
    public static T GetComponentInParents<T>(this GameObject gameObject)
    where T : Component
    {
        for (Transform t = gameObject.transform; t != null; t = t.parent)
        {
            T result = t.GetComponent<T>();

            if (result != null)
                return result;
        }

        return null;
    }

    /// <summary>
    /// Set the layer of this GameObject and all of its children.
    /// </summary>
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform t in gameObject.transform)
            t.gameObject.SetLayerRecursively(layer);
    }

    /// <summary>
    /// Enables or disables all colliders on an object and its children.
    /// </summary>
    public static void SetCollisionRecursively(this GameObject gameObject, bool enable)
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
            collider.enabled = enable;
    }

    /// <summary>
    /// Enables or disables all renderers on an object and its children.
    /// </summary>
    public static void SetVisualRecursively(this GameObject gameObject, bool enable)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
            renderer.enabled = enable;
    }

    #endregion GameObject

    #region Color

    /// <summary>
    /// Returns a color with a specified alpha value.
    /// </summary>
    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    #endregion Color

    #region String

    /// <summary>
    /// Returns true if string is alphanumeric.
    /// </summary>
    public static bool IsAlphaNum(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;

        for (int i = 0; i < str.Length; i++)
        {
            if (!(char.IsLetter(str[i])) && (!(char.IsNumber(str[i]))))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Creates a performance friendly string using StringBuilder.
    /// </summary>
    public static string Build(this string str, params string[] stringParams)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(str);

        for (int i = 0; i < stringParams.Length; i++)
        {
            builder.Append(stringParams[i]);
        }

        return builder.ToString();
    }

    #endregion String

    #region int

    /// <summary>
    /// Formats an int to a string with thousands seperated eg 21000 becoms 21,000.
    /// </summary>
    public static string ThousandsSeperated(this int num)
    {
        return string.Format("{0:n0}", num);
    }

    #endregion int

    #region float

    /// <summary>
    /// Formats an float to a string with thousands seperated eg 21000 becoms 21,000.
    /// </summary>
    public static string ThousandsSeperated(this float num)
    {
        return string.Format("{0:n0}", num);
    }

    #endregion float

    #region double

    /// <summary>
    /// Formats an float to a string with thousands seperated eg 21000 becoms 21,000.
    /// </summary>
    public static string ThousandsSeperated(this double num)
    {
        return string.Format("{0:n0}", num);
    }

    #endregion double

    #region RectTransform

    /// <summary>
    /// Returns a Rect in screen space of this RectTransform. Useful for UI elements mapped to world objects.
    /// </summary>
    public static Rect RectTransformToScreenSpace(this RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect((Vector2)transform.position - (size * 0.5f), size);
    }

    #endregion RectTransform

    #region Action

    /// <summary>
    /// Invoke this action if the action is not null.
    /// </summary>
    public static void InvokeSafely(this Action action)
    {
        if (action != null)
        {
            action();
        }
    }

    /// <summary>
    /// Invoke this action along with a single parameter if the action is not null.
    /// </summary>
    public static void InvokeSafely<T>(this Action<T> action, T t)
    {
        if (action != null)
        {
            action(t);
        }
    }

    /// <summary>
    /// Invoke this action along with two parameters if the action is not null.
    /// </summary>
    public static void InvokeSafely<T1, T2>(this Action<T1, T2> action, T1 tOne, T2 tTwo)
    {
        if (action != null)
        {
            action(tOne, tTwo);
        }
    }

    /// <summary>
    /// Invoke this action along with three parameters if the action is not null.
    /// </summary>
    public static void InvokeSafely<T1, T2, T3>(this Action<T1, T2, T3> action, T1 tOne, T2 tTwo, T3 tThree)
    {
        if (action != null)
        {
            action(tOne, tTwo, tThree);
        }
    }

    #endregion Action

    #region List

    /// <summary>
    /// Returns the first element from this list.
    /// </summary>
    public static T First<T>(this List<T> list)
    {
        return list[0];
    }

    /// <summary>
    /// Returns the last element from this list.
    /// </summary>
    public static T Last<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }

    /// <summary>
    /// Returns a random element from this list.
    /// </summary>
    public static T Random<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    #endregion List
}