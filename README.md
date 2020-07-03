# ShoppingCart

## Description

ShoppingCart is the CA project for NUS-ISS SA .Net Core module.

It's an application that supports basic shopping functionalities

Using ASP.Net Core and Razor, typical tractional SSR application.
Ajax is only used for dynamic content rendering, not an example for modern web development which means separate frontend and backend, in other words, nesting view logic inside controller

## Install

After cloning the repository from github, run following command to restore dependencies

```bash
dotnet restore
```

Run following command to start local server:

```bash
dotnet run
```

## Features

-   Customized double-hashed password middleware to secure the website
-   Use Razor `@RenderSection` to define the skeleton of the view pages (Component-based UI) -- Basics of modern framework like React
