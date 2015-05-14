ABOUT

This my reference architecture put together with patterns & practices and ideas collected from various places.
The architecture is aiming at having as much shared code as reasonable in a structured, clean, SOLID way - using Command Query Separation.

I want to promote a ground for stable platform agnostic development with:

* Clean and SOLID code, no more complexity than needed
* DDD, with focus on the domain & business
* Scalability, for performance
* Pluggability, you should be able to persist stuff as you need, depending on platform
* Re-usability, copy & paste should be avoided (but not in all cases!)
* Maintainability, adding functionality and implementing changes should be easy

WHAT IS CQS

Command/Query separation. Instead of thinking of applicatiopns as working with regular CRUD on database rows depending on what the user does,
you see the reality of the domain (the business) and thefore use 'tasks' or 'commands & queries' in way that follows domain driven design. 
For every Query/Command there is at least one handler. You can make many things happen on one Command using Decorators.

INSPIRATION

* Martin Fowler, (EoAA)
* Udi Dahan (CQRS)
* Magnus Backeus (Speaker, motivator and experienced in CQS)
* Ludwig Stuyck (Writeup on Onion architecture and CQS)
* Xamarin team (Software architecture reference guides)
* ASP.Net team (Ideal implementations of several patterns)
* CQS Implementation example: https://cuttingedge.it/blogs/steven/pivot/entry.php?id=91

TODO
' Create a logging decorator and cross cutting concern
' Create a QueryProcessor, CommandProcessor because the resulting code is so ugly
' Handlers should be called async
' Go async all the way
' Investigate if Command/Query processor and handlers can use Topics/Mediator
* How to handle validation? 
	* Also domain logic validation which happens in the command handler which is a part of an application service might throw a custom exception with BrokenRules collection which could be caught by the client (MVC controller -> catch BrokenRulesException and append error list to ModelState)
* Create a MvvM application with UICommands bound to other Commands.
* Make an architecture map
* Write blog
* Investigate if Command/Query processor and handlers can use Service Bus/ Event hub

LINKS & RESOURCES
* https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=91 Commands in CQS
* https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=93 Returning data from command handlers
* https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=95 Writing Highly Maintainable WCF Services
* https://simpleinjector.readthedocs.org/en/latest/advanced.html#decorators
* http://stackoverflow.com/questions/14420276/well-designed-query-commands-and-or-specifications read on about QueryProcessor
