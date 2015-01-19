using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using MoreLinq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using System.Collections.Concurrent;

namespace WipSim.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ConcurrentDictionary<Guid, BurndownTick> lastRunBurndownTicks;
        DispatcherTimer timer = new DispatcherTimer();
        IStep stepper = new StepByWorkerLimit();

        public ICommand StepCommand { get; set; }
        public ICommand CompleteCommand { get; set; }

        public ICommand GeneratePartiallyEqualWorkCommand { get; set; }
        public ICommand GenerateEqualWorkCommand { get; set; }
        public ICommand GenerateRandomWorkCommand { get; set; }

        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<BurndownTick> BurndownTicks { get; set; }
        public DoubleCollection Ticks { get; set; }

        public MainViewModel()
        {

            StepCommand = new RelayCommand(Step, CanStep);
            CompleteCommand = new RelayCommand(Complete, CanStep);

            GeneratePartiallyEqualWorkCommand = new RelayCommand(CreatePartiallyEqualTasks);
            GenerateEqualWorkCommand = new RelayCommand(CreateEqualTasks);
            GenerateRandomWorkCommand = new RelayCommand(CreateRandomTasks);

            Ticks = new DoubleCollection(new List<double> { 1, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 });
            BurndownTicks = new ObservableCollection<BurndownTick>();
            lastRunBurndownTicks = new ConcurrentDictionary<Guid, BurndownTick>();

            CreateRandomTasks();
        }

        private void CreatePartiallyEqualTasks()
        {
            Reset();
            if (Tasks == null) Tasks = new ObservableCollection<Task>();
            Tasks.Clear();
            var r = new Random();
            for (int i = 1; i <= NumberOfTasks; i++)
            {
                double analysis = (r.NextDouble() / 3.0);
                double dev = (r.NextDouble() / 3.0);
                double left = 1 - analysis - dev;
                double portion = left / 3;
                double test = portion;
                analysis += portion;
                dev += portion;
                analysis *= 10;
                dev *= 10;
                test *= 10;

                var analysisRemainder = analysis % 1;
                var devRemainder = dev % 1;
                var testRemainder = test % 1;
                analysis -= analysisRemainder;
                dev -= devRemainder;
                test -= testRemainder;

                var remaining = 10 - analysis - dev - test;
                dev += remaining;

                Tasks.Add(new Task(i, analysis, dev, test));
            }
            MaxConstraint = NumberOfTasks;
            TotalWorkGenerated = (int)Tasks.Sum(t => t.TotalWork);
            MaxWorkerWipLimit = Tasks.Count;
            lastRunBurndownTicks.TryAdd(Guid.NewGuid(), new BurndownTick(-1, 0, Tasks.Count));

        }

        private void CreateEqualTasks()
        {
            Reset();
            if (Tasks == null) Tasks = new ObservableCollection<Task>();
            Tasks.Clear();
            var r = new Random();
            for (int i = 1; i <= NumberOfTasks; i++)
            {
                var analysis = 3;
                var dev = 5;
                var test = 2;
                Tasks.Add(new Task(i, analysis, dev, test));
            }
            MaxConstraint = NumberOfTasks;
            TotalWorkGenerated = (int)Tasks.Sum(t => t.TotalWork);
            MaxWorkerWipLimit = Tasks.Count;
            lastRunBurndownTicks.TryAdd(Guid.NewGuid(), new BurndownTick(-1, 0, Tasks.Count));
        }

        public void CreateRandomTasks()
        {
            Reset();
            if (Tasks == null) Tasks = new ObservableCollection<Task>();
            Tasks.Clear();
            var r = new Random();
            for (int i = 1; i <= NumberOfTasks; i++)
            {
                var analysis = r.Next(1, 10);
                var dev = r.Next(1, 10);
                var test = r.Next(1, 10);
                Tasks.Add(new Task(i, analysis, dev, test));
            }
            MaxConstraint = NumberOfTasks;
            TotalWorkGenerated = (int)Tasks.Sum(t => t.TotalWork);
            MaxWorkerWipLimit = Tasks.Count;
            lastRunBurndownTicks.TryAdd(Guid.NewGuid(), new BurndownTick(-1, 0, Tasks.Count));
        }

        private void Reset()
        {
            //MessageBox.Show("HI");
            ChartVisibility = Visibility.Collapsed;
            stepper.Reset();
            lastRunBurndownTicks = new ConcurrentDictionary<Guid, BurndownTick>();
            BurndownTicks.Clear();
            timer.Stop();
            AverageCompletionTime = 0;
            MinTime = 0;
            WastedManDays = 0;
            StandardDeviation = 0;
            Variance = 0;
            MaxTime = 0;
            Day = 0;
        }

        public bool IsComplete
        {
            get { return Tasks.All(t => t.Complete); }
        }

        private void Complete()
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += (o, s) => { if (!IsComplete) { Step(); } else { timer.Stop(); } };
            timer.Start();
        }

        public int TestCount
        {
            get { return Tasks.Count(t => t.Position == Column.Test); }
        }

        public int DevCount
        {
            get { return Tasks.Count(t => t.Position == Column.Dev); }
        }

        public int AnalysisCount
        {
            get { return Tasks.Count(t => t.Position == Column.Analysis); }
        }

        private void Step()
        {
            Day++;

            stepper.Step(this);

            ThreadPool.QueueUserWorkItem(new WaitCallback(CalculateStats));
        }

        private void CalculateStats(object o)
        {
            var complete = Tasks.Where(t => t.Complete);
            if (complete.Any())
            {
                MinTime = complete.Min(t => t.CompletionTime);
                MaxTime = complete.Max(t => t.CompletionTime);
                AverageCompletionTime = complete.Average(t => t.CompletionTime);
                StandardDeviation = StdDev(Tasks.Where(t => t.Complete)
                    .Select(t => (double)t.CompletionTime));
            }

            var totalRemainingTasks = Tasks.Count(t => !t.Complete);
            foreach (var task in complete)
            {
                if (lastRunBurndownTicks.Any(t => t.Value.TaskId == task.Id)) continue;
                lastRunBurndownTicks.TryAdd(Guid.NewGuid(), new BurndownTick(task.Id, Day, totalRemainingTasks));
            }

            if (Tasks.All(t => t.Complete))
            {
                lock (this)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        BurndownTicks.Clear();
                        var cleanTicks = lastRunBurndownTicks.DistinctBy(t => t.Value.StepNumber)
                            .OrderBy(t => t.Value.StepNumber).ToList();
                        foreach (var item in cleanTicks)
                        {
                            BurndownTicks.Add(item.Value);
                        }

                        ChartVisibility = Visibility.Visible;
                    });
                }
            }
        }

        public double StdDev(IEnumerable<double> values)
        {
            double ret = 0;
            int count = values.Count();
            if (count > 1)
            {
                double avg = values.Average();
                double sum = values.Sum(d => (d - avg) * (d - avg));
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }

        private bool CanStep()
        {
            return !IsComplete;
        }

        #region INPC

        /// <summary>
        /// The <see cref="MaxWorkerWipLimit" /> property's name.
        /// </summary>
        public const string MaxWorkerWipLimitPropertyName = "MaxWorkerWipLimit";

        private int maxWorkerWipLimit = 1;

        /// <summary>
        /// Sets and gets the MaxWorkerWipLimit property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxWorkerWipLimit
        {
            get
            {
                return maxWorkerWipLimit;
            }

            set
            {
                if (maxWorkerWipLimit == value)
                {
                    return;
                }

                RaisePropertyChanging(MaxWorkerWipLimitPropertyName);
                maxWorkerWipLimit = value;
                RaisePropertyChanged(MaxWorkerWipLimitPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="WorkerWipLimit" /> property's name.
        /// </summary>
        public const string WorkerWipLimitPropertyName = "WorkerWipLimit";

        private int workerWipLimit = 3;

        /// <summary>
        /// Sets and gets the WorkerWipLimit property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int WorkerWipLimit
        {
            get
            {
                return workerWipLimit;
            }

            set
            {
                if (workerWipLimit == value)
                {
                    return;
                }

                RaisePropertyChanging(WorkerWipLimitPropertyName);
                workerWipLimit = value;
                RaisePropertyChanged(WorkerWipLimitPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="TotalWorkGenerated" /> property's name.
        /// </summary>
        public const string TotalWorkGeneratedPropertyName = "TotalWorkGenerated";

        private int totalWorkGenerated = 0;

        /// <summary>
        /// Sets and gets the TotalWorkGenerated property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TotalWorkGenerated
        {
            get
            {
                return totalWorkGenerated;
            }

            set
            {
                if (totalWorkGenerated == value)
                {
                    return;
                }

                RaisePropertyChanging(TotalWorkGeneratedPropertyName);
                totalWorkGenerated = value;
                RaisePropertyChanged(TotalWorkGeneratedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MaxConstraint" /> property's name.
        /// </summary>
        public const string MaxConstraintPropertyName = "MaxConstraint";

        private int maxConstraint = 10;

        /// <summary>
        /// Sets and gets the MaxConstraint property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxConstraint
        {
            get
            {
                return maxConstraint;
            }

            set
            {
                if (maxConstraint == value)
                {
                    return;
                }

                RaisePropertyChanging(MaxConstraintPropertyName);
                maxConstraint = value;
                RaisePropertyChanged(MaxConstraintPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MinTime" /> property's name.
        /// </summary>
        public const string MinTimePropertyName = "MinTime";

        private int minTime = 0;

        /// <summary>
        /// Sets and gets the MinTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MinTime
        {
            get
            {
                return minTime;
            }

            set
            {
                if (minTime == value)
                {
                    return;
                }

                RaisePropertyChanging(MinTimePropertyName);
                minTime = value;
                RaisePropertyChanged(MinTimePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MaxTime" /> property's name.
        /// </summary>
        public const string MaxTimePropertyName = "MaxTime";

        private int maxTime = 0;

        /// <summary>
        /// Sets and gets the MaxTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxTime
        {
            get
            {
                return maxTime;
            }

            set
            {
                if (maxTime == value)
                {
                    return;
                }

                RaisePropertyChanging(MaxTimePropertyName);
                maxTime = value;
                RaisePropertyChanged(MaxTimePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="AverageCompletionTime" /> property's name.
        /// </summary>
        public const string AverageCompletionTimePropertyName = "AverageCompletionTime";

        private double averageCompletionTime = 0D;

        /// <summary>
        /// Sets and gets the AverageCompletionTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double AverageCompletionTime
        {
            get
            {
                return averageCompletionTime;
            }

            set
            {
                if (averageCompletionTime == value)
                {
                    return;
                }

                RaisePropertyChanging(AverageCompletionTimePropertyName);
                averageCompletionTime = value;
                RaisePropertyChanged(AverageCompletionTimePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="WastedManDays" /> property's name.
        /// </summary>
        public const string WastedManDaysPropertyName = "WastedManDays";

        private int wastedManDays = 0;

        /// <summary>
        /// Sets and gets the WastedManDays property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int WastedManDays
        {
            get
            {
                return wastedManDays;
            }

            set
            {
                if (wastedManDays == value)
                {
                    return;
                }

                RaisePropertyChanging(WastedManDaysPropertyName);
                wastedManDays = value;
                RaisePropertyChanged(WastedManDaysPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="NumberOfTasks" /> property's name.
        /// </summary>
        public const string NumberOfTasksPropertyName = "NumberOfTasks";

        private int numberOfTasks = 15;

        /// <summary>
        /// Sets and gets the NumberOfTasks property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int NumberOfTasks
        {
            get
            {
                return numberOfTasks;
            }

            set
            {
                if (value == 0)
                {
                    value = 1;
                }
                if (numberOfTasks == value)
                {
                    return;
                }

                RaisePropertyChanging(NumberOfTasksPropertyName);
                numberOfTasks = value;
                RaisePropertyChanged(NumberOfTasksPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="EnableSwarming" /> property's name.
        /// </summary>
        public const string EnableSwarmingPropertyName = "EnableSwarming";

        private bool? enableSwarming = false;

        /// <summary>
        /// Sets and gets the EnableSwarming property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool? EnableSwarming
        {
            get
            {
                return enableSwarming;
            }

            set
            {
                if (enableSwarming == value)
                {
                    return;
                }

                RaisePropertyChanging(EnableSwarmingPropertyName);
                enableSwarming = value;
                RaisePropertyChanged(EnableSwarmingPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Day" /> property's name.
        /// </summary>
        public const string DayPropertyName = "Day";

        private int day = 0;

        /// <summary>
        /// Sets and gets the Day property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int Day
        {
            get
            {
                return day;
            }

            set
            {
                if (day == value)
                {
                    return;
                }

                RaisePropertyChanging(DayPropertyName);
                day = value;
                RaisePropertyChanged(DayPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="TeamSize" /> property's name.
        /// </summary>
        public const string TeamSizePropertyName = "TeamSize";

        private int teamSize = 3;

        /// <summary>
        /// Sets and gets the TeamSize property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TeamSize
        {
            get
            {
                return teamSize;
            }

            set
            {
                if (teamSize == value)
                {
                    return;
                }

                RaisePropertyChanging(TeamSizePropertyName);
                teamSize = value;
                RaisePropertyChanged(TeamSizePropertyName);
                MaxWorkerWipLimit = Tasks.Count;
            }
        }

        /// <summary>
        /// The <see cref="AnalysisWIP" /> property's name.
        /// </summary>
        public const string AnalysisWIPPropertyName = "AnalysisWIP";

        private int analysisWIP = 1;

        /// <summary>
        /// Sets and gets the AnalysisWIP property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int AnalysisWIP
        {
            get
            {
                return analysisWIP;
            }

            set
            {
                if (analysisWIP == value)
                {
                    return;
                }

                RaisePropertyChanging(AnalysisWIPPropertyName);
                analysisWIP = value;
                RaisePropertyChanged(AnalysisWIPPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="DevWIP" /> property's name.
        /// </summary>
        public const string DevWIPPropertyName = "DevWIP";

        private int devWIP = 1;

        /// <summary>
        /// Sets and gets the DevWIP property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int DevWIP
        {
            get
            {
                return devWIP;
            }

            set
            {
                if (devWIP == value)
                {
                    return;
                }

                RaisePropertyChanging(DevWIPPropertyName);
                devWIP = value;
                RaisePropertyChanged(DevWIPPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="TestWIP" /> property's name.
        /// </summary>
        public const string TestWIPPropertyName = "TestWIP";

        private int testWIP = 1;

        /// <summary>
        /// Sets and gets the TestWIP property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TestWIP
        {
            get
            {
                return testWIP;
            }

            set
            {
                if (testWIP == value)
                {
                    return;
                }

                RaisePropertyChanging(TestWIPPropertyName);
                testWIP = value;
                RaisePropertyChanged(TestWIPPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ChartVisibility" /> property's name.
        /// </summary>
        public const string ChartVisibilityPropertyName = "ChartVisibility";

        private Visibility chartVisibility = Visibility.Collapsed;

        /// <summary>
        /// Sets and gets the ChartVisibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility ChartVisibility
        {
            get
            {
                return chartVisibility;
            }

            set
            {
                if (chartVisibility == value)
                {
                    return;
                }

                RaisePropertyChanging(ChartVisibilityPropertyName);
                chartVisibility = value;
                RaisePropertyChanged(ChartVisibilityPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LimitByTasksPerWorker" /> property's name.
        /// </summary>
        public const string LimitByTasksPerWorkerPropertyName = "LimitByTasksPerWorker";

        private bool? limitByTasksPerWorker = true;

        /// <summary>
        /// Sets and gets the LimitByTasksPerWorker property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool? LimitByTasksPerWorker
        {
            get
            {
                return limitByTasksPerWorker;
            }

            set
            {
                if (limitByTasksPerWorker == value)
                {
                    return;
                }

                RaisePropertyChanging(LimitByTasksPerWorkerPropertyName);
                limitByTasksPerWorker = value;
                RaisePropertyChanged(LimitByTasksPerWorkerPropertyName);
                LimitByColumn = !limitByTasksPerWorker;
                LimitByColumnVisibility = limitByColumn.Value ? Visibility.Visible : Visibility.Collapsed;
                LimitByTaskPerWorkerVisibility = limitByColumn.Value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// The <see cref="LimitByColumn" /> property's name.
        /// </summary>
        public const string LimitByColumnPropertyName = "LimitByColumn";

        private bool? limitByColumn = false;

        /// <summary>
        /// Sets and gets the LimitByColumn property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool? LimitByColumn
        {
            get
            {
                return limitByColumn;
            }

            set
            {
                if (limitByColumn == value)
                {
                    return;
                }

                RaisePropertyChanging(LimitByColumnPropertyName);
                limitByColumn = value;
                RaisePropertyChanged(LimitByColumnPropertyName);
                LimitByTasksPerWorker = !limitByColumn;
                LimitByColumnVisibility = limitByColumn.Value ? Visibility.Visible : Visibility.Collapsed;
                LimitByTaskPerWorkerVisibility = limitByColumn.Value ? Visibility.Collapsed : Visibility.Visible;
                if (LimitByColumn.Value)
                {
                    stepper = new StepByColumnLimit();
                }
            }
        }

        /// <summary>
        /// The <see cref="LimitByTaskPerWorkerVisibility" /> property's name.
        /// </summary>
        public const string LimitByTaskPerWorkerVisibilityPropertyName = "LimitByTaskPerWorkerVisibility";

        private Visibility limitByTaskPerWorkerVisibility = Visibility.Visible;

        /// <summary>
        /// Sets and gets the LimitByTaskPerWorkerVisibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility LimitByTaskPerWorkerVisibility
        {
            get
            {
                return limitByTaskPerWorkerVisibility;
            }

            set
            {
                if (limitByTaskPerWorkerVisibility == value)
                {
                    return;
                }

                RaisePropertyChanging(LimitByTaskPerWorkerVisibilityPropertyName);
                limitByTaskPerWorkerVisibility = value;
                RaisePropertyChanged(LimitByTaskPerWorkerVisibilityPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LimitByColumnVisibility" /> property's name.
        /// </summary>
        public const string LimitByColumnVisibilityPropertyName = "LimitByColumnVisibility";

        private Visibility limitByColumnVisibility = Visibility.Collapsed;

        /// <summary>
        /// Sets and gets the LimitByColumnVisibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility LimitByColumnVisibility
        {
            get
            {
                return limitByColumnVisibility;
            }

            set
            {
                if (limitByColumnVisibility == value)
                {
                    return;
                }

                RaisePropertyChanging(LimitByColumnVisibilityPropertyName);
                limitByColumnVisibility = value;
                RaisePropertyChanged(LimitByColumnVisibilityPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Variance" /> property's name.
        /// </summary>
        public const string VariancePropertyName = "Variance";

        private double variance = 0;

        /// <summary>
        /// Sets and gets the Variance property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Variance
        {
            get
            {
                return variance;
            }

            set
            {
                if (variance == value)
                {
                    return;
                }

                RaisePropertyChanging(VariancePropertyName);
                variance = value;
                RaisePropertyChanged(VariancePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="StandardDeviation" /> property's name.
        /// </summary>
        public const string StandardDeviationPropertyName = "StandardDeviation";

        private double standardDeviation = 0;

        /// <summary>
        /// Sets and gets the StandardDeviation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double StandardDeviation
        {
            get
            {
                return standardDeviation;
            }

            set
            {
                if (standardDeviation == value)
                {
                    return;
                }

                RaisePropertyChanging(StandardDeviationPropertyName);
                standardDeviation = value;
                RaisePropertyChanged(StandardDeviationPropertyName);
            }
        }

        #endregion
    }
}