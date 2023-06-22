using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpfcurd.command
{
    public class Relaycommand : ICommand
    {
        Action  executeAction;
        Func< bool> canExecute;
        bool canExecuteCache;
        public Relaycommand(Action executeAction, Func< bool> canExecute, bool canExecuteCache)
        {
            this.canExecute = canExecute;
            this.executeAction = executeAction;
            this.canExecuteCache = canExecuteCache;
        }
        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }
            else
            {
                return canExecute();
            }

        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            { 
                CommandManager.RequerySuggested -= value;
            }
        }
        public void Execute(object parameter)
        {

            executeAction();
        }
    }
}

