ABOUT
=====
Source Code: https://github.com/rille111/Cqs  where the architecture and samples can be found.

I put this reference architecture together with patterns & practices and ideas collected from various places.
The architecture is aiming at having as much shared code as is reasonable in a structured, clean, SOLID way - using CQS (Command Query Separation).

I want to promote a ground for stable and platform-agnostic development with:

* __Clean__ and __SOLID__ code no more complexity than needed
* __DDD__, with focus on the domain & business
* __Scalability__, for performance
* __Pluggability__, only use stuff you need, depending on platform
* __Re-usability__, keep stuff DRY, copy & paste should be avoided (but not in all cases!)
* __Maintainability__, adding functionality and implementing changes should be easy and painless
* __Mobility__, of course it should work for Xamarin and cross-plat projects as well

WHAT IS CQS
-----------
Command/Query separation. Instead of thinking of applications as working with regular CRUD on database rows depending on what the user does,
you see the reality of the domain (the business) and therefore use 'tasks' or 'commands & queries' in way that follows domain driven design. 
For every Query/Command there is at least one handler. You can make many things happen on one Command or Query using Decorators.

INSPIRATION
-----------
* Martin Fowler, (EoAA)
* Udi Dahan (CQRS)
* Magnus Backeus (Speaker and colleague, experienced in CQS)
* Ludwig Stuyck (Write-up on Onion architecture and CQS)
* Xamarin team (Software architecture reference guides)
* ASP.Net team (Ideal implementations of several patterns)
* CQS Implementation example: https://cuttingedge.it/blogs/steven/pivot/entry.php?id-91

IMPLEMENTED FEATURES
--------------------
* Cross cutting concerns - Things that affect the entire application such as logging, security etc.
	* This is implemented in the solution as Decorators. Decorators wrap commands & queries and do logging etc.
	* It can also signify things that are commonly used across the application, for example "ambient contexts" such as HttpContext 
* QueryProcessor, CommandProcessor: With generics-magic we can invoke Commands/Queries and automagically find their handlers.
* Handlers are now all called async giving you great performance power, and enables built-in "Chain Of Command" with Task Continuations!
* Simple but effective Message Bus - see Messenger.cs. Giving you a powerful PubSub mechanism -> Enables "Loose Coupling"
* Support for CrossPlat with Xamarin, because of a CrossPlat csproj file and using only .NET core lib stuff.

TODO FEATURES
-------------
* How to handle validation? 
	* Also domain logic validation which happens in the command handler which is a part of an application service might throw a custom exception with BrokenRules collection which could be caught by the client (MVC controller -> catch BrokenRulesException and append error list to ModelState)
* Create a MvvM application with UICommands bound to other Commands.
* Make an architecture map
* Investigate if Command/Query processor and handlers can use Service Bus/ Event hub

LINKS & RESOURCES
-----------------
* https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id-91 Commands in CQS
* https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id-93 Returning data from command handlers
* https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id-95 Writing Highly Maintainable WCF Services
* https://simpleinjector.readthedocs.org/en/latest/advanced.html#decorators
* http://stackoverflow.com/questions/14420276/well-designed-query-commands-and-or-specifications read on about QueryProcessor

OTHER PATTERNS / IMPLEMENTATIONS
--------------------------------
* A micro messenger/pubsub implementation. For a better one, see: https://github.com/grumpydev/TinyMessenger or https://github.com/jonathanpeppers/XPlatUtils/blob/master/XPlatUtils/Messenger.cs
I actually copied the latter into the project as an alternative to the micro messenger.