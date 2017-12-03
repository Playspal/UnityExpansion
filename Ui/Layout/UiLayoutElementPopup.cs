namespace UnityExpansion.UI
{
    /// <summary>
    /// Popups is a temporary layout elements that used to provide urgent information to user.
    /// Usually popups presented as small window with title, message and buttons
    /// like "Are you sure, you want to quit? Yes / No".
    /// General rule for popups: they should contain full screen shading element to block access to overlaid content.
    /// Popups can be shown at any time and in any order, allowed to show unlimited amount of same popups.
    /// After popup is hidden it will be destroyed.
    /// </summary>
    /// <seealso cref="UnityExpansion.UI.UiLayoutElement" />
    public class UiLayoutElementPopup : UiLayoutElement
    {
        /// <summary>
        /// MonoBehavior Start handler.
        /// In inherited classes always use base.Start() when overriding this method.
        /// </summary>
        protected override void Start()
        {
            base.Start();

            Show();
        }

        /// <summary>
        /// Performed when Hide processes completed.
        /// In inherited classes always use base.HideEnd() when overriding this method.
        /// </summary>
        protected override void HideEnd()
        {
            base.HideEnd();

            Destroy();
        }
    }
}