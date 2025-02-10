Feature: Testing Framework
  As a coder who's been burned before
  I want to know my test framework actually works
  So that I don't waste hours debugging the wrong thing

  Scenario: Running a basic test
    Given I have written a test
    When I run the test suite
    Then it should actually do something useful