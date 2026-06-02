using System.Windows.Input;

namespace CarSalesApp.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _execute();
    }
}