using System.ComponentModel.DataAnnotations;

namespace Microting.eFormApi.BasePn.Models.Auth
{
    public class ForgotPasswordModel
    {
        [Required] public string Email { get; set; }
    }
}