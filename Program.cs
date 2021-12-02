using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static System.Console;


public class InstitutoContext : DbContext
{

    public DbSet<Alumno> Alumnos { get; set; }
    public DbSet<Modulo> Modulos { get; set; }
    public DbSet<Matricula> Matriculas { get; set; }

    public string connString { get; private set; }

    public InstitutoContext()
    {
        var database = "EF10Maider"; // "EF{XX}Nombre" => EF00Santi
        connString = $"Server=185.60.40.210\\SQLEXPRESS,58015;Database={database};User Id=sa;Password=Pa88word;MultipleActiveResultSets=true";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(connString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Matricula>().HasIndex(m => new
                {
                    m.ModuloID,
                    m.AlumnoID
                }).IsUnique();
    }

}
public class Alumno
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int AlumnoID { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public decimal Efectivo { get; set; }
    public string Pelo { get; set; }

}
public class Modulo
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ModuloID { get; set; }
    public string Titulo { get; set; }
    public int Creditos { get; set; }
    public int Curso { get; set; }
}
public class Matricula
{
    [Key]
    public int MatriculaID { get; set; }
    public int AlumnoID { get; set; }
    public int ModuloID { get; set; }
    public Alumno alumno { get; set; }
    public Modulo modulo { get; set; }
}

class Program
{
    static void GenerarDatos()
    {
        using (var db = new InstitutoContext())
        {
            // Borrar todo
            db.Alumnos.RemoveRange(db.Alumnos);
            db.Modulos.RemoveRange(db.Modulos);
            db.Matriculas.RemoveRange(db.Matriculas);
            
            // Añadir Alumnos
            // Id de 1 a 7
            db.Alumnos.Add(new Alumno{AlumnoID=1, Nombre="Ana", Edad=20, Efectivo=50, Pelo="Moreno"});
            db.Alumnos.Add(new Alumno{AlumnoID=2, Nombre="Maria", Edad=20, Efectivo=50, Pelo="Rubio"});
            db.Alumnos.Add(new Alumno{AlumnoID=3, Nombre="Juan", Edad=20, Efectivo=50, Pelo="Moreno"});
            db.Alumnos.Add(new Alumno{AlumnoID=4, Nombre="Pepe", Edad=20, Efectivo=50, Pelo="Rubio"});
            db.Alumnos.Add(new Alumno{AlumnoID=5, Nombre="Pedro", Edad=20, Efectivo=50, Pelo="Moreno"});
            db.Alumnos.Add(new Alumno{AlumnoID=6, Nombre="Maider", Edad=20, Efectivo=50, Pelo="Rubio"});
            db.Alumnos.Add(new Alumno{AlumnoID=7, Nombre="Unai", Edad=20, Efectivo=50, Pelo="Moreno"});
            // Añadir Módulos
            // Id de 1 a 10
            db.Modulos.Add(new Modulo{ModuloID=1, Titulo="Lengua", Creditos=100, Curso=1});
            db.Modulos.Add(new Modulo{ModuloID=2, Titulo="Euskera", Creditos=100, Curso=1});
            db.Modulos.Add(new Modulo{ModuloID=3, Titulo="Ingles", Creditos=200, Curso=2});
            db.Modulos.Add(new Modulo{ModuloID=4, Titulo="Frances", Creditos=100, Curso=1});
            db.Modulos.Add(new Modulo{ModuloID=5, Titulo="Fisica", Creditos=300, Curso=4});
            db.Modulos.Add(new Modulo{ModuloID=6, Titulo="Quimica", Creditos=100, Curso=1});
            db.Modulos.Add(new Modulo{ModuloID=7, Titulo="Matematicas", Creditos=400, Curso=3});
            db.Modulos.Add(new Modulo{ModuloID=8, Titulo="Historia", Creditos=100, Curso=2});
            db.Modulos.Add(new Modulo{ModuloID=9, Titulo="Informatica", Creditos=500, Curso=1});
            db.Modulos.Add(new Modulo{ModuloID=10, Titulo="Tecnologia", Creditos=100, Curso=3});
            // Matricular Alumnos en Módulos
            db.Add(new Matricula{AlumnoID=1, ModuloID=4});
            db.Add(new Matricula{AlumnoID=2, ModuloID=3});
            db.Add(new Matricula{AlumnoID=3, ModuloID=2});
            db.Add(new Matricula{AlumnoID=4, ModuloID=5});
            db.Add(new Matricula{AlumnoID=5, ModuloID=8});
            db.Add(new Matricula{AlumnoID=6, ModuloID=9});
            db.Add(new Matricula{AlumnoID=7, ModuloID=1});
            db.Add(new Matricula{AlumnoID=1, ModuloID=1});
            db.Add(new Matricula{AlumnoID=7, ModuloID=2});
            db.SaveChanges();
        }
    }

    static void BorrarMatriculaciones()
    {
        // Borrar las matriculas d
        // AlumnoId multiplo de 3 y ModuloId Multiplo de 2;
        // AlumnoId multiplo de 2 y ModuloId Multiplo de 5;
        using (var db = new InstitutoContext())
        {
           var matriculas = db.Matriculas;
            foreach(var element in matriculas){ 
                if (element.AlumnoID % 3 == 0 && element.AlumnoID % 2 == 0) {
                    Console.WriteLine(element.AlumnoID);
                    db.Matriculas.Remove(element);
                } 
                if (element.AlumnoID % 2 == 0 && element.AlumnoID % 5 == 0) {
                    Console.WriteLine(element.AlumnoID);
                    db.Matriculas.Remove(element);
                }
            }
            db.SaveChanges();
        }
    }
    
    static void RealizarQuery()
    {
        using (var db = new InstitutoContext())
        {
            // Las queries que se piden en el examen

            // Aparecen los que tienen el cabello moreno
            WriteLine("query 1");
            var query1 = db.Alumnos.Where(o => o.Pelo == "Moreno");
            foreach (var lista in query1){
                Console.WriteLine(lista.Nombre);
            }
            
            // Ordena los modulos
            WriteLine("query 2");
            var col3 = db.Matriculas.OrderBy(m => m.ModuloID);
            foreach (var lista2 in col3){
                WriteLine(lista2);
            }

            // Aparecen los que tienen 20 años de edad
            WriteLine("query 3");
            var query2 = db.Alumnos.Where(o => o.Edad == 20);
            foreach (var item in query2){
                WriteLine(item);
            }
            
        }
    }

    static void Main(string[] args)
    {
        GenerarDatos();
        BorrarMatriculaciones();
        RealizarQuery();
    }

}