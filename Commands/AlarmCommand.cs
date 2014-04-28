using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorldCupGuide.Commands
{
    public class AlarmCommand : ICommand
    {
        public Action<object> m_Action;

        public AlarmCommand(Action<object> action)
        {
            m_Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return null != parameter;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (null != m_Action)
            {
                m_Action(parameter);
            }
        }
    }
}
