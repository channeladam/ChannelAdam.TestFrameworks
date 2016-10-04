Feature: TextTesting

Scenario: Should have no differences listed when comparing two identical text samples
Given two text samples with the same words
When the two text samples are compared
Then the two text samples are treated as the same

Scenario: Should error when two text samples have different words
Given two text samples with the different words
When the two text samples are compared
Then the two text samples are treated as different

Scenario: Should allow overriding of differences via a delegate method when two text samples have different words
Given two text samples with the different words
And a delegate method exists to filter out the changes
When the two text samples are compared
Then the two text samples are treated as the same

Scenario: Should allow overriding of differences via an event when two text samples have different words
Given two text samples with the different words
And an event handler exists to filter out the changes
When the two text samples are compared
Then the two text samples are treated as the same
