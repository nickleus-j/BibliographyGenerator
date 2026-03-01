using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Bibliographía
{
    public class RelayObjectCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayObjectCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) =>
            _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) =>
            _execute(parameter);

        public event EventHandler? CanExecuteChanged;
    }
}
