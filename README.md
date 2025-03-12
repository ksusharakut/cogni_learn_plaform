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

### 2. Installing PostgreSQL

1. Go to [PostgreSQL Downloads](https://www.postgresql.org/download/windows/), click "Download the installer".
2. Download the latest version (e.g., PostgreSQL 16) and run the installer (e.g., `postgresql-16.0-1-windows-x64.exe`).
3. Set up PostgreSQL:
   - Install to the default folder (e.g., `C:\Program Files\PostgreSQL\16`).
   - Set a password for the **postgres** user (e.g., `mypassword123` — remember it!).
   - Keep the default port (`5432`).
4. Check it: Open **pgAdmin 4** from the Start menu, log in with your password. If you see the interface, it’s working!

### 3. Installing Git

1. Visit [Git for Windows](https://git-scm.com/download/win) and download "64-bit Git for Windows Setup".
2. Run the installer and click "Next" everywhere, then "Install" and "Finish".
3. Check it: In **Command Prompt**, type `git --version`. If you see `git version 2.43.0.windows.1`, it’s ready!

### 4. Installing Postman (optional)

1. Go to [Postman Downloads](https://www.postman.com/downloads/) and click "Download the App" for Windows.
2. Open the downloaded file (e.g., `Postman-win64-10.20.0-Setup.exe`) and click "Next" and "Install".
3. Launch Postman after installation.

## Downloading the Project

1. Open **Command Prompt** (Win + R, type `cmd`, press Enter).
2. Create a folder:
   ```bash
   mkdir CotniteqLearn
   cd CotniteqLearn
   ```

## Setting Up the Project

### 1. Setting Up the Database

Open pgAdmin 4 from the Start menu.

In the left panel, right-click "Servers" → "Create" → "Server".

Name it, e.g., LocalServer.

Go to the "Connection" tab and set:

Host name/address — localhost

Port — 5432

Username — postgres

Password — mypassword123 (or your password)

Click "Save".

Create a database:

Right-click LocalServer → "Create" → "Database"

Name it cogniteq_learn_platform

Click "Save"

### 2. Configuring the Connection String

In the project folder, go to WebAPI/.

Create a new file named appsettings.json using Notepad.

Paste this into the file:
{
"Logging": {
"LogLevel": {
"Default": "Information",
"Microsoft.AspNetCore": "Warning"
}
},
"AllowedHosts": "\*",
"ConnectionStrings": {
"DefaultConnection": "Host=localhost;Port=5432;Database=cogniteq_learn_platform;Username=postgres;Password=mypassword123"
},
"Auth": {
"DefaultUserRole": "user"
},
"Jwt": {
"Key": "your-secret-key-here",
"Issuer": "https://localhost:7247/",
"Audience": "https://localhost:7247/"
}
}

Replace mypassword123 with your PostgreSQL password if it’s different.

Save the file (Ctrl + S).

Note: This file won’t be tracked by Git as it’s listed in .gitignore.

### 3. Creating Database Tables

Open Command Prompt and navigate to the project folder.

Run:
dotnet ef database update --project Infrastructure --startup-project WebAPI

Wait for a message like "Done".

If there’s an error, check the password in appsettings.json.

### 4. Running the Project

In Command Prompt, navigate to the WebAPI folder:
cd WebAPI

Run the application:
dotnet run

Wait for a message like:
Now listening on: https://localhost:7057
Open your browser and go to https://localhost:7057/swagger — this is the API interface page.

## Testing the Project

### 1. Registering a User

On the Swagger page, find POST /api/auth/register and click "Try it out".

In "Request body", enter:
{
"email": "testuser@example.com",
"password": "Password123!",
"firstName": "Test",
"lastName": "User"
}

Click "Execute".

In "Responses", you’ll see code 200 and a message like "User successfully registered!".

### 2. Logging In

Find POST /api/auth/login, click "Try it out".

Enter:
{
"email": "testuser@example.com",
"password": "Password123!"
}

Click "Execute".

You’ll get a token (a long string like eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ...). Copy it.

### 3. Checking Course Progress

Find GET /api/user-progress/course/{courseId}/progress, click "Try it out".

Enter courseId: 1.

Click the green "Authorize" lock at the top of Swagger.

Enter Bearer <your-token>, e.g.,

Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ...

Click "Authorize" and close the window.

Click "Execute".

You’ll see a response like:

{
"courseId": 1,
"totalChapters": 2,
"completedChapters": 0,
"totalLessons": 5,
"completedLessons": 0,
"totalQuestions": 10,
"correctAnswers": 0,
"completionPercentage": 0.00
}

### 4. Answering a Multiple-Choice Question

Find POST /api/user-progress/question/{questionId}/submit/multiple-choice, click "Try it out".

Enter questionId: 1 and in "Request body":
{
"answerOptionId": 1
}

Ensure the token is entered (see step above).

Click "Execute".

You’ll see:

{
"isCorrect": true,
"message": "Correct answer!"
}

## Troubleshooting

Database won’t connect:

Check if PostgreSQL is running in pgAdmin (server should be green).

Verify the password in appsettings.json.

Swagger doesn’t open:

Ensure the address is https://localhost:7057/swagger.

Check Command Prompt for errors.

Authorization error:

Make sure the token starts with "Bearer " and is fully copied.

## Conclusion

You’ve now set up and tested Cogniteq Learn! 🎉
If anything’s unclear, contact the project author at <your-email>.

Thank you for using Cogniteq Learn! 🚀
