![DevBook Client Deploy](https://github.com/shindy001/DevBook.Web/actions/workflows/deploy-devbook-webapp.yml/badge.svg)

# DevBook.Web

DevBook.Web is an application for experimental purposes. Hosted at [https://ambitious-sky-0c0191303.4.azurestaticapps.net/](https://ambitious-sky-0c0191303.4.azurestaticapps.net/).
<br><br>
Uses [DevBookServer](https://github.com/shindy001/devbook-server) webapi as backend.

## Current state of Features (more to come)
- [x] Authentication (.net identity)
  - Token based
  - Auto refresh on expiration
  - Registration (no email confirmation at least for now)
- [x] Time Management (for signed in users only)
  - Time tracking by creating a task or running a timer
  - Projects to enable settings like hourly rate, task assigning and earnings computation
- [x] Sudoku
  - Standard segmented board
  - Uses remote api to get a new board
  - Increment/Decrement numbers by mouse clicks
  - Can be auto solved by backtracking algorithm (cheating)
  - Board state is saved to local storage on navigation change
  - Solution can be validated (incorrent numbers are highlighted)
- [ ] Time Management analytics - time tracking / earnings graphs
- [ ] Time Management exports - export to excel stylesheet
- [ ] Server integration tests
- [ ] CI tests pipeline

![DevBook_Dashboard_info](https://github.com/shindy001/DevBook.Web/assets/23438364/7c60d4b9-d1ea-43b8-af1b-cc1aef513601)
![DevBook_Sudoku_info](https://github.com/shindy001/DevBook.Web/assets/23438364/2c921cd4-f286-4295-b4f3-d341c61ee989)

## Technology stack
  - .NET v8
  - Blazor (WASM)
  - MudBlazor (as a component lib)
  - Tailwind (css lib)
  - Azure Static Web Apps (WASM hosting)

## Dev Requirements
- `Visual Studio` or `VSCode with C# Dev Kit`
- .net 8 SDK
- Tailwind - cli executable added to env path (client debug starts tailwind watch to hot generate / reload css), [https://tailwindcss.com/blog/standalone-cli](https://tailwindcss.com/blog/standalone-cli)
- Blazor WASM (ASP.net and web development workload)

## Deployment
Deployed is handled by ```github actions (manually)```. WebApp is deployed to [Azure Static Web Apps](https://azure.microsoft.com/en-gb/products/app-service/static).
## How to run
1. Open DevBook.Web.sln
1. Select DevBook.Web.Client.WASM configuration and start "https" configuration (if run as debug, this will also start tailwind watch in terminal to hot reload tailwind css - this requires to have tailwind cli executable in env path)
