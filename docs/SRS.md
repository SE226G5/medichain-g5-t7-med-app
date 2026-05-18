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
*   **2.1.2 User Interfaces:** [Describe the logical characteristics of your UI. Are you following a shared design system?].
*   
*   **2.1.3 Hardware Interfaces:** [None.the system operates through standard computer and tablet screens.].
*   
*   **2.1.4 Software Interfaces:** [Specify OS requirements, database dependencies, or third-party libraries].
*   
*   **2.1.5 Communications Interfaces:** [Define networking protocols used, e.g., HTTP/REST, WebSockets].
*   
*   **2.1.6 Memory & Operational Constraints:** [State minimum RAM, storage, and normal operating assumptions].

### 2.2 Product Functions
* **Instruction:** Provide a high-level, bulleted summary of the major functions your software performs. Do not go into deep detail here (save it for Section 3).

### 2.3 User Characteristics
* **Instruction:** Who will use your specific module? (e.g., Lab Technicians, Doctors, System Admins). Describe their technical expertise level.

### 2.4 Constraints, Assumptions, and Dependencies
* **Instruction:** List any factors that limit your development (e.g., medical data privacy laws, reliance on another team finishing their API first, specific coding languages mandated).

---

## 3. Specific Requirements (Agile Approach)
* **Instruction:** This section translates traditional functional requirements into Agile User Stories. Every feature must be traceable to the project management board.

### 3.1 External Interface Requirements
* Detailed architecture, endpoint links, and JSON request/response formats are fully documented in Appendix A
### 3.2 System Features & User Stories
* **Instruction:** Organize your requirements by Feature. For each feature, write the underlying requirements as User Stories and link them to your GitHub Issues.

#### 3.2.1 Feature: [Insert Feature Name, e.g., Patient Registration]
*   **Description:** [Briefly describe the feature].
*   **Priority:** [High / Medium / Low].
*   **User Stories:**
    *   **Story 1:** As a [User Role], I want to [Action/Goal] so that [Benefit/Value]. 
        * *Acceptance Criteria:* [List what must be true for this to be considered 'Done'].
        * *GitHub Issue:* [Link to Issue, e.g., #12]
    *   **Story 2:** As a [User Role], I want to [Action/Goal] so that [Benefit/Value].
        * *Acceptance Criteria:* [List criteria].
        * *GitHub Issue:* [Link to Issue, e.g., #13]

#### 3.2.2 Feature: [Insert Feature Name]
*   [Repeat the structure above for all module features].

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

<img width="764" height="450" alt="ERD" src="https://github.com/user-attachments/assets/7b5100a4-4adc-4136-a931-c415b17eceb3" />



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

[<img width="2281" height="1296" alt="Module7 drawio" src="https://github.com/user-attachments/assets/8c62c82b-99bb-4d30-8582-ea0a40f176ca" />]

[https://app.diagrams.net/#G1FCoI0mmtDmUcbFQzGzVSbyXYra6UOVFs#%7B%22pageId%22%3A%22tCMl2Ofq-hfzwy98Vaxk%22%7D]


Use Case Diagram (Requirements & Analysis)

[use case diagram image here – Student 2 Task]
[use case diagram link here – Student 2 Task]


Activity Diagram (Process Modeling)

[activity diagram image here – Student 3 Task]
[activity diagram link here – Student 3 Task]

Data Design & ERD Schema (Database Design)

[database schema diagram image here – Student 4 Task]
[database schema diagram link here – Student 4 Task]

Sequence Diagram (Interaction Design)

[sequence diagram image here – Student 5 Task]
[sequence diagram link here – Student 5 Task]

UI/UX Wireframes (Frontend)

[UI screen designs and wireframes here – Student 6 Task]

### Appendix B: GitHub Traceability Checklist
* **Instruction for Team Members:** Before submitting this SRS, ensure that:
  * [ ] Every User Story in Section 3.2 has a corresponding GitHub Issue.
  * [ ] Every GitHub Issue has an appropriate label (e.g., `enhancement`, `requirement`).
  * [ ] Pull Requests reference the Issue IDs (e.g., `Closes #12`). 
