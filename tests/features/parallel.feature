Feature: Parallel task allocation
    Multiple workers should grab tasks immediately

    Scenario: Four workers and four tasks
        Given 4 workers and 4 tasks in backlog
        When the simulation starts
        Then all tasks should be allocated in the first tick

    Scenario: Respecting WIP limits
        Given 4 workers with max WIP of 2 each
        And 8 tasks in backlog  
        When the simulation starts
        Then all workers should have 2 tasks each