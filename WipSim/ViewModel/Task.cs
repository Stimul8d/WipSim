using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WipSim.ViewModel
{
    [DebuggerDisplay("ID:{Id} AN:{LeftInAnalysis} DE:{LeftInDev} TE:{LeftInTest}")]
    public class Task : ViewModelBase
    {
        public Task(int id, double leftInAnalysis, double leftInDev, double leftInTest)
        {
            this.Id = id;
            this.LeftInAnalysis = leftInAnalysis;
            this.LeftInDev = leftInDev;
            this.LeftInTest = leftInTest;
            this.MaxTotalWork = leftInAnalysis + leftInDev + leftInTest;
        }

        /// <summary>
        /// The <see cref="EndTime" /> property's name.
        /// </summary>
        public const string EndTimePropertyName = "EndTime";

        private int endTime = 0;

        /// <summary>
        /// Sets and gets the EndTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                if (endTime == value)
                {
                    return;
                }

                RaisePropertyChanging(EndTimePropertyName);
                endTime = value;
                RaisePropertyChanged(EndTimePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="StartTime" /> property's name.
        /// </summary>
        public const string StartTimePropertyName = "StartTime";

        private int startTime = 0;

        /// <summary>
        /// Sets and gets the StartTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                if (startTime == value)
                {
                    return;
                }

                RaisePropertyChanging(StartTimePropertyName);
                startTime = value;
                RaisePropertyChanged(StartTimePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="TotalWork" /> property's name.
        /// </summary>
        public const string TotalWorkPropertyName = "TotalWork";

        /// <summary>
        /// Sets and gets the TotalWork property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double TotalWork
        {
            get
            {
                return (leftInAnalysis + leftInDev + leftInTest);
            }
        }

        /// <summary>
        /// The <see cref="MaxTotalWork" /> property's name.
        /// </summary>
        public const string MaxTotalWorkPropertyName = "MaxTotalWork";

        private double maxTotalWork = 0;

        /// <summary>
        /// Sets and gets the MaxTotalWork property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MaxTotalWork
        {
            get
            {
                return maxTotalWork;
            }

            set
            {
                if (maxTotalWork == value)
                {
                    return;
                }

                RaisePropertyChanging(MaxTotalWorkPropertyName);
                maxTotalWork = value;
                RaisePropertyChanged(MaxTotalWorkPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CycleTime" /> property's name.
        /// </summary>
        public const string CycleTimePropertyName = "CycleTime";

        private int cycleTime = 0;

        /// <summary>
        /// Sets and gets the CycleTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int CycleTime
        {
            get
            {
                return cycleTime;
            }

            set
            {
                if (cycleTime == value)
                {
                    return;
                }

                RaisePropertyChanging(CycleTimePropertyName);
                cycleTime = value;
                RaisePropertyChanged(CycleTimePropertyName);
                RaisePropertyChanged(TotalWorkPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LeadTime" /> property's name.
        /// </summary>
        public const string LeadTimePropertyName = "LeadTime";

        private int leadTime = 0;

        /// <summary>
        /// Sets and gets the LeadTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int LeadTime
        {
            get
            {
                return leadTime;
            }

            set
            {
                if (leadTime == value)
                {
                    return;
                }

                RaisePropertyChanging(LeadTimePropertyName);
                leadTime = value;
                RaisePropertyChanged(LeadTimePropertyName);
                RaisePropertyChanged(TotalWorkPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LeftInTest" /> property's name.
        /// </summary>
        public const string LeftInTestPropertyName = "LeftInTest";

        private double leftInTest = 0;

        /// <summary>
        /// Sets and gets the LeftInTest property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double LeftInTest
        {
            get
            {
                return leftInTest;
            }

            set
            {
                if (leftInTest == value)
                {
                    return;
                }

                RaisePropertyChanging(LeftInTestPropertyName);
                leftInTest = value;
                RaisePropertyChanged(LeftInTestPropertyName);
                RaisePropertyChanged(TotalWorkPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LeftInDev" /> property's name.
        /// </summary>
        public const string LeftInDevPropertyName = "LeftInDev";

        private double leftInDev = 0;

        /// <summary>
        /// Sets and gets the LeftInDev property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double LeftInDev
        {
            get
            {
                return leftInDev;
            }

            set
            {
                if (leftInDev == value)
                {
                    return;
                }

                RaisePropertyChanging(LeftInDevPropertyName);
                leftInDev = value;
                RaisePropertyChanged(LeftInDevPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LeftInAnalysis" /> property's name.
        /// </summary>
        public const string LeftInAnalysisPropertyName = "LeftInAnalysis";

        private double leftInAnalysis = 0;

        /// <summary>
        /// Sets and gets the LeftInAnalysis property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double LeftInAnalysis
        {
            get
            {
                return leftInAnalysis;
            }

            set
            {
                if (leftInAnalysis == value)
                {
                    return;
                }

                RaisePropertyChanging(LeftInAnalysisPropertyName);
                leftInAnalysis = value;
                RaisePropertyChanged(LeftInAnalysisPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="Id" /> property's name.
        /// </summary>
        public const string IdPropertyName = "Id";

        private int id = 1;

        /// <summary>
        /// Sets and gets the Id property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                if (id == value)
                {
                    return;
                }

                RaisePropertyChanging(IdPropertyName);
                id = value;
                RaisePropertyChanged(IdPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="InBacklog" /> property's name.
        /// </summary>
        public const string InBacklogPropertyName = "InBacklog";

        private Visibility inBacklog = Visibility.Visible;

        /// <summary>
        /// Sets and gets the InBacklog property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility InBacklog
        {
            get
            {
                return inBacklog;
            }

            set
            {
                if (inBacklog == value)
                {
                    return;
                }

                RaisePropertyChanging(InBacklogPropertyName);
                inBacklog = value;
                RaisePropertyChanged(InBacklogPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="InAnalysis" /> property's name.
        /// </summary>
        public const string InAnalysisPropertyName = "InAnalysis";

        private Visibility inAnalysis = Visibility.Hidden;

        /// <summary>
        /// Sets and gets the InAnalysis property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility InAnalysis
        {
            get
            {
                return inAnalysis;
            }

            set
            {
                if (inAnalysis == value)
                {
                    return;
                }

                RaisePropertyChanging(InAnalysisPropertyName);
                inAnalysis = value;
                RaisePropertyChanged(InAnalysisPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="InDev" /> property's name.
        /// </summary>
        public const string InDevPropertyName = "InDev";

        private Visibility inDev = Visibility.Hidden;

        /// <summary>
        /// Sets and gets the InDev property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility InDev
        {
            get
            {
                return inDev;
            }

            set
            {
                if (inDev == value)
                {
                    return;
                }

                RaisePropertyChanging(InDevPropertyName);
                inDev = value;
                RaisePropertyChanged(InDevPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="InTest" /> property's name.
        /// </summary>
        public const string InTestPropertyName = "InTest";

        private Visibility inTest = Visibility.Hidden;

        /// <summary>
        /// Sets and gets the InTest property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility InTest
        {
            get
            {
                return inTest;
            }

            set
            {
                if (inTest == value)
                {
                    return;
                }

                RaisePropertyChanging(InTestPropertyName);
                inTest = value;
                RaisePropertyChanged(InTestPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="InDone" /> property's name.
        /// </summary>
        public const string InDonePropertyName = "InDone";

        private Visibility inDone = Visibility.Hidden;

        /// <summary>
        /// Sets and gets the InDone property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility InDone
        {
            get
            {
                return inDone;
            }

            set
            {
                if (inDone == value)
                {
                    return;
                }

                RaisePropertyChanging(InDonePropertyName);
                inDone = value;
                RaisePropertyChanged(InDonePropertyName);
            }
        }

        public Column Position
        {
            get
            {
                if (inBacklog == Visibility.Visible) return Column.Backlog;
                if (inAnalysis == Visibility.Visible) return Column.Analysis;
                if (inDev == Visibility.Visible) return Column.Dev;
                if (inTest == Visibility.Visible) return Column.Test;

                return Column.Done;
            }
        }

        public double LeftInColumn
        {
            get
            {
                switch (Position)
                {
                    case Column.Analysis:
                        return LeftInAnalysis;
                    case Column.Dev:
                        return LeftInDev;
                    case Column.Test:
                        return LeftInTest;
                    default:
                        return 0;
                }
            }
        }

        public void MoveRightOne(int day)
        {
            switch (Position)
            {
                case Column.Backlog:
                    MovePosition(Column.Analysis, day);
                    break;
                case Column.Analysis:
                    MovePosition(Column.Dev, day);
                    break;
                case Column.Dev:
                    MovePosition(Column.Test, day);
                    break;
                case Column.Test:
                    MovePosition(Column.Done, day);
                    break;
                case Column.Done:
                    break;
                default:
                    break;
            }
        }

        public void MovePosition(Column column, int day)
        {
            switch (column)
            {
                case Column.Backlog:
                    InBacklog = Visibility.Visible;
                    InAnalysis = Visibility.Hidden;
                    InDev = Visibility.Hidden;
                    InTest = Visibility.Hidden;
                    InDone = Visibility.Hidden;
                    break;
                case Column.Analysis:
                    StartTime = day;
                    InBacklog = Visibility.Hidden;
                    InAnalysis = Visibility.Visible;
                    InDev = Visibility.Hidden;
                    InTest = Visibility.Hidden;
                    InDone = Visibility.Hidden;
                    break;
                case Column.Dev:
                    InBacklog = Visibility.Hidden;
                    InAnalysis = Visibility.Hidden;
                    InDev = Visibility.Visible;
                    InTest = Visibility.Hidden;
                    InDone = Visibility.Hidden;
                    break;
                case Column.Test:
                    InBacklog = Visibility.Hidden;
                    InAnalysis = Visibility.Hidden;
                    InDev = Visibility.Hidden;
                    InTest = Visibility.Visible;
                    InDone = Visibility.Hidden;
                    break;
                case Column.Done:
                    EndTime = day;
                    InBacklog = Visibility.Hidden;
                    InAnalysis = Visibility.Hidden;
                    InDev = Visibility.Hidden;
                    InTest = Visibility.Hidden;
                    InDone = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        public bool Complete
        {
            get { return EndTime > 0; }
        }

        public int CompletionTime
        {
            get { return EndTime - StartTime; }
        }

        public void DontWork()
        {
            this.LeadTime++;
        }

        internal double Work(double dayPercentageReduction)
        {
            CycleTime++;
            LeadTime++;
            switch (this.Position)
            {
                case Column.Analysis:
                    LeftInAnalysis -= dayPercentageReduction;
                    if (LeftInAnalysis < 0)
                    {
                        var remaining = LeftInAnalysis;
                        LeftInAnalysis = 0;
                        return remaining;
                    }
                    break;
                case Column.Dev:
                    LeftInDev -= dayPercentageReduction;
                    if (LeftInDev < 0)
                    {
                        var remaining = LeftInDev;
                        LeftInDev = 0;
                        return remaining;
                    }
                    break;
                case Column.Test:
                    LeftInTest -= dayPercentageReduction;
                    if (LeftInTest < 0)
                    {
                        var remaining = LeftInTest;
                        LeftInTest = 0;
                        return remaining;
                    }
                    break;
                default:
                    return 0;
            }
            return 0;
        }
    }
}
