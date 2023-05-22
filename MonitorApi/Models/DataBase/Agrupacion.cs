﻿using System.ComponentModel.DataAnnotations;

namespace MonitorApi.Models.DataBase
{
    public class Agrupacion
    {
        public int AgrupacionID { get; set; }

        [StringLength(1000)]
        [Required]
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public List<Monitore>? Monitors { get; set; }
    }
}
