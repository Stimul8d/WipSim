Feature: Task Completion
  As a simulation watcher
  I want workers to move to new tasks when one is done
  So we can see actual flow not just one task per worker

  Scenario: Worker finishes task and moves on
    Given I load the simulation with 2 tasks
    When I start the simulation
    And wait 10 seconds
    Then both tasks should be complete