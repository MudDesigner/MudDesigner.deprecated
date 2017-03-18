using System;
using System.Windows.Input;

namespace InteractiveServer
{
    class CommandDelegate<TCommandData> : ICommand
    {
        private Action<TCommandData> executeCallback;
        private Func<TCommandData, bool> canExecuteCallback;

        public CommandDelegate(Action<TCommandData> executeCallback)
        {
            this.executeCallback = executeCallback;
            this.canExecuteCallback = (data) => true;
        }

        public event EventHandler CanExecuteChanged;

        public void RefreshState(object requestor) => this.CanExecuteChanged?.Invoke(requestor, EventArgs.Empty);

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return this.canExecuteCallback(default(TCommandData));
            }

            if (parameter != null && !(parameter is TCommandData))
            {
                throw new InvalidOperationException($"The parameter provided was not of type {typeof(TCommandData).Name}");
            }

            return this.canExecuteCallback((TCommandData)parameter);
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
            {
                this.executeCallback(default(TCommandData));
            }

            if (parameter != null && !(parameter is TCommandData))
            {
                throw new InvalidOperationException($"The parameter provided was not of type {typeof(TCommandData).Name}");
            }

            this.executeCallback((TCommandData)parameter);
        }
    }
}
