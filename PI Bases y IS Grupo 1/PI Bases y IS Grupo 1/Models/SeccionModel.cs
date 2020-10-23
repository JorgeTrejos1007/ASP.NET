﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Para las librerias de Required y Display. Para hacer binding de las propiedades del modelo vista

using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace PIBasesISGrupo1.Models
{
    public class SeccionModel
    {
        [Required(ErrorMessage = "Es necesario que ingreses el nombre de la sección")]
        [RegularExpression(@"^[a-z|A-Z|0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombreSeccion {get; set;}

        [Required(ErrorMessage = "Es necesario que ingreses el nombre del curso")]
        [RegularExpression(@"^[a-z|A-Z|0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombreCurso { get; set; }

        public List<MaterialModel> listaMateriales { get; set; }
    }
}
