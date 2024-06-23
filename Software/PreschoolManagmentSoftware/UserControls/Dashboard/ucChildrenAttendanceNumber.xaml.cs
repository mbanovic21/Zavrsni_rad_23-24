using BusinessLogicLayer.DBServices;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PreschoolManagmentSoftware.UserControls.Dashboard
{
    public partial class ucChildrenAttendanceNumber : UserControl, INotifyPropertyChanged
    {
        private readonly AttendanceServices _attendanceServices = new AttendanceServices();
        private double _animatedNumber;

        public double AnimatedNumber
        {
            get { return _animatedNumber; }
            set
            {
                _animatedNumber = value;
                OnPropertyChanged(nameof(AnimatedNumber));
            }
        }

        public ucChildrenAttendanceNumber()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ucChildrenNumber_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNumber();
        }

        private async void LoadNumber()
        {
            var currentDate = DateTime.Now.ToString("dd.M.yyyy.");
            int attendanceCount = await Task.Run(() => _attendanceServices.GetAttendancesCountByDate(currentDate));
            AnimateNumber(attendanceCount);
        }

        private void AnimateNumber(int targetNumber)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = targetNumber,
                Duration = new Duration(TimeSpan.FromSeconds(2)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            this.BeginAnimation(AnimatedNumberProperty, animation);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static readonly DependencyProperty AnimatedNumberProperty =
            DependencyProperty.Register("AnimatedNumber", typeof(double), typeof(ucChildrenAttendanceNumber), new PropertyMetadata(0.0));
    }
}
