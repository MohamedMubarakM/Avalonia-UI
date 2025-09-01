using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia1.Views.Pages;

namespace Avalonia1.ViewModels
{
    public enum PageType { Home, Page1}

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Control _currentView = new HomePage();

        public Control CurrentView
        {
            get => _currentView;
            private set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand NavigateCommand { get; }

        public MainWindowViewModel()
        {
            NavigateCommand = new RelayCommand<PageType>(page =>
            {
                CurrentView = page switch
                {
                    PageType.Home  => new HomePage(),
                    PageType.Page1 => new Page1View(),
                    _              => new HomePage()
                };
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // ---- Minimal ICommand helpers (no warnings) ----

    public sealed class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (parameter is null)
            {
                if (typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) is null)
                    return _canExecute?.Invoke(default!) ?? true;
                return _canExecute?.Invoke((T?)parameter!) ?? true;
            }
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is null)
            {
                if (typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) is null)
                { _execute(default!); return; }
                _execute((T?)parameter!);
                return;
            }
            _execute((T)parameter);
        }

        // No-op explicit event implementation to avoid CS0067
        event EventHandler? ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}
