# Use Case Specifications — Order Management System

**Phase:** Analysis (Phase A)  
**Version:** 2.1 | **Date:** May 2026  
**Course:** Software Analysis and Design — Ben-Gurion University

> **Purpose of this document:**  
> This file is the primary analytical input for the development phase. It maps each functional requirement to a Use Case and provides detailed VP18-style specifications for 5 representative UCs. An AI coding agent given any single UC specification below has sufficient information to generate the corresponding panel, entity method, and stored procedure without additional clarification.

> **How to read the specs:**  
> Each detailed UC has two layers. The formal spec (preconditions, flow of events, extensions) is written at the **analysis level** — behavioral and technology-neutral. A clearly marked **Implementation Notes** section below each spec maps the behavioral steps to specific classes, methods, and stored procedures. This separation reflects the order of thinking: understand *what* the system must do before deciding *how* to build it.

---

## 0. Authentication — Non-Functional Requirement

**Login/Logout are NOT use cases in this system.**

Authentication is a cross-cutting non-functional requirement (security). The `LoginPanel` is a technical precondition that runs before any actor can reach a functional panel. It is recorded here as an architectural note, not as a use case:

- The application starts at `LoginPanel`.
- The system verifies the worker's identity using their ID and password.
- On success: the authenticated worker is held in session; the system navigates to the management panel (Shift Manager) or the orders panel (Worker).
- All UC preconditions below may assume the actor is already authenticated.

---

## 1. Actors

| Actor | Type | Role in the System |
|---|---|---|
| **Worker** | Human — Primary (left) | Views assigned orders and order details. Can mark orders as completed. |
| **Shift Manager** | Human — Primary (right) | Extends Worker via actor generalization. Additionally creates orders and manages workers and the product catalog. |
| **System Admin** | Human — Secondary (right) | Manages lookup master data: worker titles. |

**Actor Generalization:** Shift Manager IS-A Worker. Shift Manager inherits all Worker use cases and adds management-specific ones.

---

## 2. Use Case Inventory

| UC ID | Diagram ID | Name | Primary Actor | Status | Detailed Spec |
|---|---|---|---|---|---|
| UC-01 | uc-view-orders | View My Orders | Worker | ✅ Implemented | — |
| UC-02 | uc-view-details | View Order Details | Worker | ✅ Implemented | ✅ §5.3 |
| UC-03 | uc-mark-complete | Mark Order as Completed | Worker | ✅ Implemented | ✅ §5.1 |
| UC-04 | uc-create-delivery | Create Delivery Order | Shift Manager | ✅ Implemented | ✅ §5.2 |
| UC-05 | uc-create-pickup | Create Pickup Order | Shift Manager | ✅ Implemented | — |
| UC-06 | uc-add-worker | Add New Worker | Shift Manager | ✅ Implemented | — |
| UC-07 | uc-update-worker | Update Worker Details | Shift Manager | ✅ Implemented | ✅ §5.4 (CRUD #1) |
| UC-08 | uc-delete-worker | Delete Worker | Shift Manager | ✅ Implemented | — |
| UC-09 | uc-find-worker | Find Worker by ID | Shift Manager | ✅ Implemented | — (include target) |
| UC-10 | uc-manage-product | Manage Product Catalog | Shift Manager | 🔮 Future | ✅ §5.5 (CRUD #2) |
| UC-11 | uc-manage-titles | Manage Worker Titles | System Admin | 🔮 Future | — |

---

## 3. UC Diagram

See [order_management_use_case_diagram.html](../order_management_use_case_diagram.html) for the interactive visual diagram.

The diagram is split into two tabs because the total UC count (11) exceeds 8:

- **Tab 1 — Orders:** Worker, Shift Manager
- **Tab 2 — Worker & Catalog Management:** Shift Manager, System Admin

---

## 4. UC Relationships

### 4.1 Actor Generalization

```
Shift Manager ──▷ Worker
```

Shift Manager IS-A Worker. Arrow direction: child to parent (hollow triangle at parent end).

### 4.2 «include»

**UC-09: Find Worker by ID** is an `«include»` target used by:
- UC-07: Update Worker Details
- UC-08: Delete Worker

**Justification:** Both Update and Delete require the Shift Manager to first locate the worker by ID. This sub-behavior is *mandatory* (always executes) and *shared* by exactly two base UCs. UC-09 has no direct actor association — it is a sub-behavior only.

### 4.3 «extend»

**UC-03: Mark Order as Completed** `«extend»`s UC-02: View Order Details.

**Justification:** The base UC (View Order Details) runs correctly and completely without this extension. The extension is conditional — it occurs only when the assigned worker *decides* to mark the order complete while viewing it, and only when the order status is "Open". The base UC does not depend on this action.

---

## 5. Detailed Use Case Specifications

The five UCs below follow the Visual Paradigm 18 specification format as taught in class. For each UC, this document provides everything an AI coding agent needs to generate the implementation: actor, preconditions, step-by-step flow of events, extensions (error handling), and user stories with acceptance criteria.

---

### 5.1 UC-03 — Mark Order as Completed

| Field | Value |
|---|---|
| **UC ID** | UC-03 |
| **Diagram ID** | uc-mark-complete |
| **Name** | Mark Order as Completed |
| **Primary Actor** | Worker |
| **Secondary Actors** | — |
| **Status** | ✅ Implemented |
| **Linked USs** | US-31 |
| **Relationship** | «extend» of UC-02 (View Order Details) |
| **Raised by** | דן מזרחי (עובד שטח) — ראיון 2; רינת לוי (מנהלת משמרת) — ראיון 1 |

**Description:** While viewing an open order, the worker marks it as completed. The extension fires only when the order status is "Open" and the worker actively chooses to mark it.

**Preconditions:**
- The worker is authenticated and the order details screen is displayed.
- The selected order's status is "Open".

**Postconditions (success):**
- The order's status is changed to "Completed".
- The change is persisted to permanent storage.
- The "Mark as Completed" button is no longer visible on the screen.

**Postconditions (failure / abort):**
- The order's status remains "Open". The system displays an error message.

**Main Success Scenario — Flow of Events:**

| # | Actor / System | Action |
|---|---|---|
| 1 | **Worker** | Clicks "Mark as Completed" on the order details screen. |
| 2 | **System** | Changes the order's status to "Completed". |
| 3 | **System** | Persists the status change to permanent storage. |
| 4 | **System** | Confirms success to the worker and hides the "Mark as Completed" button. |

**Extensions:**

| Extension | Condition | System Response |
|---|---|---|
| 3a | Persistence fails | Reverts the status to "Open". Displays an error message. The button remains visible. |

**Extend Condition (VP18 extension point):**

This UC extends UC-02 at extension point `EP-1`. The condition is:  
*"Order status is 'Open' AND the worker explicitly clicks 'Mark as Completed'."*

**User Stories:**

> As a **worker**, I want to mark an order as completed directly from the details screen, so that the status is updated in real time without navigating to a separate screen.

---

> **Implementation Notes** *(Design phase — not part of the UC spec)*
>
> | Behavioral step | Implementation |
> |---|---|
> | Order details screen | `OrderDetailsPanel` |
> | Change status in memory (step 2) | `Program.orders` list — set `order.Status = "Completed"` |
> | Persist to permanent storage (step 3) | `SP_UpdateOrderStatus(orderId, status)` |
> | Revert on failure (ext. 3a) | Reset `order.Status = "Open"` on the in-memory object |

---

### 5.2 UC-04 — Create Delivery Order

| Field | Value |
|---|---|
| **UC ID** | UC-04 |
| **Diagram ID** | uc-create-delivery |
| **Name** | Create Delivery Order |
| **Primary Actor** | Shift Manager |
| **Secondary Actors** | — |
| **Status** | ✅ Implemented |
| **Linked USs** | US-19 |
| **Raised by** | רינת לוי (מנהלת משמרת) — ראיון 1 |

**Description:** A shift manager creates a new delivery order, assigning it to a worker and specifying delivery address, date, and total price.

**Preconditions:**
- The authenticated user has the Shift Manager role.
- At least one worker is registered in the system.

**Postconditions (success):**
- A new delivery order is persisted to permanent storage.
- The new order is immediately available in the system.
- The system returns to the management dashboard.

**Postconditions (failure / abort):**
- No order is created. No data is modified.

**Main Success Scenario — Flow of Events:**

| # | Actor / System | Action |
|---|---|---|
| 1 | **Shift Manager** | Selects "Create Delivery Order" from the management dashboard. |
| 2 | **System** | Displays the Create Delivery Order form with an auto-generated order ID and a list of available workers. |
| 3 | **Shift Manager** | Selects an assigned worker from the list. |
| 4 | **Shift Manager** | Enters: Order Date, Total Price, Delivery Address, Expected Delivery Date. |
| 5 | **Shift Manager** | Submits the form. |
| 6 | **System** | Validates that all required fields are filled and that Total Price is a non-negative number. |
| 7 | **System** | Creates the delivery order. |
| 8 | **System** | Persists the order to permanent storage. |
| 9 | **System** | Makes the new order available in the system. |
| 10 | **System** | Returns to the management dashboard. |

**Extensions:**

| Extension | Condition | System Response |
|---|---|---|
| 6a | Any required field is empty | Highlights the missing fields. Stays on form. Returns to step 4. |
| 6b | Total Price is negative or non-numeric | Displays an error: "Total price must be a non-negative number." Returns to step 4. |
| 8a | Persistence fails | Displays an error. The order is not added to the system. Returns to step 4. |

**User Stories:**

> As a **shift manager**, I want to create a delivery order and assign it to a worker, so that the delivery is tracked with an address and expected date.

---

> **Implementation Notes** *(Design phase — not part of the UC spec)*
>
> | Behavioral step | Implementation |
> |---|---|
> | Management dashboard (steps 1, 10) | `CRUDPanel` |
> | Create Delivery Order form (step 2) | `CreateDeliveryOrderPanel` |
> | List of available workers (step 2) | `Program.workers` → populate `ComboBox` |
> | Create the order (step 7) | `new DeliveryOrder(workerId, date, price, address, deliveryDate)` |
> | Persist to storage (step 8) | `SP_add_order` (Orders table) + `SP_add_delivery_order` (DeliveryOrders table) |
> | Make available in system (step 9) | `Program.orders.Add(newOrder)` |

---

### 5.3 UC-02 — View Order Details

| Field | Value |
|---|---|
| **UC ID** | UC-02 |
| **Diagram ID** | uc-view-details |
| **Name** | View Order Details |
| **Primary Actor** | Worker |
| **Secondary Actors** | — |
| **Status** | ✅ Implemented |
| **Linked USs** | US-22 |
| **Extension point** | UC-03 (Mark Order as Completed) may extend this UC |
| **Raised by** | דן מזרחי (עובד שטח) — ראיון 2 |

**Description:** A worker selects an order from their assigned-orders list and views its full details: type-specific fields and an itemized product table.

**Preconditions:**
- The worker is authenticated and the orders list screen is displayed.
- At least one order exists in the list.

**Postconditions (success):**
- No data is modified.
- The order details screen is displayed with full order and item data.

**Main Success Scenario — Flow of Events:**

| # | Actor / System | Action |
|---|---|---|
| 1 | **Worker** | Selects an order from the orders list. |
| 2 | **System** | Retrieves the selected order. |
| 3 | **System** | Determines the order type (Delivery or Pickup). |
| 4 | **System** | Retrieves the items associated with the order. |
| 5 | **System** | Retrieves the product details for each item. |
| 6 | **System** | Displays the order details: Order ID, Date, Total Price, assigned worker, order type, and type-specific fields (Delivery: address + expected date; Pickup: branch + pickup time). |
| 7 | **System** | Displays the items in a table: Product Name, Quantity, Unit Price, Line Total. |
| 8 | **Worker** | Reviews the order details. |
| 9 | **Worker** | Clicks "Back". |
| 10 | **System** | Returns to the orders list. |

**Extensions:**

| Extension | Condition | System Response |
|---|---|---|
| 4a | The order has no associated items | Displays the order header normally. The items table shows "No items recorded." |
| **UC-03 extend point** | Worker marks the order complete (step 8) | Execute UC-03: Mark Order as Completed. |

**User Stories:**

> As a **worker**, I want to click on an order to see its full details including all products and quantities, so that I have everything I need to fulfil the order.

> As a **shift manager**, I want to see the complete details of any order, so that I can resolve customer queries without asking the assigned worker.

---

> **Implementation Notes** *(Design phase — not part of the UC spec)*
>
> | Behavioral step | Implementation |
> |---|---|
> | Orders list screen (steps 1, 10) | `WatchOrdersPanel` |
> | Order details screen (step 6) | `OrderDetailsPanel` |
> | Retrieve selected order (step 2) | Lookup in `Program.orders` by order ID |
> | Retrieve items (step 4) | Filter `Program.orderItems` by order ID |
> | Retrieve product details (step 5) | Lookup in `Program.products` by product ID |
> | Items table (step 7) | `DataGridView` on `OrderDetailsPanel` |

---

### 5.4 UC-07 — Update Worker Details *(CRUD UC #1)*

| Field | Value |
|---|---|
| **UC ID** | UC-07 |
| **Diagram ID** | uc-update-worker |
| **Name** | Update Worker Details |
| **Primary Actor** | Shift Manager |
| **Secondary Actors** | — |
| **Status** | ✅ Implemented |
| **Linked USs** | US-10, US-11 |
| **Includes** | UC-09: Find Worker by ID |
| **Raised by** | רינת לוי (מנהלת משמרת) — ראיון 1 |

**Description:** A shift manager locates a worker by ID (via `«include»` UC-09) and updates their name and/or title.

**Preconditions:**
- The authenticated user has the Shift Manager role.
- At least one worker is registered in the system.

**Postconditions (success):**
- The worker's updated name and/or title are persisted to permanent storage.
- The updated data is immediately reflected in the system.
- The system returns to the management dashboard.

**Postconditions (failure / abort):**
- Neither the stored data nor the system's in-session state is modified.

**Main Success Scenario — Flow of Events:**

| # | Actor / System | Action |
|---|---|---|
| 1 | **Shift Manager** | Selects "Update / Delete Worker" from the management dashboard. |
| 2 | **System** | Displays the worker search screen. |
| 3 | **Shift Manager** | Enters a Worker ID and submits the search. |
| 4 | *(«include» UC-09)* | The system finds the worker by ID. |
| 5 | **System** | Displays the worker's current name and title in editable fields. |
| 6 | **Shift Manager** | Changes the name and/or title. |
| 7 | **Shift Manager** | Clicks "Update". |
| 8 | **System** | Validates that the name field is not empty. |
| 9 | **System** | Persists the changes to permanent storage. |
| 10 | **System** | Reflects the updated data in the system immediately. |
| 11 | **System** | Returns to the management dashboard. |

**Extensions:**

| Extension | Condition | System Response |
|---|---|---|
| 4a | No worker found for the entered ID (from UC-09) | Displays "Worker not found." Clears the search field. Returns to step 3. |
| 8a | Name field is empty | Displays "Worker name is required." Returns to step 6. |
| 9a | Persistence fails | Displays an error. In-session state is NOT updated. Returns to step 6. |

**CRUD Fields:**

| Field | Type | Constraints | Editable |
|---|---|---|---|
| Worker ID | Integer | Primary Key, auto-assigned | No (read-only) |
| Worker Name | Text (max 100) | Required | Yes |
| Title | Enumerated list | Must be a valid title value | Yes |
| Password | Text | Not shown on this screen | No |

**Permissions:** Shift Manager only.

**User Stories:**

> As a **shift manager**, I want to update a worker's name or title, so that the system reflects changes in their role or personal information.

---

> **Implementation Notes** *(Design phase — not part of the UC spec)*
>
> | Behavioral step | Implementation |
> |---|---|
> | Management dashboard (steps 1, 11) | `CRUDPanel` |
> | Worker search screen (step 2) | `UpdateDeletePanel` |
> | Find worker (step 4) | `Worker.Seek(id)` — searches `Program.workers` |
> | Persist changes (step 9) | `worker.updateWorker(newName, newTitle)` → `SP_update_worker` |
> | Reflect in system (step 10) | Update `name` and `title` on the in-memory `Worker` object in `Program.workers` |
> | Title dropdown values | `TitleHelper.GetTitles()` (from `Title` enum) |

---

### 5.5 UC-10 — Manage Product Catalog *(CRUD UC #2)*

| Field | Value |
|---|---|
| **UC ID** | UC-10 |
| **Diagram ID** | uc-manage-product |
| **Name** | Manage Product Catalog |
| **Primary Actor** | Shift Manager |
| **Secondary Actors** | — |
| **Status** | 🔮 Future (structure exists; UI panels not yet implemented) |
| **Linked USs** | US-37, US-38, US-39, US-40, US-41 |
| **Raised by** | רינת לוי (מנהלת משמרת) — ראיון 1; עמי בן-דוד (מנכ"ל) — ראיון 3 |

**Description:** A shift manager performs create, update, or delete operations on products in the catalog. Deletion is blocked when the product is referenced by existing order items.

**Preconditions (Create):**
- The authenticated user has the Shift Manager role.

**Preconditions (Update / Delete):**
- The authenticated user has the Shift Manager role.
- The product to act on has been identified.

**Postconditions (Create — success):**
- A new product is persisted to permanent storage.
- The new product is immediately available in the system.

**Postconditions (Update — success):**
- The product's updated data is persisted to permanent storage.
- The change is immediately reflected in the system.

**Postconditions (Delete — success):**
- The product is removed from permanent storage.
- The product is no longer available in the system.

**Main Success Scenario — Create Product:**

| # | Actor / System | Action |
|---|---|---|
| 1 | **Shift Manager** | Selects "Add Product" from the product catalog screen. |
| 2 | **System** | Displays the create product form with an auto-generated product ID. |
| 3 | **Shift Manager** | Enters product name and price. |
| 4 | **Shift Manager** | Submits the form. |
| 5 | **System** | Validates that all fields are filled and price is a non-negative number. |
| 6 | **System** | Creates and persists the new product. |
| 7 | **System** | Makes the product immediately available in the system. |
| 8 | **System** | Returns to the product catalog screen. |

**Alternative Scenario — Delete Product:**

| # | Actor / System | Action |
|---|---|---|
| 1 | **Shift Manager** | Selects a product and clicks "Delete". |
| 2 | **System** | Checks whether any existing order references this product. |
| 3 | **System** | `[No references]` Removes the product from permanent storage and from the system. |

**Extensions:**

| Extension | Condition | System Response |
|---|---|---|
| 5a (Create) | Any required field is empty or price is invalid | Displays a validation error. Returns to step 3. |
| 2a (Delete) | The product is referenced by one or more existing orders | Displays "This product is used in existing orders and cannot be deleted." Aborts. |
| 6a / 3a | Persistence fails | Displays an error. The system state is not modified. |

**CRUD Fields:**

| Field | Type | Constraints | Editable |
|---|---|---|---|
| Product ID | Integer | Primary Key, auto-assigned | No (read-only) |
| Product Name | Text (max 100) | Required | Yes |
| Price | Decimal | Required, ≥ 0 | Yes |

**Permissions:** Shift Manager only.

**User Stories:**

> As a **shift manager**, I want to add a new product to the catalog, so that it becomes available for inclusion in orders.

> As a **shift manager**, I want to update a product's name or price, so that the catalog always reflects current information.

> As a **shift manager**, I want to delete a discontinued product, so that it no longer appears in the product selection when creating orders.

---

> **Implementation Notes** *(Design phase — not part of the UC spec)*
>
> | Behavioral step | Implementation |
> |---|---|
> | Product catalog screen | Planned: `ManageProductPanel` |
> | Create and persist (step 6) | `new Product(name, price)` → `SP_add_product` (Products table) |
> | Make available in system (step 7) | `Program.products.Add(newProduct)` |
> | Check for references (Delete step 2) | Search `Program.orderItems` for any item with this product ID |
> | Remove from storage (Delete step 3) | `SP_delete_product(productId)` |
> | Remove from system (Delete step 3) | `Program.products.Remove(product)` |

---

## 6. AI Agent Usage Notes

The following fields in each UC spec are directly actionable by a coding agent:

| UC field | Generates |
|---|---|
| Primary Actor + Preconditions | Role check at the top of the event handler |
| Flow of Events steps | The ordered method calls and UI transitions in the button click handler |
| Extensions | The `if/else` validation blocks and `catch` clauses |
| CRUD Fields table | The entity class properties and form controls |
| Postconditions | The mutation that must happen after a successful operation |
| **Implementation Notes** | The specific class names, method signatures, and stored procedures |
| Linked USs | The acceptance criteria to verify in testing |

The most precise input for code generation is: **preconditions + flow of events + extensions + CRUD fields + implementation notes**. The description and user stories provide context but are redundant once the spec is complete.
