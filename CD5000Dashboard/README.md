# CD5000 Dashboard (Blazor Server)

## Overview

This project is a **Blazor Server dashboard application** that connects to a SQLCipher-encrypted SQLite database and provides:

* Operator-focused summary analytics
* A read-only table browser for database inspection

---

## Features

### Dashboard Analytics

* Total transaction count
* Transactions by hour (table + chart)
* Rail cars missing RFID tags
* Average transaction duration by vehicle
* Average transaction duration by unit train
* Top products by transaction count

### Table Browser

* Browse all database tables
* View table contents dynamically
* Search/filter table data
* Read-only access (no editing)

---

## Technologies Used

* .NET / Blazor Server
* C#
* Dapper
* Microsoft.Data.Sqlite
* SQLitePCLRaw (SQLCipher support)
* Chart.js
* Bootstrap

---

## Database Setup

This application uses a SQLCipher-encrypted SQLite database.

### Steps

1. Place the database file in the following folder:

   ```
   CD5000Dashboard/DataFiles/
   ```

2. Ensure the file name matches:

   ```
   cd5000.Test.sqlcipher4.db
   ```

3. In Visual Studio:

   * Right-click the file
   * Set **Copy to Output Directory → Copy if newer**


## Running the Application

1. Clone the repository
2. Open the solution in Visual Studio
3. Ensure the database file is placed correctly
4. Run the application (`F5`)

The dashboard will open in your browser.

---

## Notes

* The application is intentionally **read-only**
* Database access is handled via repository/service layers
* Chart.js is used for data visualization

---

## Summary

This project demonstrates:

* Blazor Server development
* Secure database connectivity using SQLCipher
* Data querying with Dapper
* UI design for reporting and analytics
* Clean separation of concerns using services and models

---
