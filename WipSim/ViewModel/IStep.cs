using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WipSim.ViewModel
{
    public interface IStep
    {
        void Step(MainViewModel mainVm);
        void Reset();
    }
}
