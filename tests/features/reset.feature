Feature: Simulation reset
    Reset should return the sim to initial state

    Scenario: Reset from running state
        Given a simulation with multiple tasks and workers
        When tasks are in progress and I reset
        Then all tasks should be cleared
        And time should be 00:00
        And sim should be stopped

    Scenario: Reset settings preserved 
        Given I change task arrival rate to 3
        When I reset the simulation
        Then arrival rate should still be 3