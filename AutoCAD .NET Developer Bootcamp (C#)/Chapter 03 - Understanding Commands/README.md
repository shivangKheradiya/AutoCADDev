# Chapter 03 - Understanding Commands

### Chapter Objective

In this chapter, we will learn how AutoCAD commands are created, registered, and executed using the AutoCAD .NET API.

Commands are the foundation of every AutoCAD plugin. Whether you are creating drawing automation tools, validation systems, or enterprise applications, every plugin starts with commands.

By the end of this chapter, you will be able to create multiple custom commands, interact with AutoCAD users, and display information through the AutoCAD command line.

---

## Learning Outcomes

After completing this chapter, you will be able to:

- Understand the purpose of the CommandMethod attribute.
- Understand how commands are registered in AutoCAD.
- Create and execute custom commands.
- Interact with users through the Editor class.
- Display messages in the AutoCAD command window.
- Organize commands within a plugin.
- Create multiple production-ready commands.

---

# What Is an AutoCAD Command?

An AutoCAD command is an action that can be executed from the command line.

Examples of built-in commands:

```text
LINE
CIRCLE
MOVE
COPY
LAYER
```

When a user types a command, AutoCAD searches for its implementation and executes the associated logic.

The AutoCAD .NET API allows developers to create their own custom commands.

Examples:

```text
HELLO
ABOUT
VERSION
CHECKDRAWING
CREATELAYERS
```

---

# Why Are Commands Important?

Commands serve as the entry point into your plugin.

Every AutoCAD tool eventually begins with command execution.

Examples:

```text
User Types Command
        |
        v
AutoCAD Executes Plugin
        |
        v
Plugin Performs Business Logic
```

Without commands, users would have no way to access your functionality.

---

# AutoCAD Command Workflow

```text
Create Command
      |
      v
Compile DLL
      |
      v
NETLOAD
      |
      v
Command Registered
      |
      v
Execute Command
```

Understanding this workflow is essential before moving on to entities, layers, blocks, and automation.

---

# The CommandMethod Attribute

The AutoCAD .NET API uses the CommandMethod attribute to identify commands.

Example:

```csharp
[CommandMethod("HELLO")]
```

The string inside the attribute becomes the command name visible inside AutoCAD.

Example:

```csharp
[CommandMethod("VERSION")]
```

creates:

```text
VERSION
```

inside AutoCAD.

---

# Command Structure

Basic command structure:

```csharp
[CommandMethod("HELLO")]
public void Hello()
{
}
```

The CommandMethod attribute registers the command.

The method contains the code that executes when the command is called.

---

# Understanding Command Registration

AutoCAD automatically discovers commands when:

1. The DLL is loaded.
2. The assembly is scanned.
3. Methods containing CommandMethod are found.
4. Commands become available.

Example:

```csharp
[CommandMethod("HELLO")]
public void Hello()
{
}
```

After:

```text
NETLOAD
```

the command becomes available.

```text
HELLO
```

---

# Creating Your First Command

Create the following class:

```csharp
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;

namespace AutoCADBootcamp.Chapter03
{
    public class BasicCommands
    {
        [CommandMethod("HELLO")]
        public void Hello()
        {
            Application.ShowAlertDialog(
                "Hello AutoCAD Developer!"
            );
        }
    }
}
```

---

# Executing the Command

Load your plugin:

```text
NETLOAD
```

Select:

```text
AutoCADBootcamp.Chapter03.dll
```

Execute:

```text
HELLO
```

Result:

```text
Hello AutoCAD Developer!
```

Congratulations.

You have created a custom AutoCAD command.

---

# Working with Multiple Commands

A single class can contain multiple commands.

Example:

```csharp
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;

namespace AutoCADBootcamp.Chapter03
{
    public class BasicCommands
    {
        [CommandMethod("HELLO")]
        public void Hello()
        {
            Application.ShowAlertDialog(
                "Hello AutoCAD Developer!"
            );
        }

        [CommandMethod("ABOUT")]
        public void About()
        {
            Application.ShowAlertDialog(
                "AutoCAD .NET Developer Bootcamp"
            );
        }
    }
}
```

AutoCAD registers both commands.

Available commands:

```text
HELLO
ABOUT
```

---

# Using the Editor Class

The Editor class provides access to the AutoCAD command window.

It is one of the most frequently used classes in AutoCAD development.

Before working with entities, layers, or user input, developers usually communicate through the Editor.

---

# Getting the Current Document

First obtain the active document.

```csharp
Document doc =
    Application.DocumentManager
               .MdiActiveDocument;
```

---

# Accessing the Editor

The active document contains an Editor.

```csharp
Editor ed = doc.Editor;
```

Now messages can be written to the AutoCAD command line.

---

# Displaying User Messages

The Editor class provides:

```csharp
WriteMessage()
```

Example:

```csharp
ed.WriteMessage(
    "\nHello AutoCAD Developer!"
);
```

Output:

```text
Hello AutoCAD Developer!
```

---

# Why Use Editor Instead of Message Boxes?

Message boxes interrupt the user.

Command line messages are more professional and behave like native AutoCAD commands.

Preferred:

```csharp
ed.WriteMessage(...)
```

Less Preferred:

```csharp
Application.ShowAlertDialog(...)
```

Most production plugins use:

```text
Editor
```

for status messages.

---

# Example Command Using Editor

```csharp
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCADBootcamp.Chapter03
{
    public class BasicCommands
    {
        [CommandMethod("HELLO")]
        public void Hello()
        {
            Document doc =
                Application.DocumentManager
                           .MdiActiveDocument;

            Editor ed = doc.Editor;

            ed.WriteMessage(
                "\nHello AutoCAD Developer!"
            );
        }
    }
}
```

---

# Command Naming Best Practices

Use meaningful names.

Good Examples:

```text
CREATEPUMP
VALIDATE
IMPORTEXCEL
CREATELAYERS
EXPORTREPORT
```

Poor Examples:

```text
TEST1
ABC
COMMAND1
```

Commands should describe their purpose clearly.

---

# Organizing Commands

As projects grow, commands should be grouped logically.

Example:

```text
Commands
|
+-- GeneralCommands.cs
+-- DrawingCommands.cs
+-- LayerCommands.cs
+-- BlockCommands.cs
```

This improves maintainability.

---

# Mini Project

## Objective

Create three custom AutoCAD commands.

The goal is to understand command registration, command execution, and user interaction.

---

# Command 1 - HELLO

```csharp
[CommandMethod("HELLO")]
public void Hello()
{
    Application.ShowAlertDialog(
        "Hello AutoCAD Developer!"
    );
}
```

Expected Result:

```text
HELLO
```

Output:

```text
Hello AutoCAD Developer!
```

---

# Command 2 - ABOUT

```csharp
[CommandMethod("ABOUT")]
public void About()
{
    Application.ShowAlertDialog(
        "AutoCAD .NET Developer Bootcamp"
    );
}
```

Expected Result:

```text
ABOUT
```

Output:

```text
AutoCAD .NET Developer Bootcamp
```

---

# Command 3 - VERSION

```csharp
[CommandMethod("VERSION")]
public void Version()
{
    Application.ShowAlertDialog(
        "Version 1.0"
    );
}
```

Expected Result:

```text
VERSION
```

Output:

```text
Version 1.0
```

---

# Complete Mini Project Solution

```csharp
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;

namespace AutoCADBootcamp.Chapter03
{
    public class BasicCommands
    {
        [CommandMethod("HELLO")]
        public void Hello()
        {
            Application.ShowAlertDialog(
                "Hello AutoCAD Developer!"
            );
        }

        [CommandMethod("ABOUT")]
        public void About()
        {
            Application.ShowAlertDialog(
                "AutoCAD .NET Developer Bootcamp"
            );
        }

        [CommandMethod("VERSION")]
        public void Version()
        {
            Application.ShowAlertDialog(
                "Version 1.0"
            );
        }
    }
}
```

---

# Chapter Summary

In this chapter we learned:

- What AutoCAD commands are.
- How custom commands are created.
- How the CommandMethod attribute works.
- How AutoCAD registers commands.
- How to interact with users using the Editor class.
- How to display messages in AutoCAD.
- How to create multiple commands inside a single plugin.

---

# Assignment

### Exercise 1

Create a command named:

```text
AUTHOR
```

Display your name.

---

### Exercise 2

Create a command named:

```text
COMPANY
```

Display your company name.

---

### Exercise 3

Create a command named:

```text
COURSEINFO
```

Display information about this training course.

---

# Interview Questions

1. What is the purpose of the CommandMethod attribute?
2. How does AutoCAD discover custom commands?
3. What happens after NETLOAD is executed?
4. What is the Editor class?
5. What is the purpose of WriteMessage()?
6. Why are command-line messages preferred over message boxes?
7. Can multiple commands exist within a single plugin?
8. What are command naming best practices?

---

# Next Chapter

## Chapter 04 - Understanding AutoCAD Architecture

In the next chapter, we will learn:

- Application Object
- Document Object
- Database Object
- Entity Architecture
- AutoCAD Object Hierarchy
- Navigation through AutoCAD's core components

Understanding this architecture is essential before creating and manipulating drawing entities.