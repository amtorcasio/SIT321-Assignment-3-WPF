using SARMS.Content;
using SARMS.Users;
using System.ComponentModel;

namespace SARMS.Data
{
    public class StudentAssessment : INotifyPropertyChanged
    {
        private Account _account;
        private Assessment _assessment;
        private double _mark;

        public event PropertyChangedEventHandler PropertyChanged;

        public Account account
        {
            get { return _account; }
            set { _account = value; }
        }
        public Assessment Assessment
        {
            get { return _assessment; }
            set { _assessment = value; }
        }
        public double Mark
        {
            get { return _mark; }
            set
            {
                _mark = value;
                OnPropertyChanged("Mark");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
