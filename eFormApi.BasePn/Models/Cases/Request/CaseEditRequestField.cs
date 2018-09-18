using System.Collections.Generic;

namespace Microting.eFormApi.BasePn.Models.Cases.Request
{
    public class CaseEditRequestField
    {
        public string FieldType { get; set; }
        public List<CaseEditRequestFieldValue> FieldValues { get; set; }
    }
}