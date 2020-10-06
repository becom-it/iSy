using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeInfo.Services
{
    public class CanvasClickInvoker
    {
        private Action<string> _action;

        public CanvasClickInvoker(Action<string> callBack)
        {
            _action = callBack;
        }

        [JSInvokable("iSy")]
        public void EmployeeClicked(string path)
        {
            _action.Invoke(path);
        }
    }
}
