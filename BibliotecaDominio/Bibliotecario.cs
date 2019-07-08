using BibliotecaDominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDominio
{
    public class Bibliotecario
    {
        public const string EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE = "El libro no se encuentra disponible";
        private  IRepositorioLibro libroRepositorio;
        private  IRepositorioPrestamo prestamoRepositorio;

        public Bibliotecario(IRepositorioLibro libroRepositorio, IRepositorioPrestamo prestamoRepositorio)
        {
            this.libroRepositorio = libroRepositorio;
            this.prestamoRepositorio = prestamoRepositorio;
        }

        public void Prestar(string isbn, string nombreUsuario)
        {
            DateTime? fechaEntrega;
            var prestado = prestamoRepositorio.ObtenerLibroPrestadoPorIsbn(isbn);
            if (prestado == null)
            {
                bool esPalindroma = Palindroma(isbn);
                if (esPalindroma)
                {
                    // devolver que no se puede prestar el libro
                }

                var libro = libroRepositorio.ObtenerPorIsbn(isbn);

                //calcular fecha de entrega
                int numeroISBN = CalcularNumeroISBN(isbn);
                if (numeroISBN < 30)
                {
                    fechaEntrega = null;
                }
                else
                {
                    fechaEntrega = CalcularFechaEntrega();
                }

                //Creo el objeto para prestar el libro
                Prestamo prestarLibro = new Prestamo(DateTime.Now, libro, fechaEntrega, nombreUsuario);
                prestamoRepositorio.Agregar(prestarLibro);
            }
        }

        public Boolean Palindroma(String cadena)
        {
            if (cadena.Length < 2)
                return true;
            
            if (cadena[0] == cadena[cadena.Length - 1])
            {
                return Palindroma(cadena.Substring(1, cadena.Length - 2));
            }

            return false;
        }

        public int CalcularNumeroISBN(string isbn)
        {
            int acumISBN = 0;
            int numero = 0;
            for (int i = 0; i < isbn.Length; i++)
            {
                if (Int32.TryParse(isbn[i].ToString(), out numero))
                {
                    acumISBN += numero;
                }
            }

            return acumISBN;
        }

        public DateTime CalcularFechaEntrega()
        {
            DateTime fecha = DateTime.Now.AddDays(-1);

            for (int i = 0; i < 15; i++)
            {
                fecha = fecha.AddDays(1);

                if (fecha.DayOfWeek == DayOfWeek.Sunday)
                    i -= 1;
            }

            if (fecha.DayOfWeek == DayOfWeek.Sunday)
                fecha = fecha.AddDays(1);

            return fecha;
        }


        public bool EsPrestado(string isbn)
        {
            var prestado = prestamoRepositorio.ObtenerLibroPrestadoPorIsbn(isbn);

            if (prestado == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
