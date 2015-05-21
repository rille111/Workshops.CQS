GETTING STARTED
===============

Either download, clone or create a submodule for this repo
==========================================================
\Core\ contains the projects you should directly add reference to. Actually only Cqs.Infrastructure OR the Xamarin counterpart.
The other projects are really only tests and examples, so you can copy/paste and get started quickly.

Copy & Paste
============
* Copy \2. Core\<all projects> - Add references to at least the infrastructure project. 
The persistence project is handy if you play around or need a cached in-memory repo.
* Copy \3. Demo\Cqs.Domain\ - either copy or get inspired by the classes here into your own domain/business layer.
* Copy \3. Demo\Cqs.Presentation.Console\Utils\* - copy classes into probably each of your UI projects. 
	* Because IoC.cs is dependent on SimpleInjector, you may need to edit that class.
	* The logger only works for Console, you may want to change logger, edit or just delete it.
* \3. Demo\Cqs.Presentation.Console\Boot\IoC.cs - copy and edit it according to what you need and what IoC container you use.

Start coding
============
Get inspiration from * \3. Demo\Cqs.Presentation.Console\program.cs, all examples are currently here.