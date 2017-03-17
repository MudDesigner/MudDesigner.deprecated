using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InteractiveServer
{
    class AsyncCommandDelegate : ICommand
    {
        private Func<Task> onExecuteCallback;
        private Func<bool> canExecuteCallback;

        public AsyncCommandDelegate(Func<Task> onExecute)
            => this.onExecuteCallback = onExecute;

        public AsyncCommandDelegate(Func<Task> onExecute, Func<bool> canExecute) : this(onExecute)
            => this.canExecuteCallback = canExecute;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => this.canExecuteCallback?.Invoke() ?? true;

        public async void Execute(object parameter) => await this.onExecuteCallback?.Invoke();
    }
}
