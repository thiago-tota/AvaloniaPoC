using System;
using System.Threading.Tasks;
using RenderImage.Shared.Helper;

namespace RenderImage.ViewModel
{
    public class ViewModelBase : BindableBase
    {
        //Events triggered to be handled by the Parent of the User Control.

        /// <summary>
        /// arg1 is the message do be displayed.
        /// </summary>
        public event Action<string> OnNotificationMessage = delegate { };

        /// <summary>
        /// arg1 is the message to be displayed.
        /// arg2 is the text to be displayed on the button.
        /// </summary>
        public event Action<string?, string?> OnDialogPopUpOneButton = delegate { };

        /// <summary>
        /// arg1 is the message to be displayed.
        /// arg2 is the text to be displayed on the confirm button.
        /// arg3 is the text to be displayed on the cancel button.
        /// </summary>
        public event Func<string, string?, string?, Task<bool>?> OnDialogPopUpTwoButtons = delegate { return default; };

        /// <summary>
        /// Call Navigate method.
        /// T1: Where to navigate
        /// T2: List of NamedParamenters
        /// </summary>
        public event Action<string> OnNavigate = delegate { };

        /// <summary>
        /// Set the Window Size of MainWindow.
        /// </summary>
        public event Action<int, int> OnSetWindowSize = delegate { };

        /// <summary>
        /// Display loading progress bar
        /// </summary>
        public event Action<bool> OnShowProgressBar = delegate { };

        public void ExecuteNotificationMessageEvent(string message)
        {
            OnNotificationMessage(message);
        }

        public void ExecuteDialogPopUpOneButtonEvent(string message, string? buttonText = default)
        {
            OnDialogPopUpOneButton(message, buttonText);
        }

        public Task<bool>? ExecuteDialogPopUpTwoButtonsEvent(string message, string? acceptText = default, string? cancelText = default)
        {
            return OnDialogPopUpTwoButtons(message, acceptText, cancelText);
        }

        public void ExecuteNavigateEvent(string message)
        {
            OnNavigate(message);
        }

        public void ExecuteSetWindowSizeEvent(int width, int height)
        {
            OnSetWindowSize(width, height);
        }

        public void ExecuteShowProgressBar(bool visible)
        {
            OnShowProgressBar(visible);
        }
    }
}
