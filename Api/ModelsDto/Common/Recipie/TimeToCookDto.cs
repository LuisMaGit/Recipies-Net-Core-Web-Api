using System.ComponentModel.DataAnnotations;

namespace Api.ModelsDto.Common.Recipie
{
    public class TimeToCookDto
    {
        [Range(0, 24)] public int Hours { get; set; }
        [Range(0, 60)] public int Minutes { get; set; }
        [Range(0, 60)] public int Seconds { get; set; }
    }
}