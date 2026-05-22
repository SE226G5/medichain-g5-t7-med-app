# Database Schema

## 1. Entity-Relationship Diagram (ERD)
The structural and logical layout representing the normalization boundaries and schema definitions for Module 7 (MED-APP):

[<img width="764" height="450" alt="ERD" src="https://github.com/user-attachments/assets/7b5100a4-4adc-4136-a931-c415b17eceb3" />]

*🔗 Google Drive Project Link:* [https://drive.google.com/file/d/19TUzbkHyBE4hMii8rerfndmvbH20hMoG/view?usp=sharing](https://drive.google.com/file/d/19TUzbkHyBE4hMii8rerfndmvbH20hMoG/view?usp=sharing)

## 2. Tables List
The primary database layers designed to enforce dynamic data integrity are structured across these main tables:

| Table Name | Purpose / Description |
| :--- | :--- |
| `Samples` | Core Registry. Acts as the master directory entry for any medical specimen entering the tracking module. It maps the absolute unique payload key (`sample_id`) to the current global state of the workflow and locks the current technician identity. |
| `Tracking_Logs` | State Machine Logging. A strictly linear transaction table capturing every internal handoff (Receipt -> Analysis -> Clinical Review). It implements database-level timestamping (`start_time`, `end_time`) to compute precise turnaround times (TAT) and triggers an automated warning flag if predefined phase thresholds are violated. |
| `Test_Results` | Clinical Payload Datastore. Deconstructs incoming structured payloads (such as the integration JSON schema) into atomic test entities. This normalizes relational storage for items like test types, quantitative numerical data, and safety boundaries before medical validation. |

## 3. Shared Data (Integration Points)
*Which tables or data do you share with other teams?*

* **Shared Table/ID:** `sample_id`
  * **Shared With:** * **Module 6 (LAB-TRK):** To consume and receive raw diagnostic payload arrays via inbound API calls (`GET /api/v1/samples/ready-for-approval`).
    * **Module 4 (REF-TRK) & Module 5 (REV-BIL):** To safely expose the verified approval status and final secure read-only report PDF URL (`GET /api/v1/med-app/results/{sample_id}/report`).
