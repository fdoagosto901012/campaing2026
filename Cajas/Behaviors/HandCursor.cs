using Microsoft.Maui.Controls;

#if WINDOWS
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System.Reflection;
using Windows.System;
#endif

namespace Cajas.Behaviors
{
    public static class HandCursor
    {
        public static readonly BindableProperty EnabledProperty =
            BindableProperty.CreateAttached(
                "Enabled",
                typeof(bool),
                typeof(HandCursor),
                false,
                propertyChanged: OnEnabledChanged);

        public static bool GetEnabled(BindableObject bindable)
        {
            return (bool)bindable.GetValue(EnabledProperty);
        }

        public static void SetEnabled(
            BindableObject bindable,
            bool value)
        {
            bindable.SetValue(EnabledProperty, value);
        }

        private static void OnEnabledChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (bindable is not VisualElement element)
                return;

            element.HandlerChanged -= Element_HandlerChanged;
            element.HandlerChanged += Element_HandlerChanged;

            Apply(element);
        }

        private static void Element_HandlerChanged(
            object? sender,
            EventArgs e)
        {
            if (sender is VisualElement element)
            {
                Apply(element);
            }
        }

        private static void Apply(VisualElement element)
        {
#if WINDOWS
            if (element.Handler?.PlatformView
                is not UIElement nativeElement)
            {
                return;
            }

            nativeElement.PointerEntered -= OnPointerEntered;
            nativeElement.PointerExited -= OnPointerExited;

            if (!GetEnabled(element))
            {
                SetCursor(
                    nativeElement,
                    InputSystemCursorShape.Arrow);

                return;
            }

            nativeElement.PointerEntered += OnPointerEntered;
            nativeElement.PointerExited += OnPointerExited;
#endif
        }

#if WINDOWS
        private static void OnPointerEntered(
            object sender,
            PointerRoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                SetCursor(
                    element,
                    InputSystemCursorShape.Hand);
            }
        }

        private static void OnPointerExited(
            object sender,
            PointerRoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                SetCursor(
                    element,
                    InputSystemCursorShape.Arrow);
            }
        }

        private static void SetCursor(
            UIElement element,
            InputSystemCursorShape cursorShape)
        {
            var protectedCursorProperty =
                typeof(UIElement).GetProperty(
                    "ProtectedCursor",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            if (protectedCursorProperty is null)
                return;

            var cursor =
                InputSystemCursor.Create(cursorShape);

            protectedCursorProperty.SetValue(
                element,
                cursor);
        }
#endif
    }
}
