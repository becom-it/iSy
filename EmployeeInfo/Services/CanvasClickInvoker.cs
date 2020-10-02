using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeInfo.Services
{
    public class CanvasClickInvoker
    {
        private Action _action;

        public CanvasClickInvoker(Action callBack)
        {
            _action = callBack;
        }

        [JSInvokable("iSy")]
        public void EmployeeClicked()
        {
            _action.Invoke();
        }
    }
}
