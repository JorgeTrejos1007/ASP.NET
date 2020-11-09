using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PIBasesISGrupo1.Handler
{
    public class FilterHandler
    {
        private BaseDeDatosHandler baseDeDatos;

        public FilterHandler()
        {
            baseDeDatos = new BaseDeDatosHandler();
        }

        private List<string> ObtenerRolDeUsuario(string emailUsuario)
        {
            List<string> rolesDelUsuario = new List<string>();

            string consulta = "SELECT rolUsuarioPK FROM Rol WHERE emailUsuarioFK=@email";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@email", emailUsuario);
            rolesDelUsuario = baseDeDatos.obtenerDatosDeColumna(comandoParaConsulta,"rolUsuarioPK");
            return rolesDelUsuario;

        }

        public bool verificarRol(string emailUsuario, params string[] rolesPerimitidos) {
            bool rolVerificado = false;
            List<string> rolesDelUsuario = ObtenerRolDeUsuario(emailUsuario);

            foreach (var rolPermitido in rolesPerimitidos) {
                if (rolesDelUsuario.Contains(rolPermitido) == true)
                {
                    rolVerificado = true;
                    break;
                }
            }

            return rolVerificado;

        }
        
    }




}
