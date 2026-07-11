using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfChat.Helpers;

public static class FocusHelper
{
    public static readonly DependencyProperty IsFocusedProperty =
        DependencyProperty.RegisterAttached(
            "IsFocused",
            typeof(bool),
            typeof(FocusHelper),
            new PropertyMetadata(false, OnIsFocusedChanged));

    public static bool GetIsFocused(DependencyObject obj) =>
        (bool)obj.GetValue(IsFocusedProperty);

    public static void SetIsFocused(DependencyObject obj, bool value) =>
        obj.SetValue(IsFocusedProperty, value);

    private static void OnIsFocusedChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not UIElement element)
            return;

        if ((bool)e.NewValue)
        {
            element.Dispatcher.BeginInvoke(() =>
            {
                element.Focus();
                Keyboard.Focus(element);

                // Reset the property so it can be triggered again.
                SetIsFocused(element, false);
            }, DispatcherPriority.Input);
        }
    }
}