using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WipSim.ViewModel
{
    public class StepByWorkerLimit : IStep
    {
        List<TaskAssignment> taskAssignments = new List<TaskAssignment>();

        public void Reset()
        {
            taskAssignments = new List<TaskAssignment>();
        }

        public void Step(MainViewModel mainVm)
        {
            AssignTasks(mainVm, mainVm.WorkerWipLimit);
            Work(mainVm);
            AssignTasks(mainVm, mainVm.WorkerWipLimit);
        }

        private void Work(MainViewModel mainVm)
        {
            foreach (var worker in Enumerable.Range(0, mainVm.TeamSize))
            {
                var assignments = taskAssignments.Where(t => t.WorkerIx == worker).ToList();
                if (assignments.Count() == 0) mainVm.WastedManDays++;
                double focus = 1D / assignments.Count();
                foreach (var assignment in assignments.ToArray())
                {
                    assignment.Task.Work(focus);
                    if (Math.Round(assignment.Task.LeftInColumn, 2) <= 0)
                    {
                        assignment.Task.MoveRightOne(mainVm.Day);
                        if (assignment.Task.Position == Column.Done)
                        {
                            taskAssignments.Remove(assignment);
                        }
                    }
                }
            }
        }

        public void AssignTasks(MainViewModel mainVm, int maxPerWorker)
        {
            foreach (var worker in Enumerable.Range(0, mainVm.TeamSize))
            {
                var assignments = taskAssignments.Where(a => a.WorkerIx == worker);

                maxPerWorker -= assignments.Count();

                var workersTasks = mainVm.Tasks.Where(t => t.Position == Column.Backlog
                    && !taskAssignments.Any(a=>a.Task.Id == t.Id)).Take(maxPerWorker);
                foreach (var task in workersTasks)
                {
                    taskAssignments.Add(new TaskAssignment { WorkerIx = worker, Task = task });
                    if (task.LeftInColumn == 0) task.MoveRightOne(mainVm.Day);
                }
            }
        }
    }

    public class TaskAssignment
    {
        public int WorkerIx { get; set; }
        public Task Task { get; set; }
    }
}

