using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejerciciojuegoporturnos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string nombre = PedirNombre();
            Jugador jugador = CrearJugador(nombre);
            List<Jugador> enemigos = CrearEnemigos();

            Juego juego = new Juego(jugador, enemigos);
            juego.Jugar();
        }

        static string PedirNombre()
        {
            Console.Write("Ingrese el nombre del jugador: ");
            return Console.ReadLine();
        }

        static Jugador CrearJugador(string nombre)
        {
            Console.WriteLine("Creación del jugador " + nombre);
            Console.Write("Ingrese la vida del jugador (menor o igual a 100): ");
            int vidaJugador = LeerEntero(1, 100);
            Console.Write("Ingrese el daño del jugador (menor o igual a 100): ");
            int damageJugador = LeerEntero(1, 100);
            return new Jugador(nombre, vidaJugador, damageJugador);
        }

        static List<Jugador> CrearEnemigos()
        {
            List<Jugador> enemigos = new List<Jugador>();
            enemigos.Add(new EnemigoMelee("1", 30, 10));
            enemigos.Add(new EnemigoRango("2", 20, 15, 3));
            enemigos.Add(new EnemigoMelee("3", 40, 8));
            return enemigos;
        }

        static int LeerEntero(int min, int max)
        {
            int valor;
            while (!int.TryParse(Console.ReadLine(), out valor) || valor < min || valor > max)
            {
                Console.WriteLine("Ingrese un número válido entre " + min + " y " + max + ".");
            }
            return valor;
        }
    }

    class Jugador
    {
        public string Nombre { get; }
        public int Vida { get; protected set; }
        protected int Damage { get; }

        public Jugador(string nombre, int vida, int damage)
        {
            Nombre = nombre;
            Vida = vida;
            Damage = damage;
        }

        public void RecibirDamage(int damageRecibido)
        {
            Vida -= damageRecibido;
        }

        public int ObtenerDamage()
        {
            return Damage;
        }

        public bool EstaVivo()
        {
            return Vida > 0;
        }
    }

    class EnemigoMelee : Jugador
    {
        public EnemigoMelee(string nombre, int vida, int damage) : base(nombre, vida, damage) { }
    }

    class EnemigoRango : Jugador
    {
        private int balas;

        public EnemigoRango(string nombre, int vida, int damage, int balas) : base(nombre, vida, damage)
        {
            this.balas = balas;
        }

        public void Disparar()
        {
            if (balas > 0)
            {
                balas--;
            }
        }

        public bool TieneBalas()
        {
            return balas > 0;
        }
    }

    class Juego
    {
        private Jugador jugador;
        private List<Jugador> enemigos;

        public Juego(Jugador jugador, List<Jugador> enemigos)
        {
            this.jugador = jugador;
            this.enemigos = enemigos;
        }

        public void Jugar()
        {
            for (int i = 0; i < enemigos.Count; i++)
            {
                if (!jugador.EstaVivo())
                    break;

                Console.WriteLine("Elije a qué enemigo atacar:");

                for (int j = 0; j < enemigos.Count; j++)
                {
                    Console.WriteLine((j + 1) + ") " + enemigos[j].Nombre);
                }

                int opcion;
                while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > enemigos.Count)
                {
                    Console.WriteLine("Opción inválida. Por favor, elige un número válido.");
                }

                Jugador enemigoElegido = enemigos[opcion - 1];

                Console.WriteLine("Inicio de pelea con enemigo " + enemigoElegido.Nombre);

                while (jugador.EstaVivo() && enemigoElegido.EstaVivo())
                {
                    RealizarTurnoJugador(enemigoElegido);
                    if (!jugador.EstaVivo() || !enemigoElegido.EstaVivo())
                        break;

                    RealizarTurnoEnemigo(enemigoElegido);

                    Console.WriteLine();
                }

                if (jugador.EstaVivo())
                {
                    Console.WriteLine("¡Has derrotado al enemigo!");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("¡Has sido derrotado por el enemigo!");
                    Console.WriteLine();
                    break;
                }

                enemigos.RemoveAt(opcion - 1); // Eliminar al enemigo derrotado de la lista
            }

            if (jugador.EstaVivo())
            {
                Console.WriteLine("¡Has ganado todas las peleas!");
            }
            else
            {
                Console.WriteLine("¡Has perdido todas las peleas!");
            }
        }

        private void RealizarTurnoJugador(Jugador enemigo)
        {
            Console.WriteLine("¡Turno de " + jugador.Nombre + "!");

            Console.WriteLine("Atacar al enemigo " + enemigo.Nombre + " (Vida: " + enemigo.Vida + ")");
            enemigo.RecibirDamage(jugador.ObtenerDamage());
        }

        private void RealizarTurnoEnemigo(Jugador enemigo)
        {
            Console.WriteLine("Turno del enemigo " + enemigo.Nombre + ":");

            jugador.RecibirDamage(enemigo.ObtenerDamage());
            Console.WriteLine("El enemigo ataca. Vida de " + jugador.Nombre + ": " + jugador.Vida);
        }
    }
}
