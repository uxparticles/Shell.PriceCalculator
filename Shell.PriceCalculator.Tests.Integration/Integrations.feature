Feature: Integrations
	

Scenario: Empty Basket
	Given I have an empty basket to price
	When I send the basket to the pricing Engine
	Then the result should contain the following messages
	| message             |
	| Basket is empty |
	 

Scenario: Basket With No Offers
	Given I have a basket to price with the following items
	| name  |
	| Milk  |
	| Milk  |
	| Beans |
	| Bread |
	When I send the basket to the pricing Engine
	Then the result should contain the following messages
	| message               |
	| (No offers available) |
	And The subtotal should be equal to 3.55
	And the total should be equal to 3.55

Scenario: Basket With 1 Offer
	Given I have a basket to price with the following items
	| name  |
	| Apple |
	| Apple |
	When I send the basket to the pricing Engine
	Then the result should contain the following messages
	| message               |
	|  apple 10.00% off: -£0.2 |
	And The subtotal should be equal to 2
	And the total should be equal to 1.8

Scenario: Basket With 1 Conditional Offer
	Given I have a basket to price with the following items
	| name  |
	| Beans |
	| Beans |
	| Bread |
	| Bread |
	When I send the basket to the pricing Engine
	Then the result should contain the following messages
	| message                                    |
	| 1 breads received 50.00% discount: -£0.150 |
	And The subtotal should be equal to 1.9
	And the total should be equal to 1.75

Scenario: Basket With multiple Conditional Offers
	Given I have a basket to price with the following items
	| name  |
	| Beans |
	| Beans |
	| Beans |
	| Beans |
	| Bread |
	| Bread |
	When I send the basket to the pricing Engine
	Then the result should contain the following messages
	| message                                     |
	| All bread received 50.00% discount: -£0.300 |
	And The subtotal should be equal to 3.2
	And the total should be equal to 2.9

Scenario: Basket With unpriced items
	Given I have a basket to price with the following items
	| name  |
	| Beans |
	| Milk  |
	| Mango |	
	When I send the basket to the pricing Engine
	Then the result should contain the following messages
	| message                             |
	| Price for item Mango is unavailable |
	And The subtotal should be equal to 1.95
	And the total should be equal to 0