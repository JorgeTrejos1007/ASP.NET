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

        public bool validarUsuario(string email,string password) {
            bool usuarioValido = false;
            //string consulta = "SELECT COUNT(*) FROM Usuario WHERE email like @email AND password like @password";
            string consulta = "SELECT count(1) FROM Usuario WHERE email=@email AND password=@password";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@email", email);
            comandoParaConsulta.Parameters.AddWithValue("@password", password);
            usuarioValido=baseDeDatos.saberSiExisteTupla(comandoParaConsulta)>0;

            return usuarioValido;

        }



    }
}
