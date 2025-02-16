Feature: Task Allocation
  As a simulation watcher
  I want engineers to pick up available tasks
  So work can start flowing

  Scenario: Engineer grabs next task
    Given I load the simulation
    And there is one engineer
    When I start the simulation
    Then they should pick up the first task