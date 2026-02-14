using System.ComponentModel.DataAnnotations;

namespace Microting.eFormApi.BasePn.Infrastructure.Models.Auth;

public class ForgotPasswordModel
{
    [Required] public string Email { get; set; }
}