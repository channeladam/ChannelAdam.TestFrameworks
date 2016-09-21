Feature: XmlTesting

Scenario: Should treat two xml samples as equal - when they have the same namespace urls but different namespace prefixes
Given two xml samples with the same namespace urls but different namespace prefixes
When the two xml samples are compared
Then the two xml samples are treated as equal

Scenario: Should treat two xml samples as equal - when they have the same child nodes but in a different order
Given two xml samples with the same child nodes but in a different order
When the two xml samples are compared
Then the two xml samples are treated as equal

Scenario: Should error when two xml samples have different elements
Given two xml samples with the different elements
When the two xml samples are compared
Then the two xml samples are treated as different

Scenario: Should error when two xml samples have the same elements but a different value
Given two xml samples with the same elements but a different value
When the two xml samples are compared
Then the two xml samples are treated as different
