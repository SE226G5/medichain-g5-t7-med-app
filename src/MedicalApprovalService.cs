using System;

namespace MediChain.Module7.Services
{
    public class MedicalApprovalService
    {
        public string ApproveResult(string currentStatus, bool isDoctor)
        {
            if (!isDoctor)
            {
                return "Error: Unauthorized. Only doctors can approve results.";
            }

            if (currentStatus == "Ready_for_Approval")
            {
                return "Approved_and_Locked";
            }
            else if (currentStatus == "Approved_and_Locked")
            {
                return "Error: Result is already approved and locked.";
            }
            else
            {
                return "Error: Result is not ready for approval.";
            }
        }

        public string RequestOverride(string currentStatus, string modificationReason)
        {
            if (currentStatus != "Approved_and_Locked")
            {
                return "Error: Override is only allowed for locked results.";
            }

            if (string.IsNullOrWhiteSpace(modificationReason))
            {
                return "Error: Modification reason is strictly required.";
            }

            return "Override_Requested_Successfully";
        }
        //---------------------------------------------------Hussein Resha---------------------------------------------------
        public string GenerateReportMetadata(string sampleId, string currentStatus, bool isPaymentVerified, int resultsCount)
        {
            if (string.IsNullOrEmpty(sampleId))
            {
                return "Error: Invalid sample ID.";
            }

            if (currentStatus != "Approved_and_Locked")
            {
                return "Error: Cannot generate report for unapproved or unlocked results.";
            }

            if (!isPaymentVerified)
            {
                return "Error: Payment or insurance coverage is not verified.";
            }

            if (resultsCount <= 0)
            {
                return "Error: Clinical payload datastore cannot be empty.";
            }

            return "Report_Generation_Ready_URL_Generated";
        }

        public string GenerateReportMetadataRefactored(string sampleId, string currentStatus, bool isPaymentVerified, int resultsCount)
        {
            if (string.IsNullOrEmpty(sampleId) || 
                currentStatus != "Approved_and_Locked" || 
                !isPaymentVerified || 
                resultsCount <= 0)
            {
                return "Error: Report generation criteria not met.";
            }

            return "Report_Generation_Ready_URL_Generated";
        }
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------
