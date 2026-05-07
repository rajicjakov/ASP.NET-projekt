# Sitemap

## Route Definitions

### Home Routes
- `/` → `HomeController.Index()` → View: `Views/Home/Index.cshtml`
- `/Privacy` → `HomeController.Privacy()` → View: `Views/Home/Privacy.cshtml`
- `/Error` → `HomeController.Error()` → View: `Views/Home/Error.cshtml` (route exists, but view file is not present in the current project tree)

### Browse Routes
- `/Browse` → `BrowseController.Browse(string searchTerm = "", string[] selectedDifficulties = null, string[] selectedTunings = null)` → View: `Views/Browse/Browse.cshtml`
- `/Browse/{id}` → `BrowseController.Details(int id)` → View: `Views/Browse/Details.cshtml`

### Editor Routes
- `/Editor/Create` → `EditorController.Create()` → View: `Views/Editor/Create.cshtml`

### Login Routes
- `/Login/Login` → `LoginController.Login()` → View: `Views/Login/Login.cshtml`

## Routing Notes
- `HomeController` uses explicit root routing via `[Route("")]` on the controller and action.
- `BrowseController` uses `[Route("[controller]")]` and `[Route("[controller]/{id}")]` for browse listing and details.
- `EditorController` and `LoginController` use `[Route("[controller]")]` so their actions map under `/Editor` and `/Login`.
- The app also defines a conventional default route in `Program.cs`:
  - `/{controller=Home}/{action=Index}/{id?}`
  - This means standard MVC URLs like `/Home/Index` and `/Home/Privacy` are also available in addition to the explicit routes above.
