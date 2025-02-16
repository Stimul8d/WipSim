Feature: Simulation Timer
  As a user watching task flow
  I want to see time passing
  So I can track how long things take

  Scenario: Starting the timer
    Given I load the simulation
    When I click start
    Then the timer should start from 00:00
    And after 1 second it should show 00:01