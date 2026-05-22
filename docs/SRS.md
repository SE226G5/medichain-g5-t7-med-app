# Software Requirements Specification (SRS)
## Project: [MediChain (Medical Laboratory Chain Management System)]
## Module/Subsystem: [Medical Approval & Locking Subsystem (MED-APP – Module 7)]
**Version:** 1.0  
**Date:** [2026-05-15]

---

## 1. Introduction
### 1.1 Purpose
* This document defines the detailed functional and non-functional requirements for the MED-APP system.

Integration Team Perspective

As the Integration Team, this document not only describes Module 7 independently, but also governs and organizes the mechanisms of data flow, integration points, and security between our subsystem and the other subsystems (LAB-TRK and REF-TRK) to ensure consistency throughout the medical sample lifecycle.

### 1.2 Scope
* This system governs and authorizes laboratory medical test results after they are released by the laboratory and before they are published.

What the system WILL do:
Provide a dedicated and secure interface for laboratory doctors to review, sign, and approve results.
Automatically and immediately lock laboratory records in the database after approval to prevent further modification.
Manage a strict override workflow when modifications to locked values are requested, including documenting the reason, user, and timestamp.
Generate a final secure read-only PDF medical report.
What the system WILL NOT do:
The system will not enter technical laboratory test values (this is handled by Module 6 – LAB-TRK).
The system will not process patient financial payments (this is handled by Module 5 – REV-BIL).

### 1.3 Definitions, Acronyms, and Abbreviations
This section establishes a shared vocabulary for all stakeholders — developers, testers,
 UI designers, and integration partners. Every term used in the SRS, the activity diagrams
, or the business rules table is defined here. Ambiguous terminology is the most common source of misaligned implementations;
 this glossary eliminates that risk.


ERP     Enterprise Resource Planning. An integrated software system that manages core business processes. MediChain is a hospital-specific ERP.

SRS       	Software Requirements Specification. This document — a formal description of the system's functional and non-functional requirements, following IEEE 830 standard.

UML	   Unified Modeling Language. A standardized visual notation for software design. Used here for Activity Diagrams (behavioral diagrams).

Activity Diagram	   A UML behavioral diagram that shows the sequential and parallel flow of actions across multiple system actors, using swimlanes, decision nodes, and guard conditions.

Swimlane	    A vertical partition in an activity diagram that assigns responsibility for each action to a specific actor (e.g., Patient, Appointment System, Database, External System).

Decision Node    	A diamond-shaped node in an activity diagram representing a Boolean condition. Control flow branches into paths labeled with guard conditions [Yes] / [No].

Guard Condition	   The Boolean expression labeling a transition from a decision node (e.g., [Available] or [Not Available]). Determines which path control flow follows.

Business Rule (BR)	A domain-level constraint that the system must enforce before permitting a state change. Identified in this document as BR-01 through BR-10.

NFR	   Non-Functional Requirement. A system quality attribute that constrains how the system performs its functions (e.g., response time, reliability, maintainability).

REST API	Representational State Transfer Application Programming Interface. The communication protocol used between the Appointments module and external systems.

Race Condition    	A concurrency defect where two users attempt to book the same time slot simultaneously, potentially resulting in a double-booking. Prevented by BR-06 (atomic slot lock).

Process Modeling	The role responsible for defining, drawing, and documenting the process flows, business rules, and validation logic for a given system module.

### 1.4 References
* IEEE 830 Standard for Software Requirements Specification.
General Project Specification Document for MediChain.
Organization Repository Link: [ Main Repository Link Here]

### 1.5 Overview
* Section 2 presents the overall system description and interface integrations.
Section 3 focuses on software features and Agile User Stories.
Section 4 contains appendices including architectural diagrams and API specifications.

---

## 2. Overall Description
### 2.1 Product Perspective
* For Subsystem Teams

The MED-APP system is an integrated subsystem that interacts directly with the shared centralized database.

For the Integration Team

This module represents the “Final Security and Confirmation Gateway.”
It receives raw data from Module 6, locks it after approval, and allows Modules 4 and 5 to consume and display or bill the approved data.

*   **2.1.1 System Interfaces:**
*    [
*    Inbound APIs

Our system consumes the GET Endpoint provided by Team 6 (LAB-TRK) to retrieve all samples with the status "Ready for Approval".
(API details are provided in Appendix A.)

Outbound APIs

Our system provides a dedicated GET Endpoint for the External Referrals Gateway (REF-TRK) to retrieve the final report URL (PDF) and result details, provided that the result has already been approved and locked.

Team 4 Repository Link

https://github.com/SE226G5/medichain-g5_t4_ref_trk
*    ].
*    
*   **2.1.2 User Interfaces:** The graphical user interface for this module follows a unified, shared design system tailored specifically for laboratory doctors to ensure maximum speed, efficiency, and precision when handling sensitive medical data:"
Approval Dashboard: Displays a comprehensive list of completed medical test results awaiting official verification, sorted automatically by priority level and sample collection time.
Result Review Screen: Features a comparative data table displaying the patient’s current test values alongside standard reference ranges, with out-of-range critical values dynamically highlighted in an alert color (e.g., bright red).
Locking Confirmation Dialog: A secure pop-up modal that triggers when the doctor clicks "Approve & Lock", presenting a summary of the data and requiring a final confirmation or digital signature.
Result Modification Workflow Screen: A restricted interface that appears only when an authorized user attempts to unlock a sealed record, forcing them to fill out a mandatory justification input field.
*   
*   **2.1.3 Hardware Interfaces:** [​The Medical Approval & Locking module interfaces with the following laboratory and hospital hardware components:

​Barcode Scanners: Integrated with the laboratory equipment interface to automatically scan tube barcodes, ensuring the matching of physical blood/tissue samples with the digital patient record before displaying results for approval.

​Authentication Tokens/Keycards: Supports physical RFID/smart card readers attached to terminal computers for secure biometric or card-based fast login, mandatory for Laboratory Doctors prior to authorizing a clinical result approval.].
*   
*   **2.1.4 Software Interfaces
The Medical Approval & Locking Subsystem (MED-APP) interfaces with standard server-side enterprise technologies and secure cryptographic libraries to guarantee absolute row-level locking, unalterable audit trails, and secure data publishing:

* **Enterprise Database Engine (Data Locking Layer):** Microsoft SQL Server / PostgreSQL. The module utilizes strict database-level transactional locks and triggers to instantly switch a sample's status to `LOCKED` upon medical sign-off, rendering the original rows read-only for standard application queries.
* **Object-Relational Mapper (ORM):** Entity Framework Core (EF Core 8.0). Used to securely map physical database records to core C# Domain Models without exposure to custom raw SQL, thereby preventing SQL Injection vulnerabilities during approval and override workflows.
* **Document Generation & Cryptographic Interface:** * *PDF Compilation Engine:* iTextSharp / QuestPDF library integrated to dynamically compile approved laboratory results into structured, tamper-evident, read-only PDF medical reports.
    * *Cryptographic Signing Component:* System.Security.Cryptography framework utilized to apply digital signatures (SHA-256 hashing) to individual generated PDF reports, ensuring file authenticity for consuming external systems.
* **Serialization & Integration Component:** System.Text.Json / Newtonsoft.Json library utilized to serialize outward-facing analytical models and safely parse complex multi-nested JSON payloads received from Module 6 (LAB-TRK) into verified operational server schemas.
*   
*   **2.1.5 Communications Interfaces:** [​To ensure reliable and distributed data transit across the MediChain ecosystem, the module uses the following communication standards:

​Protocol & Format: All interactions between the Frontend client and the Backend core occur over secure HTTPS via synchronous RESTful APIs, utilizing JSON (JavaScript Object Notation) for payload delivery.

​Database Connection: Employs persistent connection pooling via TCP/IP to communicate with the centralized database system , utilizing TLS 1.3 encryption for database streams.

​Asynchronous Integration: Uses a message queue broker (e.g., RabbitMQ or Kafka) to communicate asynchronously with external systems, such as triggering the reporting service to compile the read-only PDF document once a result is locked.].
*   
*   **2.1.6 Memory & Operational Constraints:** [​The execution environment of this subsystem must adhere to the following baseline operational thresholds to prevent bottlenecks during approval surges:

​Memory (RAM): The server-side module container requires a dedicated allocation of a minimum of 4 GB RAM to concurrently process intense workflows (such as heavy data verification and multi-user lock actions).

​Storage Capacity: The storage microservice must allocate an initial base of 50 GB of high-speed SSD storage space, growing dynamically, exclusively for archiving the generated read-only immutable PDF clinical reports and historical audit logs.

​Execution Timeout: Any approval validation process or locking API pipeline must terminate and rollback transaction states if it exceeds an operational execution boundary of 5 seconds.].

### 2.2 Product Functions
* **Instruction:** [The MED-APP subsystem provides the following major functions:
  - Review and approve laboratory test results before they are released.
  - Prevent unapproved laboratory results from being published or accessed.
  - Lock approved laboratory results to prevent unauthorized modifications.
  - Manage modification requests for locked results through a controlled approval workflow.
  - Record all approval and modification actions in secure audit logs.
  - Generate final read-only medical reports after approval.
  - Allow authorized users to view approved laboratory reports.
  - Track approval details including responsible user, timestamp, and modification history.
  - Ensure that only authorized laboratory doctors can approve or modify results.
  - Support secure integration with other MediChain subsystems.].

### 2.3 User Characteristics
* **Instruction:** [The MED-APP subsystem will be used by multiple categories of users with different responsibilities and technical expertise levels.
	1. Laboratory Doctors
	  - Responsible for reviewing, approving, and validating laboratory test results.
	  - Can request modifications for locked results when necessary.
	  - Expected to have moderate computer literacy and strong medical domain knowledge.
	2. Laboratory Administrators
	  - Responsible for monitoring approval workflows and supervising modification requests.
	  - Can review audit logs and system activities.
	  - Expected to have advanced technical knowledge of the system.
	3. External Doctors
	  - Responsible for viewing final approved laboratory reports related to their patients.
	  - Have read-only access to approved reports.
	  - Expected to have basic computer usage skills.
	4. System Administrators
	  - Responsible for managing users, permissions, system configuration, and maintenance.
	  - Monitor security, access control, and system reliability.
	  - Expected to have advanced technical and administrative expertise.
].

### 2.4 Constraints, Assumptions, and Dependencies
* **Instruction:** [
	Constraints : 
	[ - Approved laboratory results must not be modified directly after being locked.
	 - All approval and modification actions must be logged with user identity and timestamp.
	 - Only authorized laboratory doctors are allowed to approve laboratory results.
	 - Final approved reports must be generated as read-only documents.
	 - The subsystem must comply with secure medical data handling and privacy requirements.
	 - The subsystem must integrate with the shared MediChain database and APIs.
	 - Unauthorized users must not access patient reports or approval functions.].
	 Assumptions :
	[ - Users are authenticated before accessing the subsystem.
	 - Laboratory test results are received from the LAB-TRK subsystem.
	 - Payment and billing verification are handled by the REV-BIL subsystem.
	 - The shared database server and network infrastructure are continuously available.
	 - Users have stable internet connectivity while using the system.
	 - Other MediChain subsystems provide valid and correctly formatted data.].
	 Dependencies : 
	[ - The subsystem depends on the LAB-TRK subsystem for receiving completed laboratory test results.
	 - The subsystem depends on the REV-BIL subsystem to verify payment status before releasing reports.
 	 - The subsystem depends on the REF-TRK subsystem for publishing approved reports to external doctors.
	 - The subsystem depends on the authentication and authorization service for user identity verification and role management.
	 - The subsystem depends on the central MediChain database for storing approvals, audit logs, and final reports.]
]
---

## 3. Specific Requirements (Agile Approach)
* **Instruction:** This section translates traditional functional requirements into Agile User Stories. Every feature must be traceable to the project management board.

### 3.1 External Interface Requirements
* Pre-Approval Validation Logic: The interface strictly disables the "Approve" button if any required test result fields are empty or if the payment/billing coverage status has not been verified (integrated with the Billing Module).  
Automated Locking Logic: The moment the laboratory doctor clicks "Confirm Approval", the interface logic immediately converts all data input fields from "Editable" to "Read-Only" mode across the UI.  
Modification Request Logic: When requesting an edit on a locked file, the "Save Changes" button remains completely disabled until the "Reason for Modification" text field receives a minimum of 15 characters, while automatically logging user metadata in the background.*
### 3.2 System Features & User Stories
* **Instruction:** Organize your requirements by Feature. For each feature, write the underlying requirements as User Stories and link them to your GitHub Issues.

#### 3.2.1 Feature: Medical Result Approval.
*   **Description:**Enables laboratory doctors to review, validate, and officially approve medical test results, ensuring no unverified reports are issued.  
*   **Priority:** High
*   **User Stories:**
    *   **Story 1:** As a Laboratory Doctor, I want to review pending lab results and click 'Approve' so that the results are officially validated for production.
     * *Acceptance Criteria:* The system must prevent any report from being issued or viewed by patients or external doctors before formal approval.  
A clear visual indicator (e.g., a "Pending Approval" badge) must be displayed next to unapproved records.
    * *GitHub Issue:* [https://github.com/meladrajoh/MediChain/issues/71]
    *   **Story 2:** As a Laboratory Doctor, I want to see visual alerts for out-of-range (critical) medical data so that I can focus my clinical review on high-risk cases.
    * *Acceptance Criteria:*Patient test results must be automatically cross-referenced against saved normal ranges based on age.
  * *GitHub Issue:* [https://github.com/meladrajoh/MediChain/issues/72]

#### 3.2.2 Feature: Result Locking & Tamper Prevention   
Description: Locks medical records immediately upon approval to safeguard data integrity and prevent unauthorized data alterations.  
Priority: High
User Stories:
Story 1: As a System Administrator, I want the system to instantly lock results upon medical approval so that no laboratory user can directly overwrite or modify them.
Acceptance Criteria:
All data input elements must switch to a permanent Read-Only state immediately after approval is submitted.  
The system must generate a finalized, unalterable PDF/view-only final report.  
GitHub Issue: https:[//github.com/meladrajoh/MediChain/issues/73]
###3.2.3 Feature: Secure Modification Workflow & Audit Trail
Description: Governs exceptional unlock requests for sealed medical results and logs all changes for compliance and accountability.  
Priority: High
User Stories:
Story 1: As a Laboratory Director, I want any request to unlock or modify a locked result to pass through a strict approval path while logging the reason, user, and timestamp so that we maintain a full audit trail for medical compliance.
Acceptance Criteria:
The system must block any edits until a valid text justification for the modification is provided.  
The system must automatically capture and log: (authorized user identity, precise timestamp, old value, newly modified value, and modification reason).  
GitHub Issue: https:[//github.com/meladrajoh/MediChain/issues/74
]
### 3.3 Performance Requirements

       Operation	                                     Max Response Time	                                       Source / Rationale
Slot availability check (BR-01, BR-02 combined)	         < 1.5 seconds         	Ensures smooth UX during slot selection; delay causes user abandonment.

Full business rules validation (BR-01 through BR-05)	 < 2.0 seconds         	Total pre-booking validation pipeline; must feel instantaneous to user.

Slot lock acquisition (BR-06)	                        < 200 milliseconds      Any delay creates a Race Condition window for concurrent users.

Booking confirmation (DB write, BR-07)	                < 500 milliseconds	    Atomic write must complete quickly to release lock and confirm to user.

Slot status release on cancellation (BR-09)	            < 1.0 second           	Freed slots must be available to other patients within seconds.


Notification dispatch — async (BR-10)	                < 30 seconds            (best-effort) Fire-and-forget; must not block the booking or cancellation response.


Full booking flow — end to end (happy path)          	< 4.0 seconds                      	Total perceived time from "Confirm" tap to confirmation screen.

                                                       Throughput & Concurrency Requirements
    Requirement	                                              Metric / Threshold
	  
Minimum concurrent users supported	                     50 simultaneous active users
Peak booking requests per minute                         200 booking attempts / minute without degradation
Double-booking prevention guarantee	                     100% — zero tolerance (BR-01 + BR-06 + BR-07)
Slot lock collision rate under peak load                 0% successful double-bookings even under concurrent requests
Database availability check queries / second	         Up to 500 queries/second on Doctor_Schedules table

                                      Scalability Requirements

•	The availability validation algorithm must scale linearly with the number of doctors — adding a new           doctor to the system must not degrade response times for existing doctors.

•	The slot-locking mechanism (BR-06) must be implemented using a distributed lock (e.g., database-level         row lock or Redis lock) to support horizontal scaling of the application server.

•	All performance thresholds above must be maintained when the system is operating at 80% of its maximum        concurrent user capacity.


### 3.4 Logical Database Requirements

Architectural Database Summary
The tracking database enforces dynamic data integrity across three distinct, normalized operational layers:
1. **Core Sample Registry (`Samples`):** Acts as the master directory entry for any medical specimen entering the tracking module. It maps the absolute unique payload key (`sample_id`) to the current global state of the workflow and locks the current technician identity.

2. **State Machine Logging (`Tracking_Logs`):** A strictly linear transaction table capturing every internal handoff (Receipt $\rightarrow$ Analysis $\rightarrow$ Clinical Review). It implements database-level timestamping (`start_time`, `end_time`) to compute precise turnaround times (TAT) and triggers an automated warning flag if predefined phase thresholds are violated.

3. **Clinical Payload Datastore (`Test_Results`):** Deconstructs incoming structured payloads (such as the integration JSON schema) into atomic test entities. This normalizes relational storage for items like test types, quantitative numerical data, and safety boundaries before medical validation.

### 3.5 Software System Attributes
This section documents the Non-Functional Requirements (NFRs) of the Appointments module. Unlike functional requirements (what the system does), these attributes define how well it does it — the quality dimensions that determine whether the system is fit for production in a hospital environment.

3.5.1  Reliability
Reliability defines the system's ability to perform its required functions under stated conditions for a specified period.

    Attribute	                                               Requirement
Double-booking prevention	100% guaranteed — the atomic slot lock (BR-06) and atomic DB write (BR-07) must                              together eliminate all double-bookings, even under concurrent load. This is a                                zero-tolerance requirement.

System uptime            	The Appointments module must achieve 99.5% uptime during hospital operating                                  hours (6:00 AM – 10:00 PM). Planned maintenance must occur during off-hours.

Data consistency           	All appointment status transitions must be atomic. Partial state changes (e.g.,                              slot locked but booking not saved) must trigger automatic rollback within 30                                 seconds.

Fault tolerance	                If the External Notification System (BR-10) is unavailable, the booking or                                   cancellation must still complete successfully. Notification delivery is                                      best- effort and must not block core transactions.

Slot lock auto-release   	If a booking confirmation is not completed within the 5-minute lock window 
                            (BR-06), the slot must be automatically released with 100% certainty — no                                    orphaned locks permitted.


3.5.2  Maintainability
Maintainability defines how easily the system can be modified to fix defects, improve performance, or adapt to changing requirements.

Business rule isolation	Each business rule (BR-01 through BR-10) must be implemented as an independently testable unit. Changing one rule (e.g., updating the cancellation window from 2 hours to 3 hours in BR-08) must require no changes to unrelated modules.

3.5.3  Security و هذه تعتبر من اهم افكار المشروع بشكل خاص
Attribute	                                         Requirement
Authentication                	    All API endpoints for booking, modifying, or cancelling appointments                                         must require a valid authenticated session token. Unauthenticated                                            requests must be rejected with HTTP 401.

Authorization	                    Patients may only access their own appointment records. Reception staff                                      may access all patient appointments. The "Manager" privilege override in                                     BR-08 must be enforced  server-side, not client-side.

Input validation	     All incoming request parameters (patient ID, doctor ID, appointment datetime) must                           be validated against expected types and ranges before any business rule is                                   evaluated.


3.5.4  Usability
Attribute	                                                          Requirement
Error messaging     	                             Every business rule failure must produce a user-                                                             readable error message that names the specific violated                                                      constraint (e.g., "This slot is already booked" for BR-                                                      01, not a generic "Error occurred").

Feedback latency	                                 Users must receive visual feedback (loading indicator                                                        or response) within 500ms of any interaction, even if                                                        the full operation takes longer.

Cancellation clarity	                             The UI must display the 2-hour cancellation deadline                                                         (BR-08) prominently on each appointment card so                                                               patients are aware before attempting late cancellation.

---

## 4. Appendices
### Appendix A: Glossary & Models




* 1. Inbound APIs (From Module 6)
Source Subsystem

Sample Tracking System (LAB-TRK – Module 6)

Description

Retrieve samples ready for medical approval (technically locked).

Endpoint

GET /api/v1/samples/ready-for-approval

Response (JSON)
{
  "sample_id": "ST-1002-2026",
  "status": "Ready_for_Approval",
  "lab_technician": "Technician_Name",
  "results": [
    {
      "test_name": "Glucose",
      "raw_result": "95",
      "reference_range": "70-100",
      "unit": "mg/dL"
    }
  ],
  "timestamp": "2026-05-12T14:30:00Z"
}


2. Outbound & Internal APIs (MED-APP Interfaces)
A. Medical Approval & Result Locking Interface (Internal – UI)
Endpoint

POST /api/v1/med-app/results/{sample_id}/approve

Body
{
  "doctor_id": "DOC-789",
  "approval_timestamp": "2026-05-15T10:30:00Z"
}
Response (200 OK)
{
  "status": 200,
  "message": "Result successfully approved and locked.",
  "record_status": "LOCKED"
}

B. Override Modification Request Interface (Internal – UI)
Endpoint

POST /api/v1/med-app/results/{sample_id}/override

Body
{
  "doctor_id": "DOC-789",
  "reason_for_change": "Correction of typographical error",
  "new_value": "15.5",
  "request_time": "2026-05-15T11:00:00Z"
}

Response (201 Created)
{
  "status": 201,
  "message": "Modification request logged."
}
C. Report Retrieval Interface for Other Teams (Outbound – Teams 4 & 5)
Endpoint

GET /api/v1/med-app/results/{sample_id}/report

Response (200 OK)
{
  "status": 200,
  "patient_name": "Ahmed Mohammed",
  "status_text": "APPROVED_AND_LOCKED",
  "report_url": "https://medichain.com/reports/pdf/1002.pdf",
  "is_locked": true
}

3. Architectural & Design Diagrams
Component Diagram & API Specs (Integration Architecture)
\\processing modeling YousefAbaas





[<img width="2281" height="1296" alt="Module7 drawio" src="https://github.com/user-attachments/assets/8c62c82b-99bb-4d30-8582-ea0a40f176ca" />]

[https://drive.google.com/file/d/1MiZ1TGiLtLRJkO_8JVupdeM_gyNcouH2/view?usp=drive_link]


Use Case Diagram (Requirements & Analysis)

[<img alt="استخدام Case.png" data-hpc="true" style="max-width: 100%;" src="https://github.com/SE226G5/medichain-g5-t7-med-app/blob/main/docs/images/Use%20Case.png?raw=true">]
[https://github.com/SE226G5/medichain-g5-t7-med-app/blob/main/docs/MyProject.vpp]


Activity Diagram (Process Modeling)

[ <img width="612" height="819" alt="SAVE_٢٠٢٦٠٥١٨_٢٢٤٤٢٣" src="https://github.com/user-attachments/assets/4dd9acdd-ed3b-4373-ace6-0e397d2ef4b9" />
 ]
[https://drive.google.com/file/d/1H7IVp_2Wp6LOK0YOFpdjPmDlcAp_rIhT/view?usp=drivesdk ]

Data Design & ERD Schema (Database Design) 

[<img width="764" height="450" alt="ERD" src="https://github.com/user-attachments/assets/7b5100a4-4adc-4136-a931-c415b17eceb3" />]
[https://drive.google.com/file/d/19TUzbkHyBE4hMii8rerfndmvbH20hMoG/view?usp=sharing]

Sequence Diagram (Interaction Design)

[(https://github.com/SE226G5/medichain-g5-t7-med-app/blob/main/docs/images/IMG_20260518_175230_257.jpg)]
[<img width="764" height="450" alt="Sequence" src="https://github.com/SE226G5/medichain-g5-t7-med-app/blob/main/docs/images/IMG_20260518_175230_257.jpg" />]
[https://drive.google.com/file/d/1TceLXdOpNG5RXakv4Ry1kwcoUre0QJXS/view?usp=drivesdk]

UI/UX Wireframes (Frontend)

[
Screen 1: Medical Approval Dashboard### (لوحة تحكم الاعتماد الطبي)
gantt
    title SCREEN 1: MEDICAL APPROVAL
DASHBOARD (MED-APP)
    axisFormat %H:%M
    section [ MediChain System ]
    Logged in: Dr. Samer (Lab Doctor) : active, 08:00, 09:00
    section PENDING APPROVALS
    Sample #SAM-901 (John Doe) - Fasting Glucose : done, 08:00, 08:20
    Sample #SAM-902 (Jane Smith) - Complete Blood : active, 08:20, 08:40
    Sample #SAM-903 (Ali Hassan) - Lipid Profile : crit, 08:40, 09:00
###Screen 2: Result Review & Verification Screen (شاشة مراجعة وتدقيق النتائج)
gantt
    title SCREEN 2: RESULT REVIEW & VERIFICATION
    axisFormat %H:%M
    section PATIENT DATA
    John Doe (Age: 45 / Male) : active, 08:00, 09:00
    section CLINICAL VALUES
    Hemoglobin: 14.2 g/dL (NORMAL) : done, 08:00, 08:20
    Fasting Glucose: 240 mg/dL (CRITICAL HIGH) : crit, 08:20, 08:50
    section SYSTEM STATUS
    [X] Financial Payment Verified (REV-BIL Linked) : active, 08:50, 09:00
###Screen 3: Automated Locking Confirmation Dialog (نافذة تأكيد القفل الآلي)
gantt
    title SCREEN 3: LOCKING CONFIRMATION DIALOG
    axisFormat %H:%M
    section WARNING SYSTEM
    CRITICAL WARNING: SEAL RECORD PERMANENTLY : crit, 08:00, 08:40
    All data fields will switch to READ-ONLY mode : active, 08:40, 09:00
###Screen 4: Secure Modification Request Window (نافذة طلب التعديل الآمن)
gantt
    title SCREEN 4: SECURE MODIFICATION ACCESS PATH
    axisFormat %H:%M
    section AUDIT SECURITY
    User Authenticated: Dr. Samer (Lab Doctor) : active, 08:00, 08:30
    Mandatory Reason Input (Min 15 Characters) : crit, 08:30, 09:00
	
]

### Appendix B: GitHub Traceability Checklist
* **Instruction for Team Members:** Before submitting this SRS, ensure that:
  * [ ] Every User Story in Section 3.2 has a corresponding GitHub Issue.
  * [ ] Every GitHub Issue has an appropriate label (e.g., `enhancement`, `requirement`).
  * [ ] Pull Requests reference the Issue IDs (e.g., `Closes #12`). 
