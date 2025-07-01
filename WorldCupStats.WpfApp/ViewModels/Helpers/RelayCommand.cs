using System;
using System.Windows.Input;

namespace WorldCupStats.WpfApp.ViewModels.Helpers
{
    // Comando sin parámetros
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // ¿Se puede ejecutar el comando?
        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        // Ejecuta la acción
        public void Execute(object? parameter) => _execute();

        // Evento para avisar que CanExecute cambió, para que WPF actualice los controles
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    // Comando con parámetro genérico
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Validar si se puede ejecutar
        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null) return true;

            // Intentar castear el parámetro al tipo esperado
            if (parameter == null && typeof(T).IsValueType)
                return false; // no se puede castear null a tipo valor

            if (parameter is T validParam)
                return _canExecute(validParam);

            // En caso de que no sea del tipo correcto, devolver false
            return false;
        }

        // Ejecutar con el parámetro convertido
        public void Execute(object? parameter)
        {
            if (parameter is T validParam)
                _execute(validParam);
            else if (parameter == null && typeof(T).IsClass)
                _execute(default!); // permitir parámetro null para tipos referencia
            else
                throw new ArgumentException($"El parámetro debe ser de tipo {typeof(T).Name}");
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

    }
}
