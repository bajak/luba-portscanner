using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UI
{
    public abstract class PropertyBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate bool ValidatorFunc(object value, [CallerMemberName] String propertyName = null);

        protected bool SetProperty<T>(T value, ValidatorFunc validatorFunc = null, [CallerMemberName] String propertyName = null)
        {
            if (propertyName == null)
                return false;
            if (!_propertyValues.ContainsKey(propertyName))
                _propertyValues.Add(propertyName, null);
            else if (Equals(_propertyValues[propertyName], value))
                return false;

            if (validatorFunc != null && !validatorFunc(value, propertyName))
                return false;

            _propertyValues[propertyName] = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected T GetProperty<T>([CallerMemberName] String propertyName = null)
        {
            if (propertyName == null || !_propertyValues.ContainsKey(propertyName))
                return default(T);
            return (T)_propertyValues[propertyName];
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
