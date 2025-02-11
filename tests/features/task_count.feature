Feature: Task Count Control
  As a user setting up the simulation
  I want to see my selected number of tasks immediately
  So I can verify the starting state

  Scenario: Changing starting task count
    Given I load the simulation
    When I change starting tasks to 5 
    Then there should be exactly 5 tasks in the grid