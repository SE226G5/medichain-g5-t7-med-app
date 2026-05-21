using System;

namespace MediChain.Module7.Services
{
    public class MedicalApprovalService
    {
        // الدالة الأولى: التحقق من صلاحية اعتماد النتيجة وتغيير حالتها لقفلها
        public string ApproveResult(string currentStatus, bool isDoctor)
        {
            // شرط 1: التحقق من الصلاحيات (يجب أن يكون طبيباً)
            if (!isDoctor)
            {
                return "Error: Unauthorized. Only doctors can approve results.";
            }

            // شرط 2: التحقق من حالة العينة
            if (currentStatus == "Ready_for_Approval")
            {
                // إذا كانت جاهزة، يتم اعتمادها وقفلها
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

        // الدالة الثانية: معالجة طلب التعديل الاستثنائي لنتيجة مقفلة
        public string RequestOverride(string currentStatus, string modificationReason)
        {
            // شرط 1: لا يمكن طلب تعديل إلا إذا كانت النتيجة مقفلة أساساً
            if (currentStatus != "Approved_and_Locked")
            {
                return "Error: Override is only allowed for locked results.";
            }

            // شرط 2: إجبار الطبيب على كتابة سبب التعديل (لا يمكن أن يكون فارغاً)
            if (string.IsNullOrWhiteSpace(modificationReason))
            {
                return "Error: Modification reason is strictly required.";
            }

            // إذا تحققت الشروط، يتم فتح مسار التعديل
            return "Override_Requested_Successfully";
        }
    }
}
