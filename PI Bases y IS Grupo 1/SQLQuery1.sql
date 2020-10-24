SELECT I.nombreCursoFK,E.tipoArchivo AS tipo,E.archivoImagen AS imagen   FROM Inscribirse I JOIN Curso C ON I.nombreCursoFK= C.nombre JOIN Usuario E ON C.emailEducadorFK= E.email WHERE I.emailEstudianteFK = 'stevegc112016@gmail.com';

SELECT * FROM Usuario;