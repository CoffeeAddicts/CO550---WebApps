﻿using System.ComponentModel.DataAnnotations;

namespace FitToFight.Models
{
    public class AppSetting
    {
        [Key]
        [Required]
        public string Key { get; set; }
        public string? ValueString { get; set; }
        public bool? ValueBool { get; set; }
        public int? ValueInt { get; set; }
        public float? ValueDecimal { get; set; }
        public string? Explanation { get; set; }
    }
}
