using System;
using System.ComponentModel;
using System.Windows;

namespace RenderImage.Wpf
{
    internal class ViewModelLocator
    {
        public static DependencyProperty AutoHookedUpViewModelProperty = DependencyProperty.RegisterAttached("AutoHookedUpViewModel",
                                                        typeof(bool),
                                                        typeof(ViewModelLocator),
                                                        new PropertyMetadata(false, AutoHookedUpViewModelChanged));

        public static bool GetAutoHookedUpViewModel(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(AutoHookedUpViewModelProperty);
        }

        public static void SetAutoHookedUpViewModel(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(AutoHookedUpViewModelProperty, value);
        }

        private static void AutoHookedUpViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d)) return;

            const string ViewModelAssembly = "RenderImage.ViewModel";
            var viewTypeName = d.GetType().FullName;
            var viewModelTypeName = viewTypeName.EndsWith("View", StringComparison.OrdinalIgnoreCase)
                                                            ? $"{viewTypeName}Model"
                                                            : $"{viewTypeName}ViewModel";
            viewModelTypeName = viewModelTypeName.Replace(".Wpf.", ".ViewModel.");
            var viewModelType = Type.GetType($"{viewModelTypeName}, {ViewModelAssembly}", true, true);
            var viewModel = Activator.CreateInstance(viewModelType);
            ((FrameworkElement)d).DataContext = viewModel;
        }
    }
}
