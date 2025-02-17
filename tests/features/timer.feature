Feature: Simulation Timer
    Track work days to show realistic task timelines

    Scenario: Starting the timer
        Given I load the simulation
        When I click start
        Then the timer should start from D0:00
        And after 1 second it should show D0:04