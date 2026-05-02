# Requirements Document — Order Management System

**Version:** 2.0  
**Date:** April 2026  
**Course:** Software Analysis and Design (SAD) — Ben-Gurion University, Industrial Engineering and Management  
**Project Type:** Desktop Application (Windows Forms / C# / SQL Server)

---

## 1. Project Overview

The system is an internal **Order Management System** for a small retail business. It allows shift managers to manage workers and process customer orders, while regular workers can view the orders assigned to them. The system operates as a single-window desktop application with role-based access control.

---

## 2. Actors

| Actor | Description |
|---|---|
| **Shift Manager** | A senior worker (title: Shift Manager) with full system access. Can manage workers, products, and orders. |
| **Regular Worker** | A worker with limited access. Can view their own assigned orders and order details. |
| **System Administrator** | Manages the database, deploys the application, and configures lookup data. Not a day-to-day user. |

---

## 3. Functional Requirements — User Stories

Each user story is labeled with its implementation status:

- ✅ **Implemented** — fully built in the current version
- 🚧 **Partially Implemented** — data/structure exists but no complete UI
- 🔮 **Future** — planned for a future version, not yet implemented

---

### 3.1 Authentication & Session Management

---

**US-01 — Worker Login** ✅

> As a **worker**, I want to log in using my ID and password, so that I can access the system and see the features appropriate to my role.

**Acceptance Criteria:**
- The system presents a login screen with an ID field and a password field.
- If the ID does not match any worker, an error message is shown.
- If the password is incorrect, a separate error message is shown.
- On successful login, the system routes the user based on their role.

---

**US-02 — Role-Based Routing After Login** ✅

> As a **shift manager**, I want to be taken to the management dashboard after login, so that I can immediately access all management functions.

> As a **regular worker**, I want to be taken directly to my orders list after login, so that I can start working without irrelevant options.

**Acceptance Criteria:**
- Workers with the "Shift Manager" title are routed to the CRUD management panel.
- All other workers are routed to their personal orders view.
- The routing is determined at login time based on the title stored in the database.

---

**US-03 — Logout** ✅

> As a **worker**, I want to be able to log out, so that my session is closed and the login screen is shown for the next user.

**Acceptance Criteria:**
- A "Back" / logout button is available on all main screens.
- Pressing it returns the user to the login screen.
- No user-specific data persists on screen after logout.

---

**US-04 — Change My Password** 🔮

> As a **worker**, I want to change my own password, so that I can maintain the security of my account.

**Acceptance Criteria:**
- Worker navigates to a "Change Password" screen from the main menu.
- Worker must enter their current password before setting a new one.
- New password must be at least 6 characters.
- A confirmation field must match the new password.
- The system saves the updated (hashed) password to the database.
- If the current password is wrong, an error is shown and no change is made.

---

**US-05 — Manager Resets a Worker's Password** 🔮

> As a **shift manager**, I want to reset another worker's password, so that a locked-out worker can regain access without technical support.

**Acceptance Criteria:**
- The manager searches for a worker by ID.
- A "Reset Password" button is shown alongside Update/Delete.
- Clicking it sets the worker's password to a temporary default value.
- The system displays the temporary password to the manager so they can communicate it to the worker.

---

**US-06 — Account Lockout After Failed Login Attempts** 🔮

> As a **system administrator**, I want the system to lock an account after 5 consecutive failed login attempts, so that brute-force attacks are prevented.

**Acceptance Criteria:**
- After 5 consecutive failed password attempts for a given worker ID, the account is locked.
- A locked account cannot log in even with the correct password.
- The manager can unlock an account from the worker management screen.
- A counter of failed attempts is stored in the database and resets on successful login.

---

**US-07 — View My Profile** 🔮

> As a **worker**, I want to view my own profile (name, ID, title), so that I can confirm my account information is correct.

**Acceptance Criteria:**
- A "My Profile" option is accessible from any main screen.
- The profile screen shows: Worker ID, Full Name, Title.
- The worker cannot edit ID or title from this screen (only password via US-04).

---

**US-08 — Session Timeout** 🔮

> As a **system administrator**, I want inactive sessions to automatically expire after 30 minutes, so that unattended terminals do not remain logged in.

**Acceptance Criteria:**
- A background timer tracks time since the last user interaction.
- After 30 minutes of inactivity, the system returns to the login screen.
- Any unsaved data is discarded with a brief warning message before logout.

---

### 3.2 Worker Management

---

**US-09 — Add a New Worker** ✅

> As a **shift manager**, I want to add new workers to the system, so that they can log in and be assigned to orders.

**Acceptance Criteria:**
- The manager navigates to the "Create Worker" screen from the management panel.
- The form requires a Worker ID, Full Name, and Title (from a dropdown).
- ID and Name are mandatory; an error is shown if either is empty.
- The worker is saved to the database and immediately available in the in-memory list.
- After saving, the system returns to the management panel.

---

**US-10 — Search for a Worker by ID** ✅

> As a **shift manager**, I want to search for a worker by their ID, so that I can view or modify their details.

**Acceptance Criteria:**
- The "Update / Delete Worker" screen includes an ID search field and a Search button.
- If a matching worker is found, their name and title are displayed for editing.
- If no match is found, an error message is displayed.

---

**US-11 — Update Worker Details** ✅

> As a **shift manager**, I want to update a worker's name or title, so that the system reflects changes in their role or personal information.

**Acceptance Criteria:**
- After finding a worker (US-10), the name and title fields become editable.
- Clicking "Update" saves changes to both the database and the in-memory list.
- The system returns to the management panel after saving.

---

**US-12 — Delete a Worker** ✅

> As a **shift manager**, I want to delete a worker, so that former employees no longer appear in the system.

**Acceptance Criteria:**
- After finding a worker (US-10), a "Delete" button is shown.
- Clicking "Delete" removes the worker from the database and the in-memory list.
- All orders associated with that worker are also removed (cascading delete).
- The system returns to the management panel after deletion.

---

**US-13 — Assign a Role Title to a Worker** ✅

> As a **shift manager**, when creating or updating a worker, I want to assign a role from a fixed list of titles, so that roles are consistent across the system.

**Acceptance Criteria:**
- Available titles are: Shift Manager, Team Leader, New Worker.
- Titles are presented in a dropdown (ComboBox).
- The selected title is stored in the database and in-memory.

---

**US-14 — View All Workers** 🔮

> As a **shift manager**, I want to see a list of all workers, so that I can get an overview of the team without searching one by one.

**Acceptance Criteria:**
- A "View Workers" screen shows all workers in a table.
- Columns: Worker ID, Name, Title.
- Clicking a row navigates to that worker's update/delete screen.

---

**US-15 — Search Workers by Name** 🔮

> As a **shift manager**, I want to search for workers by name, so that I can find a worker even if I do not know their ID.

**Acceptance Criteria:**
- A name search field is available on the workers list screen.
- The table filters to show only workers whose name contains the search text.
- Clearing the search field restores the full list.

---

**US-16 — Filter Workers by Title** 🔮

> As a **shift manager**, I want to filter the workers list by title, so that I can quickly see all workers of a specific role.

**Acceptance Criteria:**
- A "Filter by Title" dropdown is available on the workers list screen.
- Selecting a title filters the table to show only workers with that title.
- A "Show All" option clears the filter.

---

**US-17 — Deactivate a Worker (Soft Delete)** 🔮

> As a **shift manager**, I want to deactivate a worker instead of permanently deleting them, so that their historical order data is preserved while they can no longer log in.

**Acceptance Criteria:**
- Workers have an "active / inactive" status field in the database.
- Deactivated workers cannot log in.
- Deactivated workers do not appear in worker dropdowns when creating orders.
- The manager can view deactivated workers via a "Show Inactive" toggle on the workers list.
- A deactivated worker can be reactivated.

---

**US-18 — View a Worker's Full Order History** 🔮

> As a **shift manager**, I want to view all orders ever assigned to a specific worker, so that I can assess their workload and history.

**Acceptance Criteria:**
- From the workers list, clicking "View Orders" for a worker opens a filtered orders view.
- The view shows all orders (all dates, all types) for that worker.
- The manager can click any order to see its full details.

---

### 3.3 Order Management

---

**US-19 — Create a Delivery Order** ✅

> As a **shift manager**, I want to create a delivery order assigned to a worker, so that the order is recorded with a delivery address and expected delivery date.

**Acceptance Criteria:**
- The manager selects a worker from a dropdown of all workers.
- The order ID is auto-generated and displayed (read-only).
- Required fields: order date, total price, delivery address, expected delivery date.
- All fields are mandatory; an error is shown for any missing field.
- The order is saved to the `Orders` and `DeliveryOrders` tables and added to the in-memory lists.
- The system returns to the management panel after saving.

---

**US-20 — Create a Pickup Order** ✅

> As a **shift manager**, I want to create a pickup order so that a customer can collect their order from a specific branch at a scheduled time.

**Acceptance Criteria:**
- Required fields: worker (dropdown), order date, total price, branch location, pickup date and time.
- All fields are mandatory.
- The order is saved to the `Orders` and `PickupOrders` tables.
- The system returns to the management panel after saving.

---

**US-21 — View My Assigned Orders** ✅

> As a **regular worker**, after logging in, I want to see all orders assigned to me, so that I know what I am responsible for.

**Acceptance Criteria:**
- The orders panel loads automatically for non-manager workers after login.
- Columns: Order ID, Order Date, Total Price, Order Type, Type-Specific Details.
- Only orders assigned to the logged-in worker are shown.

---

**US-22 — View Order Details** ✅

> As a **worker**, I want to click on an order to see its full details including the products in it, so that I have all the information I need.

**Acceptance Criteria:**
- Clicking any row in the orders list opens an Order Details screen.
- Shows: Order ID, Date, Total Price, assigned worker name, and type-specific details (address/branch/time).
- A product table shows each item: Product ID, Product Name, Category, Quantity, Unit Price, Line Total.
- A "Back" button returns to the orders list.

---

**US-23 — View All Orders (Manager View)** 🔮

> As a **shift manager**, I want to see all orders across all workers in a single table, so that I have a complete picture of business activity.

**Acceptance Criteria:**
- A "View All Orders" screen is accessible from the management panel.
- Columns: Order ID, Date, Total Price, Worker Name, Order Type, Details.
- The manager can click any row to see full order details.

---

**US-24 — Search for an Order by ID** 🔮

> As a **shift manager**, I want to search for a specific order by its ID, so that I can quickly retrieve it without scrolling.

**Acceptance Criteria:**
- A search field accepts an Order ID.
- Clicking "Search" filters the table to show only the matching order.
- If no match is found, an appropriate message is shown.

---

**US-25 — Filter Orders by Date Range** 🔮

> As a **shift manager**, I want to filter orders by a date range, so that I can focus on orders relevant to a specific period.

**Acceptance Criteria:**
- "From Date" and "To Date" date pickers are available on the orders screen.
- Clicking "Filter" shows only orders whose order date falls within the range.
- Clicking "Clear" restores all orders.

---

**US-26 — Filter Orders by Order Type** 🔮

> As a **shift manager**, I want to filter orders by type (Delivery / Pickup), so that I can manage each type separately.

**Acceptance Criteria:**
- A "Type" dropdown contains: All, Delivery, Pickup.
- Selecting a type filters the table accordingly.

---

**US-27 — Filter Orders by Assigned Worker** 🔮

> As a **shift manager**, I want to filter the orders list by worker, so that I can quickly see all orders belonging to a specific person.

**Acceptance Criteria:**
- A "Worker" dropdown on the orders screen lists all workers.
- Selecting a worker filters the table to show only their orders.
- A "Show All" option clears the filter.

---

**US-28 — Update an Existing Order** 🔮

> As a **shift manager**, I want to update the details of an existing order, so that I can correct mistakes or reflect changes requested by the customer.

**Acceptance Criteria:**
- From the order details screen, an "Edit" button makes the fields editable.
- Editable fields: order date, total price, delivery address / branch / pickup time (based on type).
- The order type cannot be changed after creation.
- Changes are saved to both the database and the in-memory list.

---

**US-29 — Cancel (Delete) an Order** 🔮

> As a **shift manager**, I want to cancel an order, so that erroneous or cancelled customer orders are removed.

**Acceptance Criteria:**
- A "Cancel Order" button is available on the order details screen.
- The system asks for confirmation before deleting.
- All order items are deleted first, then the subtype record, then the parent order record.
- The order is removed from both the database and the in-memory list.

---

**US-30 — Track Order Status** 🔮

> As a **shift manager**, I want each order to have a status (Pending, In Progress, Completed, Cancelled), so that I can track where each order is in its lifecycle.

**Acceptance Criteria:**
- Orders have a `status` field with values: Pending, In Progress, Completed, Cancelled.
- New orders are created with "Pending" status.
- The status is displayed in all order lists and the detail screen.
- The manager can change the status from the order details screen.

---

**US-31 — Mark an Order as Completed** ✅

> As a **worker**, I want to mark one of my orders as "Completed" once I have fulfilled it, so that the system reflects real-world progress.

**Acceptance Criteria:**
- A "Mark as Completed" button is available on the order details screen for the assigned worker.
- Clicking it changes the order status to "Completed" and saves the change.
- Completed orders are visually distinguished in the orders list (e.g., greyed out or tagged).

---

**US-32 — Duplicate an Order** 🔮

> As a **shift manager**, I want to duplicate an existing order as a starting point for a new one, so that I can save time when creating similar orders.

**Acceptance Criteria:**
- A "Duplicate" button on the order details screen creates a new order pre-filled with the same worker, type, and items.
- The new order gets a new auto-generated ID and today's date.
- The manager can edit the pre-filled values before saving.

---

**US-33 — Print an Order** 🔮

> As a **shift manager**, I want to print the details of an order, so that a physical copy can accompany a delivery or be filed.

**Acceptance Criteria:**
- A "Print" button on the order details screen triggers the system print dialog.
- The printed output includes: Order ID, Date, Worker Name, Order Type, address/branch/pickup time, and the itemized product list.

---

**US-34 — Reassign an Order to a Different Worker** 🔮

> As a **shift manager**, I want to reassign an order from one worker to another, so that workloads can be balanced or absences covered.

**Acceptance Criteria:**
- From the order edit screen, the worker dropdown is editable.
- Selecting a different worker and saving updates the `workerId` on the order in both the database and the in-memory list.
- The order disappears from the original worker's view and appears in the new worker's view.

---

### 3.4 Product Catalog

---

**US-35 — Load Product Catalog at Startup** 🚧

> As a **worker**, I want the product catalog to be available as soon as I log in, so that product information is immediately accessible when I view order details.

**Acceptance Criteria:**
- All products are loaded from the `Products` table into memory at application startup.
- Product data is accessible throughout the application via the in-memory list.
- *(A dedicated product-browsing screen is not yet implemented.)*

---

**US-36 — View the Full Product Catalog** 🔮

> As a **shift manager**, I want to see all products in a table, so that I can review the current catalog.

**Acceptance Criteria:**
- A "Products" screen is accessible from the management panel.
- Columns: Product ID, Name, Category, Price.
- Clicking a row opens the product details view.

---

**US-37 — Add a New Product** 🔮

> As a **shift manager**, I want to add a new product to the catalog, so that it becomes available for inclusion in orders.

**Acceptance Criteria:**
- A "Create Product" form accepts: Product ID, Name, Price, and Category.
- All fields are mandatory.
- The product is saved to the `Products` table and added to the in-memory list.

---

**US-38 — Update Product Details** 🔮

> As a **shift manager**, I want to update a product's name, price, or category, so that the catalog stays accurate.

**Acceptance Criteria:**
- From the product details screen, the manager can edit Name, Price, and Category.
- Product ID cannot be changed after creation.
- Changes are saved to the database and in-memory list.

---

**US-39 — Delete a Product** 🔮

> As a **shift manager**, I want to delete a product from the catalog, so that discontinued items are no longer available.

**Acceptance Criteria:**
- A product can only be deleted if it is not referenced by any existing order item.
- If the product is in use, an error message explains why it cannot be deleted.
- On successful deletion, the product is removed from the database and in-memory list.

---

**US-40 — Search for a Product by Name** 🔮

> As a **shift manager**, I want to search for a product by name, so that I can quickly find it in a large catalog.

**Acceptance Criteria:**
- A search field on the products screen filters the table by product name (partial match).
- Clearing the field restores the full catalog.

---

**US-41 — Filter Products by Category** 🔮

> As a **shift manager**, I want to filter products by category, so that I can browse only the relevant subset.

**Acceptance Criteria:**
- A "Category" dropdown on the products screen lists all existing categories.
- Selecting a category filters the table.
- A "Show All" option clears the filter.

---

**US-42 — Add Products to an Order** 🔮

> As a **shift manager**, when creating an order, I want to add one or more products with quantities, so that the order has an itemized product list.

**Acceptance Criteria:**
- The order creation screens include a product selection section.
- Products are selected from a dropdown of the current catalog.
- The manager enters a quantity for each product.
- Unit price is auto-filled from the catalog but can be overridden.
- Multiple products can be added to a single order.
- Items are saved to the `OrderItems` table.

---

**US-43 — Remove a Product from an Order** 🔮

> As a **shift manager**, I want to remove a product line from an order, so that incorrect items can be corrected.

**Acceptance Criteria:**
- On the order edit screen, each order item has a "Remove" button.
- Removing an item deletes it from the `OrderItems` table and the in-memory list.
- The order total updates immediately to reflect the removal.

---

**US-44 — Update Product Quantity in an Order** 🔮

> As a **shift manager**, I want to change the quantity of a product within an order, so that corrections can be made without removing and re-adding the item.

**Acceptance Criteria:**
- On the order edit screen, quantity fields for each order item are editable.
- Updating a quantity saves the change to the `OrderItems` table and recalculates the line total and order total.

---

**US-45 — Auto-Calculate Order Total from Items** 🔮

> As a **shift manager**, when I add or update products in an order, I want the total price to be calculated automatically, so that I do not have to enter it manually.

**Acceptance Criteria:**
- The order total is calculated as the sum of (quantity × unit price) for all order items.
- The total updates in real time as items are added, removed, or updated.
- The auto-calculated total can be manually overridden if needed.

---

**US-46 — Apply a Discount to an Order** 🔮

> As a **shift manager**, I want to apply a percentage or fixed discount to an order, so that promotional pricing can be reflected.

**Acceptance Criteria:**
- An optional "Discount" field is available on the order creation/edit screen.
- Discount can be entered as a percentage (0–100%) or a fixed amount.
- The final total reflects the discount and is stored in the database.

---

### 3.5 Branch & Logistics

---

**US-47 — Manage Branch Locations** 🔮

> As a **system administrator**, I want to maintain a list of valid branch locations, so that pickup orders use consistent, standardized branch names.

**Acceptance Criteria:**
- A "Branches" management screen allows adding, updating, and deleting branch names.
- Branch names are stored in a `Branches` lookup table in the database.
- The branch dropdown in the Create Pickup Order screen is populated from this table.

---

**US-48 — View Upcoming Deliveries** 🔮

> As a **shift manager**, I want to see all delivery orders with a delivery date in the next 7 days, so that I can plan logistics proactively.

**Acceptance Criteria:**
- An "Upcoming Deliveries" view shows all delivery orders whose `deliveryDate` is within the next 7 days.
- Columns: Order ID, Worker Name, Delivery Address, Delivery Date.
- Orders are sorted by delivery date (soonest first).

---

**US-49 — View Scheduled Pickups** 🔮

> As a **shift manager**, I want to see all pickup orders scheduled for today and tomorrow, so that staff can be ready at the right branches.

**Acceptance Criteria:**
- A "Today's Pickups" view shows pickup orders with a `pickupTime` in the next 48 hours.
- Columns: Order ID, Worker Name, Branch Location, Pickup Time.
- Orders are sorted by pickup time.

---

**US-50 — Flag Overdue Deliveries** 🔮

> As a **shift manager**, I want the system to highlight orders whose expected delivery date has passed and are not yet completed, so that I can follow up immediately.

**Acceptance Criteria:**
- Delivery orders whose `deliveryDate` is in the past and whose status is not "Completed" or "Cancelled" are highlighted in the orders list (e.g., displayed in red).
- A dedicated "Overdue" filter on the orders screen shows only these orders.

---

### 3.6 Reporting & Analytics

---

**US-51 — View an Order Summary Dashboard** 🔮

> As a **shift manager**, I want to see a summary of key metrics (total orders, total revenue, breakdown by type), so that I can monitor business performance at a glance.

**Acceptance Criteria:**
- A "Reports" screen shows: total number of orders, total revenue, count of delivery orders, count of pickup orders, count of completed orders, count of pending orders.
- Metrics are calculated from the in-memory lists and update on screen refresh.

---

**US-52 — View Orders per Worker Report** 🔮

> As a **shift manager**, I want to see how many orders are assigned to each worker, so that I can identify uneven workload distribution.

**Acceptance Criteria:**
- A table shows each worker's name alongside their total number of assigned orders and total revenue.
- Sorted by order count (descending) by default.

---

**US-53 — View Revenue per Product Report** 🔮

> As a **shift manager**, I want to see which products generate the most revenue, so that I can make informed pricing and purchasing decisions.

**Acceptance Criteria:**
- A table shows each product with: total quantity sold and total revenue generated.
- Sorted by total revenue (descending) by default.

---

**US-54 — View Daily Orders Summary** 🔮

> As a **shift manager**, I want to see the total number of orders and total revenue for each day in a selected month, so that I can identify busy and slow days.

**Acceptance Criteria:**
- A month picker selects the target month.
- A table shows, for each day: number of orders and total revenue.
- Days with no orders are shown as zero rather than omitted.

---

**US-55 — View Top Products Report** 🔮

> As a **shift manager**, I want to see the top 5 best-selling products, so that I can quickly identify the highest-demand items.

**Acceptance Criteria:**
- The report shows the 5 products with the highest total quantity sold.
- Displayed as a ranked list with: product name, total units sold, total revenue.

---

**US-56 — Export Orders to CSV** 🔮

> As a **shift manager**, I want to export the orders list to a CSV file, so that I can analyze the data in Excel or share it with others.

**Acceptance Criteria:**
- An "Export to CSV" button is available on the orders screen.
- Clicking it opens a save-file dialog.
- The exported file contains all visible columns of the current filtered table.
- The file uses UTF-8 encoding to support Hebrew text.

---

**US-57 — Export a Report to PDF** 🔮

> As a **shift manager**, I want to export any report to a PDF file, so that I can print or archive it in a standardized format.

**Acceptance Criteria:**
- An "Export to PDF" button is available on report screens.
- The PDF mirrors the on-screen layout including the report title, date, and table.
- The PDF is generated without requiring an external service.

---

**US-58 — Filter Reports by Date Range** 🔮

> As a **shift manager**, when viewing any report, I want to filter the data by a date range, so that the report reflects only the period I care about.

**Acceptance Criteria:**
- All report screens include "From Date" and "To Date" date pickers.
- Selecting a range and clicking "Apply" recalculates the report for that period.
- The current date range is shown as a subtitle on the report.

---

### 3.7 System Administration & Configuration

---

**US-59 — Add a New Worker Title** 🔮

> As a **system administrator**, I want to add new worker titles to the lookup table, so that the system can accommodate new role types without a code change.

**Acceptance Criteria:**
- A "Manage Titles" screen lists all existing titles.
- The administrator can add a new title by entering its name.
- The new title appears immediately in all worker title dropdowns.
- Titles currently assigned to at least one worker cannot be deleted.

---

**US-60 — Add a Product Category** 🔮

> As a **system administrator**, I want to maintain a list of product categories, so that categories are standardized and reusable across all products.

**Acceptance Criteria:**
- A "Manage Categories" screen allows adding, renaming, and deleting categories.
- Categories are stored in a `Categories` lookup table.
- The category dropdown on the product form is populated from this table.
- Categories assigned to existing products cannot be deleted.

---

**US-61 — View Audit Log** 🔮

> As a **system administrator**, I want to see a log of all create, update, and delete operations, so that I can trace who did what and when.

**Acceptance Criteria:**
- Every Create, Update, and Delete action writes a record to an `AuditLog` table: timestamp, worker ID (actor), entity type, entity ID, action type, and a description.
- An "Audit Log" screen displays this table with filtering by date range and actor.

---

**US-62 — View Currently Loaded Data Summary** 🔮

> As a **system administrator**, I want to see how many records are in each in-memory list at startup, so that I can quickly verify that the database loaded correctly.

**Acceptance Criteria:**
- A "System Info" screen shows the current count of each in-memory list: Workers, Products, Orders, Order Items.
- The screen also shows the time taken to load each list at startup.

---

**US-63 — Database Connection Configuration** 🔮

> As a **system administrator**, I want to configure the database connection string from the application's settings file, so that the application can be deployed to different environments without recompiling.

**Acceptance Criteria:**
- The connection string is stored in `app.config` and read at runtime.
- If the database is unreachable at startup, a clear error message is shown with the connection details used (excluding the password), and the application exits gracefully.

---

**US-64 — Backup Data Export** 🔮

> As a **system administrator**, I want to export all system data to a structured file, so that a backup can be taken without direct SQL Server access.

**Acceptance Criteria:**
- A "Backup" option in system settings exports all in-memory data to a JSON or XML file.
- The file includes all entities with all their fields.
- A timestamp is appended to the filename automatically.

---

**US-65 — Restore Data from Backup** 🔮

> As a **system administrator**, I want to restore system data from a previously exported backup file, so that the system can be recovered after data loss.

**Acceptance Criteria:**
- A "Restore" option reads a backup file produced by US-64.
- Before restoring, the system warns that current data will be overwritten and asks for confirmation.
- The restore operation clears all current tables and re-inserts data from the file.
- After restore, the in-memory lists are reloaded from the database.

---

## 4. Non-Functional Requirements

### 4.1 Performance

| ID | Requirement | Status |
|---|---|---|
| NFR-01 | All data (workers, products, orders, order items) shall be loaded from the database into memory at startup. Subsequent reads shall operate from memory with no additional DB calls. | ✅ Implemented |
| NFR-02 | The startup load shall complete in under 3 seconds on a local SQL Server instance. | ✅ Implemented |
| NFR-03 | Any user action (button click, panel navigation) shall respond within 500 milliseconds under normal load. | ✅ Implemented |
| NFR-04 | The system shall support at least 1,000 products and 10,000 orders without degradation of startup or UI performance. | 🔮 Future |

### 4.2 Security

| ID | Requirement | Status |
|---|---|---|
| NFR-05 | Access to management functions shall be restricted to workers with the "Shift Manager" role. | ✅ Implemented |
| NFR-06 | All database operations shall use parameterized Stored Procedures to prevent SQL injection. | ✅ Implemented |
| NFR-07 | Passwords shall be stored in hashed form (e.g., bcrypt or SHA-256 with salt) in the database — never in plain text. | 🔮 Future |
| NFR-08 | A session shall automatically expire after 30 minutes of inactivity (see US-08). | 🔮 Future |
| NFR-09 | An account shall be locked after 5 consecutive failed login attempts (see US-06). | 🔮 Future |

### 4.3 Usability

| ID | Requirement | Status |
|---|---|---|
| NFR-10 | The application shall use a single-window panel-based navigation model. No additional windows shall open during normal operation. | ✅ Implemented |
| NFR-11 | Every screen shall include a "Back" button that returns to the previous screen. | ✅ Implemented |
| NFR-12 | All input validation errors shall be communicated via a clearly worded message box. | ✅ Implemented |
| NFR-13 | The user interface shall support Hebrew text throughout (labels, data entry, and database values). | ✅ Implemented |
| NFR-14 | All dropdown lists (titles, workers, products, branches) shall be sorted alphabetically or by a logical default order. | 🔮 Future |

### 4.4 Reliability & Data Integrity

| ID | Requirement | Status |
|---|---|---|
| NFR-15 | All writes to the database shall use Stored Procedures. No ad-hoc SQL strings shall be constructed in application code. | ✅ Implemented |
| NFR-16 | Every Create, Update, and Delete operation must update both the database and the in-memory list atomically within the same method. | ✅ Implemented |
| NFR-17 | Referential integrity shall be enforced at the database level using Foreign Key constraints. | ✅ Implemented |
| NFR-18 | Deleting a worker shall automatically cascade to delete all associated orders and order items. | ✅ Implemented |
| NFR-19 | The system shall handle database connection failures at startup gracefully, displaying an actionable error message rather than crashing silently. | 🔮 Future |

### 4.5 Maintainability

| ID | Requirement | Status |
|---|---|---|
| NFR-20 | Each entity class shall be responsible for its own CRUD operations, initialization (loading from DB), and search (self-contained pattern). | ✅ Implemented |
| NFR-21 | Inheritance shall be implemented using the Table-per-Subclass pattern (`DeliveryOrders` and `PickupOrders` tables each hold only their unique fields plus a FK to `Orders`). | ✅ Implemented |
| NFR-22 | Many-to-many relationships shall be implemented using an Association Class (OrderItem) both in C# and in the database. | ✅ Implemented |
| NFR-23 | The system shall be extensible to support new order types by adding a new subclass and table, without modifying existing entity classes. | 🔮 Future |
| NFR-24 | The connection string shall be stored in configuration (`app.config`), not hardcoded in source code. | 🚧 Partial |

### 4.6 Technology Constraints

| ID | Requirement |
|---|---|
| NFR-25 | The application shall be built using **C# / .NET 8** and **Windows Forms**. |
| NFR-26 | The database shall be **Microsoft SQL Server** (any edition, including Express). |
| NFR-27 | The development environment shall be **Visual Studio 2022**. |
| NFR-28 | The application shall run on Windows 10 or later. |

---

## 5. Requirements Traceability Matrix

| User Story | Status | UC ID | Key Files |
|---|---|---|---|
| US-01 Login | ✅ | NFR | `LoginPanel.cs`, `Worker.cs` (seekWorker) |
| US-02 Role-Based Routing | ✅ | NFR | `LoginPanel.cs`, `mainForm.cs` (showPanel) |
| US-03 Logout | ✅ | NFR | All panels (back_Click → LoginPanel) |
| US-04 Change Password | 🔮 | — | — |
| US-05 Manager Reset Password | 🔮 | — | — |
| US-06 Account Lockout | 🔮 | — | — |
| US-07 View My Profile | 🔮 | — | — |
| US-08 Session Timeout | 🔮 | — | — |
| US-09 Add Worker | ✅ | UC-06 | `CreateWorkerPanel.cs`, `Worker.cs` (createWorker) |
| US-10 Search Worker by ID | ✅ | UC-09 | `UpdateDeletePanel.cs`, `Worker.cs` (seekWorker) |
| US-11 Update Worker | ✅ | UC-07 | `UpdateDeletePanel.cs`, `Worker.cs` (updateWorker) |
| US-12 Delete Worker | ✅ | UC-08 | `UpdateDeletePanel.cs`, `Worker.cs` (deleteWorker) |
| US-13 Worker Titles | ✅ | UC-11 | `Title.cs`, `TitleHelper`, `Titles` DB table |
| US-14 View All Workers | 🔮 | — | — |
| US-15 Search Workers by Name | 🔮 | — | — |
| US-16 Filter Workers by Title | 🔮 | — | — |
| US-17 Deactivate Worker | 🔮 | — | — |
| US-18 Worker Order History | 🔮 | — | — |
| US-19 Create Delivery Order | ✅ | UC-04 | `CreateDeliveryOrderPanel.cs`, `DeliveryOrder.cs` |
| US-20 Create Pickup Order | ✅ | UC-05 | `CreatePickupOrderPanel.cs`, `PickupOrder.cs` |
| US-21 View My Orders | ✅ | UC-01 | `WatchOrdersPanel.cs`, `Order.cs` (getOrders) |
| US-22 View Order Details | ✅ | UC-02 | `OrderDetailsPanel.cs`, `OrderItem.cs`, `Product.cs` |
| US-23 View All Orders (Manager) | 🔮 | — | — |
| US-24 Search Order by ID | 🔮 | — | — |
| US-25 Filter Orders by Date | 🔮 | — | — |
| US-26 Filter by Order Type | 🔮 | — | — |
| US-27 Filter by Worker | 🔮 | — | — |
| US-28 Update Order | 🔮 | — | — |
| US-29 Cancel Order | 🔮 | — | — |
| US-30 Order Status Tracking | 🔮 | — | — |
| US-31 Mark Order Completed | ✅ | UC-03 | `OrderDetailsPanel.cs`, `Order.cs` (SP_UpdateOrderStatus) |
| US-32 Duplicate Order | 🔮 | — | — |
| US-33 Print Order | 🔮 | — | — |
| US-34 Reassign Order | 🔮 | — | — |
| US-35 Load Product Catalog | 🚧 | — | `Product.cs` (initProducts), `Program.cs` |
| US-36 View Product Catalog | 🔮 | UC-10 | — |
| US-37 Add Product | 🔮 | UC-10 | `Product.cs` (structure), `SP_add_product` |
| US-38 Update Product | 🔮 | UC-10 | — |
| US-39 Delete Product | 🔮 | UC-10 | — |
| US-40 Search Product by Name | 🔮 | UC-10 | — |
| US-41 Filter Products by Category | 🔮 | UC-10 | — |
| US-42 Add Products to Order | 🔮 | — | `OrderItem.cs` (structure), `SP_add_order_item` |
| US-43 Remove Product from Order | 🔮 | — | — |
| US-44 Update Product Quantity | 🔮 | — | — |
| US-45 Auto-Calculate Order Total | 🔮 | — | — |
| US-46 Apply Discount | 🔮 | — | — |
| US-47 Manage Branch Locations | 🔮 | — | — |
| US-48 View Upcoming Deliveries | 🔮 | — | — |
| US-49 View Scheduled Pickups | 🔮 | — | — |
| US-50 Flag Overdue Deliveries | 🔮 | — | — |
| US-51 Order Summary Dashboard | 🔮 | — | — |
| US-52 Orders per Worker Report | 🔮 | — | — |
| US-53 Revenue per Product | 🔮 | — | — |
| US-54 Daily Orders Summary | 🔮 | — | — |
| US-55 Top Products Report | 🔮 | — | — |
| US-56 Export to CSV | 🔮 | — | — |
| US-57 Export to PDF | 🔮 | — | — |
| US-58 Filter Reports by Date | 🔮 | — | — |
| US-59 Add Worker Title | 🔮 | UC-11 | `Titles` DB table (structure exists) |
| US-60 Add Product Category | 🔮 | — | — |
| US-61 Audit Log | 🔮 | — | — |
| US-62 Loaded Data Summary | 🔮 | — | — |
| US-63 DB Connection Config | 🔮 | — | `app.config` (partial) |
| US-64 Backup Export | 🔮 | — | — |
| US-65 Restore from Backup | 🔮 | — | — |

---

## 6. Summary

| Category | Total | ✅ Implemented | 🚧 Partial | 🔮 Future |
|---|---|---|---|---|
| Authentication & Session | 8 | 3 | 0 | 5 |
| Worker Management | 10 | 5 | 0 | 5 |
| Order Management | 16 | 5 | 0 | 11 |
| Product Catalog | 12 | 0 | 1 | 11 |
| Branch & Logistics | 4 | 0 | 0 | 4 |
| Reporting & Analytics | 8 | 0 | 0 | 8 |
| System Administration | 7 | 0 | 0 | 7 |
| **Total User Stories** | **65** | **13** | **1** | **51** |
| Non-Functional Requirements | 28 | 16 | 1 | 11 |

The current version (v1.0) delivers a working core system covering authentication, worker management, order creation (delivery and pickup), and order viewing. The 52 future user stories represent a well-structured roadmap for subsequent iterations, organized across the full order lifecycle, product catalog management, logistics, reporting, and system administration.

