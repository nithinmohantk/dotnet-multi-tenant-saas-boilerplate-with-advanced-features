Feature: Tenant Creation
    As a system administrator
    I want to create a new tenant
    So that I can onboard new customers to the SaaS platform

    Scenario: Successfully create a new tenant
        Given I have a valid tenant creation request with the following details:
            | Name        | Identifier | AdminEmail      | Plan |
            | Test Tenant | test-001   | admin@test.com  | Pro  |
        When I submit the create tenant request
        Then the tenant should be created successfully
        And the tenant ID should be a valid GUID
