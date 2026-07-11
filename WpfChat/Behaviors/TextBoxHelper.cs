using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfChat.Behaviors;

public static class TextBoxBehavior
{
    public static readonly DependencyProperty SelectAllOnFocusProperty =
        DependencyProperty.RegisterAttached(
            "SelectAllOnFocus",
            typeof(bool),
            typeof(TextBoxBehavior),
            new PropertyMetadata(false, OnSelectAllOnFocusChanged));

    public static bool GetSelectAllOnFocus(DependencyObject obj) =>
        (bool)obj.GetValue(SelectAllOnFocusProperty);

    public static void SetSelectAllOnFocus(DependencyObject obj, bool value) =>
        obj.SetValue(SelectAllOnFocusProperty, value);

    private static void OnSelectAllOnFocusChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextBox textBox)
            return;

        if ((bool)e.NewValue)
        {
            textBox.GotKeyboardFocus += OnGotKeyboardFocus;
            textBox.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }
        else
        {
            textBox.GotKeyboardFocus -= OnGotKeyboardFocus;
            textBox.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
        }
    }

    private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        ((TextBox)sender).SelectAll();
    }

    private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var textBox = (TextBox)sender;

        if (!textBox.IsKeyboardFocusWithin)
        {
            e.Handled = true;
            textBox.Focus();
        }
    }
}