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
    
        // --------------------------------------------- Ali Hammoud -----------------------------------------------------

        
        public string DetermineSurgeryPriority(string triageLevel, bool isORAvailable, int patientAge, bool hasPreExistingConditions)
        {
            string priorityResult = "Schedule_Routine_Slot";

            if (string.IsNullOrEmpty(triageLevel))
            {
                priorityResult = "Error: Triage level must be specified.";
            }
            else if (triageLevel == "Critical_Emergency")
            {
                if (!isORAvailable)
                {
                    priorityResult = "Redirect_To_Nearest_Hospital_ICU";
                }
                else
                {
                    priorityResult = "Schedule_Immediate_Surgery";
                }
            }
            else if (triageLevel == "Urgent")
            {
                if (patientAge > 70 || hasPreExistingConditions)
                {
                    priorityResult = "Schedule_Within_2_Hours";
                }
                else
                {
                    priorityResult = "Schedule_Within_6_Hours";
                }
            }

            return priorityResult;
        }

        public string DetermineSurgeryPriorityRefactored(string triageLevel, bool isORAvailable, int patientAge, bool hasPreExistingConditions)
        {
            if (string.IsNullOrEmpty(triageLevel))
            {
                return "Error: Triage level must be specified.";
            }

            return triageLevel switch
            {
                "Critical_Emergency" => isORAvailable ? "Schedule_Immediate_Surgery" : "Redirect_To_Nearest_Hospital_ICU",
                "Urgent" when (patientAge > 70 || hasPreExistingConditions) => "Schedule_Within_2_Hours",
                "Urgent" => "Schedule_Within_6_Hours",
                _ => "Schedule_Routine_Slot"
            };
        }

    }
}


