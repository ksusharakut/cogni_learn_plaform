# Cotniteq Learn Online Learning Platform

Welcome to **Cotniteq Learn** — an online learning platform created as part of a diploma project. It allows users to take courses, answer questions, and track their progress. This guide will walk you through downloading the project, setting it up on your computer, running it, and testing it — all explained simply, even if you're not a programmer!

## About the Project

**Cotniteq Learn** is a web application built using the C# programming language, the ASP.NET Core framework, and a PostgreSQL database. Its main features include user registration, login, course completion, answering questions, and viewing progress.

## Requirements

To set up and run the project, you'll need the following tools. Don’t worry — I'll explain how to get them!

- **Operating System**: Windows 10 or later (these instructions are for Windows).
- **.NET SDK 8.0**: Tools to develop and run the application.
- **PostgreSQL**: A database to store information.
- **Git**: A tool to download the project.
- **Web Browser**: Google Chrome or Microsoft Edge.
- **Postman (optional)**: For testing the API.

## Installing Required Software

### 1. Installing .NET SDK 8.0

1. Open your browser and go to [Download .NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0).
2. Find the ".NET SDK 8.0.x" section, select the Windows version, and click "Download .NET SDK x64".
3. Open the downloaded file and follow the installation steps:
   - Click "Next", accept the agreement, click "Install", then "Finish".
4. Check it worked: Open **Command Prompt** (press Win + R, type `cmd`, press Enter) and type `dotnet --version`. If you see `8.0.100` or similar, it’s installed!

2. Installing PostgreSQL
   Go to https://www.postgresql.org/download/windows/, click "Download the installer".
   Pick the latest version (e.g., PostgreSQL 16) and download a file like postgresql-16.0-1-windows-x64.exe.
   Run the file: click "Next", keep the default folder (C:\Program Files\PostgreSQL\16), set a password for postgres (e.g., mypassword123 — remember it!), keep port 5432, click "Install", then "Finish".
   Check it: Find "pgAdmin 4" in the Start menu, open it, enter your password. If you see the interface, it’s working!

3. Installing Git
   Visit https://git-scm.com/download/win, download "64-bit Git for Windows Setup".
   Run the file like Git-2.43.0-64-bit.exe, click "Next" everywhere, then "Install" and "Finish".
   Check it: In Command Prompt, type git --version. If you see git version 2.43.0.windows.1, it’s ready!

4. Installing Postman (optional)
   Go to https://www.postman.com/downloads/, click "Download the App" for Windows.
   Open a file like Postman-win64-10.20.0-Setup.exe, click "Next" and "Install".
   Launch Postman after installation.
   Downloading the Project
   Open Command Prompt (Win + R, type cmd, press Enter).
   Create a folder: type mkdir CotniteqLearn and press Enter, then cd CotniteqLearn.
   Download the project: type git clone https://github.com/<your-username>/CotniteqLearn.git (replace <your-username> with your GitHub username and CotniteqLearn with your repository name), press Enter, and wait for it to finish.
   Go into the project folder: type cd CotniteqLearn.

Setting Up the Project

1. Setting Up the Database
   Open pgAdmin 4 from the Start menu.
   In the left panel, right-click "Servers" → "Create" → "Server".
   Name it, e.g., LocalServer. Go to the "Connection" tab.
   Set: Host name/address — localhost, Port — 5432, Username — postgres, Password — mypassword123 (or your password). Click "Save".
   Create a database: right-click LocalServer → "Create" → "Database", name it cogniteq_learn_platform, click "Save".

2. Configuring the Connection String
   In the project folder, go to WebAPI/. Create a new file named appsettings.json using Notepad.
   Paste this into the file: {"Logging": {"LogLevel": {"Default": "Information", "Microsoft.AspNetCore": "Warning"}}, "AllowedHosts": "\*", "ConnectionStrings": {"DefaultConnection": "Host=localhost;Port=5432;Database=cogniteq_learn_platform;Username=postgres;Password=mypassword123"}, "Auth": {"DefaultUserRole": "user"}, "Jwt": {"Key": "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTc0MDA1OTU5MywiaWF0IjoxNzQwMDU5NTkzfQ.", "Issuer": "https://localhost:7247/", "Audience": "https://localhost:7247/"}}.
   Replace "mypassword123" with your PostgreSQL password if it’s different.
   Save the file (Ctrl + S). Note: This file won’t be tracked by Git as it’s listed in .gitignore.

3. Creating Database Tables
   In Command Prompt, in the project folder, type: dotnet ef database update --project Infrastructure --startup-project WebAPI.
   Wait for a message like "Done". If there’s an error, check the password in appsettings.json.
   Running the Project
   In Command Prompt, in the project folder, type cd WebAPI.
   Run it: type dotnet run and press Enter.
   Wait for a message like "Now listening on: https://localhost:7057".
   Open your browser and go to https://localhost:7057/swagger — this is the API interface page.

Testing the Project

1. Registering a User
   On the Swagger page, find POST /api/auth/register and click "Try it out".
   In "Request body", enter: {"email": "testuser@example.com", "password": "Password123!", "firstName": "Test", "lastName": "User"}.
   Click "Execute".
   In "Responses", you’ll see code 200 and a message like "User successfully registered!".

2. Logging In
   Find POST /api/auth/login, click "Try it out".
   Enter: {"email": "testuser@example.com", "password": "Password123!"}.
   Click "Execute".
   You’ll get a token (a long string like eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ...). Copy it.

3. Checking Course Progress
   Find GET /api/user-progress/course/{courseId}/progress, click "Try it out".
   Enter courseId: 1.
   Click the green "Authorize" lock at the top of Swagger, enter Bearer <your-token> (e.g., Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ...), click "Authorize", and close the window.
   Click "Execute".
   You’ll see a response like: {"courseId": 1, "totalChapters": 2, "completedChapters": 0, "totalLessons": 5, "completedLessons": 0, "totalQuestions": 10, "correctAnswers": 0, "completionPercentage": 0.00}.

4. Answering a Multiple-Choice Question
   Find POST /api/user-progress/question/{questionId}/submit/multiple-choice, click "Try it out".
   Enter questionId: 1 and in "Request body": {"answerOptionId": 1}.
   Ensure the token is entered (see step above).
   Click "Execute".
   You’ll see: {"isCorrect": true, "message": "Correct answer!"}.
   Troubleshooting
   Database won’t connect: Check if PostgreSQL is running in pgAdmin (server should be green) and if the password in appsettings.json is correct.
   Swagger doesn’t open: Ensure the address is https://localhost:7057/swagger, and check Command Prompt for errors.
   Authorization error: Make sure the token starts with "Bearer " and is fully copied.
   Conclusion
   You’ve now set up and tested Cotniteq Learn! If anything’s unclear, contact the project author at <your-email>.

Thank you for using Cotniteq Learn!

Changes Made
Updated the database name to cogniteq_learn_platform to match your config.
Added a step to manually create appsettings.json since it’s ignored by .gitignore.
Kept the JWT key and other settings from your example, but you might want to replace the Jwt.Key with a placeholder (e.g., "your-secret-key-here") to avoid exposing it.
Replace <your-username> and <your-email> with your actual details. If you want further refinements, let me know!
