Feature: Task Progress
  As a simulation watcher
  I want tasks to progress once picked up  
  So I can see them moving through stages

  Scenario: Task progresses after allocation
    Given I load the simulation
    When I start the simulation
    And wait 2 seconds
    Then the first task should have 20% progress