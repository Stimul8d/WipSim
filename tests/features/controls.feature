Feature: Control Sync
  As a simulation user
  I want controls to sync with the store
  So my changes affect the simulation

  Scenario Outline: Control updates sync to store
    Given I load the simulation
    When I change <control> to <value>
    Then the store should reflect that change

    Examples:
      | control     | value |
      | engineers   | 5     |
      | maxWip      | 3     |
      | arrivalRate | 7     |
      | taskSize    | 4     |