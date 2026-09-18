# Chapter 05 - Creating Your First Entities

### Chapter Objective

In this chapter, we will create our first graphical objects inside AutoCAD using the .NET API.

So far, we have learned how AutoCAD commands work and how AutoCAD stores data internally. Now, we are ready to start creating actual drawing geometry.

By the end of this chapter, you will be able to create points, lines, and circles programmatically and add them to Model Space.

---

## Learning Outcomes

After completing this chapter, you will be able to:

- Understand what an Entity is.
- Create Point entities.
- Create Line entities.
- Create Circle entities.
- Access Model Space.
- Append entities to a drawing.
- Understand how entities become visible in AutoCAD.
- Build simple drawing automation commands.

---

# What Is an Entity?

An Entity is any graphical object that appears in a drawing.

Examples:

```text
Point
Line
Circle
Arc
Polyline
Text
Dimension
Block Reference
```

Everything visible in AutoCAD is ultimately an Entity.

---

# Entity Hierarchy

```text
DBObject
    |
    +-- Entity
            |
            +-- Point
            +-- Line
            +-- Circle
            +-- Arc
            +-- Polyline
            +-- DBText
```

As AutoCAD developers, we spend most of our time creating and modifying entities.

---

# Creating Entities Programmatically

The typical workflow for creating entities is:

```text
Get Current Document
        |
Get Database
        |
Start Transaction
        |
Access Model Space
        |
Create Entity
        |
Append Entity
        |
Commit Transaction
```

This workflow will be repeated throughout this entire course.

---

# Understanding Coordinates

Before creating entities, we must understand coordinates.

AutoCAD uses:

```text
X
Y
Z
```

coordinates.

Example:

```text
(0,0,0)
```

represents the origin.

---

## Coordinate Examples

```text
(0,0,0)

(100,0,0)

(100,100,0)

(0,100,0)
```

These coordinates define positions inside the drawing.

---

# Creating a Point

The Point entity represents a single location inside the drawing.

---

## Point Example

```text
•
```

---

## Point Object

```csharp
DBPoint point =
    new DBPoint(
        new Point3d(0, 0, 0)
    );
```

---

## Point Parameters

```csharp
Point3d(
    X,
    Y,
    Z
);
```

Example:

```csharp
new Point3d(
    100,
    50,
    0
);
```

---

# Creating a Line

A Line is defined using two points.

---

## Line Example

```text
------------------------
```

---

## Line Object

```csharp
Line line =
    new Line(
        new Point3d(0, 0, 0),
        new Point3d(100, 100, 0)
    );
```

---

## Line Components

```text
Start Point
End Point
```

Example:

```text
(0,0,0) -> (100,100,0)
```

---

# Creating a Circle

A Circle requires:

```text
Center Point
Radius
```

---

## Circle Object

```csharp
Circle circle =
    new Circle(
        new Point3d(50, 50, 0),
        Vector3d.ZAxis,
        25
    );
```

---

## Circle Parameters

### Center Point

```text
(50,50,0)
```

### Normal Vector

```text
Vector3d.ZAxis
```

### Radius

```text
25
```

---

# Why Aren't Entities Visible Yet?

Many beginners create entities but do not see them appear in the drawing.

Why?

Because the entities haven't been added to the drawing database.

Creating an object:

```csharp
Line line = new Line(...);
```

creates it only in memory.

We must append it to Model Space.

---

# Understanding Model Space

Most drawing geometry is stored inside:

```text
Model Space
```

Architecture:

```text
Database
    |
    +-- BlockTable
            |
            +-- ModelSpace
```

To make an entity visible, we must add it to Model Space.

---

# Accessing Model Space

First get:

```csharp
Document
```

Then:

```csharp
Database
```

Then:

```csharp
BlockTable
```

Finally:

```csharp
Model Space
```

---

# The Role of Transactions

AutoCAD protects its database using transactions.

Every database modification should occur inside:

```text
Transaction
```

---

## Transaction Workflow

```text
Start Transaction
       |
Create Entity
       |
Append Entity
       |
Commit / Rollback Transaction
```

If the transaction is not committed:

```text
Changes Are Lost
```

---

# Creating a Point in Model Space

```csharp
DBPoint point =
    new DBPoint(
        new Point3d(0, 0, 0)
    );
```

Append it to Model Space.

Commit the transaction.

Result:

```text
Point Appears In Drawing
```

---

# Creating a Line in Model Space

```csharp
Line line =
    new Line(
        new Point3d(0, 0, 0),
        new Point3d(100, 100, 0)
    );
```

Append it to Model Space.

Commit transaction.

Result:

```text
Line Appears In Drawing
```

---

# Creating a Circle in Model Space

```csharp
Circle circle =
    new Circle(
        new Point3d(50, 50, 0),
        Vector3d.ZAxis,
        25
    );
```

Append it to Model Space.

Commit transaction.

Result:

```text
Circle Appears In Drawing
```

---

# Mini Project

## Objective

Create three different entity types:

```text
Point
Line
Circle
```

and display them in AutoCAD.

---

# Complete Mini Project

```csharp
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace AutoCADBootcamp.Chapter05
{
    public class EntityCommands
    {
        [CommandMethod("DRAWDEMO")]
        public void DrawDemo()
        {
            Document doc =
                Application.DocumentManager
                           .MdiActiveDocument;

            Database db = doc.Database;

            using (Transaction tr =
                db.TransactionManager.StartTransaction())
            {
                BlockTable bt =
                    tr.GetObject(
                        db.BlockTableId,
                        OpenMode.ForRead)
                    as BlockTable;

                BlockTableRecord ms =
                    tr.GetObject(
                        bt[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite)
                    as BlockTableRecord;

                DBPoint point =
                    new DBPoint(
                        new Point3d(0, 0, 0));

                Line line =
                    new Line(
                        new Point3d(0, 0, 0),
                        new Point3d(100, 100, 0));

                Circle circle =
                    new Circle(
                        new Point3d(50, 50, 0),
                        Vector3d.ZAxis,
                        25);

                ms.AppendEntity(point);
                tr.AddNewlyCreatedDBObject(
                    point, true);

                ms.AppendEntity(line);
                tr.AddNewlyCreatedDBObject(
                    line, true);

                ms.AppendEntity(circle);
                tr.AddNewlyCreatedDBObject(
                    circle, true);

                tr.Commit();
            }
        }
    }
}
```

---

# Expected Result

Execute:

```text
DRAWDEMO
```

AutoCAD should create:

```text
Point at origin

Line from:
(0,0,0)
to
(100,100,0)

Circle centered at:
(50,50,0)

Radius:
25
```

---

# Visual Result

```text
          ○

        /
      /
    /
  /
•

```

The exact appearance depends on your zoom level.

Use:

```text
ZOOM
E
```

to zoom to the created entities.

---

# Common Beginner Mistakes

## Forgetting Transaction

Wrong:

```csharp
Create Entity
```

Without transaction.

Result:

```text
Errors or missing objects
```

---

## Forgetting AppendEntity

Wrong:

```csharp
Line line =
    new Line(...);
```

Only creates the object in memory.

Result:

```text
Nothing appears in drawing
```

---

## Forgetting Commit

Wrong:

```csharp
tr.Commit();
```

not executed.

Result:

```text
Entities disappear
```

---

## Objects Created Outside Drawing Area

Example:

```csharp
(1000000,1000000,0)
```

Result:

```text
Objects exist but are not visible
```

---

# Chapter Summary

In this chapter we learned:

- What entities are.
- How AutoCAD represents drawing objects.
- How to create Point entities.
- How to create Line entities.
- How to create Circle entities.
- How to access Model Space.
- How to append entities to a drawing.
- How transactions are used when modifying drawings.

---

# Assignment

### Exercise 1

Create a command:

```text
DRAWPOINT
```

that creates a point at:

```text
(100,100,0)
```

---

### Exercise 2

Create a command:

```text
DRAWLINE
```

that creates a line between:

```text
(0,0,0)

and

(200,100,0)
```

---

### Exercise 3

Create a command:

```text
DRAWCIRCLE
```

that creates a circle with:

```text
Center = (0,0,0)

Radius = 50
```

---

### Exercise 4

Modify the DRAWDEMO command to create:

```text
2 Points
2 Lines
2 Circles
```

---

# Interview Questions

1. What is an Entity in AutoCAD?
2. What is the difference between creating an entity and displaying it?
3. What is Model Space?
4. Why is a Transaction required?
5. What is AppendEntity() used for?
6. What happens if Commit() is not executed?
7. How is a Line defined?
8. How is a Circle defined?

---

# Next Chapter

## Chapter 06 - Working with Transactions

In the next chapter, we will learn:

- TransactionManager
- OpenMode
- ForRead vs ForWrite
- Commit
- Abort
- Transaction Best Practices
- Safe Database Operations

Understanding transactions is essential before developing professional AutoCAD applications.