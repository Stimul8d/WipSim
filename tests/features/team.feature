Feature: Default Simulation Parameters
  As a new user
  I want sensible defaults
  So my first run doesn't go mental

  Scenario: Default parameter values
    Given I load the simulation 
    Then there should be one worker
    And max WIP should be set to 1
    And arrival rate should be set to 1
    And task size should be set to 1
    And starting tasks should be set to 1
    And all work types should be enabled