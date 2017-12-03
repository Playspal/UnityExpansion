using UnityEngine;

public static class ExtensionsGameObject
{
    /// <summary>
    /// Returns specified component in the GameObject.
    /// Adds new component to GameObject if component not added yet.
    /// </summary>
    /// <returns>Component instance</returns>
    public static T GetOrAddComponent<T>(this GameObject target) where T : Component
    {
        T component = target.GetComponent<T>();

        if(component == null)
        {
            component = target.AddComponent(typeof(T)) as T;
        }

        return component;
    }

    /// <summary>
    /// Set object's alpha using CanvasGroup component. Works with UI objects only.
    /// </summary>
    /// <param name="value">Float number from 0 to 1</param>
    public static void SetAlpha(this GameObject target, float value)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        canvasGroup = canvasGroup == null ? target.AddComponent<CanvasGroup>() : canvasGroup;

        canvasGroup.alpha = value;
    }

    /// <summary>
    /// Returns object's alpha using CanvasGroup component. Works with UI objects only.
    /// </summary>
    /// <returns>Float number from 0 to 1</returns>
    public static float GetAlpha(this GameObject target)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();

        return canvasGroup != null ? canvasGroup.alpha : 1;
    }

    /// <summary>
    /// Increases object's alpha using CanvasGroup component. Works with UI objects only.
    /// </summary>
    /// <param name="step">Float number from 0 to 1</param>
    public static void FadeIn(this GameObject target, float step)
    {
        float alpha = target.GetAlpha();

        alpha += step;
        alpha = alpha > 1 ? 1 : alpha;

        target.SetAlpha(alpha);
    }

    /// <summary>
    /// Decreases object's alpha using CanvasGroup component. Works with UI objects only.
    /// </summary>
    /// <param name="step">Float number from 0 to 1</param>
    public static void FadeOut(this GameObject target, float step)
    {
        float alpha = target.GetAlpha();

        alpha -= step;
        alpha = alpha < 0 ? 0 : alpha;

        target.SetAlpha(alpha);
    }
}