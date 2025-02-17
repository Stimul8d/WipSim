Feature: Task timing metrics
    Tasks should track work time in 4-hour ticks

    Scenario: Measuring cycle time
        Given a task started 3 ticks ago
        When the task is completed  
        Then the cycle time should show >12 hours

    Scenario: Measuring lead time 
        Given a task created 5 ticks ago
        When it completes after 3 more ticks
        Then the lead time should show >32 hours

    Scenario: Realistic task durations
        Given a complex task
        When the task goes through the system
        Then it should take around 5 ticks (20 hours) to complete