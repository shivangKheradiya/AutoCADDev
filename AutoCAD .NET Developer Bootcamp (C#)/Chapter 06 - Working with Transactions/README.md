# Chapter 06 - Working with Transactions

### Chapter Objective

In this chapter, we will learn one of the most important concepts in AutoCAD development: `Transactions`.

Every professional AutoCAD application relies on transactions to safely read and modify drawing data. Whether you are creating entities, modifying layers, updating blocks, or deleting objects, transactions are the foundation of safe database operations.

By the end of this chapter, you will understand how transactions work, how to use TransactionManager, the difference between ForRead and ForWrite, and how Commit and Abort affect database changes.

---

## Learning Outcomes

After completing this chapter, you will be able to:

- Understand why transactions are required.
- Use the TransactionManager.
- Open objects using OpenMode.
- Differentiate between `ForRead` and `ForWrite`.
- Use Commit correctly.
- Understand when Abort occurs.
- Follow transaction best practices.
- Perform safe database operations.

---

# Why Do We Need Transactions?

Imagine modifying a drawing containing thousands of objects.

What happens if:

```text
Error occurs halfway?
Application crashes?
Invalid data is written?
```

Without a transaction:

```text
Drawing Database Becomes Corrupted
```

Transactions protect the AutoCAD database by ensuring operations are performed safely.

---

# What Is a Transaction?

A Transaction is a protected work session that allows you to:

```text
Read Data
Create Data
Modify Data
Delete Data
```

and then either:

```text
Save Changes
```

or

```text
Discard Changes
```

---

# Real World Analogy

Think of a transaction like editing a document.

```text
Open Document
      |
Make Changes
      |
Save
```

or

```text
Open Document
      |
Make Changes
      |
Close Without Saving
```

The AutoCAD transaction works similarly.

---

# Transaction Lifecycle

```text
Start Transaction
        |
Read Objects
        |
Modify Objects
        |
Commit
```

or

```text
Start Transaction
        |
Modify Objects
        |
Error Occurs
        |
Abort
```

---

# Transaction Manager

The TransactionManager is responsible for creating transactions.

Every AutoCAD Database contains its own TransactionManager.

---

## Accessing TransactionManager

```csharp
Database db = doc.Database;

TransactionManager tm =
    db.TransactionManager;
```

---

# Starting a Transaction

The most common way:

```csharp
using (Transaction tr =
    db.TransactionManager
      .StartTransaction())
{
}
```

This creates a transaction scope.

Any database operation should occur inside this block.

---

# Why Use "using"?

The using statement automatically cleans up resources.

Example:

```csharp
using (Transaction tr =
    db.TransactionManager
      .StartTransaction())
{
}
```

When execution exits:

```text
Resources Are Released Automatically
```

This is the recommended approach.

---

# Understanding OpenMode

AutoCAD protects objects from accidental modification.

When retrieving an object, we must specify how it will be used.

Possible options:

```text
OpenMode.ForRead
OpenMode.ForWrite
```

---

# OpenMode.ForRead

Use ForRead when:

```text
You Only Need To View Data
```

Examples:

```text
Read Layer Name
Read Entity Data
Read Block Name
Read Text Value
```

Example:

```csharp
LayerTable lt =
    tr.GetObject(
        db.LayerTableId,
        OpenMode.ForRead
    ) as LayerTable;
```

---

# OpenMode.ForWrite

Use ForWrite when:

```text
You Intend To Modify Data
```

Examples:

```text
Create Layer
Delete Entity
Modify Text
Add Block
Append Entity
```

Example:

```csharp
LayerTable lt =
    tr.GetObject(
        db.LayerTableId,
        OpenMode.ForWrite
    ) as LayerTable;
```

---

# ForRead vs ForWrite

## ForRead

```text
Safe
Fast
No Modification Allowed
```

Purpose:

```text
Read Existing Information
```

---

## ForWrite

```text
Allows Modification
Requires Database Lock
```

Purpose:

```text
Modify Database Objects
```

---

# Best Practice

Always open objects with the minimum required permission.

Good:

```csharp
OpenMode.ForRead
```

when reading.

---

Bad:

```csharp
OpenMode.ForWrite
```

when no modification is needed.

---

# Understanding Commit

Commit permanently saves all transaction changes.

Without Commit:

```text
Changes Are Lost
```

---

## Example

```csharp
tr.Commit();
```

After Commit:

```text
Point Created
Line Created
Layer Created
```

Changes become part of the drawing database.

---

# Understanding Abort

Abort discards all changes made during the transaction.

Example:

```csharp
tr.Abort();
```

Result:

```text
Nothing Saved
```

The drawing returns to its previous state.

---

# Automatic Abort

If Commit is never called:

```text
Transaction Automatically Rolls Back
```

Example:

```csharp
using(Transaction tr =
    db.TransactionManager
      .StartTransaction())
{
    // No Commit
}
```

Result:

```text
Changes Are Discarded
```

---

# Reading Data Using a Transaction

Example:

```csharp
using (Transaction tr =
    db.TransactionManager
      .StartTransaction())
{
    BlockTable bt =
        tr.GetObject(
            db.BlockTableId,
            OpenMode.ForRead
        ) as BlockTable;

    tr.Commit();
}
```

Even simple read operations should use transactions.

---

# Modifying Data Using a Transaction

Example:

```csharp
using (Transaction tr =
    db.TransactionManager
      .StartTransaction())
{
    BlockTableRecord ms =
        tr.GetObject(
            modelSpaceId,
            OpenMode.ForWrite
        ) as BlockTableRecord;

    // Modify Drawing

    tr.Commit();
}
```

---

# Why Was BlockTable Opened ForRead?

Many beginners ask:

```csharp
BlockTable bt =
    tr.GetObject(
        db.BlockTableId,
        OpenMode.ForRead
    ) as BlockTable;
```

Why not ForWrite?

Answer:

Because we only need information from the BlockTable.

Example:

```csharp
bt[BlockTableRecord.ModelSpace]
```

We are reading.

No modifications are made.

Therefore:

```text
ForRead
```

is correct.

---

# Why Was ModelSpace Opened ForWrite?

Example:

```csharp
BlockTableRecord ms =
    tr.GetObject(
        bt[BlockTableRecord.ModelSpace],
        OpenMode.ForWrite
    ) as BlockTableRecord;
```

Why ForWrite?

Because we are modifying Model Space.

Example:

```csharp
ms.AppendEntity(line);
```

We are adding entities.

Therefore:

```text
ForWrite
```

is required.

---

# Transaction Architecture

```text
Database
     |
TransactionManager
     |
Transaction
     |
Objects
     |
Commit / Abort
```

---

# Professional Transaction Pattern

Most AutoCAD developers follow this structure:

```csharp
using (Transaction tr =
    db.TransactionManager
      .StartTransaction())
{
    // Read Objects

    // Process Data

    // Modify Objects

    tr.Commit();
}
```

Simple.

Readable.

Maintainable.

---

# Mini Project

## Objective

Create a command that safely opens the drawing database using a transaction and displays transaction status messages.

---

# TransactionDemo Command

```csharp
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCADBootcamp.Chapter06
{
    public class TransactionCommands
    {
        [CommandMethod("TRANSACTIONDEMO")]
        public void TransactionDemo()
        {
            Document doc =
                Application.DocumentManager
                           .MdiActiveDocument;

            Database db = doc.Database;

            Editor ed = doc.Editor;

            using (Transaction tr =
                db.TransactionManager
                  .StartTransaction())
            {
                ed.WriteMessage(
                    "\nTransaction Started"
                );

                BlockTable bt =
                    tr.GetObject(
                        db.BlockTableId,
                        OpenMode.ForRead
                    ) as BlockTable;

                ed.WriteMessage(
                    "\nBlockTable Opened"
                );

                tr.Commit();

                ed.WriteMessage(
                    "\nTransaction Committed"
                );
            }
        }
    }
}
```

---

# Expected Result

Execute:

```text
TRANSACTIONDEMO
```

Output:

```text
Transaction Started

BlockTable Opened

Transaction Committed
```

---

# Transaction Best Practices

## Always Use Transactions

Good:

```csharp
using(Transaction tr ...)
```

---

## Use ForRead Whenever Possible

Read-only operations should remain read-only.

---

## Use ForWrite Only When Necessary

Reduce locking and improve performance.

---

## Keep Transactions Short

Good:

```text
Start
Work
Commit
```

Bad:

```text
Start
Execute Long Logic
Wait
Perform UI Activity
Commit
```

---

## Always Commit Explicitly

Good:

```csharp
tr.Commit();
```

---

## Handle Exceptions

Professional applications typically use:

```csharp
try
{
}
catch
{
}
```

around transaction logic.

---

# Common Beginner Mistakes

## Forgetting Commit

Wrong:

```csharp
Create Entity
```

No Commit.

Result:

```text
Entity Disappears
```

---

## Using ForWrite Everywhere

Wrong:

```csharp
OpenMode.ForWrite
```

for all objects.

Result:

```text
Poor Practice
Reduced Performance
```

---

## Accessing Objects Outside Transaction

Wrong:

```csharp
Object Used After
Transaction Ends
```

Result:

```text
Runtime Errors
```

---

# Chapter Summary

In this chapter we learned:

- What transactions are.
- Why transactions are required.
- How TransactionManager works.
- How to start a transaction.
- How OpenMode works.
- Differences between ForRead and ForWrite.
- The purpose of Commit.
- The purpose of Abort.
- Professional transaction best practices.

---

# Assignment

### Exercise 1

Create a command:

```text
READDATABASE
```

Open the BlockTable using:

```text
OpenMode.ForRead
```

Display a success message.

---

### Exercise 2

Create a command:

```text
CHECKMODELSPACE
```

Access Model Space and display a status message.

---

### Exercise 3

Explain in your own words:

```text
ForRead
ForWrite
Commit
Abort
```

---

### Exercise 4

Draw the transaction lifecycle diagram.

```text
Start
Read
Modify
Commit
```

and

```text
Start
Read
Modify
Abort
```

---

# Interview Questions

1. What is a Transaction?
2. Why are transactions necessary?
3. What is TransactionManager?
4. When should OpenMode.ForRead be used?
5. When should OpenMode.ForWrite be used?
6. What does Commit do?
7. What happens when Commit is omitted?
8. What does Abort do?
9. Why should transactions be kept short?
10. Why is using ForWrite everywhere considered bad practice?

---

# Next Chapter

## Chapter 07 - Creating and Managing Layers

In the next chapter, we will learn:

- LayerTable
- LayerTableRecord
- Creating Layers
- Checking Existing Layers
- Layer Properties
- Assigning Entities to Layers
- Layer Management Automation

We will build our first real-world automation utility by creating and managing drawing layers programmatically.