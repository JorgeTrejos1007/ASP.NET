using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace PIBasesISGrupo1.Models
{
    public class ConexionModel
    {

        public SqlConnection Connection() {

            SqlConnection con;
            //string conString = @"Server=grupo1.database.windows.net;Database=Grupo1;User ID=Grupo1;password=Adriancito1;";
            string conString = @"Server=172.16.202.75;Database=BD_Grupo1;User ID=Grupo1;password=Hellencita;";
            //string conString = @"Server=DESKTOP-ODOR35T\BASESEDEDATOS;Database=BD_Grupo1;Integrated Security = True;Pooling = False";

            con = new SqlConnection(conString);
            
            return con;
        }



    }
}
