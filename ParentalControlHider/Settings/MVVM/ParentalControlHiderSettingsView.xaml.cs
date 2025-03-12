using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParentalControlHider.Settings.MVVM
{
    public partial class ParentalControlHiderSettingsView : UserControl
    {
        public ParentalControlHiderSettingsView()
        {
            InitializeComponent();
        }

        private void HandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
	        if (e.Handled)
	        {
		        return;
	        }

	        e.Handled = true;
	        var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
	        {
		        RoutedEvent = MouseWheelEvent,
		        Source = sender
	        };
	        var parent = ((Control)sender).Parent as UIElement;
	        parent?.RaiseEvent(eventArg);
        }
	}
}