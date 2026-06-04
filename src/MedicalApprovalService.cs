using System;

namespace MediChain.Module7.Services
{
    public class MedicalApprovalService
    {
        // الدالة الأولى: التحقق من صلاحية اعتماد النتيجة وتغيير حالتها لقفلها
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
    }
}
