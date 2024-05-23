using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionar.Core
{
    public abstract class ViewModel : ObservableObject
    {
        public virtual void SetParameter(object parameter)
        {
        }
    }
}
