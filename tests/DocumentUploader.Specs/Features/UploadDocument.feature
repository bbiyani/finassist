Feature: Upload Document
  Scenario: Successful document upload
    Given I am an authenticated user
    When I upload a valid PDF file
    Then the file should be stored in blob storage
    And a document.uploaded event should be published
