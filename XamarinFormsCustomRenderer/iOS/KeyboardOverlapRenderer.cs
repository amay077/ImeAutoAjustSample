using System;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using CoreGraphics;
using KeyboardOverlap.Forms.Plugin.iOSUnified;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(Page), typeof(KeyboardOverlapRenderer))]
namespace KeyboardOverlap.Forms.Plugin.iOSUnified
{
    [Preserve(AllMembers = true)]
    public class KeyboardOverlapRenderer : PageRenderer
    {
        NSObject _keyboardShowObserver;
        NSObject _keyboardHideObserver;
        private bool _isKeyboardShown;
        private double _sizeBeforeResizing;
        private double _screenHeightForGetOrientation;

        public static void Init()
        {
            var now = DateTime.Now;
            Debug.WriteLine("Keyboard Overlap plugin initialized {0}", now);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var page = Element as ContentPage;

            if (page != null)
            {
                var contentScrollView = page.Content as ScrollView;

                if (contentScrollView != null)
                    return;

                RegisterForKeyboardNotifications();
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            UnregisterForKeyboardNotifications();
        }

        void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
                _keyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardShow);
            if (_keyboardHideObserver == null)
                _keyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardHide);
        }

        void UnregisterForKeyboardNotifications()
        {
            _isKeyboardShown = false;
            if (_keyboardShowObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardShowObserver);
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardHideObserver);
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }

        protected virtual void OnKeyboardShow(NSNotification notification)
        {
            if (!IsViewLoaded || _isKeyboardShown)
                return;

            _screenHeightForGetOrientation = UIScreen.MainScreen.Bounds.Height;
            _isKeyboardShown = true;
            var activeView = View.FindFirstResponder();

            if (activeView == null)
                return;

            var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
            var isOverlapping = activeView.IsKeyboardOverlapping(View, keyboardFrame);

            if (!isOverlapping)
                return;

            var activeViewBottom = activeView.GetViewRelativeBottom(View);
            AdjustPageSize(keyboardFrame.Height, activeViewBottom);
        }

        private void OnKeyboardHide(NSNotification notification)
        {
            if (!IsViewLoaded)
                return;

            _isKeyboardShown = false;

            if (_screenHeightForGetOrientation != UIScreen.MainScreen.Bounds.Height)
            {
                _screenHeightForGetOrientation = UIScreen.MainScreen.Bounds.Height;
                _sizeBeforeResizing = -1;
                return;
            }

            if (_sizeBeforeResizing > Element.Bounds.Height)
            {
                RestorePageSize();
            }
        }

        private void AdjustPageSize(nfloat keyboardHeight, double activeViewBottom)
        {
            var pageFrame = Element.Bounds;

            _sizeBeforeResizing = pageFrame.Height;

            var newHeight = pageFrame.Height + CalculateShiftByAmount(pageFrame.Height, keyboardHeight, activeViewBottom);

            Element.LayoutTo(new Rectangle(pageFrame.X, pageFrame.Y,
                pageFrame.Width, newHeight));
        }

        private void RestorePageSize()
        {
            var pageFrame = Element.Bounds;

            Element.LayoutTo(new Rectangle(pageFrame.X, pageFrame.Y,
                pageFrame.Width, _sizeBeforeResizing));

            _sizeBeforeResizing = -1;
        }

        private double CalculateShiftByAmount(double pageHeight, nfloat keyboardHeight, double activeViewBottom)
        {
            return (pageHeight - activeViewBottom) - keyboardHeight;
        }
    }
}