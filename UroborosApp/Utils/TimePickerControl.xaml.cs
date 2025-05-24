using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace UroborosApp.Utils
{
    public partial class TimePickerControl : UserControl
    {
        public TimePickerControl()
        {
            InitializeComponent();
            HoursBox.TextChanged += UpdateTime;
            MinutesBox.TextChanged += UpdateTime;
        }

        private void UpdateTime(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(HoursBox.Text, out int hours) &&
                int.TryParse(MinutesBox.Text, out int minutes))
            {
                Time = new TimeSpan(hours, minutes, 0);
            }
        }

        public TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(TimeSpan), typeof(TimePickerControl),
            new FrameworkPropertyMetadata(TimeSpan.FromHours(12), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
