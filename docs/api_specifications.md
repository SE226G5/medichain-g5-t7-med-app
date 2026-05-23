# API Specifications

## 1. Overview
The Medical Approval Module (MED-APP) handles the final stage of medical data before it is released. It ensures results are officially approved, locked to prevent tampering, and generates the final viewable report. It also manages secure pathways for any post-approval modification requests.

## 2. Main Endpoints (Provided by MED-APP)

### Endpoint 1: Approve and Lock Medical Results
* Method: POST /api/v1/medical/results/approve
* What it does: Final approval of medical results, locks the record to prevent direct editing, and generates the final uneditable report.
* Required Data:
  * requestId (string)
  * approvedBy (string - ID of the approving doctor)
  * resultsData (object - containing the medical readings, e.g., glucoseLevel)
* Returned Data:
  * medicalApprovalStatus (string - returns "Approved")
  * isLocked (boolean - returns true)
  * finalReportUrl (string - link to the read-only final report)

### Endpoint 2: Request Modification for Locked Results
* Method: POST /api/v1/medical/results/modification-request
* What it does: Opens a special approval pathway to edit an already locked result, mandating a justification log.
* Required Data:
  * requestId (string)
  * requestedBy (string - ID of the requester)
  * reasonForModification (string - mandatory justification for the change)
  * timestamp (datetime)
* Returned Data:
  * Acknowledgment of the modification request and routing to the audit log.

## 3. Consumed External Endpoints (Integration with Team 6)

### Endpoint: Fetch Samples Ready for Approval
* Provider: Team 6 (LAB-TRK - Sample Tracking)
* Method: GET /api/v1/samples/ready-for-approval
* What it does: MED-APP calls this endpoint to retrieve all samples that have been finalized and locked by lab technicians, serving as the queue for medical officers to review and approve.
* Response Data Format (JSON):
`json
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

## 4. External Dependencies & Team Repositories
To ensure seamless integration as defined in the unified architectural contract, our module (MED-APP) physically connects with the following team repositories:

* Team 4 (REF-TRK - Referral Portal): [https://github.com/SE226G5/medichain-g5_t4_ref_trk]
* Team 5 (REV-BIL - Billing & Revenue): [ https://github.com/SE226G5/medichain-rev-bil]
* Team 6 (LAB-TRK - Sample Tracking): [https://github.com/SE226G5/medichain-g5-t6-lab-trk]
