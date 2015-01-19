using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WipSim.ViewModel
{
    public class StepByColumnLimit : IStep
    {
        private double dailyCapacity;
        public void Step(MainViewModel mainVm)
        {
            AssignTasks(mainVm);

            dailyCapacity = mainVm.TeamSize;
            var stepTasks = mainVm.Tasks.Where(t => (t.Position == Column.Analysis && t.LeftInAnalysis > 0)
                || (t.Position == Column.Dev && t.LeftInDev > 0)
                || (t.Position == Column.Test && t.LeftInTest > 0));

            var nonStepTasks = mainVm.Tasks.Where(t => t.Position != Column.Done).Except(stepTasks);

            foreach (var task in stepTasks)
            {
                if (dailyCapacity > 0)
                {
                    var count = stepTasks.Count();
                    var focus = 0D;
                    var utilizedTeamCount = mainVm.TeamSize;
                    //TODO: think about swarming here! 1 limits worker allocation
                    if (!mainVm.EnableSwarming.Value && mainVm.TeamSize > count)
                    {
                        mainVm.WastedManDays += mainVm.TeamSize - count;
                        utilizedTeamCount = count;
                    }
                    focus = (task.LeftInColumn >= 1 ? (double)utilizedTeamCount / count : task.LeftInColumn);

                    var remainingEffort = task.Work(focus);
                    dailyCapacity -= focus;
                    dailyCapacity += remainingEffort;
                }

                AssignTasks(mainVm);
            }

            foreach (var task in nonStepTasks)
            {
                task.DontWork();
            }
        }

        public void Reset()
        {
            //throw new NotImplementedException();
        }

        public void AssignTasks(MainViewModel mainVm)
        {
            var completeTest = mainVm.Tasks.Where(t => t.Position == Column.Test && t.LeftInTest <= 0);
            foreach (var test in completeTest)
            {
                test.MovePosition(Column.Done, mainVm.Day);
            }

            var completeDev = mainVm.Tasks.Where(t => t.Position == Column.Dev && t.LeftInDev <= 0);
            foreach (var dev in completeDev)
            {
                if (mainVm.TestCount >= mainVm.TestWIP) break;
                dev.MovePosition(Column.Test, mainVm.Day);
            }

            var completeAnalysis = mainVm.Tasks.Where(t => t.Position == Column.Analysis && t.LeftInAnalysis <= 0);
            foreach (var analysis in completeAnalysis)
            {
                if (mainVm.DevCount >= mainVm.DevWIP) break;
                analysis.MovePosition(Column.Dev, mainVm.Day);
            }

            var ready = mainVm.Tasks.Where(t => t.Position == Column.Backlog);
            foreach (var task in ready)
            {
                if (mainVm.AnalysisCount >= mainVm.AnalysisWIP) break;
                task.MovePosition(Column.Analysis, mainVm.Day);
            }
        }

    }
}
