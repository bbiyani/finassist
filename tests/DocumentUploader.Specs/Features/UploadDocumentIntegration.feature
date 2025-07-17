Feature: Document Upload Integration
  Scenario: Uploading a document through the real API
    When I POST a PDF file to the upload API
    Then the API should respond with 200 OK
