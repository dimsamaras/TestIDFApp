using System.ComponentModel.DataAnnotations;

namespace TestIDFApp.Models;

public class IdfModel
{
    [Required] public string ResourceUrl { get; set; }
}