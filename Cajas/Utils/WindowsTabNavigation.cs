using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.Utils
{
    public static class WindowsTabNavigation
    {
        public static readonly BindableProperty OrderProperty =
            BindableProperty.CreateAttached(
                "Order",
                typeof(int),
                typeof(WindowsTabNavigation),
                0,
                propertyChanged: OnOrderChanged);

        public static readonly BindableProperty IsTabStopProperty =
            BindableProperty.CreateAttached(
                "IsTabStop",
                typeof(bool),
                typeof(WindowsTabNavigation),
                true,
                propertyChanged: OnIsTabStopChanged);

        public static int GetOrder(BindableObject view)
        {
            return (int)view.GetValue(OrderProperty);
        }

        public static void SetOrder(
            BindableObject view,
            int value)
        {
            view.SetValue(OrderProperty, value);
        }

        public static bool GetIsTabStop(BindableObject view)
        {
            return (bool)view.GetValue(IsTabStopProperty);
        }

        public static void SetIsTabStop(
            BindableObject view,
            bool value)
        {
            view.SetValue(IsTabStopProperty, value);
        }

        private static void OnOrderChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            Configure(bindable);
        }

        private static void OnIsTabStopChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            Configure(bindable);
        }

        private static void Configure(BindableObject bindable)
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
                is Microsoft.UI.Xaml.Controls.Control control)
            {
                control.TabIndex = GetOrder(element);
                control.IsTabStop = GetIsTabStop(element);
            }
#endif
        }
    }
}
