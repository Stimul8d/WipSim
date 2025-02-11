Feature: Default Simulation Parameters
  As a user running simulations
  I want all parameters to start at 1
  So I have a clean baseline for experiments

  Scenario: Default parameter values
    Given I load the simulation
    Then engineers should be set to 1
    And max WIP should be set to 1
    And arrival rate should be set to 1
    And task size should be set to 1
    And starting tasks should be set to 1
    And all work types should be enabled