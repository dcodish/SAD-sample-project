# My Daniel Center Management System

[![Version](https://img.shields.io/badge/version-1.0-blue.svg)]()
[![Date](https://img.shields.io/badge/date-June_2026-green.svg)]()

## Project Information
*   **Version:** 1.0
*   **Date:** June 2026
*   **Course:** Software Analysis and Design (SAD) — Ben-Gurion University, Industrial Engineering and Management
*   **Project Type:** Web & Mobile Application

---

## 1. Project Overview
The "My Daniel" system is a centralized management platform for the Daniel Rowing Centre. It consolidates organizational data into a single system to efficiently manage customers, employees, equipment, payments, and water activities. 

The system replaces manual spreadsheets and fragmented applications, providing:
*   Real-time data synchronization.
*   Digital registration processes.
*   Automated payroll tracking and reporting.

---

## 2. Actors

| Actor | Description |
| :--- | :--- |
| **Center Manager** | Has overall responsibility for the center. Needs access to all management modules, BI reports, and employee profiles. |
| **Accounting Manager** | Manages billing, customer subscriptions, discount approvals, and payroll synchronization. |
| **Coordinator (Sailing/Rowing)** | Manages the day-to-day operations, including boat maintenance, instructor scheduling, and equipment allocation. |
| **Instructor** | Leads the activities. Needs to report attendance, document unusual events, and upload reserve duty approvals. |
| **Customer** | End-user participating in center activities. Needs to view their schedule, fill out registration forms, and track discount requests. |
| **System (Time System)** | An automated background actor that triggers scheduled syncs, backups, and reports. |

---
*Document generated for SAD Course, BGU.*
## 3. Functional Requirements — User Stories

Each user story is labeled with its implementation status:

*   ✅ **Implemented** — fully built in the current version
*   🚧 **Partially Implemented** — data/structure exists but no complete UI
*   🔮 **Future** — planned for a future version, not yet implemented

*(Note: As this is the initial design baseline for the new system, all features are currently marked as Future).*

---

### 3.1 Equipment & Fleet Management

**US-01 — View Boat Usage Status 🔮**
*   **As a** coordinator, **I want** to view the usage status of watercraft, **so that** I can track and plan preventive maintenance.
*   **Acceptance Criteria:**
    *   The system displays a real-time list of all active watercraft.
    *   Shows accumulated engine/usage hours since the last maintenance.
    *   Highlights boats exceeding allowed hours in red and sends an automated alert to the coordinator.

**US-02 — View Broken Equipment Report 🔮**
*   **As a** coordinator, **I want** to get a consolidated report of broken equipment, **so that** I can monitor and track repairs.
*   **Acceptance Criteria:**
    *   The system generates a report listing all boats currently under maintenance.
    *   The report includes the reason for downtime and the expected repair time.

**US-03 — Assign Boats to Students 🔮**
*   **As a** coordinator, **I want** to assign boats to students, **so that** I can ensure proper and organized allocation for each participant.
*   **Acceptance Criteria:**
    *   The system allows assigning a boat number to a student on the activity roster with a single click.

**US-04 — View External Center Contacts 🔮**
*   **As a** coordinator, if I lack sufficient boats, **I want** to view contacts from other centers to request a replacement boat.
*   **Acceptance Criteria:**
    *   The system displays a contact table of external centers, including the center name, contact person, and phone number for borrowing equipment.

**US-05 — Assign External Boat 🔮**
*   **As a** coordinator, **I want** to assign a borrowed external boat to an activity.
*   **Acceptance Criteria:**
    *   Clicking "Add boat from another center" allows selecting the borrowing center from a predefined list.
    *   The system then displays the relevant contact and allows manual entry of the borrowed boat's ID number.

**US-06 — Manage Watercraft (CRUD) 🔮**
*   **As a** coordinator, **I want** to create, update, and view marine equipment details, to track the center's fleet.
*   **Acceptance Criteria:**
    *   **Create:** New "Personal Boat File" (Number, Name, Type, Purchase Date, Status).
    *   **Read:** Equipment control report with availability status and maintenance history.
    *   **Update:** Change maintenance status after a repair/fault.
    *   **Delete:** Remove a boat from the database if permanently retired.

---

### 3.2 Activity Scheduling & Operations

**US-07 — Report Attendance and Exceptions 🔮**
*   **As an** instructor, **I want** to report attendance and document unusual events, **so that** I can track my group's progress.
*   **Acceptance Criteria:**
    *   Displays the student list for a specific activity and allows status entry: "Present", "Absent - notified in advance", or "Absent - not notified".
    *   Reporting is available during and up to one day after the activity, with an option for free-text notes.

**US-08 — View Personal Activity Schedule 🔮**
*   **As a** customer, **I want** to view my personal activity schedule to see my instructor and assigned boat for the week, **so I can** arrive prepared.
*   **Acceptance Criteria:**
    *   A dashboard shows all activities assigned to the user for the upcoming week.
    *   Includes instructor name, boat description, lesson time, and location.

**US-09 — View Instructors' Weekly Schedule 🔮**
*   **As a** coordinator, **I want** to view a consolidated weekly schedule of all instructors, **so I can** manage manpower efficiently.
*   **Acceptance Criteria:**
    *   The system provides a weekly view consolidating all instructors and their assigned lessons in a single interface.

**US-10 — Send Cancellation Alerts 🔮**
*   **As a** coordinator, **I want** to notify students about lesson cancellations due to weather or other events, **so** information is shared transparently and in real-time.
*   **Acceptance Criteria:**
    *   Allows sending a "Lesson Cancelled" alert (via in-app message or SMS) to all customers registered for a specific activity, including the cancellation reason.

**US-11 — Schedule Activities 🔮**
*   **As a** coordinator, **I want** to assign instructors, students, and boats to an activity, **to** manage operations efficiently based on availability.
*   **Acceptance Criteria:**
    *   Open a new activity by entering Date/Time, selecting a certified instructor, selecting functional boats, and linking children from the customer database based on age.

**US-12 — View Active Operations Schedule 🔮**
*   **As a** manager, **I want** to view ongoing activities to maintain oversight and control.
*   **Acceptance Criteria:**
    *   The system displays an activity calendar (diary) including the participant list, responsible instructor, and allocated equipment for each activity.

**US-13 — Update Resources for Active Events 🔮**
*   **As a** coordinator, **I want** to update resources during an activity in case of unexpected issues, **to** ensure operational continuity.
*   **Acceptance Criteria:**
    *   The system allows assigning a replacement boat during a fault, swapping instructors, and updating the participant list for existing activities.

---

### 3.3 Employee Management & Payroll Sync

**US-14 — Cross-Reference Payroll and Activity 🔮**
*   **As** management, **I want** the system to cross-reference reported hours with actual activity and manually approve discrepancies, **to** prevent payment errors.
*   **Acceptance Criteria:**
    *   Automatically compares attendance reports with the activity log.
    *   Generates an alert for any deviation exceeding two hours and allows manual correction.

**US-15 — Approve Payroll Exceptions 🔮**
*   **As** management, **I want** to approve payroll exceptions manually.
*   **Acceptance Criteria:**
    *   Requires clicking "Approve Exception" and adding free text for any reported hours that deviate by more than two hours.

**US-16 — View Missing Hours Report 🔮**
*   **As a** coordinator, **I want** to see which employees have not properly reported their work hours, **so I can** follow up with them.
*   **Acceptance Criteria:**
    *   Generates an exception report listing instructors who failed to report attendance for two or more activities.
    *   Includes instructor name, dates of missing reports, and total accrued exceptions for the month.

**US-17 — Upload Reserve Duty (Miluim) Approvals 🔮**
*   **As an** instructor, **I want** to upload reserve duty approvals to the system, **to** update my absence days.
*   **Acceptance Criteria:**
    *   Allows uploading a reserve duty approval file strictly in PDF format.

**US-18 — Sync Reserve Duty to Payroll 🔮**
*   **As an** instructor, **I want** my uploaded reserve duty to sync with payroll.
*   **Acceptance Criteria:**
    *   Automatically links the reserve duty file to the relevant reporting month and syncs with the payroll report.

**US-19 — Manage Employees (CRUD) 🔮**
*   **As a** center manager, **I want** to create, update, and view staff profiles, **to** control access permissions and scheduling.
*   **Acceptance Criteria:**
    *   **Create:** Profile with role, seniority, contact info, and certification uploads.
    *   **Read:** List of active employees with their professional certifications.
    *   **Update:** Modify roles or update certification expiration dates.
    *   **Delete:** Deactivate terminated employee accounts to prevent system access.

---

### 3.4 Customer & Subscription Management

**US-20 — Upload Discount Eligibility Documents 🔮**
*   **As a** customer, **I want** to upload discount eligibility documents online, **so** I can get quick approval without coming to the office.
*   **Acceptance Criteria:**
    *   The system allows file uploads strictly in PDF or JPEG formats to the customer's file.

**US-21 — Approve Discount Requests 🔮**
*   **As an** accounting manager, **I want** to approve discount requests in the system, **so I can** update the customer's final subscription price.
*   **Acceptance Criteria:**
    *   Allows changing request status to "Approved" and entering a valid discount percentage (0-100%).
    *   Automatically recalculates and updates the customer's payment balance upon saving.

**US-22 — Notification on New Customer Request 🔮**
*   **As an** accounting manager, **I want** to be notified of new customer requests, **so I can** address them promptly.
*   **Acceptance Criteria:**
    *   The system sends an automated "Request Received" alert when a customer uploads a new request.

**US-23 — View Customer Request Status 🔮**
*   **As an** accounting manager, **I want** to view the updated status of personal customer requests.
*   **Acceptance Criteria:**
    *   Displays the current status per customer based on their request state.

**US-24 — Auto-Update Request Status 🔮**
*   **As a** user, **I want** the system to automatically update the request status.
*   **Acceptance Criteria:**
    *   Auto-updates status values to: "Pending Document Upload", "In Progress", or "Completed", based on the processing stage of the discount or personal details form.

**US-25 — Manage Customers (CRUD) 🔮**
*   **As an** accounting manager, **I want** to create, update, and view customer details, **so I can** manage the subscriber database accurately.
*   **US-25 — Manage Customers (CRUD) 🔮**
*   **As an** accounting manager, **I want** to create, update, and view customer details, **so I can** manage the subscriber database accurately.
*   **Acceptance Criteria:**
    *   **Create:** Digital form with full name, ID, phone, email, city, DOB, join date, payment date, and signed health declaration.
    *   **Read:** List showing personal details, subscription status, and purchase history.
    *   **Update:** Edit customer details and save.
    *   **Delete:** Move inactive customers to "Archive" status.

**US-26 — Customer Notification on Request Status 🔮**
*   **As a** customer, **I want** to receive confirmation on the status of my submitted discount request, **so** I know if it was approved.
*   **Acceptance Criteria:**
    *   The system sends an automated "Approved"/"Denied" message to the customer and allows viewing approval history in a "My Approvals" personal area.

---

### 3.5 BI & Reporting

**US-27 — Generate Customer Retention Report 🔮**
*   **As a** coordinator, **I want** to see a list of last year's registered customers who haven't renewed, **so I can** retain them.
*   **Acceptance Criteria:**
    *   Generates a pre-season retention report of children registered last year but not currently registered.
    *   Includes parent names and phone numbers, and auto-sends to the accounting manager and coordinator on a defined date.

**US-28 — Generate Payroll vs. Activity BI Report 🔮**
*   **As a** center manager, **I want** detailed reports comparing reported work hours to actual activity hours, **to** analyze team efficiency and make financial decisions.
*   **Acceptance Criteria:**
    *   A tabular comparative report of reported hours vs. executed hours, showing percentage deviation.
    *   Filterable by instructor name, activity type, or branch.

**US-29 — Generate Maintenance BI Report 🔮**
*   **As** management, **I want** consolidated data on maintenance operations, **to** make financial decisions.
*   **Acceptance Criteria:**
    *   A complex BI report pulling from fault logs and personal boat files.
    *   Displays monthly metrics: fault count per boat, cumulative downtime, and cumulative repair costs.

**US-30 — Generate Attendance and Payments BI Report 🔮**
*   **As a** manager and coordinator, **I want** a report consolidating attendance and payment data, **so I can** monitor center activities.
*   **Acceptance Criteria:**
    *   A BI report showing students per lesson, registered vs. actual attendance, and financial segmentation of paying vs. non-paying students.
    *   Filterable by age group, activity type, and location.
    *   ## 4. Non-Functional Requirements

### 4.1 Security & Access

| ID | Requirement | Status |
| :--- | :--- | :--- |
| NFR-01 | Access to customer and employee data shall be strictly restricted based on pre-defined permissions for the accounting/center managers using security mechanisms to prevent unauthorized entry. | 🔮 Future |
| NFR-02 | Access to add/delete boats is restricted strictly to pre-defined permissions for the coordinator and center manager. | 🔮 Future |

---

### 4.2 Usability & Compatibility

| ID | Requirement | Status |
| :--- | :--- | :--- |
| NFR-03 | The system shall be responsive to mobile and desktop, including a Web version and a dedicated app version. | 🔮 Future |
| NFR-04 | The system shall allow importing Excel files, PDF documents, and PNG/JPEG images. | 🔮 Future |
| NFR-05 | The system shall allow exporting and viewing Excel files, JPEG/PNG images, and PDF files. | 🔮 Future |
| NFR-06 | The system will support Hebrew, English, and Russian, switchable with a single button click. | 🔮 Future |

---

### 4.3 Performance & Reliability

| ID | Requirement | Status |
| :--- | :--- | :--- |
| NFR-07 | Full automated data backup every 24 hours at 20:00, with a maximum recovery time of 4 hours. | 🔮 Future |
| NFR-08 | Automated sync of all boat data in the system every morning at 08:00. | 🔮 Future |
| NFR-09 | Automated sync of customer and instructor data once a month. | 🔮 Future |
| NFR-10 | The system must allow concurrent parallel usage by multiple authorized center employees. | 🔮 Future |

## 5. Requirements Traceability Matrix

| User Story | Status | UC ID | Source Matrix |
| :--- | :---: | :--- | :--- |
| US-01 — View Boat Usage Status | 🔮 | UC05 | Boats and Equipment matrix.jpg |
| US-02 — View Broken Equipment Report | 🔮 | UC05 | Boats and Equipment matrix.jpg |
| US-03 — Assign Boats to Students | 🔮 | UC03 | Boats and Equipment matrix.jpg |
| US-04 — View External Center Contacts | 🔮 | UC0501 | Boats and Equipment matrix.jpg |
| US-05 — Assign External Boat | 🔮 | UC0501 | Boats and Equipment matrix.jpg |
| US-06 — Manage Watercraft (CRUD) | 🔮 | UC01 | Boats and Equipment matrix.jpg |
| US-07 — Report Attendance | 🔮 | UC08 | Employees and Customers matrix.jpg |
| US-08 — View Personal Activity Schedule | 🔮 | UC06 | Boats and Equipment matrix.jpg |
| US-09 — View Instructors' Weekly Schedule | 🔮 | UC07 | Boats and Equipment matrix.jpg |
| US-10 — Send Cancellation Alerts | 🔮 | UC07 | Boats and Equipment matrix.jpg |
| US-11 — Schedule Activities | 🔮 | UC02 | Boats and Equipment matrix.jpg |
| US-12 — View Active Operations | 🔮 | UC06 | Boats and Equipment matrix.jpg |
| US-13 — Update Resources | 🔮 | UC04 | Boats and Equipment matrix.jpg |
| US-14 — Cross-Reference Payroll and Activity | 🔮 | UC07 | Employees and Customers matrix.jpg |
| US-15 — Approve Payroll Exceptions | 🔮 | UC07 | Employees and Customers matrix.jpg |
| US-16 — View Missing Hours Report | 🔮 | UC03 | Employees and Customers matrix.jpg |
| US-17 — Upload Reserve Duty | 🔮 | UC03 | Employees and Customers matrix.jpg |
| US-18 — Sync Reserve Duty to Payroll | 🔮 | UC07 | Employees and Customers matrix.jpg |
| US-19 — Manage Employees (CRUD) | 🔮 | UC01 | Employees and Customers matrix.jpg |
| US-20 — Upload Discount Documents | 🔮 | UC04 | Employees and Customers matrix.jpg |
| US-21 — Approve Discount Requests | 🔮 | UC05 | Employees and Customers matrix.jpg |
| US-22 — Notification on New Request | 🔮 | UC05 | Employees and Customers matrix.jpg |
| US-23 — View Customer Request Status | 🔮 | UC06 | Employees and Customers matrix.jpg |
| US-24 — Auto-Update Request Status | 🔮 | UC06 | Employees and Customers matrix.jpg |
| US-25 — Manage Customers (CRUD) | 🔮 | UC01 | Employees and Customers matrix.jpg |
| US-26 — Customer Notification on Status | 🔮 | UC08 | Employees and Customers matrix.jpg |
| US-27 — Generate Customer Retention Report | 🔮 | UC09 | Employees and Customers matrix.jpg |
| US-28 — Generate Payroll vs. Activity BI | 🔮 | UC08 | Boats and Equipment matrix.jpg |
| US-29 — Generate Maintenance BI | 🔮 | UC08 | Boats and Equipment matrix.jpg |
| US-30 — Generate Attendance and Payments BI | 🔮 | UC08 | Boats and Equipment matrix.jpg | 

## 6. Summary

| Category | Total | ✅ Implemented | 🚧 Partial | 🔮 Future |
| :--- | :---: | :---: | :---: | :---: |
| Equipment & Fleet Management | 6 | 0 | 0 | 6 |
| Activity Scheduling & Operations | 7 | 0 | 0 | 7 |
| Employee Management & Payroll Sync | 6 | 0 | 0 | 6 |
| Customer & Subscription Management | 7 | 0 | 0 | 7 |
| BI & Reporting | 4 | 0 | 0 | 4 |
| **Total User Stories** | **30** | **0** | **0** | **30** |
| **Non-Functional Requirements** | **10** | **0** | **0** | **10** |

---
> **Note:** This document outlines the 40 required system functionalities (30 User Stories and 10 NFRs) mapped strictly to the target architecture of the My Daniel Centre Management System. All functionalities are currently slated for development in the upcoming baseline (v1.0).
---
