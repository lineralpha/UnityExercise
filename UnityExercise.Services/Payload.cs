using System;
using System.ComponentModel.DataAnnotations;

namespace UnityExercise.Services
{
    /// <summary>
    /// The entity type.
    /// </summary>
    public class Payload
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Message { get; set; }
    }
}
