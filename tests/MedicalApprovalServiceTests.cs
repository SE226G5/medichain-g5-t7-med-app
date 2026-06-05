using Xunit;
using MediChain.Module7.Services;

namespace MediChain.Module7.Tests
{
    public class MedicalApprovalServiceTests
    {
        private readonly MedicalApprovalService _service = new MedicalApprovalService();

        [Fact]
        public void ApproveResult_WhenStatusIsReady_ShouldReturnApprovedAndLocked()
        {
            string result = _service.ApproveResult("Ready_for_Approval", true);
            Assert.Equal("Approved_and_Locked", result); // التحقق من نجاح الاعتماد
        }

        [Fact]
        public void RequestOverride_WhenReasonIsEmpty_ShouldReturnRequiredError()
        {
            string result = _service.RequestOverride("Approved_and_Locked", "");
            Assert.Equal("Error: Modification reason is strictly required.", result); 
        }


        // =================================================================
        // Hussein Resha
        // =================================================================

        [Fact] 
        public void GenerateReport_WhenSampleIdIsEmpty_ShouldReturnInvalidSampleIdError()
        {
            string result = _service.GenerateReportMetadata("", "Approved_and_Locked", true, 3);
            Assert.Equal("Error: Invalid sample ID.", result);
        }

        [Fact] 
        public void GenerateReport_WhenStatusIsNotLocked_ShouldReturnUnapprovedError()
        {
            string result = _service.GenerateReportMetadata("SAM-901", "Ready_for_Approval", true, 3);
            Assert.Equal("Error: Cannot generate report for unapproved or unlocked results.", result);
        }

        [Fact] 
        public void GenerateReport_WhenPaymentIsNotVerified_ShouldReturnPaymentError()
        {
            string result = _service.GenerateReportMetadata("SAM-901", "Approved_and_Locked", false, 3);
            Assert.Equal("Error: Payment or insurance coverage is not verified.", result);
        }

        [Fact] 
        public void GenerateReport_WhenResultsCountIsZero_ShouldReturnEmptyDataError()
        {
            string result = _service.GenerateReportMetadata("SAM-901", "Approved_and_Locked", true, 0);
            Assert.Equal("Error: Clinical payload datastore cannot be empty.", result);
        }

        [Fact] 
        public void GenerateReport_WhenAllInputsAreValid_ShouldReturnSuccessAndUrl()
        {
            string result = _service.GenerateReportMetadata("SAM-901", "Approved_and_Locked", true, 3);
            Assert.Equal("Report_Generation_Ready_URL_Generated", result);
        }


        [Fact] 
        public void GenerateReportRefactored_WhenAllInputsAreValid_ShouldReturnSuccess()
        {
            string result = _service.GenerateReportMetadataRefactored("SAM-901", "Approved_and_Locked", true, 3);
            Assert.Equal("Report_Generation_Ready_URL_Generated", result);
        }

        [Fact] 
        public void GenerateReportRefactored_WhenCriteriaAreInvalid_ShouldReturnGenericError()
        {
            string result = _service.GenerateReportMetadataRefactored("SAM-901", "Ready_for_Approval", false, 0);
            Assert.Equal("Error: Report generation criteria not met.", result);
        }
        // =================================================================
        // Assead Ibrahim
        // =================================================================
        
        [Fact]
        public void ValidateApprovalRequest_WhenUserIsNotDoctor_ShouldReturnDoctorError()
        {
            string result = _service.ValidateApprovalRequest(
                false,
                "Ready_for_Approval",
                true,
                3);
        
            Assert.Equal(
                "Error: Only laboratory doctors can approve results.",
                result);
        }
        
        [Fact]
        public void ValidateApprovalRequest_WhenStatusIsInvalid_ShouldReturnStatusError()
        {
            string result = _service.ValidateApprovalRequest(
                true,
                "Pending",
                true,
                3);
        
            Assert.Equal(
                "Error: Result is not ready for approval.",
                result);
        }
        
        [Fact]
        public void ValidateApprovalRequest_WhenPaymentIsNotVerified_ShouldReturnPaymentError()
        {
            string result = _service.ValidateApprovalRequest(
                true,
                "Ready_for_Approval",
                false,
                3);
        
            Assert.Equal(
                "Error: Payment verification is required.",
                result);
        }
        
        [Fact]
        public void ValidateApprovalRequest_WhenResultsCountIsZero_ShouldReturnResultsError()
        {
            string result = _service.ValidateApprovalRequest(
                true,
                "Ready_for_Approval",
                true,
                0);
        
            Assert.Equal(
                "Error: No laboratory results found.",
                result);
        }
        
        [Fact]
        public void ValidateApprovalRequest_WhenAllInputsAreValid_ShouldReturnSuccess()
        {
            string result = _service.ValidateApprovalRequest(
                true,
                "Ready_for_Approval",
                true,
                3);
        
            Assert.Equal(
                "Approval_Request_Validated",
                result);
        }
        
        [Fact]
        public void ValidateApprovalRequestRefactored_WhenInputsAreValid_ShouldReturnSuccess()
        {
            string result = _service.ValidateApprovalRequestRefactored(
                true,
                "Ready_for_Approval",
                true,
                3);
        
            Assert.Equal(
                "Approval_Request_Validated",
                result);
        }
        
        [Fact]
        public void ValidateApprovalRequestRefactored_WhenInputsAreInvalid_ShouldReturnGenericError()
        {
            string result = _service.ValidateApprovalRequestRefactored(
                false,
                "Pending",
                false,
                0);
        
            Assert.Equal(
                "Error: Approval validation criteria not met.",
                result);
        }
   //=============================================
        // yousef abbas
        //=========================
        

        [Fact]
        public void CalculatePatientCoPay_PremiumAndElderly_ReturnsTenPercent()
        {
            double result = _insuranceService.CalculatePatientCoPay("Premium", 70, 1000);
            Assert.Equal(100, result);
        }

        [Fact]
        public void CalculatePatientCoPay_PremiumAndYoung_ReturnsTwentyPercent()
        {
            double result = _insuranceService.CalculatePatientCoPay("Premium", 30, 1000);
            Assert.Equal(200, result);
        }

        [Fact]
        public void CalculatePatientCoPay_BasicAndHighBill_ReturnsFiftyPercent()
        {
            double result = _insuranceService.CalculatePatientCoPay("Basic", 30, 1000);
            Assert.Equal(500, result);
        }

        [Fact]
        public void CalculatePatientCoPay_BasicAndLowBill_ReturnsSeventyPercent()
        {
            double result = _insuranceService.CalculatePatientCoPay("Basic", 30, 200);
            Assert.Equal(140, result);
        }

        [Fact]
        public void CalculatePatientCoPay_NoInsurance_ReturnsFullBill()
        {
            double result = _insuranceService.CalculatePatientCoPay("None", 30, 1000);
            Assert.Equal(1000, result);
        }

        // =================================================================
        // melad rajoh
        // =================================================================

        [Fact]
        public void EvaluateRisk_CriticalAndHighUrgency_ShouldReturnBoardApproval()
        {
            string result = _service.EvaluateModificationRisk("High", "Clinical", true, 5);
            Assert.Equal("Critical_High_Risk_Requires_Board_Approval", result);
        }

        [Fact]
        public void EvaluateRisk_CriticalAndLowUrgency_ShouldReturnHighRiskReview()
        {
            string result = _service.EvaluateModificationRisk("Low", "Clinical", true, 5);
            Assert.Equal("High_Risk_Review_Required", result);
        }

        [Fact]
        public void EvaluateRisk_NotCriticalAndDelayed_ShouldReturnChiefApproval()
        {
            string result = _service.EvaluateModificationRisk("Low", "Clinical", false, 30);
            Assert.Equal("Delayed_Modification_Requires_Chief_Approval", result);
        }

        [Fact]
        public void EvaluateRisk_NotCriticalAndTypo_ShouldReturnAutoApproved()
        {
            string result = _service.EvaluateModificationRisk("Low", "Data_Entry_Typo", false, 5);
            Assert.Equal("Low_Risk_Auto_Approved", result);
        }

        [Fact]
        public void EvaluateRisk_StandardScenario_ShouldReturnStandardReview()
        {
            string result = _service.EvaluateModificationRisk("Low", "General", false, 2);
            Assert.Equal("Standard_Review_Required", result);
        }

        [Fact]
        public void EvaluateRiskRefactored_CriticalHighUrgency_ShouldReturnBoardApproval()
        {
            string result = _service.EvaluateModificationRiskRefactored("High", "Clinical", true, 5);
            Assert.Equal("Critical_High_Risk_Requires_Board_Approval", result);
        }
    }
}
