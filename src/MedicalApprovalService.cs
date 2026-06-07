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

        // --------------------------------------------------- Assead Ibrahim ---------------------------------------------------
        public string ValidateApprovalRequest(
            bool isDoctor,
            string currentStatus,
            bool isPaymentVerified,
            int resultsCount)
        {
            if (!isDoctor)
            {
                return "Error: Only laboratory doctors can approve results.";
            }
        
            if (currentStatus != "Ready_for_Approval")
            {
                return "Error: Result is not ready for approval.";
            }
        
            if (!isPaymentVerified)
            {
                return "Error: Payment verification is required.";
            }
        
            if (resultsCount <= 0)
            {
                return "Error: No laboratory results found.";
            }
        
            return "Approval_Request_Validated";
        }
        
        private bool IsApprovalRequestValid(
            bool isDoctor,
            string currentStatus,
            bool isPaymentVerified,
            int resultsCount)
        {
            return isDoctor
                && currentStatus == "Ready_for_Approval"
                && isPaymentVerified
                && resultsCount > 0;
        }
        
        public string ValidateApprovalRequestRefactored(
            bool isDoctor,
            string currentStatus,
            bool isPaymentVerified,
            int resultsCount)
        {
            if (!IsApprovalRequestValid(
                isDoctor,
                currentStatus,
                isPaymentVerified,
                resultsCount))
            {
                if (currentStatus != "Ready_for_Approval")
                {
                    return "Error: Result is not ready for approval.";
                }
                return "Error: Approval validation criteria not met.";
            }
        
            return "Approval_Request_Validated";
        }

        //------------------------------Yousef Abbas -------------------------------------------------------------------------------------------------------
        public double CalculatePatientCoPay(string insuranceType, int patientAge, double billAmount)
        {
            double coPayAmount = billAmount;

            if (insuranceType == "Premium")
            {
                if (patientAge > 65)
                {
                    coPayAmount = billAmount * 0.10; 
                }
                else
                {
                    coPayAmount = billAmount * 0.20; 
                }
            }
            else if (insuranceType == "Basic")
            {
                if (billAmount > 500)
                {
                    coPayAmount = billAmount * 0.50; 
                }
                else
                {
                    coPayAmount = billAmount * 0.70; 
                }
            }
            else
            {
                coPayAmount = billAmount; 
            }

            return coPayAmount;
        }

        public double CalculatePatientCoPayRefactored(string insuranceType, int patientAge, double billAmount)
        {
            if (insuranceType == "Premium")
                return patientAge > 65 ? billAmount * 0.10 : billAmount * 0.20;

            if (insuranceType == "Basic")
                return billAmount > 500 ? billAmount * 0.50 : billAmount * 0.70;

            return billAmount;
        }

        // =================================================================
        // Yaarub Yousef
        // =================================================================
        public string VerifyBillingStatus(string claimStatus, bool hasValidInsurance, double coveragePercentage, int daysOverdue)
        {
            string statusResult = "Billing_Pending_Review";

            if (!hasValidInsurance)
            {
                if (daysOverdue > 30)
                {
                    statusResult = "Rejected_Collection_Agency";
                }
                else
                {
                    statusResult = "Rejected_Self_Pay_Required";
                }
            }
            else if (claimStatus == "Disputed")
            {
                statusResult = "On_Hold_Legal_Audit";
            }
            else if (coveragePercentage >= 0.80)
            {
                statusResult = "Approved_Auto_Disbursement";
            }

            return statusResult;
        }

        public string VerifyBillingStatusRefactored(string claimStatus, bool hasValidInsurance, double coveragePercentage, int daysOverdue)
        {
            if (!hasValidInsurance)
            {
                return daysOverdue > 30 ? "Rejected_Collection_Agency" : "Rejected_Self_Pay_Required";
            }

            if (claimStatus == "Disputed") return "On_Hold_Legal_Audit";
            if (coveragePercentage >= 0.80) return "Approved_Auto_Disbursement";

            return "Billing_Pending_Review";
        }

        // -------------------------------------------------------- melad rajoh -------------------------------------------------------
        public string EvaluateModificationRisk(string urgency, string reasonType, bool isCriticalTest, int hoursSinceLock)
        {
            string riskLevel = "Standard_Review_Required";

            if (isCriticalTest)
            {
                if (urgency == "High")
                {
                    riskLevel = "Critical_High_Risk_Requires_Board_Approval";
                }
                else
                {
                    riskLevel = "High_Risk_Review_Required";
                }
            }
            else if (hoursSinceLock > 24)
            {
                riskLevel = "Delayed_Modification_Requires_Chief_Approval";
            }
            else if (reasonType == "Data_Entry_Typo")
            {
                riskLevel = "Low_Risk_Auto_Approved";
            }

            return riskLevel;
        }

        public string EvaluateModificationRiskRefactored(string urgency, string reasonType, bool isCriticalTest, int hoursSinceLock)
        {
            if (isCriticalTest)
            {
                return urgency == "High" ? "Critical_High_Risk_Requires_Board_Approval" : "High_Risk_Review_Required";
            }

            return EvaluateStandardRisk(reasonType, hoursSinceLock);
        }

        private string EvaluateStandardRisk(string reasonType, int hoursSinceLock)
        {
            if (hoursSinceLock > 24) return "Delayed_Modification_Requires_Chief_Approval";
            if (reasonType == "Data_Entry_Typo") return "Low_Risk_Auto_Approved";
            
            return "Standard_Review_Required";
        }
    }
}
