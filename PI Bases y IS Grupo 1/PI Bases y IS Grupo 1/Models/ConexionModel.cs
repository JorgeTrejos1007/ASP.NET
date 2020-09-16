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
            
            //string conString = @"Server=172.16.202.24;Database=BD_Grupo1;User ID=Grupo1;password=Adriancito;";
            string conString = @"Server=desktop-odor35t\basesededatos;Database=BD_Grupo1;Integrated Security = True;Pooling = False";

            con = new SqlConnection(conString);
            con.Open();
            return con;
        }



    }
}
