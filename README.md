
# Introduction
This is the webapi server for my Router.


[![License](https://poser.pugx.org/badges/poser/license.svg)](LICENSE) [![Build status](https://ci.appveyor.com/api/projects/status/6ishyo97cu16v9ys?svg=true)](https://ci.appveyor.com/project/youngytj/webapi)  

## Quick Start
* Download and install [node](https://nodejs.org/en/), [mongodb](https://www.mongodb.com/download-center?jmp=nav#community) and [.net core](https://www.microsoft.com/net/core)
* Run `cd src/WebApi` to change the work folder.
* Run `dotnet restore` to install the dependencies for C#.
* Run `npm install` to install the dependencies for node.
* Run `npm run dev` to start mongodb and the program.
* Open `http://localhost:5000/swagger/ui/index.html#/` to view the api.

## Test
* Run `dotnet restore tests/WebApi.Tests` to install the dependencies for C#.
* Run `dotnet test src/WebApi.Tests` to start tests.

## License

Copyright (c) Microsoft Corporation. All rights reserved.

Licensed under the [MIT](LICENSE) License.