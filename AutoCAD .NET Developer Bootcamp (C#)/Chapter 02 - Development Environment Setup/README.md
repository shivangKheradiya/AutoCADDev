# Chapter 02 - Development Environment Setup

### Chapter Objective

In this chapter, we will set up the complete AutoCAD .NET development environment required for building professional AutoCAD plugins.

By the end of this chapter, you will be able to create a Class Library project, reference the required AutoCAD assemblies, build a plugin DLL, and prepare your workstation for future development.

---

## Learning Outcomes

After completing this chapter, you will be able to:

- Install Visual Studio.
- Configure AutoCAD for plugin development.
- Create a Class Library project.
- Add AutoCAD .NET references.
- Understand the purpose of AutoCAD assemblies.
- Build a plugin DLL successfully.
- Understand the AutoCAD plugin development workflow.

---

# Why Do We Need a Development Environment?

To create AutoCAD plugins, we need tools that allow us to:

- Write source code
- Compile code into DLLs
- Debug applications
- Load plugins into AutoCAD
- Maintain large projects

Instead of writing code manually in a text editor, we use a modern Integrated Development Environment (IDE).

For this course, we will use:

```text
Visual Studio
```

and

```text
AutoCAD
```

as our primary development tools.

---

# Development Environment Architecture

The AutoCAD plugin development workflow follows the process below:

```text
Visual Studio
     |
     v
Write C# Code
     |
     v
Build DLL
     |
     v
NETLOAD
     |
     v
AutoCAD
     |
     v
Execute Commands
```

Throughout this training program, this workflow will be repeated in every chapter.

---

# Step 1 - Install Visual Studio

Visual Studio is Microsoft's development environment used to create .NET applications.

We will use it to:

- Write code
- Compile projects
- Manage references
- Debug applications

---

## Recommended Workloads

During Visual Studio installation, select:

```text
.NET Desktop Development
```

This workload installs everything required for AutoCAD .NET development.

---

## Verify Installation

Launch Visual Studio.

You should be able to create a new project.

---

# Step 2 - Verify AutoCAD Installation

AutoCAD must already be installed on your development machine.

Check that AutoCAD launches successfully.

Typical installation location:

```text
C:\Program Files\Autodesk\AutoCAD 2026\
```

Depending on your version, the folder name may differ.

Examples:

```text
AutoCAD 2024
AutoCAD 2025
AutoCAD 2026
```

---

# Step 3 - Locate AutoCAD .NET Assemblies

AutoCAD exposes managed APIs through several DLL files.

The most important assemblies are:

```text
AcMgd.dll
AcDbMgd.dll
AcCoreMgd.dll
```

These files are normally located inside the AutoCAD installation folder.

Example:

```text
C:\Program Files\Autodesk\AutoCAD 2026\
```

---

# Understanding AutoCAD Assemblies

Before adding references, it is useful to understand their purpose.

---

## AcMgd.dll

Provides:

- Application Services
- Document Management
- Editor Operations
- User Interaction

Examples:

```text
Application
Document
Editor
```

---

## AcDbMgd.dll

Provides:

- AutoCAD Database API
- Entities
- Layers
- Blocks

Examples:

```text
Line
Circle
LayerTable
BlockTable
```

---

## AcCoreMgd.dll

Provides:

- AutoCAD Core Functionality
- Runtime Services
- Core APIs

This assembly is required by most modern AutoCAD plugins.

---

# Step 4 - Create a Class Library Project

Launch Visual Studio.

Select:

```text
Create New Project
```

---

Choose:

```text
Class Library (.NET Framework)
```

---

Project Name:

```text
AutoCADBootcamp.Chapter02
```

---

Framework:

```text
.NET Framework 4.8
```

---

Click:

```text
Create
```

---

# Project Structure

The project should initially look like:

```text
AutoCADBootcamp.Chapter02
│
├── Class1.cs
├── References
├── Properties
└── App.config
```

---

# Step 5 - Add AutoCAD References

Right-click:

```text
References
```

Select:

```text
Add Reference
```

---

Browse to the AutoCAD installation folder.

Add:

```text
AcMgd.dll
AcDbMgd.dll
AcCoreMgd.dll
```

---

# Configure References

After adding the references:

Select each reference and set:

```text
Copy Local = False
```

---

## Why Copy Local Should Be False

If Copy Local is True:

```text
AcMgd.dll
AcDbMgd.dll
AcCoreMgd.dll
```

will be copied into your output folder.

This is unnecessary because AutoCAD already loads these assemblies.

Setting:

```text
Copy Local = False
```

keeps the build output clean.

---

# Step 6 - Create a Test Class

Delete:

```text
Class1.cs
```

Create:

```text
TestPlugin.cs
```

---

Add the following code:

```csharp
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;

namespace AutoCADBootcamp.Chapter02
{
    public class TestPlugin
    {
        [CommandMethod("TESTSETUP")]
        public void TestSetup()
        {
            Application.ShowAlertDialog(
                "Development Environment Ready!"
            );
        }
    }
}
```

---

# Understanding the Code

The namespace organizes our code.

```csharp
namespace AutoCADBootcamp.Chapter02
```

---

The class contains our commands.

```csharp
public class TestPlugin
```

---

The CommandMethod attribute registers a command inside AutoCAD.

```csharp
[CommandMethod("TESTSETUP")]
```

---

The command displays a message box.

```csharp
Application.ShowAlertDialog(...)
```

---

# Step 7 - Build the Project

Visual Studio Menu:

```text
Build
  ->
Build Solution
```

or

```text
Ctrl + Shift + B
```

---

If everything is configured correctly, the build should succeed.

Example:

```text
Build succeeded.
```

---

# Build Output

Navigate to:

```text
bin\Debug\
```

You should find:

```text
AutoCADBootcamp.Chapter02.dll
```

This DLL is your AutoCAD plugin.

---

# AutoCAD Plugin Lifecycle

Every plugin follows the same process:

```text
Write Code
       |
       v
Compile DLL
       |
       v
Load with NETLOAD
       |
       v
Execute Command
```

Understanding this lifecycle is critical for all future chapters.

---

# Mini Project

## Objective

Compile your first AutoCAD plugin DLL.

---

## Steps

### 1. Create Class Library

```text
.NET Framework 4.8
```

### 2. Add References

```text
AcMgd.dll
AcDbMgd.dll
AcCoreMgd.dll
```

### 3. Set

```text
Copy Local = False
```

### 4. Add Sample Command

```csharp
[CommandMethod("TESTSETUP")]
public void TestSetup()
{
    Application.ShowAlertDialog(
        "Development Environment Ready!"
    );
}
```

### 5. Build Solution

```text
Ctrl + Shift + B
```

---

## Expected Result

Build output:

```text
AutoCADBootcamp.Chapter02.dll
```

Congratulations.

Your AutoCAD development environment is now fully configured.

---

# Chapter Summary

In this chapter we learned:

- How to install Visual Studio.
- How to prepare AutoCAD for development.
- How to create a Class Library project.
- How to add AutoCAD assemblies.
- Why AcMgd.dll, AcDbMgd.dll, and AcCoreMgd.dll are required.
- How to build an AutoCAD plugin DLL.
- The overall AutoCAD plugin development workflow.

---

# Assignment

### Exercise 1

Locate the following files on your machine:

```text
AcMgd.dll
AcDbMgd.dll
AcCoreMgd.dll
```

Record their paths.

---

### Exercise 2

Create a new AutoCAD plugin project called:

```text
MyFirstAutoCADPlugin
```

Configure all required references.

---

### Exercise 3

Build the project successfully and verify that the DLL is generated.

---

# Interview Questions

1. Why do we use Visual Studio for AutoCAD development?
2. What is a Class Library project?
3. What is the purpose of AcMgd.dll?
4. What is the purpose of AcDbMgd.dll?
5. What is the purpose of AcCoreMgd.dll?
6. Why should Copy Local be set to False?
7. What is produced when a plugin project is built?
8. What is the role of NETLOAD in AutoCAD?

---

# Next Chapter

## Chapter 03 - Understanding Commands

In the next chapter, we will:

- “CommandMethod” Attribute
- Command Registration
- Using Editor
- User Messages
- Mini Project – Commands creation such as HELLO , ABOUT, VERSION
