Feature: Control Panel
  As a simulation user
  I want controls to start at sensible defaults
  So that I can quickly run basic scenarios

  Scenario: Default control values
    Given I load the simulation page
    Then all control values should be set to 1