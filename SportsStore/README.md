### SportsStore project

Folder structure:
- **Models** - This folder will contain the data model and the classes that provide access to the data in
the application’s database.
- **Controllers** - This folder will contain the controller classes that handle HTTP requests.
- **Views** - This folder will contain all the Razor files, grouped into separate subfolders.
- **Views/Home** - This folder will contain Razor files that are specific to the Home controller.
- **Views/Shared** - This folder will contain Razor files that are common to all controllers.
- **Infrastructure** - This folder will contain classes, that deliver the plumbing for an application but
that are not related to the application’s main functionality (not necessary to use in other projects).
- **Migrations** - This folder will contain migration classes generated by EF, which are a way to manage changes to the data model and apply those changes to the database schema.
- **Components** - This folder will contain view components, which are UI parts of the applicaton, that are independent from the controllers.
- **Pages** - This folder will contain Razor Pages, which are self-contained units with their own route, model, and view logic.