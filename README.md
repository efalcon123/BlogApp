
# Blog Application

## Overview

This Blog Application is designed to allow users to create and manage blog posts with different levels of access based on user roles. The application supports two primary user roles: **Admin** and **Editor**. Each role has different privileges to interact with the blog posts and manage users.

The application allows for the creation, editing, approval, and rejection of blog posts. Blog posts are created with specific fields and statuses, and the application ensures that the roles have appropriate permissions.

## Features

### Roles & Permissions
1. **Admin Role**:
   - **Create & Delete Users**: Admin can create new users and delete existing ones.
   - **Approve & Reject Blog Posts**: Admin can approve or reject blog posts submitted by Editors or other users.
   - **Create/Edit Own Blog Posts**: Admin can create and edit their own blog posts.

2. **Editor Role**:
   - **Create/Edit Own Blog Posts**: Editors can create new blog posts and edit their own posts.
   - **Blog Post Management**: Editors cannot approve or reject blog posts, but they can create and modify their own posts.

### Blog Post Features
- **Title**: A text box for entering the blog post title.
- **Banner Image**: A file input field for uploading images (restricted to JPG, PNG, and GIF formats). The file size is limited to 5MB.
- **Content**: A rich text WYSIWYG (What You See Is What You Get) editor for entering the blog content.
- **Blog Post Status**:
  - **Draft**: Post is in progress and not visible to the public.
  - **Published**: Post is approved and visible to the public.
  - **Rejected**: Post has been rejected by the Admin.

## Technologies Used

- **Framework**: ASP.NET Core
- **Database**: SQLite
- **Frontend**: Bootstrap 4.0 for HTML & CSS layout
- **Authentication**: Cookie-based authentication with role-based access
- **WYSIWYG Editor**: Integrated rich text editor for blog content
- **Image Upload**: Image file input with validation for file type and size

## Installation & Setup

### Prerequisites

To run this application locally, you will need the following installed on your system:

- **.NET 6.0 SDK** (or newer) – [Install .NET SDK](https://dotnet.microsoft.com/download)
- **SQLite** – You don’t need a separate server for SQLite as it uses a local file-based database.

### Steps to Run Locally

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/efalcon123/BlogWebApp.git
   cd BlogApp
   ```
2. **Set Up the Database**:

-   SQLite doesn't require a server, so the database will be a local file.
-   Make sure the connection string in `appsettings.json` points to a file-based SQLite database.

Example of connection string in `appsettings.json`:

   ```bash
  "ConnectionStrings": { "ApplicationBlogContext": "Data Source=database.db" }
  ```
  2. **Restore Dependencies**:
Run the following command to restore the NuGet packages:
```bash
	dotnet restore
```
  4. **Run Migrations** (if needed):
If this is the first time setting up the application, run the migrations to set up the database schema:
```bash
	dotnet ef database update
```
 5. ****Build and Run the Application**:**
If this is the first time setting up the application, run the migrations to set up the database schema:
```bash
	dotnet run
```
The application will be available at `https://localhost:5001` (or the configured URL).
## Usage

### Login

-   **Admin User**: Log in with an admin account (e.g., `admin@blogapp.com`) to access admin features like creating users and approving/rejecting blog posts.
-   **Editor User**: Log in with an editor account (e.g., `editor@blogapp.com`) to create and edit blog posts.

### Creating Blog Posts

-   Navigate to the **Create Blog Post** page.
-   Fill in the **Title** and **Content** fields.
-   Upload a **Banner Image** (JPG, PNG, or GIF only, less than 5MB).
-   Choose the **Status** of the blog post (Draft, Published, or Rejected).
-   **Save** the post to save it as a draft or publish it (depending on your role).

### Managing Blog Posts (Admin Only)

-   As an **Admin**, you can approve or reject blog posts.
-   Admins can view all posts and change their status.

### User Management (Admin Only)

-   **Admin** users can create and delete other users (including other Admins or Editors
## Folder Structure
```
/BlogApp
  /Data
    - ApplicationBlogContext.cs
  /Models
    - Blog.cs
    - User.cs
  /Helpers
	- UserHelper.cs
  /Pages
    - /Index.cshtml
    - /AccessDenied.cshtml
    - /User
	    - Index.cshtml
	    - Create.cshtml
	    - Details.cshtml
	    - Edit.cshtml
	    - Delete.cshtml
    - /Blog
	    - Index.cshtml
	    - Create.cshtml
	    - Details.cshtml
	    - Edit.cshtml
	    - Delete.cshtml
  /wwwroot
    /css
    /js
    /lib
  /Migrations
  - appsettings.json
  - Program.cs
```
## Contributing

If you would like to contribute to the development of this project, please fork the repository, create a branch for your changes, and submit a pull request.

## Guidelines

-   Write clean, readable code.
-   Include unit tests for any new features or changes.
-   Ensure all new features are documented in the README.
