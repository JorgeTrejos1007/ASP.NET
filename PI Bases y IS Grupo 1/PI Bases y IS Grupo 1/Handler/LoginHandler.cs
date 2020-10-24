using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PIBasesISGrupo1.Handler
{
    public class LoginHandler
    {
        private BaseDeDatosHandler baseDeDatos;

        public LoginHandler()
        {
            baseDeDatos = new BaseDeDatosHandler();
        }

        public bool validarMiembro(string email, string password)
        {
            bool MiembroValido = false;
            string consulta = "SELECT count(1) FROM Usuario WHERE email=@email AND password=@password AND pais IS NOT NULL";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@email", email);
            comandoParaConsulta.Parameters.AddWithValue("@password", password);
            MiembroValido = baseDeDatos.saberSiExisteTupla(comandoParaConsulta) > 0;
            return MiembroValido;

        }

        public bool validarEstudiante(string email, string password)
        {
            bool validarEstudiante = false;
            string consulta = "SELECT count(1) FROM Usuario WHERE email=@email AND password=@password AND pais IS NULL";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@email", email);
            comandoParaConsulta.Parameters.AddWithValue("@password", password);
            validarEstudiante = baseDeDatos.saberSiExisteTupla(comandoParaConsulta) > 0;
            return validarEstudiante;

        }



    }
}
