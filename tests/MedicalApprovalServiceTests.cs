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
    }
}
//----------------------------------------------------------------------------------------------------------------
