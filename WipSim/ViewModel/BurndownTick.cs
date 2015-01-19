using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WipSim.ViewModel
{
    [DebuggerDisplay("{StepNumber}:{TasksRemaining}")]
    public class BurndownTick : ViewModelBase
    {
        public BurndownTick(int taskId, int stepNumber, int tasksRemaining)
        {
            this.TaskId = taskId;
            this.StepNumber = stepNumber;
            this.TasksRemaining = tasksRemaining;
        }

        ///// <summary>
        ///// The <see cref="TasksRemaining" /> property's name.
        ///// </summary>
        //public const string TasksRemainingPropertyName = "TasksRemaining";

        //private int tasksRemaining = 0;

        ///// <summary>
        ///// Sets and gets the TasksRemaining property.
        ///// Changes to that property's value raise the PropertyChanged event. 
        ///// </summary>
        //public int TasksRemaining
        //{
        //    get
        //    {
        //        return tasksRemaining;
        //    }

        //    set
        //    {
        //        if (tasksRemaining == value)
        //        {
        //            return;
        //        }

        //        RaisePropertyChanging(TasksRemainingPropertyName);
        //        tasksRemaining = value;
        //        RaisePropertyChanged(TasksRemainingPropertyName);
        //    }
        //}

        ///// <summary>
        ///// The <see cref="StepNumber" /> property's name.
        ///// </summary>
        //public const string StepNumberPropertyName = "StepNumber";

        //private int stepNumber = 0;

        ///// <summary>
        ///// Sets and gets the StepNumber property.
        ///// Changes to that property's value raise the PropertyChanged event. 
        ///// </summary>
        //public int StepNumber
        //{
        //    get
        //    {
        //        return stepNumber;
        //    }

        //    set
        //    {
        //        if (stepNumber == value)
        //        {
        //            return;
        //        }

        //        RaisePropertyChanging(StepNumberPropertyName);
        //        stepNumber = value;
        //        RaisePropertyChanged(StepNumberPropertyName);
        //    }
        //}

        public int TaskId { get; set; }

        public int StepNumber { get; set; }

        public int TasksRemaining { get; set; }
    }
}
