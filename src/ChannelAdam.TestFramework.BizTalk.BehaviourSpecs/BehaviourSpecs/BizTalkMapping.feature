Feature: BizTalkMapping

Scenario: Should map successfully from xml to xml
Given xml input for an xml to xml map
When the xml to xml map is performed
Then the xml output from an xml to xml map is correct

Scenario: Should map successfully from xml to flat file
Given xml input for an xml to flat file map
When the xml to flat file map is performed
Then the flat file output from the xml to flat file map is correct

Scenario: Should map successfully from flat file to xml
Given flat file input for a flat file to xml map
When the flat file to xml map is performed
Then the xml output from the flat file to xml map is correct

Scenario: Should map successfully from flat file to flat file
Given flat file input for a flat file to flat file map
When the flat file to flat file map is performed
Then the flat file output from a flat file to flat file map is correct
