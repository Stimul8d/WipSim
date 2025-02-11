Feature: Initial Simulation State
  As a user starting the simulation
  I want to see one task in the grid
  So I can understand how work flows

  Scenario: Default task state
    Given I load the simulation
    Then there should be exactly one task
    And it should be in the first column
    And it should have default properties